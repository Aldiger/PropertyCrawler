using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using PropertyCrawler.Data;
using PropertyCrawler.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace PropertyCrawler
{
    class Program
    {
        public static Data.AppContext context = new Data.AppContext(true);
        static void Main(string[] args)
        {
            //CanadaCars();

            var code = "AB10";
            var basedUrl = "https://www.rightmove.co.uk/property-for-sale/";
            var getOpCode = $"search.html?searchLocation={code}&locationIdentifier=&useLocationIdentifier=false&buy=For+sale";
            var opCode = "";
            //GetPostalcodes();
            //var context = new Data.AppContext(true);
            //var codes = context.PostalCodes.Where(x => x.Active && x.Id > 1874).OrderBy(x => x.Id).ToList();
            //var existing = context.Urls.Where(x => x.Type == (int)Data.Type.Rent).Select(x => x.PostalCodeId).Distinct().ToList();
            //var r = codes.Select(x => x.Id).Except(existing).ToList();
            //codes = codes.Where(x => r.Contains(x.Id)).ToList();
            //var take = 400;

            //var url = context.Urls.OrderBy(x => x.Id).Take(66).ToList();
            //var list=(from url in context.Urls.Where(x => x.Active)
            //         join prop in context.Properties.Where(x=>x.Active) on url.Id equals prop.UrlId into gj
            //from subpet in gj.DefaultIfEmpty()
            //select new { url }).ToList();

            //var list1 = (from prop in context.Properties.Where(x => x.Active)
            //            join url in context.Urls.Where(x => x.Active) on prop.Url equals url.Id into gj
            //            from subpet in gj.DefaultIfEmpty()
            //            select new { prop }).ToList();
            var urls = context.Urls.Where(x => x.Active).OrderBy(x => x.Id).ToList();
            var existing = (List<int>)context.Properties.Where(x => x.Active).Select(x => (int)x.UrlId).Distinct().ToList();
            var r = urls.Select(x => x.Id).Except(existing).ToList();
            urls = (from u in urls
                   join a in r on u.Id equals a
                   select u).ToList();


            var take = urls;

            // var take= context.Urls.Where(x => x.Active && x.Id==600000);


            var partitioner = Partitioner.Create(take);
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount
            };

            Parallel.ForEach(partitioner, parallelOptions, (listItem, loopState) =>
            {
                ProcessDetails(listItem);
                //Do something
            });
            //Parallel.Invoke(

            // () =>
            // {
            //     ProcessRent(codes.Take(take).ToList());
            // }

            // //, () =>
            // // {
            // //     ProcessRent(codes.Skip(1 * take).Take(take).ToList());
            // // },

            // //() =>
            // //{
            // //    ProcessRent(codes.Skip(2 * take).Take(take).ToList());
            // //},
            // // () =>
            // //{
            // //    ProcessRent(codes.Skip(3 * take).Take(take).ToList());
            // //}
            //);


            #region proxy test
            //    // First create a proxy object
            //    var proxyHost = "202.170.83.212";
            //    var proxyPort = 3128;
            //    var uri = new UriBuilder($"{proxyHost}");
            //    uri.Port = proxyPort;
            //    var proxy = new WebProxy()
            //    {
            //        Address =uri.Uri,
            //        //BypassOnLocal = false,
            //        UseDefaultCredentials = true,

            //        // *** These creds are given to the proxy server, not the web server ***
            //        //Credentials = new NetworkCredential(
            //        //    userName: proxyUserName,
            //        //    password: proxyPassword);
            //};

            //// Now create a client handler which uses that proxy

            //var httpClientHandler = new HttpClientHandler()
            //{
            //    Proxy = proxy,
            //};

            // Omit this part if you don't need to authenticate with the web server:
            //if (needServerAuthentication)
            //{
            //    httpClientHandler.PreAuthenticate = true;
            //    httpClientHandler.UseDefaultCredentials = false;

            //    // *** These creds are given to the web server, not the proxy server ***
            //    httpClientHandler.Credentials = new NetworkCredential(
            //        userName: serverUserName,
            //        password: serverPassword);
            //    }
            //}
            // Finally, create the HTTP client object
            #endregion

        }

        static void GetPostalcodes()
        {
            var url = "https://en.wikipedia.org/wiki/List_of_postcode_districts_in_the_United_Kingdom";
            using (var client = new HttpClient(/*handler: httpClientHandler*/))
            {
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");
                client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,it;q=0.8,sq;q=0.7");
                var data = client.GetStringAsync(url).Result;
                HtmlDocument document = new HtmlDocument();
                document.LoadHtml(data);
                var linksNodes = document.DocumentNode.SelectSingleNode("//table[@class=\"wikitable sortable\"]").SelectNodes("//tr/td[2]").Select(x => x.InnerText);//.Select(x=>new PostalCode { Code });'
                var codes1 = linksNodes.Select(x => x.Replace("\n", "").Replace("non-geo", "").Replace("shared", "")
                .Replace("&#91;2&#93;", "")
                .Replace("&#91;3&#93;", "")
                .Replace("&#91;4&#93;", "")
                .Replace("&#91;5&#93;", "")
                .Replace("&#91;6&#93;", "")
                .Replace("&#91;7&#93;", "")
                .Replace("&#91;8&#93;", "")
                .Replace("&#91;9&#93;", "")
                ).SelectMany(x => x.Split(",")).SelectMany(x => x.Split(" ")).Where(x => !string.IsNullOrEmpty(x.Trim())).Distinct().ToList();
                var codes = codes1.Select(x => new PostalCode { Code = x }).ToList();
                Data.AppContext appContext = new Data.AppContext(true);
                appContext.PostalCodes.AddRange(codes);
                appContext.SaveChanges();
            }
        }
        static void Process(List<PostalCode> codes)
        {
            var code = "AB10";
            var basedUrl = "https://www.rightmove.co.uk/property-for-sale/";
            var getOpCode = $"search.html?searchLocation={code}&locationIdentifier=&useLocationIdentifier=false&buy=For+sale";
            var opCode = "";
            var context = new Data.AppContext(true);

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

        static void ProcessDetails(Url url)
        {
            var _appContext = new Data.AppContext(true);
            //sale example

            //var basedUrl = "https://www.rightmove.co.uk/property-for-sale/property-56553534.html";
            var basedUrl = "https://www.rightmove.co.uk";

            //rent example
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
                    client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");
                    client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,it;q=0.8,sq;q=0.7");

                    var links = client.GetStringAsync(basedUrl+url.PropertyUrl).Result;
                    HtmlDocument document = new HtmlDocument();
                    document.LoadHtml(links);
                    if (document.DocumentNode.InnerText.Contains("This property has been removed by the agent.")
                        || document.DocumentNode.InnerText.Contains("We are sorry but we could not find the property you have requested.") )
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
                        var data = document.DocumentNode.SelectNodes("//script").FirstOrDefault(x=>x.InnerHtml.Contains(propertyInfo));
                        var jsonString = data.InnerText.Split(@"'property',")[1].Trim();
                        var jsonData = jsonString.Remove(jsonString.Length - 3, 3);
                        var propertyData = JsonConvert.DeserializeObject<Details>(jsonData);

                        var branchData= document.DocumentNode.SelectNodes("//script").FirstOrDefault(x => x.InnerText.Contains(branchInfo));
                        jsonString = branchData.InnerText.Split(@"'branch',")[1].Trim();
                        jsonData = jsonString.Remove(jsonString.Length - 3, 3);
                        var agentData = JsonConvert.DeserializeObject<Branch>(jsonData);

                        var imagesData = document.DocumentNode.SelectNodes("//script").FirstOrDefault(x => x.InnerText.Contains(imagesInfo));
                        jsonString = imagesData.InnerText.Split(imagesInfo)[1].Trim();
                        jsonData = jsonString.Split("\"}],")[0]+ "\"}]";
                        var imageData = jsonString.Split("\"}],").Length>1? JsonConvert.DeserializeObject<List<Images>>(jsonData): new List<Images>();

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
                        if (existAgent ==null)
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
                        var propertyDescription=new PropertyDescription
                        {
                            DateAdded = dateNow,
                            DateModified = dateNow,
                            Description=description,
                            Active = true
                        };
                        //price
                        var price = new PropertyPrice
                        {
                            DateAdded = dateNow,
                            DateModified = dateNow,
                            Price = priceValue,
                            PriceQualifier=propertyData.propertyInfo.priceQualifier,
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
                            LettingType=propertyData.propertyInfo.lettingType,
                            PropertyType = propertyData.propertyInfo.propertyType,
                            PostalCode = propertyData.location.postcode,
                            PostalCodeFull= postC.Substring(0,postC.Length-2),
                            PostalCodePrefix = postC.Substring(0,2),
                            PostalCodeExtended = postC.Substring(0, postC.Length - 3).Trim(),
                            Type=(PropertyType)url.Type,
                            PropertyAdded=DateTime.TryParse(propertyData.propertyInfo.added,out DateTime temp)?temp:DateTime.MinValue,
                            Latitude = propertyData.location.latitude,
                            Longtitude = propertyData.location.longitude,
                            Added = propertyData.propertyInfo.added,
                            Address = address,
                            Price = priceValue,
                            BedroomsCount =(byte) propertyData.propertyInfo.beds,
                            FloorPlanCount = propertyData.floorplanCount,
                            PropertySubType = propertyData.propertyInfo.propertySubType,
                            UrlId=url.Id
                        };
                        property.Images.AddRange(images);
                        property.PropertyPrices.Add(price);
                        if (existAgent.Id !=0)
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

            }
        }

        static void ProcessRent(List<PostalCode> codes)
        {
            var code = "AB10";
            var basedUrl = "https://www.rightmove.co.uk/property-to-rent/";
            var getOpCode = $"search.html?searchLocation={code}&locationIdentifier=&useLocationIdentifier=false&buy=For+sale";
            var opCode = "";
            var context = new Data.AppContext(true);

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
                        var urls = linksList.Select(x => new Url { PropertyUrl = x, Type = (int)Data.PropertyType.Rent, PortalId = 1, DateModified = DateTime.Now, DateAdded = DateTime.Now, Active = true, PostalCodeId = cod.Id });
                        context.Urls.AddRange(urls);
                        context.SaveChanges();
                    }
                    catch (Exception ex)
                    {

                    }

                }


            }
        }

        //static void CanadaCars()
        //{
        //    var postalcodes = "https://www.geonames.org/postal-codes/CA/SK/saskatchewan.html";

        //    using (var client = new HttpClient(/*handler: httpClientHandler*/))
        //    {
        //        client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
        //        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");
        //        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,it;q=0.8,sq;q=0.7");

        //        var links = client.GetStringAsync(postalcodes).Result;
        //        HtmlDocument documentlinks = new HtmlDocument();
        //        documentlinks.LoadHtml(links);
        //        var linksNodes = documentlinks.DocumentNode.SelectSingleNode("//table[@class=\"restable\"]").SelectNodes("//tr/td[3]").Select(x => x.InnerText).ToList();

        //        for (int i = 1; i < linksNodes.Count; i++)
        //        {

        //        }
        //    }
        //}
    }
}
