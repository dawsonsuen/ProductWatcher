using System;
using System.IO;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NPoco;
using StructureMap;

namespace ProductWatcher
{
    class Program
    {
        public static Endpoint[] Endpoints = new[]{
            new Endpoint{
                Url = "https://shop.coles.com.au/search/resources/store/20601/productview/bySeoUrlKeyword/coca-cola-soft-drink-coke-375ml-cans",
                Company = "Coles"
            },
              new Endpoint{
                Url = "https://shop.coles.com.au/search/resources/store/20601/productview/bySeoUrlKeyword/coca-cola-soft-drink-coke-375ml-cans-7365777p",
                Company = "Coles"
            },
              new Endpoint{
                Url = "https://www.woolworths.com.au/apis/ui/product/detail/42605",
                Company = "Woolworths"
            },
              new Endpoint{
                Url = "https://www.woolworths.com.au/apis/ui/product/detail/84552",
                Company = "Woolworths"
            }
        };

        public static IConfigurationRoot Configuration { get; set; }
        public static IContainer Container { get; set; }

        public static void Main(string[] args)
        {
            SetupConfiguration();
            ConfigureServices();

            Console.WriteLine("One sec brah keep on yo coke....");

            using (var db = Container.GetInstance<IDatabase>())
            {

                foreach (var endpoint in Endpoints)
                {
                    var responseString = endpoint.Url.GetAsync().ReceiveJson<dynamic>().Result.ToString(Formatting.None);
                    dynamic cleanResponse = JsonConvert.DeserializeObject<dynamic>(responseString);

                    var price = new Price
                    {
                        OriginalData = responseString,
                        Company = endpoint.Company,
                        When = DateTime.Now
                    };

                    db.Insert(price);

                    if (cleanResponse?.catalogEntryView != null)
                    {
                        dynamic product = cleanResponse.catalogEntryView;
                        if (product[0].p1.l4 != null)
                        {
                            price.OriginalPrice = decimal.Parse(product[0].p1.l4.Value);
                            price.OnSalePrice = decimal.Parse(product[0].p1.o.Value);
                        }
                        else
                        {
                            price.OriginalPrice = decimal.Parse(product[0].p1.o.Value);
                        }

                        price.Description = $"{product[0].n.Value} {product[0].a.O3[0].Value}";

                    }
                    else if (cleanResponse?.Product != null)
                    {

                        dynamic product = cleanResponse.Product;
                        if (product.Price != null)
                        {
                            price.OriginalPrice = decimal.Parse(product.WasPrice.Value.ToString());
                            price.OnSalePrice = decimal.Parse(product.Price.Value.ToString());
                        }

                        price.Description = $"{product.Description}";

                    }
                    price.Description = price.Description.Trim();

                    db.Update(price);
                }
            };
        }

        public static void ConfigureServices()
        {
            Container = new Container();

            var services = new ServiceCollection()
                .AddSingleton<IConfigurationRoot>(Configuration)
                .AddSingleton<IDatabase>(new Database(Configuration.GetConnectionString("Default"), DatabaseType.SqlServer2012, System.Data.SqlClient.SqlClientFactory.Instance));

            Container.Populate(services);
        }

        public static void SetupConfiguration()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
               .AddJsonFile("appsettings.*.json", optional: true, reloadOnChange: false);

            Configuration = builder.Build();
        }
    }


    public class Endpoint
    {
        public string Url { get; set; }
        public string Company { get; set; }
    }
}
