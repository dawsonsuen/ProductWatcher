using System;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
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

        public virtual async Task<string> Search(string searchTerm) => await SearchAsync(searchTerm, null);

        public virtual async Task<string> SearchAsync(string searchTerm, string storeData)
        {
            var url = string.Format(SearchUrl, searchTerm);

            var b = await url.GetJsonAsync<SearchModel>();

            return b.Data.Where(x=>x.ItemType == ItemType.Product).First().Url;
        }
    }
}