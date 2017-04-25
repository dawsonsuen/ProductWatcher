using System;
using System.Net;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using ProductWatcher.Models;

namespace ProductWatcher.Apis.Woolworths
{
    public class Scraper : IScrapeProduct
    {
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
            product.CupPrice = model.Product.CupPrice ?? -1;

            if (model.Product.IsOnSpecial)
            {
                product.Price = model.Product.WasPrice ?? -1;
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

        public Task<string> Search(string searchTerm)
        {throw new NotImplementedException();
        }

        public async Task<string> SearchAsync(string searchTerm, string storeData)
        {
            var a = string.Format(SEARCH_URL, WebUtility.UrlEncode(searchTerm), "20");

            var b = await a.GetStringAsync();
            return b;
            // JArray res = b.Results;
            // return res.Select(x => ((JObject)x).Property("Id").Value.ToString()).ToArray();
        }
    }
}