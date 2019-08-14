using System;
using System.Collections.Generic;
using System.Text;

namespace PropertyCrawler.Models.Url
{
    public class Location
    {
        public double latitude { get; set; }
        public double longitude { get; set; }
    }

    public class ListingUpdate
    {
        public string listingUpdateReason { get; set; }
        public DateTime listingUpdateDate { get; set; }
    }

    public class DisplayPrice
    {
        public string displayPrice { get; set; }
        public string displayPriceQualifier { get; set; }
    }

    public class Price
    {
        public int amount { get; set; }
        public string frequency { get; set; }
        public string currencyCode { get; set; }
        public List<DisplayPrice> displayPrices { get; set; }
    }

    public class Customer
    {
        public int branchId { get; set; }
        public string brandPlusLogoURI { get; set; }
        public string contactTelephone { get; set; }
        public string branchDisplayName { get; set; }
        public string branchName { get; set; }
        public string brandTradingName { get; set; }
        public string branchLandingPageUrl { get; set; }
        public bool development { get; set; }
        public bool showReducedProperties { get; set; }
        public bool commercial { get; set; }
        public bool showOnMap { get; set; }
        public string brandPlusLogoUrl { get; set; }
    }

    public class ProductLabel
    {
        public string productLabelText { get; set; }
    }

    public class Image
    {
        public string srcUrl { get; set; }
        public string srcsetUrl { get; set; }
        public string url { get; set; }
    }

    public class PropertyImages
    {
        public List<Image> images { get; set; }
        public string mainImageSrc { get; set; }
        public string mainImageSrcset { get; set; }
        public string mainMapImageSrc { get; set; }
        public string mainMapImageSrcset { get; set; }
    }

    public class Property
    {
        public int id { get; set; }
        public int bedrooms { get; set; }
        public int numberOfImages { get; set; }
        public int numberOfFloorplans { get; set; }
        public int numberOfVirtualTours { get; set; }
        public string summary { get; set; }
        public string displayAddress { get; set; }
        public string countryCode { get; set; }
        public Location location { get; set; }
        public string propertySubType { get; set; }
        public ListingUpdate listingUpdate { get; set; }
        public bool premiumListing { get; set; }
        public bool featuredProperty { get; set; }
        public Price price { get; set; }
        public Customer customer { get; set; }
        public object distance { get; set; }
        public string transactionType { get; set; }
        public ProductLabel productLabel { get; set; }
        public bool commercial { get; set; }
        public bool development { get; set; }
        public bool residential { get; set; }
        public bool students { get; set; }
        public bool auction { get; set; }
        public bool feesApply { get; set; }
        public object feesApplyText { get; set; }
        public string displaySize { get; set; }
        public bool showOnMap { get; set; }
        public string propertyUrl { get; set; }
        public string contactUrl { get; set; }
        public string channel { get; set; }
        public DateTime firstVisibleDate { get; set; }
        public List<object> keywords { get; set; }
        public string keywordMatchType { get; set; }
        public object saved { get; set; }
        public object hidden { get; set; }
        public string propertyTypeFullDescription { get; set; }
        public PropertyImages propertyImages { get; set; }
        public string displayStatus { get; set; }
        public string formattedBranchName { get; set; }
        public string addedOrReduced { get; set; }
        public bool isRecent { get; set; }
        public string formattedDistance { get; set; }
        public string heading { get; set; }
        public bool hasBrandPlus { get; set; }
    }

    public class RadiusOption
    {
        public string value { get; set; }
        public string description { get; set; }
    }

    public class PriceOption
    {
        public string value { get; set; }
        public string description { get; set; }
    }

    public class BedroomOption
    {
        public string value { get; set; }
        public string description { get; set; }
    }

    public class AddedToSiteOption
    {
        public string value { get; set; }
        public string description { get; set; }
    }

    public class MustHaveOption
    {
        public string value { get; set; }
        public string description { get; set; }
    }

    public class DontShowOption
    {
        public string value { get; set; }
        public string description { get; set; }
    }

    public class SortOption
    {
        public string value { get; set; }
        public string description { get; set; }
    }

    public class ApplicationProperties
    {
    }

    public class Location2
    {
        public object latitude { get; set; }
        public object longitude { get; set; }
    }

    public class ListingUpdate2
    {
        public object listingUpdateReason { get; set; }
        public DateTime listingUpdateDate { get; set; }
    }

    public class DisplayPrice2
    {
        public string displayPrice { get; set; }
        public string displayPriceQualifier { get; set; }
    }

    public class Price2
    {
        public int amount { get; set; }
        public string frequency { get; set; }
        public string currencyCode { get; set; }
        public List<DisplayPrice2> displayPrices { get; set; }
    }

