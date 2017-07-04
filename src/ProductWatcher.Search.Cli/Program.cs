using System;
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
//using ProductWatcher.ES.Common;
using ProductWatcher.ES.ReadModel;
using ProductWatcher.Models;
using StructureMap;
using StructureMap.Graph.Scanning;

namespace ProductWatcher.Search.Cli
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }
        public static IContainer Container { get; set; }
        public static object _lock = new object();
        public static void Main(string[] args)
        {
            bool shouldExit = false;
            CultureInfo.CurrentCulture = new CultureInfo("en-Au");
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;

            SetupConfiguration();
            ConfigureServices();

            Console.CancelKeyPress += (sender, e) =>
            {
                shouldExit = true;
                Console.WriteLine("Please wait for tasks to finalize... we will exit after all tasks complete");
            };

            //Console.WriteLine("Wanna Debug Y/N : (Default: N)");
            //var debugLine = Console.ReadLine().Trim().ToUpperInvariant();
            var debug = false;//(debugLine == "Y" || debugLine == "YES");

            Console.WriteLine("Enter he product to search for: ");
            var search = Console.ReadLine();

            using (Container)
            {
                var allScrapers = Container.GetAllInstances<IScrapeProduct>().Where(x => !x.GetType().GetTypeInfo().IsAbstract && !x.Alcohol);
                var db = Container.TryGetInstance<IDatabase>();
                Parallel.ForEach(allScrapers, (scraper) =>
                {
                    bool notFailed = true;
                    try
                    {
                       TryGetStuff(scraper, search, db);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        try
                        {
                           TryGetStuff(scraper, search, db);
                        }
                        catch (Exception)
                        {
                            Console.WriteLine($"couldnt save {scraper.CompanyName}");
                        }
                        notFailed = false;
                    }
                    finally
                    {
                        if (notFailed) Console.WriteLine($"saved {scraper.CompanyName}");
                    }
                    //  Console.WriteLine("       --        ");
                });
            }

        }

        private static void TryGetStuff(IScrapeProduct scraper, string search, IDatabase db)
        {
            var a = scraper.Search(search).Result;
            var b = scraper.GetSearchModel(a).Result;

            //  Console.WriteLine($"Searching {scraper.CompanyName} for {search}....");
            //  Console.WriteLine(" ------------------ ");
            //  if (debug)
            //  {
            //      Console.WriteLine(a);
            //  }

            // if (scraper is Apis.Coles.Scraper)
            // {
            //     foreach (var item in b)
            //     {
            //         lock (_lock)
            //         {

            //         }
            //         // Console.WriteLine("${0} - {1}  {2}.{3}", item.Amount, item.CupSting, item.Name, item.Brand);
            //     }
            // }
            // else
            // {

            //var left = db.Query<DbModels.Product>().Where(x => b.Any(y => y.ProductCode == x.Code));
            //var newsss = b.Where(x => left.Any(y => y.Code == x.ProductCode));
            List<DbModels.Product> products = new List<DbModels.Product>();
            foreach (var item in b)
            {
                lock (_lock)
                {
                    if (!(db.Query<DbModels.Product>().Any(x => x.Code == item.ProductCode)))
                    {
                        products.Add(new DbModels.Product()
                        {
                            Id = Guid.NewGuid(),
                            Code = item.ProductCode,
                            Company = item.Company,
                            Name = $"{item.Brand} - {item.Name} - {item.Description}"
                        });
                    }
                }

                // Console.WriteLine("${0} - {1}  {2}.{3}", item.Amount, item.CupSting, item.Description, item.Brand);
            }

            lock (_lock)
            {
                db.InsertBulk(products);
            }

            //}

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
}
