using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PropertyCrawler.Data;
using PropertyCrawler.Data.Entity;
using PropertyCrawler.Data.Models;
using PropertyCrawler.Models;
using PropertyCrawler.Models.Url;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PropertyCrawlerWeb.Services
{
    public interface ICrawlerService
    {
        void Execute(List<PostalCode> postalCodes, PropertyType type, Process process, ProxyIp proxy);
        //void UrlCrawler(List<PostalCode> postalCodes, PropertyType type, Process process);
        //void PropertiesCrawler(List<PostalCode> postalCodes, Process process, PropertyType type);
        void UpdatePriceUrlCrawler(List<PostalCode> postalCodes, PropertyType type, Process process, ProxyIp proxy);
    }
    public class CrawlerService : ICrawlerService
    {

        public void Execute(List<PostalCode> postalCodes, PropertyType type, Process process, ProxyIp proxy)
        {
            if (ProcessType.LastWeek == process.Type)
            {
                UpdatePriceUrlCrawler(postalCodes, type, process, proxy);
            }
            else if (ProcessType.LastTwoWeeks == process.Type)
            {
                UpdatePriceUrlCrawler(postalCodes, type, process, proxy);
            }
            else
            {
                UpdatePriceUrlCrawler(postalCodes, type, process, proxy);
            }
        }

        #region Properties
        public void PropertiesCrawler(List<PostalCode> postalCodes, Process process, PropertyType type, ProxyIp proxy)
        {
            var _appContext = new PropertyCrawler.Data.AppContext(true);

            var portal = _appContext.Portals.FirstOrDefault(x => x.Active && x.Name.ToLower().Contains("right"));
            var urlType = _appContext.UrlTypes.Where(x => x.Active).ToList();

            foreach (var postalcode in postalCodes)
            {
                PostalCodeProperty(postalcode, portal, urlType, process, type, proxy);
            }

        }

        public void PostalCodeProperty(PostalCode postalCode, Portal portal, List<UrlType> urlTypes, Process process, PropertyType propertyType, ProxyIp proxy)
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
                    ProcessProperty(portal, listItem, urlTypes.FirstOrDefault(x => x.Id == listItem.UrlTypeId), processPostalCode,proxy);
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

        public void ProcessProperty(Portal portal, Url url, UrlType urlType, ProcessPostalCode processPostalCode, ProxyIp proxyIp)
        {
            var _appContext = new PropertyCrawler.Data.AppContext(true);
            try
            {

                var proxy = new WebProxy()
                {
                    Address = new Uri($"http://{proxyIp.Ip}"),/*:{proxyPort}*/
                    BypassProxyOnLocal = false,
                    UseDefaultCredentials = false,

                    // *** These creds are given to the proxy server, not the web server ***
                    Credentials = new NetworkCredential(
                    userName: proxyIp.Username,
                    password: proxyIp.Password)
                };

                // Now create a client handler which uses that proxy

                var httpClientHandler = new HttpClientHandler()
                {
                    Proxy = proxy,
                };


                using (var client = new HttpClient(handler: httpClientHandler, disposeHandler: true))
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
                        var images = imageData.Select(x => new PropertyCrawler.Data.Image
                        {
                            DateAdded = dateNow,
                            DateModified = dateNow,
                            Active = true,
                            Caption = x.caption,
                            Url = x.masterUrl
                        }).ToList();

                        var postC = propertyData.location.postcode?.Trim();
                        //property
                        CultureInfo enUS = new CultureInfo("en-US");
                        var property = new PropertyCrawler.Data.Property
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
                            PropertyAdded = DateTime.TryParseExact(propertyData.propertyInfo.added, "yyyyMMdd", enUS, DateTimeStyles.AssumeLocal, out DateTime temp) ? temp : DateTime.MinValue,
                            Latitude = propertyData.location.latitude,
                            Longtitude = propertyData.location.longitude,
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
                       // _appContext.PropertyDescriptions.Add(propertyDescription);
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

        #region Update Price

        public void UpdatePriceUrlCrawler(List<PostalCode> postalCodes, PropertyType type, Process process, ProxyIp proxy)
        {
            var _appContext = new PropertyCrawler.Data.AppContext(true);

            var portal = _appContext.Portals.FirstOrDefault(x => x.Active && x.Name.ToLower().Contains("right"));
            var urlTypes = _appContext.UrlTypes.Where(x => x.Active).ToList();

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
                        ProcessUpdatePriceUrl(portal, listItem, PropertyType.Sell, urlTypes, processPostalCode,proxy);
                        ProcessUpdatePriceUrl(portal, listItem, PropertyType.Rent, urlTypes, processPostalCode,proxy);
                    }
                    else
                    {
                        ProcessUpdatePriceUrl(portal, listItem, type, urlTypes, processPostalCode,proxy);
                    }
                    UpdateProcess(processPostalCode, ProcessStatus.Success);
                }
                catch (Exception ex)
                {
                    UpdateProcess(processPostalCode, ProcessStatus.Failed);
                }
            });
        }

        public void ProcessUpdatePriceUrl(Portal portal, PostalCode postalCode, PropertyType propertyType, List<UrlType> urlTypes, ProcessPostalCode processPostalCode, ProxyIp proxyIp)
        {
            var _appContext = new PropertyCrawler.Data.AppContext(true);

            var type = propertyType == PropertyType.Sell ? "SALE" : "RENT";
            var urlType = propertyType == PropertyType.Sell ? "/property-for-sale" : "/property-to-rent";

            var currentUrls = _appContext.Urls.Where(x => x.Type==(int)propertyType && x.PostalCodeId == postalCode.Id).ToList();
            var currentUrlIds = currentUrls.Select(x => x.Id).OrderBy(x=>x).ToList();
            var currentUrlPropertyCodes= currentUrls.Select(x => x.PropertyCode).OrderBy(x => x).ToList();
            var currentProperties = _appContext.Properties.Where(x => x.UrlId.HasValue && currentUrlIds.Contains(x.UrlId.Value)).ToList();

            var proxy = new WebProxy()
            {
                Address = new Uri($"http://{proxyIp.Ip}"),
                BypassProxyOnLocal = false,
                UseDefaultCredentials = false,

                // *** These creds are given to the proxy server, not the web server ***
                Credentials = new NetworkCredential(
                userName: proxyIp.Username,
                password: proxyIp.Password)
              };

        // Now create a client handler which uses that proxy

                var httpClientHandler = new HttpClientHandler()
                {
                    Proxy = proxy,
                };

// Omit this part if you don't need to authenticate with the web server:
                //if (needServerAuthentication)
                //{
                //    httpClientHandler.PreAuthenticate = true;
                //    httpClientHandler.UseDefaultCredentials = false;

                //    // *** These creds are given to the web server, not the proxy server ***
                //    httpClientHandler.Credentials = new NetworkCredential(
                //        userName: serverUserName,
                //        password: serverPassword);
                // }


            //try
            //{
                using (var client = new HttpClient(handler: httpClientHandler, disposeHandler: true))
                {
                    client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");
                    client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,it;q=0.8,sq;q=0.7");
                    var page = 0;
                    var queryString = $"/find.html?locationIdentifier={portal.OutCodeKey + postalCode.OutCode}&sortType=10&propertyTypes=&includeLetAgreed=false&mustHave=&dontShow=&furnishTypes=&keywords=";

                    var pagesHtml = client.GetStringAsync(portal.Url + urlType + queryString).Result;
                    HtmlDocument pagesDocument = new HtmlDocument();
                    pagesDocument.LoadHtml(pagesHtml);

                    var allUrlCrawlerModels = new List<UrlCrawlerModel>();

                    //a// class// propertyCard-priceLink
                    var listOfUrlProperties = new PropertyUrlDetails();

                    var jsonString = pagesDocument.DocumentNode.InnerHtml.Split("<script>window.jsonModel = ")[1].Split("}</script><script>")[0] + "}";

                    var jsonObj = JsonConvert.DeserializeObject<PropertyUrlDetails>(jsonString);
                    listOfUrlProperties.properties.AddRange(jsonObj.properties);

                    var totalcount = int.Parse(pagesDocument.DocumentNode.SelectSingleNode(".//span[@class=\"searchHeader-resultCount\"]").InnerText.Replace(",", ""));

                    var pages = (totalcount / 24) + ((totalcount % 24) == 0 ? 0 : 1);

                    for (int i = 1; i < pages; i++)
                    {
                        try
                        {
                            queryString = $"/find.html?locationIdentifier={portal.OutCodeKey + postalCode.OutCode}&sortType=10&index={24 * i}&propertyTypes=&includeLetAgreed=false&mustHave=&dontShow=&furnishTypes=&keywords=";

                            pagesHtml = client.GetStringAsync(portal.Url + urlType + queryString).Result;
                            pagesDocument = new HtmlDocument();
                            pagesDocument.LoadHtml(pagesHtml);

                            jsonString = pagesDocument.DocumentNode.InnerHtml.Split("<script>window.jsonModel = ")[1].Split("}</script><script>")[0] + "}";

                            jsonObj = JsonConvert.DeserializeObject<PropertyUrlDetails>(jsonString);

                            listOfUrlProperties.properties.AddRange(jsonObj.properties);

                        }
                        catch (Exception ex)
                        {
                            break;
                        }
                    }

                    foreach (var prop in listOfUrlProperties.properties)
                    {

                        var urlCrawlerModel = new UrlCrawlerModel
                        {
                            Price = prop.price.amount,
                            PriceQualifier = prop.price.displayPrices[0].displayPriceQualifier,
                            PropertyCode = prop.id,
                            UrlTypeId = urlTypes.FirstOrDefault(a => prop.propertyUrl.Trim().Contains(a.UrlPortion))?.Id ?? null

                        };
                        allUrlCrawlerModels.Add(urlCrawlerModel);

                    }
                    allUrlCrawlerModels= allUrlCrawlerModels.Distinct().ToList();
                    var pricePropertyList = new List<PropertyPrice>();

                

                    //Find urls that don't exist any more at page

                    //Update list with property codes
                    var propertyCodesList = allUrlCrawlerModels.Select(x => x.PropertyCode).OrderBy(x=>x).ToList();

                    //Property Urls that already exist into database
                    var currentExistentUrls = currentUrls.Where(x => x.Active && propertyCodesList.Contains(x.PropertyCode)).ToList();
                    var currentExistenturlsId = currentExistentUrls.Select(x => x.Id);

                    var toBeLogicalDeleteIds = currentUrlIds.Except(currentExistenturlsId);

                    //From current property url list  get what does not exist any more and updated into active false

                    var toBeLogicDeletedUrls = currentUrls.Where(x=>toBeLogicalDeleteIds.Contains(x.Id)) ;///currentUrls.Except(currentExistentUrls).ToList();



                    //logic delete base on propertycodes into Url and Properties table
                    var dateNow = DateTime.UtcNow;
                    foreach (var url in toBeLogicDeletedUrls)
                    {
                        var urlToBeUpdated = url;
                        urlToBeUpdated.Active = false;
                        urlToBeUpdated.DateModified = dateNow;
                        var propertyToBeUpdated = currentProperties.FirstOrDefault(x => x.UrlId.HasValue && x.UrlId == url.Id);
                        if (propertyToBeUpdated !=null)
                        {
                            propertyToBeUpdated.DateModified = dateNow;
                            propertyToBeUpdated.Active = false;
                            _appContext.Properties.Update(propertyToBeUpdated);
                        }
                        _appContext.Urls.Update(urlToBeUpdated);
                    }

                    _appContext.SaveChanges();
                    //Existent 



                    foreach (var item in allUrlCrawlerModels)
                    {
                        var existUrl = currentUrls.FirstOrDefault(x => x.PropertyCode == item.PropertyCode);
                        PropertyCrawler.Data.Property existProperty = null;
                        if (existUrl != null)
                        {
                            existProperty = currentProperties.FirstOrDefault(x => x.UrlId == existUrl.Id);

                        }

                        //Properties does not exists
                        //Insert into Url Table, Start the process Properties
                        if (existUrl == null)
                        {
                            var insertUrl = new Url
                            {
                                PropertyCode = item.PropertyCode,
                                Type = (int)propertyType,
                                PortalId = portal.Id,
                                DateModified = DateTime.Now,
                                DateAdded = DateTime.Now,
                                Active = true,
                                PostalCodeId = postalCode.Id,
                                UrlTypeId = item.UrlTypeId
                            };
                            if (existProperty != null)
                            {
                                var property = existProperty;
                                property.Price = item.Price;
                                _appContext.Properties.Update(property);
                            }

                            _appContext.Urls.Add(insertUrl);
                            _appContext.SaveChanges();

                            if (existProperty == null)
                            {
                                ProcessProperty(portal, insertUrl, urlTypes.FirstOrDefault(x => x.Id == item.UrlTypeId), processPostalCode, proxyIp);
                            }

                        }
                        //else if (existUrlProperty.prop == null)
                        //{
                        //    ProcessProperty(portal, existUrlProperty.url, urlTypes.FirstOrDefault(x => x.Id == item.UrlTypeId), processPostalCode);
                        //}
                        //Property exist 
                        //insert into priceProperty table
                        //Update Properties last Price
                        else
                        {
                            if (existProperty == null)
                            {
                                if (existUrl !=null)
                                {
                                    var updUrl = existUrl;
                                    updUrl.Active = true;
                                    _appContext.Urls.Update(updUrl);
                                    _appContext.SaveChanges();
                                    ProcessProperty(portal, updUrl, urlTypes.FirstOrDefault(x => x.Id == item.UrlTypeId), processPostalCode,proxyIp);
                                }
                                else
                                {
                                    var insertUrl = new Url
                                    {
                                        PropertyCode = item.PropertyCode,
                                        Type = (int)propertyType,
                                        PortalId = portal.Id,
                                        DateModified = DateTime.Now,
                                        DateAdded = DateTime.Now,
                                        Active = true,
                                        PostalCodeId = postalCode.Id,
                                        UrlTypeId = item.UrlTypeId
                                    };
                                    _appContext.Urls.Add(insertUrl);
                                    _appContext.SaveChanges();
                                    ProcessProperty(portal, insertUrl, urlTypes.FirstOrDefault(x => x.Id == item.UrlTypeId), processPostalCode,proxyIp);
                                }

                            }
                            else
                            {
                                var priceProperty = new PropertyPrice
                                {
                                    PropertyId = existProperty.Id,
                                    Price = item.Price,
                                    PriceQualifier = item.PriceQualifier,
                                    DateModified = DateTime.Now,
                                    DateAdded = DateTime.Now,
                                    Active = true
                                };

                                var updUrl = existUrl;
                                updUrl.Active = true;
                                _appContext.Urls.Update(updUrl);

                                var property = existProperty;
                                property.Price = item.Price;
                                property.Active = true;
                                property.DateModified = DateTime.UtcNow;

                                _appContext.PropertyPrices.Add(priceProperty);
                                _appContext.Properties.Update(property);
                                _appContext.SaveChanges();
                                //pricePropertyList.Add(priceProperty);

                            }
                        }
                    }
                 // _appContext.SaveChanges();

                }

            //}
            //catch (Exception ex)
            //{

            //}
        }

        #endregion

        public ProcessPostalCode InsertProcessPostalCode(Process process, PostalCode postalCode)
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

        public void UpdateProcess(ProcessPostalCode processPostalCode, ProcessStatus status)
        {
            var _appContext = new PropertyCrawler.Data.AppContext(true);
            var proc = _appContext.ProcessPostalCodes.FirstOrDefault(x => x.Id == processPostalCode.Id);
            proc.Status = status;
            proc.DateModified = DateTime.UtcNow;
            _appContext.ProcessPostalCodes.Update(proc);
            _appContext.SaveChanges();
        }

    }

}