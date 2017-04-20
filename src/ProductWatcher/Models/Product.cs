
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
        public string Blurb { get; set; }
        public decimal? SpecialPrice { get; set; }
        public decimal Price { get; set; }
        public bool IsOnSpecial { get; set; }
        public decimal SavingsAmount { get; set; }
        public string Unit { get; set; }
        public string PackageSize { get; set; }
        public string Brand { get; set; }
        public decimal CupPrice { get; set; }
        public string CupMesure { get; set; }
        public string CupString { get; set; }
        public bool HasCupPrice { get; set; }
        public string SmallImageLink { get; set; }
        public string MediumImageLink { get; set; }
        public string LargeImageLink { get; set; }
        public string Company { get; set; }
    }
}