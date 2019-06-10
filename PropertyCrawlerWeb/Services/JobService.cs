using Hangfire;
using PropertyCrawler.Data.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyCrawlerWeb.Services
{
    public interface IJobService
    {
        void UrlProcess();
        void PriceProcess();
        void PropertyProcess();


    }
    public class JobService: IJobService
    {
        private readonly ICrawlerService _crawlerService;

        public void UrlProcess()
        {
            //add process and list of processpostalcodes-- status processing
            var result =new Process();

            var jobId = BackgroundJob.Enqueue(
                () =>_crawlerService.UrlCrawler(null));


            //update process status
            BackgroundJob.ContinueJobWith(jobId, 
                () =>
                        Console.WriteLine("Update Process status!")
            );
        }

        public void PropertyProcess()
        {
            //add process and list of processpostalcodes-- status processing
            var result = new Process();

            var jobId = BackgroundJob.Enqueue(
                () => _crawlerService.UrlCrawler(null));


            //update process status
            BackgroundJob.ContinueJobWith(jobId,
                () =>
                        Console.WriteLine("Update Process status!")
            );
        }

        public void PriceProcess()
        {
            //add process and list of processpostalcodes-- status processing
            var result = new Process();

            var jobId = BackgroundJob.Enqueue(
                () => _crawlerService.UrlCrawler(null));


            //update process status
            BackgroundJob.ContinueJobWith(jobId,
                () =>
                        Console.WriteLine("Update Process status!")
            );
        }
    }
}