    public class Customer2
    {
        public object branchId { get; set; }
        public object brandPlusLogoURI { get; set; }
        public object contactTelephone { get; set; }
        public object branchDisplayName { get; set; }
        public object branchName { get; set; }
        public object brandTradingName { get; set; }
        public object branchLandingPageUrl { get; set; }
        public bool development { get; set; }
        public bool showReducedProperties { get; set; }
        public bool commercial { get; set; }
        public bool showOnMap { get; set; }
        public string brandPlusLogoUrl { get; set; }
    }

    public class ProductLabel2
    {
        public object productLabelText { get; set; }
    }

    public class PropertyImages2
    {
        public List<object> images { get; set; }
        public string mainImageSrc { get; set; }
        public string mainImageSrcset { get; set; }
        public string mainMapImageSrc { get; set; }
        public string mainMapImageSrcset { get; set; }
    }

    public class PropertySchema
    {
        public int id { get; set; }
        public int bedrooms { get; set; }
        public int numberOfImages { get; set; }
        public int numberOfFloorplans { get; set; }
        public int numberOfVirtualTours { get; set; }
        public object summary { get; set; }
        public object displayAddress { get; set; }
        public object countryCode { get; set; }
        public Location2 location { get; set; }
        public object propertySubType { get; set; }
        public ListingUpdate2 listingUpdate { get; set; }
        public bool premiumListing { get; set; }
        public bool featuredProperty { get; set; }
        public Price2 price { get; set; }
        public Customer2 customer { get; set; }
        public object distance { get; set; }
        public object transactionType { get; set; }
        public ProductLabel2 productLabel { get; set; }
        public bool commercial { get; set; }
        public bool development { get; set; }
        public bool residential { get; set; }
        public bool students { get; set; }
        public bool auction { get; set; }
        public bool feesApply { get; set; }
        public string feesApplyText { get; set; }
        public string displaySize { get; set; }
        public bool showOnMap { get; set; }
        public string propertyUrl { get; set; }
        public string contactUrl { get; set; }
        public string channel { get; set; }
        public DateTime firstVisibleDate { get; set; }
        public List<object> keywords { get; set; }
        public string keywordMatchType { get; set; }
        public bool saved { get; set; }
        public bool hidden { get; set; }
        public string propertyTypeFullDescription { get; set; }
        public PropertyImages2 propertyImages { get; set; }
        public string displayStatus { get; set; }
        public string formattedBranchName { get; set; }
        public string addedOrReduced { get; set; }
        public bool isRecent { get; set; }
        public string formattedDistance { get; set; }
        public string heading { get; set; }
        public bool hasBrandPlus { get; set; }
    }

    public class Model
    {
        public string text { get; set; }
        public string url { get; set; }
        public bool noFollow { get; set; }
    }

    public class SoldHousePricesLinks
    {
        public string heading { get; set; }
        public string subHeading { get; set; }
        public List<Model> model { get; set; }
        public object headingLink { get; set; }
    }

    public class Model2
    {
        public string text { get; set; }
        public string url { get; set; }
        public bool noFollow { get; set; }
    }

    public class ChannelSwitchLink
    {
        public string heading { get; set; }
        public object subHeading { get; set; }
        public List<Model2> model { get; set; }
        public object headingLink { get; set; }
    }

    public class Model3
    {
        public string text { get; set; }
        public string url { get; set; }
        public bool noFollow { get; set; }
    }

    public class SuggestedLinks
    {
        public string heading { get; set; }
        public object subHeading { get; set; }
        public List<Model3> model { get; set; }
        public object headingLink { get; set; }
    }

    public class SidebarModel
    {
        public SoldHousePricesLinks soldHousePricesLinks { get; set; }
        public object relatedHouseSearches { get; set; }
        public object relatedFlatSearches { get; set; }
        public object relatedPopularSearches { get; set; }
        public object relatedRegionsSearches { get; set; }
        public ChannelSwitchLink channelSwitchLink { get; set; }
        public object relatedStudentLinks { get; set; }
        public object branchMPU { get; set; }
        public object countryGuideMPU { get; set; }
        public SuggestedLinks suggestedLinks { get; set; }
    }

    public class SeoModel
    {
        public string canonicalUrl { get; set; }
        public string metaRobots { get; set; }
    }

    public class RecentSearchModel
    {
        public string linkDisplayText { get; set; }
        public string titleDisplayText { get; set; }
        public string searchCriteriaMobile { get; set; }
        public long createDate { get; set; }
        public string locationIdentifierAndSearchType { get; set; }
    }

    public class CurrencyCodeOption
    {
        public string value { get; set; }
        public string description { get; set; }
    }

    public class AreaSizeUnitOption
    {
        public string value { get; set; }
        public string description { get; set; }
        public string abbreviation { get; set; }
    }

