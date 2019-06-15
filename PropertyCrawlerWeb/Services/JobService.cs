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
        void Job(List<PostalCode> postalCodes, PropertyType type, ProcessType processType);

    }
    public class JobService : IJobService
    {
        private readonly ICrawlerService _crawlerService;
        private readonly IProcessRepository _processRepository;

        public void Job(List<PostalCode> postalCodes, PropertyType propertyType, ProcessType processType)
        {

           var dateNow = DateTime.UtcNow;
            var process = new Process
            {
                DateAdded = dateNow,
                Active = true,
                Status = (int)ProcessStatus.Processing,
                Type = processType
            };
            _processRepository.Add(process);
            _processRepository.Complete();

            var jobId = BackgroundJob.Enqueue(() => _crawlerService.UrlCrawler(postalCodes, propertyType, processType));


            //update process status
            BackgroundJob.ContinueJobWith(jobId, () => Execute(jobId, process.Id), JobContinuationOptions.OnAnyFinishedState);

        }
        private void Execute(string jobId,int processId)
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
                }
                _processRepository.Update(process, process.Id);
                _processRepository.Complete();
            }

        }
    }
}
