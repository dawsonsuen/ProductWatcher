using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
//using Autofac;
//using Autofac.Core;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
//using NEvilES.Pipeline;
using Newtonsoft.Json;
using NPoco;
using ProductWatcher.Apis;
using ProductWatcher.DbModels;
using ProductWatcher.ES.ReadModel;
using ProductWatcher.Models;
using StructureMap;
using StructureMap.Graph.Scanning;

namespace ProductWatcher.Cli
{
    class Program
    {

        public static IConfigurationRoot Configuration { get; set; }
        public static IContainer Container { get; set; }
        public static object _lock = new object();

        public static void Main(string[] args)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-Au");
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;

            SetupConfiguration();
            ConfigureServices();

            using (Container)
            {
                var allScrapers = Container.GetAllInstances<IScrapeProduct>().Where(x => !x.GetType().GetTypeInfo().IsAbstract);
                var db = Container.TryGetInstance<IDatabase>();
                var allProducts = db.Query<DbModels.Product>().ToArray().Batch(300);

                Parallel.ForEach(allProducts, async (product) =>
                {
                    foreach (var item in product)
                    {
                        try
                        {
                            var data = new DbModels.Data();
                            data.ProductId = item.Id;
                            data.When = DateTime.UtcNow;

                            var scraper = allScrapers.Where(x => x.CompanyName == item.Company).SingleOrDefault();

                            var rawData = await scraper.Get(item.Code);
                            data.RawData = rawData;

                            LockInsert(db, data);
                            var productModel = await scraper.GetProduct(data.RawData);
                            data.ProductModel = JsonConvert.SerializeObject(productModel);
                            LockUpdate(db, data);

                            var price = new Price
                            {
                                DataId = data.Id,
                                ProductId = item.Id,
                                OriginalPrice = productModel.Price,
                                OnSalePrice = productModel.SpecialPrice,
                                Company = productModel.Company,
                                Description = productModel.Description,
                                AdditionalData = new Dictionary<string, object>()
                                {
                                    {
                                    nameof(productModel.CupString), productModel.CupString
                                    },
                                    {
                                    nameof(productModel.CupPrice), productModel.CupPrice
                                    },
                                    {
                                    nameof(productModel.CupMesure), productModel.CupMesure
                                    },
                                    {
                                    nameof(productModel.Blurb), productModel.Blurb
                                    },
                                    {
                                    nameof(productModel.HasCupPrice), productModel.HasCupPrice
                                    },
                                    {
                                    nameof(productModel.Unit), productModel.Unit
                                    },
                                    {
                                    nameof(productModel.MediumImageLink), productModel.MediumImageLink
                                    },
                                    {
                                    nameof(productModel.LargeImageLink), productModel.LargeImageLink
                                    },
                                },
                                When = data.When
                            };

                            LockInsert(db, price);

                        }
                        catch (Exception ex)
                        {
                            //log failure
                        }
                    }
                });
            }
        }

        public static void LockInsert(IDatabase db, object poco)
        {
            lock (_lock)
            {
                db.Insert(poco);
            }
        }

        public static void LockUpdate(IDatabase db, object poco)
        {
            lock (_lock)
            {
                db.Update(poco);
            }
        }

        public static void ConfigureServices()
        {
            var container = new Container();
            container.Configure((x) =>
            {
                x.For<IDatabase>().Use(new Database(Configuration.GetConnectionString("postgres"), DatabaseType.PostgreSQL, Npgsql.NpgsqlFactory.Instance));
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.Assembly(typeof(IScrapeProduct).GetTypeInfo().Assembly);
                    scan.WithDefaultConventions();
                    scan.AddAllTypesOf<IScrapeProduct>();
                });
            });


            //builder.RegisterInstance<IConfigurationRoot>(Configuration);
            //builder.RegisterInstance(new CommandContext.User(Guid.NewGuid())).Named<CommandContext.IUser>("user");
            //builder.RegisterModule(new EventStoreDatabaseModule(Configuration.GetConnectionString("postgres")));
            //builder.RegisterModule(new EventProcessorModule(typeof(ES.Domain.Product).GetTypeInfo().Assembly, typeof(ES.ReadModel.Product).GetTypeInfo().Assembly));
            //builder.RegisterType<SqlReadModel>().AsImplementedInterfaces();
            //builder.RegisterType<IScrapeProduct>().SingleInstance().AsImplementedInterfaces();
            //builder.

            //Container = builder.Build();
            Container = container;
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
    public static class Extensions
    {
        public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items, int maxItems)
        {
            return items.Select((item, inx) => new { item, inx })
                        .GroupBy(x => x.inx / maxItems)
                        .Select(g => g.Select(x => x.item));
        }


    }
}
