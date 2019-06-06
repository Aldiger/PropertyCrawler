using System.Collections.Generic;

namespace RightMove.Data
{
    public class Property : Base
    {
        public string PropertyType { get; set; }
        public byte NumberOfBedrooms { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PostalCode { get; set; }
        public string PriceType { get; set; }
        public float Latitude { get; set; }
        public float Longtitude { get; set; }
        public string Description { get; set; }

        public int AgentId { get; set; }

        public ICollection<Image> Images { get; set; }
        public virtual Agent Agent { get; set; }

    }
}
