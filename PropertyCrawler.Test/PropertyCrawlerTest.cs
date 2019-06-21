using Hangfire;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PropertyCrawler.Data;
using PropertyCrawler.Data.Repositories;
using PropertyCrawlerWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PropertyCrawler.Test
{
    [TestClass]
    public class PropertyCrawlerTest
    {
        public PropertyCrawlerTest()
        {
            LoadDependencies();
        }

        private IJobService _jobService;
        private IPostalCodeRepository _postalCodeRepository;
        private IProcessRepository _processRepository;
        private ICrawlerService _crawlerService;
        private Data.AppContext _context;

        [TestMethod]
        public void TestMethod1()
        {
            var postalCode = _postalCodeRepository.GetAll().Take(2).ToList();
            _jobService.Job(postalCode, PropertyType.All, Data.Entity.ProcessType.Full, isScheduled: false, scheduleInterval: ScheduleInterval.Daily
                );
        }



        [TestMethod]

        public async Task PropertyCrawl()
        {

            var postalcodes = await _context.PostalCodes.Take(2).ToListAsync();
            var propertyType = PropertyType.Rent.ToString().ToLower();
            string prefixOutCode = "OUTCODE%5E";

            var basedUrl = $"https://www.rightmove.co.uk/property-to-{propertyType}/";
            var context = new PropertyCrawler.Data.AppContext(true);

            using (var client = new HttpClient(/*handler: httpClientHandler*/))
            {
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");
                client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,it;q=0.8,sq;q=0.7");

                foreach (var cod in postalcodes)
                {
                    try
                    {
                        var prices = new List<string>();
                        var page = 0;
                        var queryString = $"find.html?searchType=RENT&locationIdentifier={prefixOutCode+cod.OutCode}&index={24 * page}";
                        var links = client.GetStringAsync(basedUrl + queryString).Result;
                        HtmlDocument documentlinks = new HtmlDocument();
                        documentlinks.LoadHtml(links);
                        var linksNodes = documentlinks.DocumentNode.SelectNodes(".//a[@class=\"propertyCard-link\"]");
                        var pricesNodes = documentlinks.DocumentNode.SelectNodes(".//span[@class=\"propertyCard-priceValue\"]").Select(x=>x.InnerText.Replace("£","").Split(" ")[0]).ToList();
                        var totalcount = int.Parse(documentlinks.DocumentNode.SelectSingleNode(".//span[@class=\"searchHeader-resultCount\"]").InnerText.Replace(",", ""));

                        var pages = (totalcount / 24) + ((totalcount % 24) == 0 ? 0 : 1);

                        for (int i = 1; i < pages; i++)
                        {
                            try
                            {
                                queryString = $"find.html?searchType=RENT&locationIdentifier={prefixOutCode + cod.OutCode}&index={24 * i}&includeSSTC=false";

                                links = client.GetStringAsync(basedUrl + queryString).Result;
                                documentlinks = new HtmlDocument();
                                documentlinks.LoadHtml(links);
                                var ddd = documentlinks.DocumentNode.SelectNodes(".//a[@class=\"propertyCard-link\"]");
                                var ppp = documentlinks.DocumentNode.SelectNodes(".//span[@class=\"propertyCard-priceValue\"]").Select(x => x.InnerText.Replace("£", "").Split(" ")[0]);
                                foreach (var item in ddd)
                                {
                                    linksNodes.Add(item);
                                }
                                foreach (var item in ppp)
                                {
                                    pricesNodes.Add(item);
                                }
                            }
                            catch (Exception ex)
                            {
                                break;
                            }
                        }

                        var linksList = new List<string>();
                        foreach (var item in linksNodes)
                        {
                            linksList.Add(item.Attributes["href"].Value);
                        }
                        var pricesList = pricesNodes.Distinct().ToList();

                        linksList = linksList.Where(x=>!string.IsNullOrEmpty(x)).Distinct().ToList();
                        var urls = linksList.Select(x => new Url { PropertyUrl = x, Type = (int)PropertyType.Rent, PortalId = 1, DateModified = DateTime.Now, DateAdded = DateTime.Now, Active = true, PostalCodeId = cod.Id });
                        var price = pricesList.Select(x => new PropertyPrice { Price = Convert.ToDecimal(x), DateAdded = DateTime.Now, DateModified = DateTime.Now });
                        
                        //context.Urls.AddRange(urls);
                        //context.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

        }



        [TestMethod]
        public async Task TestMethod()
        {

            var postalcode = await _context.PostalCodes.ToListAsync();

            //var properties = await _context.Properties.OrderBy(x => x.Id).ToListAsync();
            //int count = 100000;

            //var list1 = properties.Take(count).ToList();
            //var list2 = properties.Skip(count).Take(count).ToList();
            //var list3 = properties.Skip(2 * count).Take(count).ToList();
            //var list4 = properties.Skip(3 * count).Take(count).ToList();
            //var list5 = properties.Skip(4 * count).Take(count).ToList();
            //var list6 = properties.Skip(5 * count).Take(count).ToList();
            //var list7 = properties.Skip(6 * count).ToList();

            foreach (var item in postalcode)
            {
                var stringa = item.OpCode;
                if (stringa == null)
                {
                    item.OutCode = 0;
                }
                else
                {
                    var correct = stringa.Replace("OUTCODE%5E", "");

                    var number = Convert.ToInt32(correct);

                    item.OutCode = number;

                }
                _context.PostalCodes.Update(item);
            }
            await _context.SaveChangesAsync();

            //foreach (var item in list7)
            //{
            //    var stringa = item.PostalCode.ToString();
            //    var first = stringa.Split(" ")[0];
            //    var end = stringa.Split(" ")[1];
            //    item.PostalCodeExtended = first;
            //    item.PostalCodePrefix = string.Join("", Regex.Matches(first, @"[A-Z]").Select(x => x.Value));
            //    var endDigits = Regex.Match(end, @"[0-9]").Value;
            //    item.PostalCodeFull = first + " " + endDigits;
            //     _context.Properties.Update(item);
            //}

            //await _context.SaveChangesAsync();

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
            var con = @"Server=localhost;Database=PropertiesDb;Trusted_Connection=True;";
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
            _context = serviceProvider.GetService<Data.AppContext>();



        }
    }
}
