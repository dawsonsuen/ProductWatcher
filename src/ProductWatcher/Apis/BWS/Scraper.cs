using System;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using ProductWatcher.Apis.BWS.Models;
using ProductWatcher.Models;

namespace ProductWatcher.Apis.BWS
{
    public class Scraper : IScrapeProduct
    {
        public bool Alcohol => true;
        public static string SEARCH_URL = "https://api.bws.com.au/apis/ui/Search/Suggestion?Key={0}";
        public static string PRODUCT_URL = "https://api.bws.com.au/apis/ui/Product/{0}";
        public string CompanyName => "BWS";

        public Task<string> Get(string productCode)
        {
            throw new NotImplementedException();
        }

        public Task<ProductWatcher.Models.Product> GetProduct(string rawData)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Search(string searchTerm) => await Search(searchTerm, null);

        public async Task<string> Search(string searchTerm, string storeData)
        {
            var url = string.Format(SEARCH_URL, searchTerm);

            var b = await url.GetStringAsync();
            return b;
        }

        public async Task<Search[]> GetSearchModel(string rawData)
        {
            var b = JsonConvert.DeserializeObject<BWS.Models.SearchModel>(rawData);

            return b.Products.Suggestions.Select(x =>
            {
                return new Search
                {
                    Company = CompanyName,
                    Name = x.Title,
                    Description = x.ProductName,
                    Quanity = x.ProductUnitQuantity,
                    ProductCode = x.Stockcode,
                    Brand = x.Brand,
                    Amount = x.Price
                };
            }).ToArray();
        }
    }
}