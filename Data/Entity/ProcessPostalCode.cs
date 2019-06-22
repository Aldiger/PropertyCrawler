using System.Collections.Generic;

namespace PropertyCrawler.Data.Entity
{
    public class ProcessPostalCode : Base
    {
        public ProcessStatus Status { get; set; }
        public int ProcessId { get; set; }
        public int PostalCodeId { get; set; }

        public virtual PostalCode PostalCode { get; set; }

        public virtual Process Process { get; set; }

        public virtual List<ProcessPostalCodeUrlFailed> ProcessPostalCodeUrlFails { get;set;}
    }

}
