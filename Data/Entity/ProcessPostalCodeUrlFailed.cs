namespace PropertyCrawler.Data.Entity
{
    public class ProcessPostalCodeUrlFailed : Base
    {
        public int ProcessPostalCodeId { get; set; }

        public int UrlId { get; set; }

        public string FailReason { get; set; }

        public virtual ProcessPostalCode ProcessPostalCode { get; set; }

        public virtual Url Url { get; set; }
    }

}
