using System;
using System.Collections.Generic;
using System.Text;

namespace PropertyCrawler.Data.Entity
{
    public class Process : Base
    {
        public ProcessType Type { get; set; }
        public ProcessStatus Status { get; set; }

        public virtual List<ProcessPostalCode> ProcessPostalCodes { get; set; }
    }

    public class ProcessPostalCode: Base
    {
        public ProcessStatus Status { get; set; }
        public int ProcessId { get; set; }
        public int PostalCodeId { get; set; }
    }



    public enum ProcessType
    {
        Full,
        UpdatePrice
    }
    public enum ProcessStatus
    {
        Processing,
        Failed,
        Success
    }
}
