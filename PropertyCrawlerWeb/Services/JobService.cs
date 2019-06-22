using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        Task Job(List<PostalCode> postalCodes, PropertyType type, ProcessType processType, bool isScheduled, ScheduleInterval? scheduleInterval);

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

        public async Task Job(List<PostalCode> postalCodes, PropertyType propertyType, ProcessType processType, bool isScheduled, ScheduleInterval? scheduleInterval)
        {
           // LoadDependencies();
            var dateNow = DateTime.UtcNow;
            var process = new Process
            {
                DateAdded = dateNow,
                DateModified=dateNow,
                Active = true,
                Status = (int)ProcessStatus.Processing,
                Type = processType
            };
            _appContext.Processes.Add(process);
            _appContext.SaveChanges();

            if (isScheduled)
            {

            }

            var jobId = BackgroundJob.Enqueue(() => _crawlerService.PropertiesCrawler(postalCodes, process, propertyType));


            //update process status
            BackgroundJob.ContinueJobWith(jobId, () => Execute(jobId, process.Id), JobContinuationOptions.OnAnyFinishedState);

        }
        private void Execute(string jobId, int processId)
        {
            using (Hangfire.Storage.IStorageConnection connection = JobStorage.Current.GetConnection())
            {
                Hangfire.Storage.JobData job = connection.GetJobData(jobId);
                var process = _processRepository.Get(processId);
                if (job.State == "Succeeded")
                {
                    process.Status = ProcessStatus.Success;
                    //var succeeded = connection.GetAllItemsFromSet($"batch:{batchId}:succeeded");
                    //if (succeeded.Any())
                    //{
                    //};
                }
                else
                {
                    process.Status = ProcessStatus.Failed;
                    process.DateModified = DateTime.UtcNow;
                }
                //_processRepository.Update(process, process.Id);
                _appContext.Processes.Update(process);
                _appContext.SaveChanges();
            }

        }
        //private void LoadDependencies()
        //{
        //    var con = @"Server=.\Sqlexpress;Database=PropertiesDb;Trusted_Connection=True;";
        //    var services = new ServiceCollection();
        //    services.AddDbContext<PropertyCrawler.Data.AppContext>(opts => opts.UseSqlServer(con));
        //    services.AddHangfire(x => x.UseSqlServerStorage(con));


        //    services.AddScoped<IPostalCodeRepository, PostalCodeRepository>();
        //    services.AddScoped<IProcessRepository, ProcessRepository>();
        //    services.AddScoped<IJobService, JobService>();
        //    services.AddScoped<ICrawlerService, CrawlerService>();

        //    var serviceProvider = services.BuildServiceProvider();

        //    //_jobService = serviceProvider.GetService<IJobService>();
        //    _crawlerService = serviceProvider.GetService<ICrawlerService>();
        //    //_processRepository = serviceProvider.GetService<IProcessRepository>();
        //    //_postalCodeRepository = serviceProvider.GetService<IPostalCodeRepository>();
        //    //_context = serviceProvider.GetService<Data.AppContext>();



        //}
    }
}
