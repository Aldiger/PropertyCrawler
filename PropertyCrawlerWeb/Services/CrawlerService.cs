using PropertyCrawler.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PropertyCrawlerWeb.Services
{
    public interface ICrawlerService
    {
        void UrlCrawler(List<PostalCode> postalCodes);
        void PropertyCrawler(List<PostalCode> postalCodes);
        void PriceCrawler(List<PostalCode> postalCodes);
    }
    public class CrawlerService : ICrawlerService
    {
        public void UrlCrawler(List<PostalCode> postalCodes)
        {

        }

        public void PropertyCrawler(List<PostalCode> postalCodes)
        {

        }
        public void PriceCrawler(List<PostalCode> postalCodes)
        {

        }
    }
}
