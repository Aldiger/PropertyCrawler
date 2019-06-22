using System.Collections.Generic;

namespace PropertyCrawler.Data
{
    public class Portal : Base
    {
        public Portal()
        {
            Urls = new List<Url>();
        }
        public string Name { get; set; }
        public string Url { get; set; }
        public string OutCodeKey { get; set; }

        public ICollection<Url> Urls { get; set; }
    }
}
