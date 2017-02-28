
using System;
using System.Collections.Generic;

namespace ProductWatcher.Models
{
    public class Product
    {
        public Product()
        {

        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? SpecialPrice { get; set; }
        public decimal Price { get; set; }
        public decimal DollarPerLitre { get; set; }
        public string ImgUrl { get; set; }
        public string Company { get; set; }

        public Product(Coles.ShitAsProductModel model)
        {
            this.Company = Coles.COMPANY_NAME;

            var product = model.catalogEntryView[0];
            if (product != null)
            {
                var productCode = product.p.ToString();

                this.Id = productCode.ToUpperInvariant().EndsWith("P") ? productCode.Substring(0, productCode.Length - 1) : productCode;
                this.Name = product.n;

                var quantityDescription = string.Join(" ", product.a.O3);
                this.Description = $"{this.Name} {quantityDescription}";

                if (product.P1.l4 != null) {
                    this.Price = product.P1.l4 ?? -1;
                    this.SpecialPrice = product.P1.o ?? -1;
                }
                else
                {
                    this.Price = product.P1.o ?? -1;
                }

                this.ImgUrl = product.fi;

                if (product.u2 != null) {
                    var a = product.u2.Split(' ');
                    this.DollarPerLitre = decimal.Parse(a[0],System.Globalization.NumberStyles.Currency);
                }
            }

        }

        public Product(Woolworths.ProductModel model)
        {
            this.Company = Woolworths.COMPANY_NAME;

            this.Id = model.Product.Stockcode.ToString();

            this.DollarPerLitre = model.Product.CupPrice ?? -1;

            if (model.Product.IsOnSpecial)
            {
                this.Price = model.Product.WasPrice ?? -1;

                this.SpecialPrice = model.Product.Price;

            }
            else
            {
                this.Price = model.Product.Price;
            }

            this.Name = model.Product.Name;
            this.Description = model.Product.Description;
            this.ImgUrl = model.Product.LargeImageFile;
        }
    }

    public abstract class Woolworths
    {
        public const string COMPANY_NAME = "WOOLWORTHS";

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

        public class CentreTag
        {
            public string TagContent { get; set; }
            public string TagLink { get; set; }
            public object FallbackText { get; set; }
            public string TagType { get; set; }
        }

        public class ImageTag
        {
            public string TagContent { get; set; }
            public string TagLink { get; set; }
            public string FallbackText { get; set; }
            public string TagType { get; set; }
        }

        public class HeaderTag
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

        public class FooterTag
        {
            public object TagContent { get; set; }
            public object TagLink { get; set; }
            public object FallbackText { get; set; }
            public string TagType { get; set; }
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

    public abstract class Coles
    {
        public const string COMPANY_NAME = "COLES";
        public class A
        {
            //quantity descripton
            public List<string> O3 { get; set; } = new List<string>();

            #region WTF like wow coles...who wrote this crap
            public List<string> A4 { get; set; }
            public List<string> B { get; set; }
            public List<string> E1 { get; set; }
            public List<string> F3 { get; set; }
            public List<string> H1 { get; set; }
            public List<string> H2 { get; set; }
            public List<string> I1 { get; set; }
            public List<string> L2 { get; set; }
            public List<string> O { get; set; }
            public List<string> O1 { get; set; }

            public List<string> P { get; set; }
            public List<string> P8 { get; set; }
            public List<string> P9 { get; set; }
            public List<string> S { get; set; }
            public List<string> S5 { get; set; }
            public List<string> S9 { get; set; }
            public List<string> T { get; set; }
            public List<string> T1 { get; set; }
            public List<string> T2 { get; set; }
            public List<string> U { get; set; }
            public List<string> W1 { get; set; }
            public List<string> WEIGHTEDBYEACH { get; set; }
            #endregion
        }

        public class Price
        {
            public decimal? l4 { get; set; }
            public decimal? o { get; set; }
        }

        public class CatalogEntryView
        {
            //Only shit we use

            //image
            public string fi { get; set; }
            //Brand
            public string m { get; set; }
            //Image thumb
            public string t { get; set; }
            //This is price per litre
            public string u2 { get; set; }
            //Product Description
            public string n { get; set; }
            //Price dahhh
            public Price P1 { get; set; }
            //Product code
            public string p { get; set; }

            #region more absolute Nonsense

            public A a { get; set; }
            public string pl { get; set; }
            public string s { get; set; }
            public string s9 { get; set; }

            public string t1 { get; set; }
            public string u { get; set; }
            #endregion
        }

        public class M4
        {
            public string p1 { get; set; }
        }

        public class ShitAsProductModel
        {
            public List<CatalogEntryView> catalogEntryView { get; set; } = new List<CatalogEntryView>();
            public M4 m4 { get; set; }
            public int? recordSetCount { get; set; }
            public int? recordSetStartNumber { get; set; }
            public int? recordSetTotal { get; set; }
        }
    }
}