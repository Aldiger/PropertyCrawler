using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyCrawler.Data
{
    public class PropertyDescription: Base
    {
        public string Description { get; set; }

        //[ForeignKey("Property")]
        public int? PropertyId { get; set; }

        public virtual Property Property { get; set; }
    }
}
