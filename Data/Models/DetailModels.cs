using System;
using System.Collections.Generic;
using System.Text;

namespace PropertyCrawler.Models
{
    public class Location
    {
        public string postcode { get; set; }
        public string country { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class PropertyInfo
    {
        public string propertyType { get; set; }
        public string propertySubType { get; set; }
        public string price { get; set; }
        public int beds { get; set; }
        public string added { get; set; }
        public bool soldSTC { get; set; }
        //public bool retirement { get; set; }
        public string preOwned { get; set; }
        public string ownership { get; set; }
        public bool auctionOnly { get; set; }
        public bool letAgreed { get; set; }
        public string lettingType { get; set; }
        public object furnishedType { get; set; }
        public object minSizeFt { get; set; }
        public object maxSizeFt { get; set; }
        public object minSizeAc { get; set; }
        public object maxSizeAc { get; set; }
        public bool businessForSale { get; set; }
        public string priceQualifier { get; set; }
        public string currency { get; set; }
        public object selectedPrice { get; set; }
        public object selectedCurrency { get; set; }
    }

    public class Details
    {
        public Location location { get; set; }
        public int propertyId { get; set; }
        public string viewType { get; set; }
        public int imageCount { get; set; }
        public int floorplanCount { get; set; }
        public string videoProvider { get; set; }
        public PropertyInfo propertyInfo { get; set; }
    }

    public class Branch
    {
        public int branchId { get; set; }
        public string companyName { get; set; }
        public string brandName { get; set; }
        public string branchName { get; set; }
        public string companyType { get; set; }
        public string agentType { get; set; }
        public string displayAddress { get; set; }
        public string branchPostcode { get; set; }
        public string pageType { get; set; }
    }

    public class Images
    {
        public int index { get; set; }
        public string caption { get; set; }
        public string thumbnailUrl { get; set; }
        public string masterUrl { get; set; }
    }
}
