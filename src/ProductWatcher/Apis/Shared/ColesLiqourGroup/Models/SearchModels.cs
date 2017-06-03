namespace ProductWatcher.Apis.Shared.ColesLiqourGroup.Models
{
    public class DataModel
    {
        public string Text { get; set; }
        public bool IsCategory { get; set; }
        public string Url { get; set; }
        public string Availability { get; set; }
        public string JoinerText { get; set; }
        public ItemType ItemType { get; set; }
    }

    public class SearchModel
    {
        public string Query { get; set; }
        public string[] Suggestions { get; set; }
        public DataModel[] Data { get; set; }
    }

    public enum ItemType
    {
        Category,
        Product,
        Facet,
        Summary
    }
}