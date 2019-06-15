using System.Collections.Generic;

namespace PropertyCrawler.Data
{
    public class UrlType: Base
    {
        public string UrlPortion { get; set; }
        public virtual List<Url> Urls { get; set; }
    }
}
