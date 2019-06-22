﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PropertyCrawler.Data.Entity
{
    public class Process : Base
    {
        public ProcessType Type { get; set; }
        public ProcessStatus Status { get; set; }

        public PropertyType PropertyType { get; set; }

        public virtual List<ProcessPostalCode> ProcessPostalCodes { get; set; }
    }

    public enum ProcessType
    {
        Full,
        Update_Price,
        Url,
        Properties
    }
    public enum ProcessStatus
    {
        Processing,
        Failed,
        Success
    }
}
