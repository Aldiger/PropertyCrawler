namespace PropertyCrawler.Data.Entity
{
    public class ProcessPostalCode: Base
    {
        public ProcessStatus Status { get; set; }
        public int ProcessId { get; set; }
        public int PostalCodeId { get; set; }
    }
}
