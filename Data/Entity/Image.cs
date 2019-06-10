namespace PropertyCrawler.Data
{
    public class Image : Base
    {
        public string Url { get; set; }
        public string Caption { get; set; }
        public int PropertyId { get; set; }
        public virtual Property Property { get; set; }

    }
}
