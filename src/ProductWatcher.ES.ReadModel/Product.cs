using System;
using System.Collections.Generic;

namespace ProductWatcher.ES.ReadModel
{
    public class Product
    {
        public Guid StreamId { get; set; }
        public decimal? SpecialPrice { get; set; }
        public decimal Price { get; set; }
        public string CompanyProductCode { get; set; }
        public string Upn {get;set;}

        public string Name { get; set; }
        public string Description { get; set; }

        public string PackageSize { get; set; }
        public string MessurementString { get; set; }

        public string ProductThumbnailLink { get; set; }
        public string ProductImageLink { get; set; }
        public string Company { get; set; }


        public List<string> Keywords { get; set; }
        public List<Product> RelatedProducts { get; set; }
    }
}