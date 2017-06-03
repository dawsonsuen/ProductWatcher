using System;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using ProductWatcher.Apis.BWS.Models;
using ProductWatcher.Models;

namespace ProductWatcher.Apis.BWS
{
    public class Scraper : IScrapeProduct
    {
        public bool Alcohol => true;
        public static string SEARCH_URL = "https://api.bws.com.au/apis/ui/Search/Suggestion?Key={0}";
        public string CompanyName => "BWS";

        public Task<string> Get(string productCode)
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProduct(string rawData)
        {
            throw new NotImplementedException();
        }

        public async Task<string> Search(string searchTerm) => await SearchAsync(searchTerm, null);

        public async Task<string> SearchAsync(string searchTerm, string storeData)
        {
            var url = string.Format(SEARCH_URL, searchTerm);

            //var b = await url.GetStringAsync();
            var b = await url.GetJsonAsync<SearchModel>();

            return b.Products.Suggestions.First().ParentStockcode;
        }
    }
}