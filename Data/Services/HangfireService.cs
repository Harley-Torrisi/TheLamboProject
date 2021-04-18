using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.States;
using Hangfire.Storage;
using Microsoft.Extensions.Configuration;

namespace TheLamboProject.Data.Services
{
    public class HangfireService
    {
        private string PriceCacheCronTime { get; set; }
        private string PriceCacheQueueName { get; set; }

        public HangfireService()
        {
            var settings = Program.Configuration.GetSection("HangfireSettings");
            PriceCacheCronTime = settings["PriceCacheCronFrequency"];
            PriceCacheQueueName = settings["PriceCacheQueue"];
        }

        public void StartCachingPrices()
        {
            // Cleanup if needed?
            StopCachingPrices();

            RecurringJob.AddOrUpdate(() => Program.CoinspotService.CacheCurrentPrices(), PriceCacheCronTime, queue: PriceCacheQueueName);
        }

        public void StopCachingPrices()
        {
            var allJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
            allJobs.Where(x => x.Queue == PriceCacheQueueName).ToList().ForEach(job =>
                RecurringJob.RemoveIfExists(job.Id)
            );
        }
    }
}
