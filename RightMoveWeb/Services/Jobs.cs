using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RightMoveWeb.Services
{
    public interface IJobs
    {
        Task RunUrlGetProcess();
        
    }
    public class Jobs: IJobs
    {
        public async Task RunUrlGetProcess()
        {
            RecurringJob.AddOrUpdate(
                            () => Console.WriteLine("Recurring!"),
                            Cron.Weekly);
        }
    }
}
