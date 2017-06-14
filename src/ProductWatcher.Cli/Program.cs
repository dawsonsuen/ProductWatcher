using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using NPoco;
using ProductWatcher.DbModels;
using StructureMap;

namespace ProductWatcher.Cli
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static IContainer Container { get; set; }
        public static object _lock = new object();

        public static void LockInsert(IDatabase db, object poco)
        {
            //Console.WriteLine(poco.GetType().FullName);
            lock (db)
            {
                db.Insert(poco);
            }
        }

        public static void LockUpdate(IDatabase db, object poco)
        {
            lock (db)
            {
                db.Update(poco);
            }
        }

        public static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-Au");
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;
            SetupConfiguration();
            ConfigureServices();

            Console.WriteLine("One sec brah keep on yo coke....");

            using (var db = Container.GetInstance<IDatabase>())
            {
                db.OpenSharedConnection();
                var productList = db.Query<DbModels.Product>().ToList();

                Parallel.ForEach(productList, (x) =>
               // productList.ForEach(x =>
                {
                    try
                    {
                        Models.Product product = null;
                        var data = new DbModels.Data();
                        data.ProductId = x.Id;
                        data.When = DateTime.UtcNow;

                        if (ApiConstants.Coles.Is(x.Company))
                        {
                            data.RawData = ApiConstants.Coles.GetProductJson(x.Code);
                            LockInsert(db, data);
                            product = ApiConstants.Coles.GetProductFromJson(data.RawData);
                        }
                        else if (ApiConstants.Woolworths.Is(x.Company))
                        {
                            data.RawData = ApiConstants.Woolworths.GetProductJson(x.Code);
                            LockInsert(db, data);
                            product = ApiConstants.Woolworths.GetProductFromJson(data.RawData);
                        }
                        else
                        {
                            throw new NotSupportedException($"Oops looks like this company type is not supported: '{x.Company}', id: {x.Id}, product code: {x.Code} ");
                            //insert error log
                        }
                        data.ProductModel = JsonConvert.SerializeObject(product);
                        LockUpdate(db, data);

                        var price = new Price
                        {
                            DataId = data.Id,
                            ProductId = x.Id,
                            OriginalPrice = product.Price,
                            OnSalePrice = product.SpecialPrice,
                            Company = product.Company,
                            Description = product.Description,
                            AdditionalData = new Dictionary<string, object>(){
                                 {
                                     "$/L", product.DollarPerLitre
                                 },
                                 {
                                     "imgUrl",
                                     product.ImgUrl
                                 },

                            },
                            When = data.When
                        };

                        LockInsert(db, price);


                    }
                    catch (System.Exception)
                    {

                        throw;
                    }

                });


            };
        }

        public static void ConfigureServices()
        {
            Container = new Container();

            var services = new ServiceCollection()
                .AddSingleton<IConfigurationRoot>(Configuration)
                .AddSingleton<IDatabase>(new Database(Configuration.GetConnectionString("postgres"), DatabaseType.PostgreSQL, Npgsql.NpgsqlFactory.Instance));

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
