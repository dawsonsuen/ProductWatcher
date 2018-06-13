using System.Collections.Generic;

namespace ProductWatcher.Apis.BWS.Models
{
    public class RecommendedProduct : Woolworths.Models.Product
    {
        public object PercentageOffTag { get; set; }
        public object FixedPricePromoTag { get; set; }
        public bool IsFeaturedTag { get; set; }
        public int OverallRating { get; set; }
        public int NumberOfReviews { get; set; }
        public bool IsWatched { get; set; }
        public string BrandName { get; set; }
        public string Title { get; set; }
        public object ImageVariants { get; set; }
        public object AdditionalDetails { get; set; }
        public long ParentStockCode { get; set; }
        public long StockOnHand { get; set; }
        public long BackorderStockOnHand { get; set; }
        public long DeliveryStockOnHand { get; set; }
        public long PickupStockOnHand { get; set; }
    }

    public class ImageTag : Woolworths.Models.ImageTag
    {
    }

    public class CentreTag: Woolworths.Models.CentreTag
    {
    }

    public class FooterTag : Woolworths.Models.FooterTag
    {
    }



    public class PercentageOffTag
    {
        public int PromotionalPrice { get; set; }
        public int SavedAmount { get; set; }
        public object TopText { get; set; }
        public object BottomText { get; set; }
        public int ProductMultiplier { get; set; }
        public object TagContent { get; set; }
        public object TagLink { get; set; }
        public object FallbackText { get; set; }
        public string TagType { get; set; }
        public string TagTestType { get; set; }
        public object TagTestValue { get; set; }
    }

    public class FixedPricePromoTag
    {
        public decimal? PromotionalPrice { get; set; }
        public decimal? SavedAmount { get; set; }
        public string TopText { get; set; }
        public string BottomText { get; set; }
        public int ProductMultiplier { get; set; }
        public object TagContent { get; set; }
        public object TagLink { get; set; }
        public object FallbackText { get; set; }
        public string TagType { get; set; }
        public string TagTestType { get; set; }
        public string TagTestValue { get; set; }
    }

    public class AdditionalDetail
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public object Value { get; set; }

        public T GetValue<T>()
        {
            if (Value is T) return (T)Value;

            return default(T);
        }
    }

    public class Product : RecommendedProduct
    {
        public Nutrition Nutrition { get; set; }
        public string VideoUrl { get; set; }
        public List<RecommendedProduct> RecommendedProducts { get; set; }
        public List<RecommendedProduct> ProductsInSameOffer { get; set; }
    }

    public class ProductModel
    {
        public long PackDefaultStockCode { get; set; }
        public long PackParentStockCode { get; set; }
        public List<Product> Products { get; set; }
        public string PackMessage { get; set; }
    }

    public class Nutrition
    {

    }
}