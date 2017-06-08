using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using ProductWatcher.Apis.Shared.ColesLiqourGroup.Models;
using ProductWatcher.Models;

namespace ProductWatcher.Apis.Shared.ColesLiqourGroup
{
    public abstract class BaseScraper : IScrapeProduct
    {
        public virtual string SearchUrl => string.Empty;
        public virtual string CompanyName => string.Empty;
        public bool Alcohol => true;

        public virtual async Task<string> Get(string productCode)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<Product> GetProduct(string rawData)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<string> Search(string searchTerm) => await Search(searchTerm, null);

        public virtual async Task<string> Search(string searchTerm, string storeData)
        {
            var url = string.Format(SearchUrl, WebUtility.UrlEncode(searchTerm));

            var b = await url.GetStringAsync();
            return b;
        }
        public async Task<Search[]> GetSearchModel(string rawData)
        {
            var b = JsonConvert.DeserializeObject<Models.SearchModel>(rawData);

            return b.Data.Where(x => x.ItemType == ItemType.Product).Select(x =>
            {
                return new Search
                {
                    Company = CompanyName,
                    Name = x.Text,
                    ImageUrl = x.Url
                };
            }).ToArray();
        }
    }
}