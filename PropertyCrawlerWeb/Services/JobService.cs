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
        void Job(List<PostalCode> postalCodes, PropertyCrawler.Data.PropertyType type, ProcessType processType);

    }
    public class JobService : IJobService
    { 
        private readonly ICrawlerService _crawlerService;
        private readonly IProcessRepository _processRepository;

        public void Job(List<PostalCode> postalCodes, PropertyCrawler.Data.PropertyType propertyType, ProcessType processType)
        {

           var dateNow = DateTime.UtcNow;
            var result = new Process
            {
                DateAdded = dateNow,
                DateModified = dateNow,
                Active = true,
                Status = (int)ProcessStatus.Processing,
                Type = processType
            };
            //insert process w

            var jobId = BackgroundJob.Enqueue(() => _crawlerService.UrlCrawler(postalCodes, propertyType, processType));


            //update process status
            BackgroundJob.ContinueJobWith(jobId, () => Execute(jobId, result.Id), JobContinuationOptions.OnAnyFinishedState);

        }
        private void Execute(string jobId,int processId)
        {
            using (Hangfire.Storage.IStorageConnection connection = JobStorage.Current.GetConnection())
            {
                Hangfire.Storage.JobData job = connection.GetJobData(jobId);
                var process= _processRepository
                if (job.State == "Succeeded")
                {
                    
                    //var succeeded = connection.GetAllItemsFromSet($"batch:{batchId}:succeeded");
                    //if (succeeded.Any())
                    //{
                    //};
                }
                else
                {

                }
            }
        }
    }
}
