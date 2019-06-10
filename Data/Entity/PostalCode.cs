using System.Collections.Generic;

namespace PropertyCrawler.Data
{
    public class PostalCode : Base
    {
        public PostalCode()
        {
            Urls = new List<Url>();
        }
        public string Code { get; set; }
        public string OpCode { get; set; }
        public int OutCode { get; set; }

        public ICollection<Url> Urls { get; set; }
    }
}
