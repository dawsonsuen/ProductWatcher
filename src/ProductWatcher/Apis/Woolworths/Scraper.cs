using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using ProductWatcher.Models;

namespace ProductWatcher.Apis.Woolworths
{
    public class Scraper : IScrapeProduct
    {
        public bool Alcohol => false;
        public string CompanyName => "Woolworths";
        public const string PRICE_URL = "https://www.woolworths.com.au/apis/ui/product/detail/{0}";
        public const string SEARCH_URL = "https://www.woolworths.com.au/apis/ui/Search/products?IsSpecial=false&PageNumber=1&PageSize={1}&SearchTerm={0}&SortType=Personalised";

        public async Task<string> Get(string productCode)
        {
            var priceUrl = string.Format(PRICE_URL, productCode);
            var data = await priceUrl.GetStringAsync();
            return data;
        }

        public async Task<Product> GetProduct(string rawData)
        {
            var model = JsonConvert.DeserializeObject<Apis.Woolworths.Models.ProductModel>(rawData);
            var product = new ProductWatcher.Models.Product();

            product.Company = CompanyName;
            product.Id = model.Product.Stockcode.ToString();

            if (product.HasCupPrice)
            {
                product.CupPrice = model.Product.CupPrice.Value;
            }

            if (model.Product.IsOnSpecial)
            {
                product.Price = model.Product.WasPrice.Value;
                product.SpecialPrice = model.Product.Price;
            }
            else
            {
                product.Price = model.Product.Price;
            }

            product.Name = model.Product.Name;
            product.Description = model.Product.Description.Trim();
            product.MediumImageLink = model.Product.LargeImageFile;

            return product;
        }

        public async Task<string> Search(string searchTerm) => await Search(searchTerm, "35");

        public async Task<string> Search(string searchTerm, string storeData)
        {
            var a = string.Format(SEARCH_URL, WebUtility.UrlEncode(searchTerm), "35");

            var b = await a.GetStringAsync();

            return b;
        }

        public async Task<Search[]> GetSearchModel(string rawData)
        {
            var b = JsonConvert.DeserializeObject<Woolworths.Models.SearchModel>(rawData);

            if (b.Products == null) return new Search[0];

            var filteredProducts = new List<Search>();

            foreach (var product in b.Products.SelectMany(x => x.Products))
            {
                if (filteredProducts.Any(x => x.ProductCode == product.Stockcode.ToString())) continue;

                filteredProducts.Add(new Search
                {
                    Company = CompanyName,
                    Brand = product.Brand,
                    Name = product.Name,
                    Description = product.Description,
                    ProductCode = product.Stockcode.ToString(),
                    Amount = product.Price,
                    CupSting = product.CupString
                });
            }

            return filteredProducts.ToArray();
        }
    }
}