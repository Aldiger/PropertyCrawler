using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyCrawler.Data;
using PropertyCrawler.Data.Repositories;
using PropertyCrawlerWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PropertyCrawler.Test
{
    [TestClass]
    public class PropertyCrawlerTest
    {
        public PropertyCrawlerTest()
        {
            LoadDependencies();
        }

        private  IJobService _jobService;
        private IPostalCodeRepository _postalCodeRepository;
        private IProcessRepository _processRepository;
        private ICrawlerService _crawlerService;

        [TestMethod]
        public void TestMethod1()
        {
            var postalCode=_postalCodeRepository.GetAll().Take(2).ToList();
            _jobService.Job(postalCode, PropertyType.All, Data.Entity.ProcessType.Full, isScheduled:false, scheduleInterval:ScheduleInterval.Daily
                );
        }


        [TestMethod]
        public void TestMethod2()
        {
            var appcontext = new PropertyCrawler.Data.AppContext(true);
            var ddd = _postalCodeRepository.GetAll().ToList();
            var dateNow = DateTime.UtcNow;
            ///property-for-sale/property-81702935.html
            appcontext.UrlTypes.AddRange(new List<UrlType>
            {
                new UrlType
                {
                    DateAdded = dateNow,
                    Active = true,
                    DateModified=dateNow,
                    UrlPortion="/property-for-sale/property-",
                },
                new UrlType
                {
                    DateAdded = dateNow,
                    Active = true,
                    DateModified=dateNow,
                    UrlPortion="/property-to-rent/property-",
                },
                 new UrlType
                {
                    DateAdded = dateNow,
                    Active = true,
                    DateModified=dateNow,
                    UrlPortion="/commercial-property-for-sale/property-",
                },
                new UrlType
                {
                    DateAdded = dateNow,
                    Active = true,
                    DateModified=dateNow,
                    UrlPortion="/commercial-property-to-let/property-",
                }
            }
            );

        }

        

        private void LoadDependencies()
        {
            var con = @"Server=.\SQLExpress;Database=PropertiesDb;Trusted_Connection=True;";
            var services = new ServiceCollection();
            services.AddDbContext<Data.AppContext>(opts => opts.UseSqlServer(con));
            services.AddHangfire(x => x.UseSqlServerStorage(con));


            services.AddTransient<IPostalCodeRepository, PostalCodeRepository>();
            services.AddTransient<IProcessRepository, ProcessRepository>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<ICrawlerService, CrawlerService>();

            var serviceProvider = services.BuildServiceProvider();

            _jobService = serviceProvider.GetService<IJobService>();
            _crawlerService = serviceProvider.GetService<ICrawlerService>();
            _processRepository = serviceProvider.GetService<IProcessRepository>();
            _postalCodeRepository = serviceProvider.GetService<IPostalCodeRepository>();
            


        }
    }
}
