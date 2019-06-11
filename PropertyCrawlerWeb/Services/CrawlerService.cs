﻿using HtmlAgilityPack;
using Newtonsoft.Json;
using PropertyCrawler.Data;
using PropertyCrawler.Data.Models;
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
        void UrlCrawler(List<PostalCode> postalCodes, PropertyCrawler.Data.Type type, bool full);
        void PropertiesCrawler(List<PostalCode> postalCodes, bool full);
        void PriceCrawler(List<PostalCode> postalCodes, bool full);
    }
    public class CrawlerService : ICrawlerService
    {
        public void UrlCrawler(List<PostalCode> postalCodes, PropertyCrawler.Data.Type type,  bool full)
        {

        }

        public void PropertiesCrawler(List<PostalCode> postalCodes, bool full)
        {
            var _appContext = new PropertyCrawler.Data.AppContext(true);


            var basedUrl = "https://www.rightmove.co.uk";

            var take = _appContext.Urls.Where(x => x.Active).ToList();


            var partitioner = Partitioner.Create(take);
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            Parallel.ForEach(partitioner, parallelOptions, (listItem, loopState) =>
            {
                ProcessProperty(basedUrl, listItem.PropertyUrl, listItem.Id);
            });


        }

        public void PriceCrawler(List<PostalCode> postalCodes, bool full)
        {

        }

        private void ProcessProperty(string basedUrl, string url, int urlId)
        {
            var _appContext = new PropertyCrawler.Data.AppContext(true);
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");
                    client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,it;q=0.8,sq;q=0.7");

                    var links = client.GetStringAsync(basedUrl + url).Result;
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(links);
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
                    var imageData = JsonConvert.DeserializeObject<List<Images>>(jsonData);

                    //Description itemprop="description"   itemprop="description"
                    var description = document.DocumentNode.SelectSingleNode(".//p[@itemprop=\"description\"]").InnerText.Trim();
                    //Full address  //address   itemprop=address
                    var address = document.DocumentNode.SelectSingleNode(".//address[@itemprop=\"address\"]").InnerText.Trim();
                    //agent phone number// class branch-telephone-number
                    var agentPhoneNumber = document.DocumentNode.SelectSingleNode(".//a[@class=\"branch-telephone-number\"]").InnerText.Trim();

                    var dateNow = DateTime.UtcNow;
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
                        Price = propertyData.propertyInfo.price,
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

                    //property
                    var property = new Property
                    {
                        DateAdded = dateNow,
                        DateModified = dateNow,
                        Active = true,
                        LettingType = propertyData.propertyInfo.lettingType,
                        PropertyType = propertyData.propertyInfo.propertyType,
                        PostalCode = propertyData.location.postcode,
                        Latitude = propertyData.location.latitude,
                        Longtitude = propertyData.location.longitude,
                        Added = propertyData.propertyInfo.added,
                        Address = address,
                        BedroomsCount = (byte)propertyData.propertyInfo.beds,
                        FloorPlanCount = propertyData.floorplanCount,
                        PropertySubType = propertyData.propertyInfo.propertySubType,
                        UrlId = urlId
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
            catch(Exception ex)
            {

            }
        } 

        private void ProcessUrlRent(List<PostalCode> codes)
        {

                var code = "AB10";
                var basedUrl = "https://www.rightmove.co.uk/property-to-rent/";
                var getOpCode = $"search.html?searchLocation={code}&locationIdentifier=&useLocationIdentifier=false&buy=For+sale";
                var opCode = "";
                var context = new PropertyCrawler.Data.AppContext(true);

            using (var client = new HttpClient(/*handler: httpClientHandler*/))
            {
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");
                client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,it;q=0.8,sq;q=0.7");

                foreach (var cod in codes)
                {
                    try
                    {
                        //getOpCode = $"search.html?searchLocation={cod.Code}&locationIdentifier=&useLocationIdentifier=false&buy=For+sale";

                        //var data = client.GetStringAsync(basedUrl + getOpCode).Result;

                        //HtmlDocument document = new HtmlDocument();
                        //document.LoadHtml(data);
                        //opCode = document.GetElementbyId("locationIdentifier").Attributes["value"].Value;
                        //if (string.IsNullOrEmpty(opCode.Trim()))
                        //{
                        //    cod.Active = false;
                        //    cod.DateModified = DateTime.Now;
                        //    context.PostalCodes.Update(cod);
                        //    context.SaveChanges();
                        //    continue;
                        //}
                        //opCode = opCode.Replace("^", "%5E");

                        //cod.OpCode = opCode;
                        //cod.DateModified = DateTime.Now;
                        //context.PostalCodes.Update(cod);

                        var page = 0;
                        var queryString = $"find.html?searchType=RENT&locationIdentifier={cod.OpCode}&index={24 * page}";
                        var links = client.GetStringAsync(basedUrl + queryString).Result;
                        HtmlDocument documentlinks = new HtmlDocument();
                        documentlinks.LoadHtml(links);
                        var linksNodes = documentlinks.DocumentNode.SelectNodes(".//a[@class=\"propertyCard-link\"]");

                        var totalcount = int.Parse(documentlinks.DocumentNode.SelectSingleNode(".//span[@class=\"searchHeader-resultCount\"]").InnerText.Replace(",", ""));

                        var pages = (totalcount / 24) + ((totalcount % 24) == 0 ? 0 : 1);

                        for (int i = 1; i < pages; i++)
                        {
                            try
                            {
                                queryString = $"find.html?searchType=SALE&locationIdentifier={cod.OpCode}&index={24 * i}&includeSSTC=false";

                                links = client.GetStringAsync(basedUrl + queryString).Result;
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
                        var urls = linksList.Select(x => new Url { PropertyUrl = x, Type =(int)PropertyCrawler.Data.Type.Rent, PortalId = 1, DateModified = DateTime.Now, DateAdded = DateTime.Now, Active = true, PostalCodeId = cod.Id });
                        context.Urls.AddRange(urls);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                    }
                }
            }

            
        }

        private void ProcessUrlSell(List<PostalCode> codes)
        {
                var code = "AB10";
                var basedUrl = "https://www.rightmove.co.uk/property-for-sale/";
                var getOpCode = $"search.html?searchLocation={code}&locationIdentifier=&useLocationIdentifier=false&buy=For+sale";
                var opCode = "";
                var context = new PropertyCrawler.Data.AppContext(true);

                using (var client = new HttpClient(/*handler: httpClientHandler*/))
                {
                    client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");
                    client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,it;q=0.8,sq;q=0.7");

                    foreach (var cod in codes)
                    {
                        try
                        {
                            getOpCode = $"search.html?searchLocation={cod.Code}&locationIdentifier=&useLocationIdentifier=false&buy=For+sale";

                            var data = client.GetStringAsync(basedUrl + getOpCode).Result;

                            HtmlDocument document = new HtmlDocument();
                            document.LoadHtml(data);
                            opCode = document.GetElementbyId("locationIdentifier").Attributes["value"].Value;
                            if (string.IsNullOrEmpty(opCode.Trim()))
                            {
                                cod.Active = false;
                                cod.DateModified = DateTime.Now;
                                context.PostalCodes.Update(cod);
                                context.SaveChanges();
                                continue;
                            }
                            opCode = opCode.Replace("^", "%5E");

                            cod.OpCode = opCode;
                            cod.DateModified = DateTime.Now;
                            context.PostalCodes.Update(cod);

                            var page = 0;
                            var queryString = $"find.html?searchType=SALE&locationIdentifier={opCode}&index={24 * page}&includeSSTC=false";
                            var links = client.GetStringAsync(basedUrl + queryString).Result;
                            HtmlDocument documentlinks = new HtmlDocument();
                            documentlinks.LoadHtml(links);
                            var linksNodes = documentlinks.DocumentNode.SelectNodes(".//a[@class=\"propertyCard-link\"]");

                            var totalcount = int.Parse(documentlinks.DocumentNode.SelectSingleNode(".//span[@class=\"searchHeader-resultCount\"]").InnerText);

                            var pages = (totalcount / 24) + ((totalcount % 24) == 0 ? 0 : 1);

                            for (int i = 1; i < pages; i++)
                            {
                                queryString = $"find.html?searchType=SALE&locationIdentifier={opCode}&index={24 * i}&includeSSTC=false";
                                links = client.GetStringAsync(basedUrl + queryString).Result;
                                documentlinks = new HtmlDocument();
                                documentlinks.LoadHtml(links);
                                var ddd = documentlinks.DocumentNode.SelectNodes(".//a[@class=\"propertyCard-link\"]");
                                foreach (var item in ddd)
                                {
                                    linksNodes.Add(item);
                                }
                            }

                            var linksList = new List<string>();
                            foreach (var item in linksNodes)
                            {
                                linksList.Add(item.Attributes["href"].Value);
                            }
                            linksList = linksList.Distinct().ToList();
                            var urls = linksList.Select(x => new Url { PropertyUrl = x, Type = 1, PortalId = 1, DateModified = DateTime.Now, DateAdded = DateTime.Now, Active = true, PostalCodeId = cod.Id });
                            context.Urls.AddRange(urls);
                            context.SaveChanges();
                        }
                        catch (Exception ex)
                        {

                        }

                    }


            }

        }
    }
}
