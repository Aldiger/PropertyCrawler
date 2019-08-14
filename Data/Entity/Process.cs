using System;
using System.Collections.Generic;
using System.Text;

namespace PropertyCrawler.Data.Entity
{
    public class Process : Base
    {
        public ProcessType Type { get; set; }
        public ProcessStatus Status { get; set; }
        public int? JobId { get; set; }
        public int? Retry { get; set; }

        public PropertyType PropertyType { get; set; }


        public virtual List<ProcessPostalCode> ProcessPostalCodes { get; set; }
    }

    public enum ProcessType
    {
        Update_Price,
        LastWeek,
        LastTwoWeeks
    }
    public enum ProcessStatus
    {
        Processing,
        Failed,
        Success
    }
}
