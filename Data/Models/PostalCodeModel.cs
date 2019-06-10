using System;
using System.Collections.Generic;
using System.Text;

namespace PropertyCrawler.Data.Models
{
    public class PostalCodeModel
    {
        public int Id { get; set; }

        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public bool Active { get; set; }
        public string Code { get; set; }
        public string OpCode { get; set; }

        public int Properties { get; set; }

    }
}
