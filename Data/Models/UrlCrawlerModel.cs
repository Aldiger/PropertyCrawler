using System;
using System.Collections.Generic;
using System.Text;

namespace PropertyCrawler.Data.Models
{
    public class UrlCrawlerModel
    {
        public int PropertyCode { get; set; }
        public decimal Price { get; set; }
        public string PriceQualifier { get; set; }
        public int? UrlTypeId { get; set; }
    }
}
