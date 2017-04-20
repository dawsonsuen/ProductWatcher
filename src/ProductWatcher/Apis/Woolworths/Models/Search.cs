using System.Collections.Generic;

namespace ProductWatcher.Apis.Woolworths.Models
{
    public class ProductItem
    {
        public Product[] Products { get; set; }
        public string Name { get; set; }
    }

    public class ChildCategory
    {
        public string RichRelevanceId { get; set; }
        public string Name { get; set; }
        public string UrlFriendlyName { get; set; }
        public string ImageFile { get; set; }
        public string RetinaImageFile { get; set; }
        public int ProductCount { get; set; }
        public bool IsRestricted { get; set; }
        public bool IsEligibleForRanking { get; set; }
    }

    public class VisualShoppingAisleResponse
    {
        public string RichRelevanceId { get; set; }
        public string Name { get; set; }
        public string UrlFriendlyName { get; set; }
        public string Color { get; set; }
        public string TextColor { get; set; }
        public string NumberColor { get; set; }
        public string IconFile { get; set; }
        public ChildCategory[] ChildCategories { get; set; }
        public bool HasLandingPage { get; set; }
    }

    public class Result
    {
        public string Name { get; set; }
        public string Term { get; set; }
        public Dictionary<string,string> ExtraOutputFields { get; set; }
        public object Min { get; set; }
        public object Max { get; set; }
        public bool Applied { get; set; }
        public int Count { get; set; }
    }

    public class Aggregation
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string Type { get; set; }
        public string FilterType { get; set; }
        public string FilterDataType { get; set; }
        public Result[] Results { get; set; }
        public string State { get; set; }
        public int Rank { get; set; }
        public bool AdditionalResults { get; set; }
        public object DesignType { get; set; }
    }

    public class SearchModel
    {
        public ProductItem[] Products { get; set; }
        public object Corrections { get; set; }
        public int SearchResultsCount { get; set; }
        public VisualShoppingAisleResponse[] VisualShoppingAisleResponse { get; set; }
        public Aggregation[] Aggregations { get; set; }
    }
}