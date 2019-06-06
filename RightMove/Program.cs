using HtmlAgilityPack;
using RightMove.Data;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RightMove
{
    class Program
    {

        static void Main(string[] args)
        {

            
                        var code = "AB10";
            var basedUrl = "https://www.rightmove.co.uk/property-for-sale/";
            var getOpCode = $"search.html?searchLocation={code}&locationIdentifier=&useLocationIdentifier=false&buy=For+sale";
            var opCode = "";
            //GetPostalcodes();
            var context = new Data.AppContext(true);
            var codes = context.PostalCodes.Where(x => x.Active && x.Id>1874).OrderBy(x => x.Id).ToList();
            var existing = context.Urls.Where(x => x.Type == (int)Data.Type.Rent).Select(x => x.PostalCodeId).Distinct().ToList();
            var r = codes.Select(x=>x.Id).Except(existing).ToList();
            codes = codes.Where(x => r.Contains(x.Id)).ToList();
            var take = 400;

            var url = context.Urls.OrderBy(x => x.Id).Take(66).ToList();
            ProcessDetails(url);
            //var partitioner = Partitioner.Create(codes);
            //var parallelOptions = new ParallelOptions
            //{
            //    MaxDegreeOfParallelism = Environment.ProcessorCount
            //};

            //Parallel.ForEach(partitioner, parallelOptions, (listItem, loopState) =>
            //{
            //    //Do something
            //});
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

        static void ProcessDetails(List<Url> urls)
        {
            var basedUrl = "https://www.rightmove.co.uk/property-for-sale/property-56553534.html";

            using (var client = new HttpClient(/*handler: httpClientHandler*/))
            {
                client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3");
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");
                client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9,it;q=0.8,sq;q=0.7");

                var links = client.GetStringAsync(basedUrl).Result;
                HtmlDocument documentlinks = new HtmlDocument();
                documentlinks.LoadHtml(links);
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
                        var urls = linksList.Select(x => new Url { PropertyUrl = x, Type = (int)Data.Type.Rent, PortalId = 1, DateModified = DateTime.Now, DateAdded = DateTime.Now, Active = true, PostalCodeId = cod.Id });
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
