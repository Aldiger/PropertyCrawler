using System.Collections.Generic;

namespace RightMove.Data
{
    public class Agent : Base
    {
        public string Name { get; set; }
        public int Code { get; set; }
        public string LogoUrl { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }

        public ICollection<Property> Properties { get; set; }
    }
}
