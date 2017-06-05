using System.Collections.Generic;

namespace ProductWatcher.Apis.Woolworths.Models
{

    #region irrelevant
    public class PrimaryCategory
    {
        public string Department { get; set; }
        public string Aisle { get; set; }
        public int VisualShoppingAisleId { get; set; }
        public int DisplayOrder { get; set; }
        public object OverrideName { get; set; }
        public string Instance { get; set; }
    }

    public class AdditionalAttributes
    {
    }

    public class CentreTag : BaseTag
    {
        public string TagContent { get; set; }
        public string TagLink { get; set; }
        public object FallbackText { get; set; }
        public string TagType { get; set; }
    }

    public class ImageTag : BaseTag
    {
        public string TagContent { get; set; }
        public string TagLink { get; set; }
        public string FallbackText { get; set; }
        public string TagType { get; set; }
    }

    public class HeaderTag : BaseTag
    {
        public object BackgroundColor { get; set; }
        public object BorderColor { get; set; }
        public object TextColor { get; set; }
        public object Content { get; set; }
        public string TagLink { get; set; }
        public string Promotion { get; set; }
    }

    public class SapCategories
    {
        public string SapDepartmentName { get; set; }
        public string SapCategoryName { get; set; }
        public string SapSubCategoryName { get; set; }
        public string SapSegmentName { get; set; }
    }

    public class FooterTag : BaseTag
    {
        public object TagContent { get; set; }
        public object TagLink { get; set; }
        public object FallbackText { get; set; }
        public string TagType { get; set; }
    }

    public class BaseTag
    {
        public string TagTestType { get; set; }
        public object TagTestValue { get; set; }
    }
    #endregion

    public class Product
    {
        public int Stockcode { get; set; }
        public decimal? CupPrice { get; set; }
        public string CupMeasure { get; set; }
        public string CupString { get; set; }
        public bool HasCupPrice { get; set; }
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string UrlFriendlyName { get; set; }
        public string Description { get; set; }
        public string SmallImageFile { get; set; }
        public string MediumImageFile { get; set; }
        public string LargeImageFile { get; set; }
        public bool IsNew { get; set; }
        public bool IsOnSpecial { get; set; }
        public bool IsEdrSpecial { get; set; }
        public decimal? SavingsAmount { get; set; }
        public decimal? WasPrice { get; set; }
        public int QuantityInTrolley { get; set; }
        public string Unit { get; set; }
        public int MinimumQuantity { get; set; }
        public bool IsInTrolley { get; set; }
        public string Source { get; set; }
        public int SupplyLimit { get; set; }
        public string PackageSize { get; set; }
        public bool IsPmDelivery { get; set; }
        public bool IsForCollection { get; set; }
        public bool IsForDelivery { get; set; }
        public CentreTag CentreTag { get; set; }
        public bool IsCentreTag { get; set; }
        public ImageTag ImageTag { get; set; }
        public HeaderTag HeaderTag { get; set; }
        public bool HasHeaderTag { get; set; }
        public int UnitWeightInGrams { get; set; }
        public string SupplyLimitMessage { get; set; }
        public string SmallFormatDescription { get; set; }
        public string FullDescription { get; set; }
        public bool IsAvailable { get; set; }
        public bool AgeRestricted { get; set; }
        public int DisplayQuantity { get; set; }
        public object RichDescription { get; set; }
        public bool IsDeliveryPass { get; set; }
        public bool HideWasSavedPrice { get; set; }
        public SapCategories SapCategories { get; set; }
        public string Brand { get; set; }
        public FooterTag FooterTag { get; set; }
        public bool IsFooterEnabled { get; set; }
        public object Diagnostics { get; set; }
    }


    public class ProductModel
    {
        public Product Product { get; set; }
        public object Nutrition { get; set; }
        public object VideoUrl { get; set; }
        public PrimaryCategory PrimaryCategory { get; set; }
        public AdditionalAttributes AdditionalAttributes { get; set; }
        public List<object> DetailsImagePaths { get; set; }
        public object NutritionalInformation { get; set; }
    }
}
