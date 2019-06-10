namespace PropertyCrawler.Data
{
    public class PropertyPrice :Base
    {
        public decimal Price { get; set; }
        public string PriceQualifier { get; set; }

        public int PropertyId { get; set; }

        public virtual Property Property { get; set; }
    }
}
