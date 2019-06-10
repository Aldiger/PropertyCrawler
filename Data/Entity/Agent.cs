using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyCrawler.Data
{
    public class Agent : Base
    {
        public Agent()
        {
            Properties = new List<Property>();
        }
        public int AgentCode { get; set; }
        public string PhoneNumber { get; set; }

        public string CompanyName { get; set; }
        public string BrandName { get; set; }
        public string BranchName { get; set; }
        public string CompanyType { get; set; }
        public string AgentType { get; set; }
        public string DisplayAddress { get; set; }
        public string BranchPostcode { get; set; }

        public ICollection<Property> Properties { get; set; }
    }
}
