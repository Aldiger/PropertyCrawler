namespace PropertyCrawler.Data.Entity
{
    public class ProcessPostalCode : Base
    {
        public ProcessStatus Status { get; set; }
        public int ProcessId { get; set; }
        public int PostalCodeId { get; set; }

        public virtual PostalCode PostalCode { get; set; }

        public virtual Process Process { get; set; }
    }


    public class ProcessPostalCodeUrlFailed : Base
    {
        public int ProcessPostalCodeId { get; set; }

        public int UrlId { get; set; }

        public virtual ProcessPostalCode ProcessPostalCode { get; set; }

        public virtual Url Url { get; set; }
    }

}
