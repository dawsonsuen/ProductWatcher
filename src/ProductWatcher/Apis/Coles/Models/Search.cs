using System;

namespace ProductWatcher.Apis.Coles.Models
{
    public class Search
    {
        public SearchProduct[] Results { get; set; }
        public string Keyword { get; set; }
        public long NoOfResults { get; set; }
        public long Start { get; set; }
    }
    public class Location
    {
        public string Aisle { get; set; }
        public decimal Order { get; set; }
        public string Description { get; set; }
        public string AisleSide { get; set; }
        public long Facing { get; set; }
        public long Shelf { get; set; }
        public string LayoutId { get; set; }
        public string LayoutName { get; set; }
    }

    public class Promotion
    {
        public long PromotionId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal WasPrice { get; set; }
        public decimal SaveAmount { get; set; }
        public string UnitOfMeasure { get; set; }
        public string UnitPrice { get; set; }
        public string PriceDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal SavePercent { get; set; }
    }

    public class SearchProduct
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public string Brand { get; set; }
        public string ImageUri { get; set; }
        public Location[] Locations { get; set; }
        public Promotion[] Promotions { get; set; }
        public Tag[] Tags { get; set; }
    }

    public class Tag : BaseTag
    {
        public TagType TagType { get; set; }
    }
    public class BaseTag
    {
        public string Label { get; set; }
        public string Name { get; set; }
    }
    public class TagType : BaseTag { }
}