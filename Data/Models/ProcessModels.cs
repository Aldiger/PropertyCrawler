using PropertyCrawler.Data.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace PropertyCrawler.Data.Models
{
    public class ProcessModels
    {
        public int ProcessCount { get; set; }
        public int ProcessFailedCount { get; set; }

        public int ProcessSuccessfullCount { get; set; }
    }

    public class ProcessVM
    {
        public int Id { get; set; }

        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public bool Active { get; set; }
        public ProcessType Type { get; set; }
        public ProcessStatus Status { get; set; }

        public PropertyType PropertyType { get; set; }

        public List<string> PostalCode { get; set; }
    }
}
