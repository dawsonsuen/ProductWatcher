using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        public async Task<string> GetAsync(string productCode)
        {
            var priceUrl = string.Format(PRICE_URL, productCode);
            var data = await priceUrl.GetStringAsync();

            return data;
        }

        public async Task<Product> GetProductAsync(string rawData) => (await GetProducts(rawData)).FirstOrDefault();

        public async Task<Product[]> GetProducts(string rawData)
        {
            var model = JsonConvert.DeserializeObject<Coles.Models.ShitAsProductModel>(rawData);

            var products = model.catalogEntryView.Select(x =>
            {
                var product = new Product();
                product.Company = CompanyName;
                if (x != null)
                {
                    var catalogItemCode = x.p.ToString();

                    product.Id = catalogItemCode.ToUpperInvariant().EndsWith("P") ? catalogItemCode.Substring(0, catalogItemCode.Length - 1) : catalogItemCode;

                    var brand = x.m ?? string.Empty;
                    product.Name = x.n;

                    if (!product.Name.ToUpperInvariant().Contains(brand.ToUpperInvariant()))
                    {
                        product.Name = brand != string.Empty ? $"{brand} {x.n}" : x.n;
                    }

                    product.Brand = brand;

                    var quantityDescription = string.Join(" ", x.a.O3);
                    product.Description = $"{product.Name} {quantityDescription}";

                    if (x.P1.l4 != null)
                    {
                        product.Price = x.P1.l4 ?? -1;
                        product.SpecialPrice = x.P1.o ?? -1;
                    }
                    else
                    {
                        product.Price = x.P1.o ?? -1;
                    }
                    product.SmallImageLink = x.t;
                    product.MediumImageLink = x.fi;

                    //if (x.u2 != null)
                    //{
                        //var a = x.u2.Split(' ');

                        product.CupString = x?.u2 ?? "";
                        //product.CupPrice = decimal.Parse(a[0], System.Globalization.NumberStyles.Currency);
                    //}
                }
                return product;
            });

            return products.ToArray();
        }

        public async Task<string> SearchAsync(string searchTerm) => await SearchAsync(searchTerm, null);
        public async Task<string> SearchAsync(string searchTerm, string storeData)
        {
            //var a = string.Format(SEARCH_URL, searchTerm, "20", "560");
            var a = string.Format(PRICE_URL, WebUtility.UrlEncode(searchTerm));

            //var b = await a.WithHeaders(SEARCH_URL_HEADERS).GetStringAsync();
            var b = await a.GetStringAsync();

            return b;
        }

        public async Task<Search[]> GetSearchModelAsync(string rawData)
        {
            var b = await GetProducts(rawData);
            return b.Select(x =>
            {
                return new Search
                {
                    Company = CompanyName,
                    Name = x.Name,
                    Brand = x.Brand,
                    ImageUrl = x.SmallImageLink,
                    ProductCode = x.Id.ToString(),
                    Amount = x.Price,
                    CupSting = x.CupString
                };
            }).ToArray();
        }
    }
}