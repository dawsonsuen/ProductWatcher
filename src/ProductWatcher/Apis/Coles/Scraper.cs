using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProductWatcher.Models;

namespace ProductWatcher.Apis.Coles
{
    public class Scraper : IScrapeProduct
    {
        public bool Alcohol => false;
        public string CompanyName => "Coles";
        public const string PRICE_URL = "https://shop.coles.com.au/search/resources/store/20601/productview/bySearchTerm/{0}";
        public const string SEARCH_URL = "https://api.coles.com.au/customer/v1/coles/products/search?limit={1}&q={0}&start=0&storeId={2}&type=SKU";
        public static Dictionary<string, string> SEARCH_URL_HEADERS = new Dictionary<string, string>
            {
                    { "X-Coles-API-Key" , "046bc0d4-3854-481f-80dc-85f9e846503d"},
                    { "X-Coles-API-Secret", "e6ab96ff-453b-45ba-a2be-ae8d7c12cadf"}
            };

        public async Task<string> Get(string productCode)
        {
            var priceUrl = string.Format(PRICE_URL, productCode);
            var data = await priceUrl.GetStringAsync();

            return data;
        }

        public async Task<Product> GetProduct(string rawData)
        {
            var model = JsonConvert.DeserializeObject<Coles.Models.ShitAsProductModel>(rawData);
            var product = new Product();
            product.Company = CompanyName;

            var catalogItem = model.catalogEntryView[0];
            if (catalogItem != null)
            {
                var catalogItemCode = catalogItem.p.ToString();

                product.Id = catalogItemCode.ToUpperInvariant().EndsWith("P") ? catalogItemCode.Substring(0, catalogItemCode.Length - 1) : catalogItemCode;
                product.Name = catalogItem.n;

                var quantityDescription = string.Join(" ", catalogItem.a.O3);
                product.Description = $"{product.Name} {quantityDescription}";

                if (catalogItem.P1.l4 != null)
                {
                    product.Price = catalogItem.P1.l4 ?? -1;
                    product.SpecialPrice = catalogItem.P1.o ?? -1;
                }
                else
                {
                    product.Price = catalogItem.P1.o ?? -1;
                }

                product.MediumImageLink = catalogItem.fi;

                if (catalogItem.u2 != null)
                {
                    var a = catalogItem.u2.Split(' ');

                    product.CupPrice = decimal.Parse(a[0], System.Globalization.NumberStyles.Currency);
                }
            }

            return product;
        }

        public async Task<string> Search(string searchTerm)
        {
            return await SearchAsync(searchTerm, null);
        }

        public async Task<string> SearchAsync(string searchTerm, string storeData)
        {
            var a = string.Format(SEARCH_URL, searchTerm, "20", "560");

            var b = await a.WithHeaders(SEARCH_URL_HEADERS).GetStringAsync();

            return b;
        }
    }
}