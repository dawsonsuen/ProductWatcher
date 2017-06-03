using System;

namespace ProductWatcher.Apis.BWS.Models
{
    public class SearchModel
    {
        public dynamic Terms { get; set; }
        public dynamic Stores { get; set; }
        public ProductsModel Products { get; set; }
    }

    public class ProductsModel
    {
        public long Order { get; set; }
        public SuggestionsModel[] Suggestions { get; set; }
    }
    public class SuggestionsModel
    {
        public string Brand { get; set; }
        public string Title { get; set; }
        public string VolumeSize { get; set; }
        public string Stockcode { get; set; }
        public string ParentStockcode { get; set; }
        public decimal Price { get; set; }
        public bool IsSpecial { get; set; }
        public decimal AmountSaved { get; set; }
        public string ImageFile { get; set; }
        public ProductTag ProductTag { get; set; }
        public string ProductName { get; set; }
        public string WebPackType { get; set; }
        public string Image1 { get; set; }
        public long ProductUnitQuantity { get; set; }
    }

    public class ProductTag
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string Link { get; set; }

        //TODO: @ebyte23 this is probably an enum
        public string Type { get; set; }

        public long SessionGroupID { get; set; }

        //TODO: Make Sure we use these foor notifications
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string TagTestType { get; set; }
        public string TagTestValue { get; set; }
        public string LeftType { get; set; }
        public string LeftLink { get; set; }
        public string LeftLinkTarget { get; set; }
        public bool OverrideLeftImage { get; set; }
        public bool FooterTag { get; set; }
        public string AltText { get; set; }
    }



}