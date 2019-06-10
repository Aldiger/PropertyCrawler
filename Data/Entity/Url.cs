using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyCrawler.Data
{
    public class Url : Base
    {
        public int PropertyCode { get; set; }
        public string PropertyUrl { get; set; }
        public int Type { get; set; }

        public int PostalCodeId { get; set; }
        public int PortalId { get; set; }
        //[ForeignKey("Property")]
        public int? PropertyId { get; set; }

        public virtual PostalCode PostalCode { get; set; }
        public virtual Portal Portal { get; set; }
        public virtual Property Property { get; set; }
    }
}
