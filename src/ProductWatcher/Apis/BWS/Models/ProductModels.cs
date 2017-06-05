using System.Collections.Generic;

namespace ProductWatcher.Apis.BWS.Models
{
    public class ProductModels
    {

    }

    public class RecommendedProduct : Woolworths.Models.Product
    {
        public object PercentageOffTag { get; set; }
        public object FixedPricePromoTag { get; set; }
        public bool IsFeaturedTag { get; set; }
        public int OverallRating { get; set; }
        public int NumberOfReviews { get; set; }
        public bool IsWatched { get; set; }
        public object BrandName { get; set; }
        public object Title { get; set; }
        public object ImageVariants { get; set; }
        public object AdditionalDetails { get; set; }
        public int ParentStockCode { get; set; }
        public int StockOnHand { get; set; }
        public int BackorderStockOnHand { get; set; }
        public object DeliveryStockOnHand { get; set; }
        public object PickupStockOnHand { get; set; }
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
        public double PromotionalPrice { get; set; }
        public double SavedAmount { get; set; }
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
        public object Nutrition { get; set; }
        public string VideoUrl { get; set; }
        public List<RecommendedProduct> RecommendedProducts { get; set; }
        public List<RecommendedProduct> ProductsInSameOffer { get; set; }
    }

    public class RootObject
    {
        public long PackDefaultStockCode { get; set; }
        public long PackParentStockCode { get; set; }
        public List<Product> Products { get; set; }
        public string PackMessage { get; set; }
    }
}