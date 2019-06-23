using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PropertyCrawler.Data;
using PropertyCrawler.Data.Entity;
using PropertyCrawler.Data.Models;
using PropertyCrawler.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace PropertyCrawlerWeb.Services
{
    public interface ICrawlerService
    {
        void UrlCrawler(List<PostalCode> postalCodes, PropertyType type, Process process);
        void PropertiesCrawler(List<PostalCode> postalCodes, Process process, PropertyType type);
    }
    public class CrawlerService : ICrawlerService
    {
        //public void Url(List<PostalCode> postalCodes, PropertyType type, ProcessType processType)
        //{
        //    var _appContext = new PropertyCrawler.Data.AppContext(true);
        //    var urltypes = _appContext.UrlTypes.Where(x => x.Active).ToList();
        //    var portal = _appContext.Portals.FirstOrDefault(x => x.Name.Contains("rightmove"));

        //    if (postalCodes == null || postalCodes.Count == 0)
        //    {
        //        postalCodes = _appContext.PostalCodes.Where(x => x.Active).ToList();
        //    }

        //    var partitioner = Partitioner.Create(postalCodes);
        //    var parallelOptions = new ParallelOptions
        //    {
        //        MaxDegreeOfParallelism = Environment.ProcessorCount
        //    };

        //    Parallel.ForEach(partitioner, parallelOptions, (listItem, loopState) =>
        //    {
        //        ProcessUrl(portal, listItem, type, processPostalCode);
        //    });
        //}
        //public void PriceCrawler(List<PostalCode> postalCodes)
        //{

        //}

        #region Properties
        public void PropertiesCrawler(List<PostalCode> postalCodes, Process process, PropertyType type)
        {
            var _appContext = new PropertyCrawler.Data.AppContext(true);

            var portal = _appContext.Portals.FirstOrDefault(x => x.Active && x.Name.ToLower().Contains("right"));
            var urlType = _appContext.UrlTypes.Where(x => x.Active).ToList();

            foreach (var postalcode in postalCodes)
            {
                PostalCodeProperty(postalcode, portal, urlType, process, type);
            }

        }

        public void PostalCodeProperty(PostalCode postalCode, Portal portal, List<UrlType> urlTypes, Process process, PropertyType propertyType)
        {
            var _appContext = new PropertyCrawler.Data.AppContext(true);

            var dateNow = DateTime.UtcNow;
            var processPostalCode = new ProcessPostalCode
            {
                DateAdded = dateNow,
                Active = true,
                Status = (int)ProcessStatus.Processing,
                PostalCodeId = postalCode.Id,
                DateModified = dateNow,
                ProcessId = process.Id
            };
            _appContext.ProcessPostalCodes.Add(processPostalCode);

            _appContext.SaveChanges();

            List<Url> urls = new List<Url>();
            if (propertyType == PropertyType.All)
            {
                //get urls that are not in property table 
                urls = (from u in _appContext.Urls.Where(x => x.Active && x.PostalCodeId == postalCode.Id)
                        join p in _appContext.Properties on u.Id equals p.UrlId into gj
                        from x in gj.AsEnumerable()
                        select u).ToList();

            }
            else
            {
                //get urls that are not in property table 
                urls = (from u in _appContext.Urls.Where(x => x.Active
                        && x.PostalCodeId == postalCode.Id && x.Type == (int)propertyType)
                        join p in _appContext.Properties on u.Id equals p.UrlId into gj
                        from x in gj.AsEnumerable()
                        select u).ToList();
            }


            try
            {
                var partitioner = Partitioner.Create(urls);
                var parallelOptions = new ParallelOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                };

                Parallel.ForEach(partitioner, parallelOptions, (listItem, loopState) =>
                {
                    ProcessProperty(portal, listItem, urlTypes.FirstOrDefault(x => x.Id == listItem.UrlTypeId), processPostalCode);
                });

                processPostalCode.Status = ProcessStatus.Success;
                _appContext.ProcessPostalCodes.Update(processPostalCode);
            }
            catch (Exception ex)
            {
                processPostalCode.Status = ProcessStatus.Failed;
                _appContext.ProcessPostalCodes.Update(processPostalCode);
                _appContext.SaveChanges();
            }
        }

        public void ProcessProperty(Portal portal, Url url, UrlType urlType, ProcessPostalCode processPostalCode)
        {
            var _appContext = new PropertyCrawler.Data.AppContext(true);
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");
                    client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,it;q=0.8,sq;q=0.7");

                    var links = client.GetStringAsync(portal.Url + urlType.UrlPortion + url.PropertyCode + ".html").Result;

                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(links);
                    if (document.DocumentNode.InnerText.Contains("This property has been removed by the agent.")
                        || document.DocumentNode.InnerText.Contains("We are sorry but we could not find the property you have requested."))
                    {
                        url.DateModified = DateTime.UtcNow;
                        url.Active = false;
                        _appContext.Entry(url).State = EntityState.Modified;
                        _appContext.Urls.Update(url);
                        _appContext.SaveChanges();
                    }
                    else
                    {
                        var propertyInfo = @"'property',";
                        var branchInfo = @"'branch',";
                        var imagesInfo = @"images : ";
                        var data = document.DocumentNode.SelectNodes("//script").FirstOrDefault(x => x.InnerHtml.Contains(propertyInfo));
                        var jsonString = data.InnerText.Split(@"'property',")[1].Trim();
                        var jsonData = jsonString.Remove(jsonString.Length - 3, 3);
                        var propertyData = JsonConvert.DeserializeObject<Details>(jsonData);

                        var branchData = document.DocumentNode.SelectNodes("//script").FirstOrDefault(x => x.InnerText.Contains(branchInfo));
                        jsonString = branchData.InnerText.Split(@"'branch',")[1].Trim();
                        jsonData = jsonString.Remove(jsonString.Length - 3, 3);
                        var agentData = JsonConvert.DeserializeObject<Branch>(jsonData);

                        var imagesData = document.DocumentNode.SelectNodes("//script").FirstOrDefault(x => x.InnerText.Contains(imagesInfo));
                        jsonString = imagesData.InnerText.Split(imagesInfo)[1].Trim();
                        jsonData = jsonString.Split("\"}],")[0] + "\"}]";
                        var imageData = jsonString.Split("\"}],").Length > 1 ? JsonConvert.DeserializeObject<List<Images>>(jsonData) : new List<Images>();

                        //Description itemprop="description"   itemprop="description"
                        var description = document.DocumentNode.SelectSingleNode(".//p[@itemprop=\"description\"]").InnerText.Trim();
                        //Full address  //address   itemprop=address
                        var address = document.DocumentNode.SelectSingleNode(".//address[@itemprop=\"address\"]").InnerText.Trim();
                        //agent phone number// class branch-telephone-number
                        var agentPhoneNumber = document.DocumentNode.SelectSingleNode(".//a[@class=\"branch-telephone-number\"]").InnerText.Trim();

                        var dateNow = DateTime.UtcNow;
                        var priceValue = !string.IsNullOrEmpty(propertyData.propertyInfo.price) ? (Decimal.TryParse(propertyData.propertyInfo.price, out decimal tem) ? tem : 0) : 0;
                        //agent
                        var existAgent = _appContext.Agents.FirstOrDefault(x => x.AgentCode == agentData.branchId && x.Active);
                        if (existAgent == null)
                        {
                            existAgent = new Agent
                            {
                                DateAdded = dateNow,
                                DateModified = dateNow,
                                DisplayAddress = agentData.displayAddress,
                                Active = true,
                                AgentCode = agentData.branchId,
                                AgentType = agentData.agentType,
                                BranchName = agentData.branchName,
                                BranchPostcode = agentData.branchPostcode,
                                BrandName = agentData.brandName,
                                CompanyName = agentData.companyName,
                                CompanyType = agentData.companyType,
                                PhoneNumber = agentPhoneNumber,
                            };
                        }

                        //description
                        var propertyDescription = new PropertyDescription
                        {
                            DateAdded = dateNow,
                            DateModified = dateNow,
                            Description = description,
                            Active = true
                        };
                        //price
                        var price = new PropertyPrice
                        {
                            DateAdded = dateNow,
                            DateModified = dateNow,
                            Price = priceValue,
                            PriceQualifier = propertyData.propertyInfo.priceQualifier,
                            Active = true
                        };

                        //Images
                        var images = imageData.Select(x => new Image
                        {
                            DateAdded = dateNow,
                            DateModified = dateNow,
                            Active = true,
                            Caption = x.caption,
                            Url = x.masterUrl
                        }).ToList();

                        var postC = propertyData.location.postcode?.Trim();
                        //property
                        var property = new Property
                        {
                            DateAdded = dateNow,
                            DateModified = dateNow,
                            Active = true,
                            LettingType = propertyData.propertyInfo.lettingType,
                            PropertyType = propertyData.propertyInfo.propertyType,
                            PostalCode = propertyData.location.postcode,
                            PostalCodeFull = postC.Substring(0, postC.Length - 2),
                            PostalCodePrefix = postC.Substring(0, 2),
                            PostalCodeExtended = postC.Substring(0, postC.Length - 3).Trim(),
                            Type = (PropertyType)url.Type,
                            PropertyAdded = DateTime.TryParse(propertyData.propertyInfo.added, out DateTime temp) ? temp : DateTime.MinValue,
                            Latitude = propertyData.location.latitude,
                            Longtitude = propertyData.location.longitude,
                            Added = propertyData.propertyInfo.added,
                            Address = address,
                            Price = priceValue,
                            BedroomsCount = (byte)propertyData.propertyInfo.beds,
                            FloorPlanCount = propertyData.floorplanCount,
                            PropertySubType = propertyData.propertyInfo.propertySubType,
                            UrlId = url.Id
                        };
                        property.Images.AddRange(images);
                        property.PropertyPrices.Add(price);
                        if (existAgent.Id != 0)
                        {
                            property.AgentId = existAgent.Id;
                        }
                        else
                        {
                            property.Agent = existAgent;
                        }
                        property.PropertyDescription = propertyDescription;

                        _appContext.Properties.Add(property);
                        _appContext.SaveChanges();

                    }
                }
            }
            catch (Exception ex)
            {
                var dateNow = DateTime.UtcNow;
                var processPostalCodeUrlFailed = new ProcessPostalCodeUrlFailed
                {
                    DateAdded = dateNow,
                    Active = true,
                    DateModified = dateNow,
                    UrlId = url.Id,
                    ProcessPostalCodeId = processPostalCode.Id,
                    FailReason = ex.Message

                };
                _appContext.ProcessPostalCodeUrlFails.Add(processPostalCodeUrlFailed);

                _appContext.SaveChanges();
            }
        }
        #endregion

        #region Url

        public void UrlCrawler(List<PostalCode> postalCodes, PropertyType type, Process process)
        {
            var _appContext = new PropertyCrawler.Data.AppContext(true);

            var portal = _appContext.Portals.FirstOrDefault(x => x.Active && x.Name.ToLower().Contains("right"));

            var partitioner = Partitioner.Create(postalCodes);
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            Parallel.ForEach(partitioner, parallelOptions, (listItem, loopState) =>
            {
                var processPostalCode = InsertProcessPostalCode(process, listItem);
                try
                {
                    if (type==PropertyType.All)
                    {
                        ProcessUrl(portal, listItem,PropertyType.Rent, processPostalCode);
                        ProcessUrl(portal, listItem, PropertyType.Sell, processPostalCode);
                    }
                    else
                    {
                        ProcessUrl(portal, listItem, type, processPostalCode);
                    }
                }
                catch (Exception ex)
                {
                    processPostalCode.Status = ProcessStatus.Failed;
                    _appContext.ProcessPostalCodes.Update(processPostalCode);
                    _appContext.SaveChanges();
                }
            });
        }
        public void ProcessUrl(Portal portal, PostalCode postalCode, PropertyType propertyType,  ProcessPostalCode processPostalCode)
        {
            var _appContext = new PropertyCrawler.Data.AppContext(true);
            var type = propertyType == PropertyType.Sell ? "SALE" : "RENT";

            using (var client = new HttpClient(/*handler: httpClientHandler*/))
            {
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");
                client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,it;q=0.8,sq;q=0.7");
                var page = 0;
                var queryString = $"find.html?searchType={type}&locationIdentifier={portal.OutCodeKey + postalCode.OutCode}&index={24 * page}";
                var links = client.GetStringAsync(portal.Url + queryString).Result;
                HtmlDocument documentlinks = new HtmlDocument();
                documentlinks.LoadHtml(links);
                var linksNodes = documentlinks.DocumentNode.SelectNodes(".//a[@class=\"propertyCard-link\"]");

                var totalcount = int.Parse(documentlinks.DocumentNode.SelectSingleNode(".//span[@class=\"searchHeader-resultCount\"]").InnerText.Replace(",", ""));

                var pages = (totalcount / 24) + ((totalcount % 24) == 0 ? 0 : 1);

                for (int i = 1; i < pages; i++)
                {
                    try
                    {
                        queryString = $"find.html?searchType={type}&locationIdentifier={portal.OutCodeKey + postalCode.OutCode}&index={24 * i}&includeSSTC=false";

                        links = client.GetStringAsync(portal.Url + queryString).Result;
                        documentlinks = new HtmlDocument();
                        documentlinks.LoadHtml(links);
                        var ddd = documentlinks.DocumentNode.SelectNodes(".//a[@class=\"propertyCard-link\"]");
                        foreach (var item in ddd)
                        {
                            linksNodes.Add(item);
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
                linksList = linksList.Distinct().Where(x=>!string.IsNullOrEmpty(x)).ToList();

                var list = linksList.Select(x => int.Parse(x)).ToList();

                var existing= _appContext.Urls.Where(x => list.Contains(x.PropertyCode)).Select(x=>x.PropertyCode);
                list = list.Where(x => !existing.Contains(x)).ToList();

                var urls = list.Select(x =>
                new Url
                {
                    PropertyCode = x,
                    Type =(int) propertyType,
                    PortalId = portal.Id,
                    DateModified = DateTime.Now,
                    DateAdded = DateTime.Now,
                    Active = true,
                    PostalCodeId = postalCode.Id
                });
                _appContext.Urls.AddRange(urls);
                _appContext.SaveChanges();
            }
        }

        #endregion

        #region Update Price

        public void UpdatePriceUrlCrawler(List<PostalCode> postalCodes, PropertyType type, Process process)
        {
            var _appContext = new PropertyCrawler.Data.AppContext(true);

            var portal = _appContext.Portals.FirstOrDefault(x => x.Active && x.Name.ToLower().Contains("right"));

            var partitioner = Partitioner.Create(postalCodes);
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            Parallel.ForEach(partitioner, parallelOptions, (listItem, loopState) =>
            {
                var processPostalCode = InsertProcessPostalCode(process, listItem);
                try
                {
                    if (type == PropertyType.All)
                    {
                        ProcessUpdatePriceUrl(portal, listItem, PropertyType.Rent, processPostalCode);
                        ProcessUpdatePriceUrl(portal, listItem, PropertyType.Sell, processPostalCode);
                    }
                    else
                    {
                        ProcessUpdatePriceUrl(portal, listItem, type, processPostalCode);
                    }
                }
                catch (Exception ex)
                {
                    processPostalCode.Status = ProcessStatus.Failed;
                    _appContext.ProcessPostalCodes.Update(processPostalCode);
                    _appContext.SaveChanges();
                }
            });
        }
        public void ProcessUpdatePriceUrl(Portal portal, PostalCode postalCode, PropertyType propertyType, ProcessPostalCode processPostalCode)
        {
            var _appContext = new PropertyCrawler.Data.AppContext(true);
            var type = propertyType == PropertyType.Sell ? "SALE" : "RENT";
            using (var client = new HttpClient(/*handler: httpClientHandler*/))
            {
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");
                client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,it;q=0.8,sq;q=0.7");
                var page = 0;
                var queryString = $"find.html?searchType={type}&locationIdentifier={portal.OutCodeKey + postalCode.OutCode}&index={24 * page}";
                var links = client.GetStringAsync(portal.Url + queryString).Result;
                HtmlDocument documentlinks = new HtmlDocument();
                documentlinks.LoadHtml(links);
                var linksNodes = documentlinks.DocumentNode.SelectNodes(".//a[@class=\"propertyCard-link\"]");

                var totalcount = int.Parse(documentlinks.DocumentNode.SelectSingleNode(".//span[@class=\"searchHeader-resultCount\"]").InnerText.Replace(",", ""));

                var pages = (totalcount / 24) + ((totalcount % 24) == 0 ? 0 : 1);

                for (int i = 1; i < pages; i++)
                {
                    try
                    {
                        queryString = $"find.html?searchType={type}&locationIdentifier={portal.OutCodeKey + postalCode.OutCode}&index={24 * i}&includeSSTC=false";

                        links = client.GetStringAsync(portal.Url + queryString).Result;
                        documentlinks = new HtmlDocument();
                        documentlinks.LoadHtml(links);
                        var ddd = documentlinks.DocumentNode.SelectNodes(".//a[@class=\"propertyCard-link\"]");
                        foreach (var item in ddd)
                        {
                            linksNodes.Add(item);
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
                linksList = linksList.Distinct().ToList();


                /// find if property exists in Properties table 
                /// 
                /// 
                /// if NOT
                /// Insert into Url Table
                /// insert into Properties table
                /// if Yes
                /// Insert price into PriceProperty table


                var urls = linksList.Select(x =>
                new Url
                {
                    PropertyUrl = x,
                    Type = (int)PropertyType.Rent,
                    PortalId = 1,
                    DateModified = DateTime.Now,
                    DateAdded = DateTime.Now,
                    Active = true,
                    PostalCodeId = postalCode.Id
                });
                _appContext.Urls.AddRange(urls);
                _appContext.SaveChanges();
            }
        }

        #endregion



        //private void ProcessUrlRent(List<PostalCode> codes)
        //{
        //    var code = "AB10";
        //    var basedUrl = "https://www.rightmove.co.uk/property-to-rent/";
        //    var getOpCode = $"search.html?searchLocation={code}&locationIdentifier=&useLocationIdentifier=false&buy=For+sale";
        //    var opCode = "OUTCODE % 5E";
        //    var context = new PropertyCrawler.Data.AppContext(true);

        //    using (var client = new HttpClient(/*handler: httpClientHandler*/))
        //    {
        //        client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
        //        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");
        //        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,it;q=0.8,sq;q=0.7");

        //        foreach (var cod in codes)
        //        {
        //            try
        //            {
        //                #region Old
        //                //getOpCode = $"search.html?searchLocation={cod.Code}&locationIdentifier=&useLocationIdentifier=false&buy=For+sale";

        //                //var data = client.GetStringAsync(basedUrl + getOpCode).Result;

        //                //HtmlDocument document = new HtmlDocument();
        //                //document.LoadHtml(data);
        //                //opCode = document.GetElementbyId("locationIdentifier").Attributes["value"].Value;
        //                //if (string.IsNullOrEmpty(opCode.Trim()))
        //                //{
        //                //    cod.Active = false;
        //                //    cod.DateModified = DateTime.Now;
        //                //    context.PostalCodes.Update(cod);
        //                //    context.SaveChanges();
        //                //    continue;
        //                //}
        //                //opCode = opCode.Replace("^", "%5E");

        //                //cod.OpCode = opCode;
        //                //cod.DateModified = DateTime.Now;
        //                //context.PostalCodes.Update(cod);
        //                #endregion
        //                var page = 0;
        //                var queryString = $"find.html?searchType=RENT&locationIdentifier={cod.OpCode}&index={24 * page}";
        //                var links = client.GetStringAsync(basedUrl + queryString).Result;
        //                HtmlDocument documentlinks = new HtmlDocument();
        //                documentlinks.LoadHtml(links);
        //                var linksNodes = documentlinks.DocumentNode.SelectNodes(".//a[@class=\"propertyCard-link\"]");

        //                var totalcount = int.Parse(documentlinks.DocumentNode.SelectSingleNode(".//span[@class=\"searchHeader-resultCount\"]").InnerText.Replace(",", ""));

        //                var pages = (totalcount / 24) + ((totalcount % 24) == 0 ? 0 : 1);

        //                for (int i = 1; i < pages; i++)
        //                {
        //                    try
        //                    {
        //                        queryString = $"find.html?searchType=SALE&locationIdentifier={cod.OpCode}&index={24 * i}&includeSSTC=false";

        //                        links = client.GetStringAsync(basedUrl + queryString).Result;
        //                        documentlinks = new HtmlDocument();
        //                        documentlinks.LoadHtml(links);
        //                        var ddd = documentlinks.DocumentNode.SelectNodes(".//a[@class=\"propertyCard-link\"]");
        //                        foreach (var item in ddd)
        //                        {
        //                            linksNodes.Add(item);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        break;
        //                    }
        //                }

        //                var linksList = new List<string>();
        //                foreach (var item in linksNodes)
        //                {
        //                    linksList.Add(item.Attributes["href"].Value);
        //                }
        //                linksList = linksList.Distinct().ToList();
        //                var urls = linksList.Select(x => new Url { PropertyUrl = x, Type = (int)PropertyType.Rent, PortalId = 1, DateModified = DateTime.Now, DateAdded = DateTime.Now, Active = true, PostalCodeId = cod.Id });
        //                context.Urls.AddRange(urls);
        //                context.SaveChanges();
        //            }
        //            catch (Exception ex)
        //            {

        //            }
        //        }
        //    }


        //}

        //private void ProcessUrlSell(List<PostalCode> codes)
        //{
        //    var code = "AB10";
        //    var basedUrl = "https://www.rightmove.co.uk/property-for-sale/";
        //    var getOpCode = $"search.html?searchLocation={code}&locationIdentifier=&useLocationIdentifier=false&buy=For+sale";
        //    var opCode = "";
        //    var context = new PropertyCrawler.Data.AppContext(true);

        //    using (var client = new HttpClient(/*handler: httpClientHandler*/))
        //    {
        //        client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
        //        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");
        //        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,it;q=0.8,sq;q=0.7");

        //        foreach (var cod in codes)
        //        {
        //            try
        //            {
        //                getOpCode = $"search.html?searchLocation={cod.Code}&locationIdentifier=&useLocationIdentifier=false&buy=For+sale";

        //                var data = client.GetStringAsync(basedUrl + getOpCode).Result;

        //                HtmlDocument document = new HtmlDocument();
        //                document.LoadHtml(data);
        //                opCode = document.GetElementbyId("locationIdentifier").Attributes["value"].Value;
        //                if (string.IsNullOrEmpty(opCode.Trim()))
        //                {
        //                    cod.Active = false;
        //                    cod.DateModified = DateTime.Now;
        //                    context.PostalCodes.Update(cod);
        //                    context.SaveChanges();
        //                    continue;
        //                }
        //                opCode = opCode.Replace("^", "%5E");

        //                cod.OpCode = opCode;
        //                cod.DateModified = DateTime.Now;
        //                context.PostalCodes.Update(cod);

        //                var page = 0;
        //                var queryString = $"find.html?searchType=SALE&locationIdentifier={opCode}&index={24 * page}&includeSSTC=false";
        //                var links = client.GetStringAsync(basedUrl + queryString).Result;
        //                HtmlDocument documentlinks = new HtmlDocument();
        //                documentlinks.LoadHtml(links);
        //                var linksNodes = documentlinks.DocumentNode.SelectNodes(".//a[@class=\"propertyCard-link\"]");

        //                var totalcount = int.Parse(documentlinks.DocumentNode.SelectSingleNode(".//span[@class=\"searchHeader-resultCount\"]").InnerText);

        //                var pages = (totalcount / 24) + ((totalcount % 24) == 0 ? 0 : 1);

        //                for (int i = 1; i < pages; i++)
        //                {
        //                    queryString = $"find.html?searchType=SALE&locationIdentifier={opCode}&index={24 * i}&includeSSTC=false";
        //                    links = client.GetStringAsync(basedUrl + queryString).Result;
        //                    documentlinks = new HtmlDocument();
        //                    documentlinks.LoadHtml(links);
        //                    var ddd = documentlinks.DocumentNode.SelectNodes(".//a[@class=\"propertyCard-link\"]");
        //                    foreach (var item in ddd)
        //                    {
        //                        linksNodes.Add(item);
        //                    }
        //                }

        //                var linksList = new List<string>();
        //                foreach (var item in linksNodes)
        //                {
        //                    linksList.Add(item.Attributes["href"].Value);
        //                }
        //                linksList = linksList.Distinct().ToList();
        //                var urls = linksList.Select(x => new Url { PropertyUrl = x, Type = 1, PortalId = 1, DateModified = DateTime.Now, DateAdded = DateTime.Now, Active = true, PostalCodeId = cod.Id });
        //                context.Urls.AddRange(urls);
        //                context.SaveChanges();
        //            }
        //            catch (Exception ex)
        //            {

        //            }

        //        }


        //    }

        //}

        //private void ProcessUrl(PostalCode postalCode, Portal portal, PropertyType propertyType, List<UrlType> urlTypes)
        //{
        //    var basedUrl = portal.Url;

        //    var context = new PropertyCrawler.Data.AppContext(true);

        //    var currentUrlList = context.Urls.Where(x => x.PostalCodeId == postalCode.Id && x.Active).ToList();

        //    var iterations = 1;
        //    if (propertyType == PropertyType.All)
        //    {
        //        iterations = 2;
        //    }

        //    using (var client = new HttpClient())
        //    {
        //        client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
        //        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");
        //        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,it;q=0.8,sq;q=0.7");

        //        for (int a = 0; a < iterations; a++)
        //        {

        //            try
        //            {
        //                #region Old
        //                //var getOpCode = $"search.html?searchLocation={cod.OpCode}&locationIdentifier=&useLocationIdentifier=false&buy=For+sale";
        //                //var opCode = "";
        //                //getOpCode = $"search.html?searchLocation={cod.Code}&locationIdentifier=&useLocationIdentifier=false&buy=For+sale";

        //                //var data = client.GetStringAsync(basedUrl + getOpCode).Result;

        //                //HtmlDocument document = new HtmlDocument();
        //                //document.LoadHtml(data);
        //                //opCode = document.GetElementbyId("locationIdentifier").Attributes["value"].Value;
        //                //if (string.IsNullOrEmpty(opCode.Trim()))
        //                //{
        //                //    cod.Active = false;
        //                //    cod.DateModified = DateTime.Now;
        //                //    context.PostalCodes.Update(cod);
        //                //    context.SaveChanges();
        //                //    continue;
        //                //}
        //                //opCode = opCode.Replace("^", "%5E");

        //                //cod.OpCode = opCode;
        //                //cod.DateModified = DateTime.Now;
        //                //context.PostalCodes.Update(cod);
        //                #endregion

        //                var propertyTypeText = "";
        //                if (propertyType == PropertyType.Rent)
        //                {
        //                    propertyTypeText = "RENT";
        //                }
        //                else if (propertyType == PropertyType.Sell)
        //                {
        //                    propertyTypeText = "SALE";
        //                }
        //                else
        //                {
        //                    if (a == 0)
        //                    {
        //                        propertyTypeText = "SALE";
        //                    }
        //                    else
        //                    {
        //                        propertyTypeText = "RENT";
        //                    }

        //                }


        //                var page = 0;
        //                var queryString = $"find.html?searchType={propertyTypeText}&locationIdentifier={postalCode.OpCode}&index={24 * page}";

        //                var links = client.GetStringAsync(basedUrl + queryString).Result;
        //                HtmlDocument documentlinks = new HtmlDocument();
        //                documentlinks.LoadHtml(links);
        //                var linksNodes = documentlinks.DocumentNode.SelectNodes(".//a[@class=\"propertyCard-link\"]");

        //                var totalcount = int.Parse(documentlinks.DocumentNode.SelectSingleNode(".//span[@class=\"searchHeader-resultCount\"]").InnerText.Replace(",", ""));

        //                var pages = (totalcount / 24) + ((totalcount % 24) == 0 ? 0 : 1);

        //                for (int i = 1; i < pages; i++)
        //                {
        //                    try
        //                    {
        //                        queryString = $"find.html?searchType={propertyTypeText}&locationIdentifier={postalCode.OpCode}&index={24 * i}&includeSSTC=false";

        //                        links = client.GetStringAsync(basedUrl + queryString).Result;
        //                        documentlinks = new HtmlDocument();
        //                        documentlinks.LoadHtml(links);
        //                        var ddd = documentlinks.DocumentNode.SelectNodes(".//a[@class=\"propertyCard-link\"]");
        //                        foreach (var item in ddd)
        //                        {
        //                            linksNodes.Add(item);
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        break;
        //                    }
        //                }

        //                var linksList = new Dictionary<int, int>();
        //                foreach (var item in linksNodes)
        //                {
        //                    if (item.Attributes["href"].ValueLength > 0)
        //                    {
        //                        var value = item.Attributes["href"].Value;
        //                        var splitedbyDot = value.Split('.')[0];
        //                        var splitedByDash = splitedbyDot.Split('-');
        //                        var number = splitedByDash[splitedByDash.Length - 1];
        //                        if (int.TryParse(number, out int temp))
        //                        {

        //                            linksList.Add(temp, urlTypes.FirstOrDefault(x => value.Contains(x.UrlPortion)).Id);
        //                        }
        //                    }
        //                }

        //                var urls = linksList.Select(x => new Url
        //                {
        //                    PropertyUrl = x.ToString(),
        //                    Type = (int)PropertyType.Rent,
        //                    PortalId = portal.Id,
        //                    DateModified = DateTime.Now,
        //                    DateAdded = DateTime.Now,
        //                    Active = true,
        //                    PostalCodeId = postalCode.Id,
        //                    PropertyCode = x.Key,
        //                    UrlTypeId = x.Value
        //                });

        //                //if the url exist
        //                //get price and price offers
        //                //update property table column Price
        //                //insert into propertyprice table
        //                //update modifieddate of url item
        //                //if the url is new
        //                //insert into url table as new item
        //                //start the process of getting details of the property

        //                //if the url is not active anymore

        //                //update url item: active=false, datemofied, update property active to false


        //                //var firstNotSecond = list1.Except(list2).ToList();
        //                //var secondNotFirst = list2.Except(list1).ToList();

        //                context.Urls.AddRange(urls);
        //                context.SaveChanges();
        //            }
        //            catch (Exception ex)
        //            {

        //            }

        //        }
        //    }


        //}


        private ProcessPostalCode InsertProcessPostalCode(Process process, PostalCode postalCode)
        {
            var _appContext = new PropertyCrawler.Data.AppContext(true);
            var dateNow = DateTime.UtcNow;
            var processPostalCode = new ProcessPostalCode
            {
                DateAdded = dateNow,
                Active = true,
                Status = (int)ProcessStatus.Processing,
                PostalCodeId = postalCode.Id,
                DateModified = dateNow,
                ProcessId = process.Id
            };
            _appContext.ProcessPostalCodes.Add(processPostalCode);

            _appContext.SaveChanges();

            return processPostalCode;
        }
    }

}