    public class SizeOption
    {
        public string value { get; set; }
        public string description { get; set; }
        public string abbreviation { get; set; }
    }

    public class PriceTypeOption
    {
        public string value { get; set; }
        public string description { get; set; }
        public string abbreviation { get; set; }
    }

    public class SidebarSlot
    {
        public string id { get; set; }
        public string adUnitPath { get; set; }
        public List<List<int>> sizes { get; set; }
        public List<object> mappings { get; set; }
    }

    public class Targeting
    {
        public string key { get; set; }
        public string value { get; set; }
    }

    public class DfpModel
    {
        public List<object> contentSlots { get; set; }
        public List<SidebarSlot> sidebarSlots { get; set; }
        public List<Targeting> targeting { get; set; }
    }

    public class NoResultsModel
    {
        public List<object> suggestionPods { get; set; }
    }

    public class Location3
    {
        public int id { get; set; }
        public string displayName { get; set; }
        public string shortDisplayName { get; set; }
        public string locationType { get; set; }
        public string listingCurrency { get; set; }
    }

    public class SearchParameters
    {
        public string locationIdentifier { get; set; }
        public string numberOfPropertiesPerPage { get; set; }
        public string radius { get; set; }
        public string sortType { get; set; }
        public string index { get; set; }
        public List<object> propertyTypes { get; set; }
        public string viewType { get; set; }
        public List<object> mustHave { get; set; }
        public List<object> dontShow { get; set; }
        public List<object> furnishTypes { get; set; }
        public string channel { get; set; }
        public string areaSizeUnit { get; set; }
        public string currencyCode { get; set; }
        public List<object> keywords { get; set; }
    }

    public class Option
    {
        public string value { get; set; }
        public string description { get; set; }
    }

    public class Pagination
    {
        public int total { get; set; }
        public List<Option> options { get; set; }
        public string first { get; set; }
        public string last { get; set; }
        public string page { get; set; }
    }

    public class IndividualFeatureSwitchState
    {
        public string label { get; set; }
        public string state { get; set; }
        public bool shouldLog { get; set; }
    }

    public class FeatureUser
    {
        public string uniqueIdentifier { get; set; }
    }

    public class FeatureSwitchStateForUser
    {
        public List<IndividualFeatureSwitchState> individualFeatureSwitchStates { get; set; }
        public FeatureUser featureUser { get; set; }
    }

    public class RootObject
    {
        public List<Property> properties { get; set; }
        public string resultCount { get; set; }
        public string searchParametersDescription { get; set; }
        public List<RadiusOption> radiusOptions { get; set; }
        public List<PriceOption> priceOptions { get; set; }
        public List<BedroomOption> bedroomOptions { get; set; }
        public List<AddedToSiteOption> addedToSiteOptions { get; set; }
        public List<MustHaveOption> mustHaveOptions { get; set; }
        public List<DontShowOption> dontShowOptions { get; set; }
        public List<object> furnishOptions { get; set; }
        public List<object> letTypeOptions { get; set; }
        public List<SortOption> sortOptions { get; set; }
        public ApplicationProperties applicationProperties { get; set; }
        public string staticMapUrl { get; set; }
        public string shortLocationDescription { get; set; }
        public long timestamp { get; set; }
        public bool bot { get; set; }
        public string deviceType { get; set; }
        public PropertySchema propertySchema { get; set; }
        public SidebarModel sidebarModel { get; set; }
        public SeoModel seoModel { get; set; }
        public string mapViewUrl { get; set; }
        public string legacyUrl { get; set; }
        public string listViewUrl { get; set; }
        public string pageTitle { get; set; }
        public string metaDescription { get; set; }
        public RecentSearchModel recentSearchModel { get; set; }
        public int maxCardsPerPage { get; set; }
        public string countryCode { get; set; }
        public int countryId { get; set; }
        public List<CurrencyCodeOption> currencyCodeOptions { get; set; }
        public List<AreaSizeUnitOption> areaSizeUnitOptions { get; set; }
        public List<SizeOption> sizeOptions { get; set; }
        public List<PriceTypeOption> priceTypeOptions { get; set; }
        public bool showFeaturedAgent { get; set; }
        public bool commercialChannel { get; set; }
        public string disambiguationPagePath { get; set; }
        public DfpModel dfpModel { get; set; }
        public NoResultsModel noResultsModel { get; set; }
        public string urlPath { get; set; }
        public object tileGeometry { get; set; }
        public List<object> geohashTerms { get; set; }
        public string comscore { get; set; }
        public object cookiePolicies { get; set; }
        public string formattedExchangeRateDate { get; set; }
        public Location3 location { get; set; }
        public SearchParameters searchParameters { get; set; }
        public Pagination pagination { get; set; }
        public FeatureSwitchStateForUser featureSwitchStateForUser { get; set; }
    }
}
