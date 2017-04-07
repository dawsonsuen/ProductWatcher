using ProductWatcher.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Flurl.Http;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Net;

namespace ProductWatcher
{
    public class ApiConstants
    {
        public abstract class Coles
        {
            public const string PRICE_URL = "https://shop.coles.com.au/search/resources/store/20601/productview/bySearchTerm/{0}";
            public const string SEARCH_URL = "https://api.coles.com.au/customer/v1/coles/products/search?limit={1}&q={0}&start=0&storeId={2}&type=SKU";
            public static Dictionary<string, string> SEARCH_URL_HEADERS = new Dictionary<string, string>
            {
                { "X-Coles-API-Key" , "046bc0d4-3854-481f-80dc-85f9e846503d"},
                { "X-Coles-API-Secret", "e6ab96ff-453b-45ba-a2be-ae8d7c12cadf"}
            };

            public static string GetProductJson(string productCode)
            {
                var a = string.Format(PRICE_URL, productCode);
                var data = a.GetStringAsync().Result;
                return data;
            }
            public static Product GetProductModel(string productCode) => GetProductFromJson(GetProductJson(productCode));

            public static Product GetProductFromJson(string json) =>
                new Product(Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Coles.ShitAsProductModel>(json));

            public static bool Is(string c) => Models.Coles.COMPANY_NAME == c.ToUpperInvariant();

            public static string[] SearchProducts(string search)
            {
                var a = string.Format(SEARCH_URL, search, "20", "560");

                var b = a.WithHeaders(SEARCH_URL_HEADERS).GetJsonAsync<dynamic>().Result;
                JArray res = b.Results;
                return res.Select(x => ((JObject)x).Property("Id").Value.ToString()).ToArray();
            }
        }

        public abstract class Woolworths
        {
            public const string PRICE_URL = "https://www.woolworths.com.au/apis/ui/product/detail/{0}";
            public const string SEARCH_URL = "https://www.woolworths.com.au/apis/ui/Search/products?IsSpecial=false&PageNumber=1&PageSize={1}&SearchTerm={0}&SortType=Personalised";

            public static bool Is(string c) => Models.Woolworths.COMPANY_NAME == c.ToUpperInvariant();

            public static string GetProductJson(string productCode)
            {
                var a = string.Format(PRICE_URL, productCode);
                var data = a.GetStringAsync().Result;
                return data;
            }
            public static Product GetProductModel(string productCode) => GetProductFromJson(GetProductJson(productCode));

            public static Product GetProductFromJson(string json) =>
                new Product(Newtonsoft.Json.JsonConvert.DeserializeObject<Models.Woolworths.ProductModel>(json));

            public static string SearchProducts(string search)
            {

                var a = string.Format(SEARCH_URL, WebUtility.UrlEncode(search), "20");

                var b = a.GetStringAsync().Result;
                return b;
                // JArray res = b.Results;
                // return res.Select(x => ((JObject)x).Property("Id").Value.ToString()).ToArray();
            }
        }
    }

}
