using Hangfire;
using PropertyCrawler.Data;
using PropertyCrawler.Data.Entity;
using PropertyCrawler.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyCrawlerWeb.Services
{
    public interface IJobService
    {
        Task Job(List<PostalCode> postalCodes, PropertyType type, ProcessType processType, bool isScheduled, ScheduleInterval? scheduleInterval, ProxyIp proxyIp);

    }
    public class JobService : IJobService
    {
        public JobService(ICrawlerService crawlerService, IProcessRepository processRepository)
        {
            _crawlerService = crawlerService;
            _processRepository = processRepository;
        }
        private readonly ICrawlerService _crawlerService;
        private readonly IProcessRepository _processRepository;
        private PropertyCrawler.Data.AppContext _appContext = new PropertyCrawler.Data.AppContext(true);

        public async Task Job(List<PostalCode> postalCodes, PropertyType propertyType, ProcessType processType, bool isScheduled, ScheduleInterval? scheduleInterval, ProxyIp proxyIp)
        {
            var process = InsertProcess(processType, propertyType);
            
            if (isScheduled)
            {

                var cron = Cron.Daily();
                switch (scheduleInterval)
                {
                    case ScheduleInterval.Monthly:
                        cron = Cron.Monthly();
                        break;
                    case ScheduleInterval.Daily:
                        cron = Cron.Daily();
                        break;
                    case ScheduleInterval.Once_in_2_Days:
                        cron = Cron.DayInterval(2);
                        break;
                    case ScheduleInterval.Once_in_4_Days:
                        cron = Cron.DayInterval(4);
                        break;
                    case ScheduleInterval.Twice_a_Month:
                        cron = Cron.DayInterval(15);
                        break;
                    case ScheduleInterval.Weekly:
                        cron = Cron.DayInterval(7);
                        break;
                    default:
                        break;
                }

                RecurringJob.AddOrUpdate(
                    () => Recurrency(postalCodes,propertyType, process,proxyIp, processType),
                    cron);
            }
            else
            {
                var jobId = BackgroundJob.Enqueue(() => _crawlerService.Execute(postalCodes, propertyType, process, proxyIp, processType));
                if (int.TryParse(jobId, out int temp))
                {
                    UpdateProcess(temp, process);
                }
                //update process status
                BackgroundJob.ContinueJobWith(jobId, () => Execute(jobId, process.Id), JobContinuationOptions.OnAnyFinishedState);
            }
        }
        public void Recurrency(List<PostalCode> postalCodes, PropertyType propertyType, Process process, ProxyIp proxyIp, ProcessType processType)
        {
            var jobId = BackgroundJob.Enqueue(() => _crawlerService.Execute(postalCodes, propertyType, process, proxyIp, processType));

            if (int.TryParse(jobId, out int temp))
            {
                UpdateProcess(temp, process);
            }
            //update process status
            BackgroundJob.ContinueJobWith(jobId, () => Execute(jobId, process.Id), JobContinuationOptions.OnAnyFinishedState);
        }
        public void Execute(string jobId, int processId)
        {
            using (Hangfire.Storage.IStorageConnection connection = JobStorage.Current.GetConnection())
            {
                Hangfire.Storage.JobData job = connection.GetJobData(jobId);
                var process = _processRepository.Get(processId);
                if (job.State == "Succeeded")
                {
                    process.Status = ProcessStatus.Success;
                }
                else
                {
                    process.Status = ProcessStatus.Failed;
                }
                UpdateProcess(process, process.Status);
            }

        }

        public Process InsertProcess(ProcessType processType, PropertyType propertyType)
        {
            var dateNow = DateTime.UtcNow;
            var process = new Process
            {
                DateAdded = dateNow,
                DateModified = dateNow,
                Active = true,
                Retry=0,
                Status = (int)ProcessStatus.Processing,
                Type = processType,
                PropertyType = propertyType
            };
            _appContext.Processes.Add(process);
            _appContext.SaveChanges();
            return process;
        }

        public void UpdateProcess(Process process, ProcessStatus status)
        {
            var proc = _appContext.Processes.FirstOrDefault(x=>x.Id==process.Id);
            proc.Status = status;
            proc.DateModified = DateTime.UtcNow;
            _appContext.Processes.Update(proc);
            _appContext.SaveChanges();
        }

        public void UpdateProcess(int jobId, Process process)
        {
            var proc = _appContext.Processes.FirstOrDefault(x => x.Id == process.Id);
            proc.JobId = jobId;
            proc.DateModified = DateTime.UtcNow;
            proc.Retry = proc.Retry.HasValue ? ++proc.Retry : 1;
            _appContext.Processes.Update(proc);
            _appContext.SaveChanges();
        }
    }
}
