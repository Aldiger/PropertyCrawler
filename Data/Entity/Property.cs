using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PropertyCrawler.Data
{
    public class Property : Base
    {
        public Property()
        {
            Images = new List<Image>();
            PropertyPrices = new List<PropertyPrice>();
        }
        public string Added { get; set; }
        public string PropertyType { get; set; }
        public string PropertySubType { get; set; }
        public byte BedroomsCount { get; set; }
        public int FloorPlanCount { get; set; }
        public string LettingType { get; set; }

        public string Address { get; set; }

        public string PostalCode { get; set; }
        public double Latitude { get; set; }
        public double Longtitude { get; set; }

        public int AgentId { get; set; }
        [ForeignKey("PropertyDescription")]
        public int? PropertyDescriptionId { get; set; }
        [ForeignKey("Url")]
        public int? UrlId { get; set; }

        public virtual Agent Agent { get; set; }
        public virtual PropertyDescription PropertyDescription { get;set;}
        public virtual Url Url { get; set; }

        public virtual List<Image> Images { get; set; }
        public virtual List<PropertyPrice> PropertyPrices { get; set; }
    }
}
