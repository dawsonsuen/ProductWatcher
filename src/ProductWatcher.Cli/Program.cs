using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
//using Autofac;
//using Autofac.Core;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NEvilES.Pipeline;
using Newtonsoft.Json;
using NPoco;
using ProductWatcher.Apis;
using ProductWatcher.DbModels;
using ProductWatcher.ES.Common;
using ProductWatcher.ES.ReadModel;
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
            bool shouldExit = false;
            SetupConfiguration();
            ConfigureServices();

            Console.CancelKeyPress += (sender, e) =>
            {
                shouldExit = true;
                Console.WriteLine("Please wait for tasks to finalize... we will exit after all tasks complete");
            };
            Console.WriteLine("Wanna Debug Y/N : (Default: N)");
            var debugLine = Console.ReadLine().Trim().ToUpperInvariant();
            var debug = (debugLine == "Y" || debugLine == "YES");

            Console.WriteLine("Enter he product to search for: ");
            var search = Console.ReadLine();

            using (Container)
            {
                foreach (var scraper in Container.GetAllInstances<IScrapeProduct>().Where(x => !x.GetType().GetTypeInfo().IsAbstract))
                {
                    Console.WriteLine($"Searching {scraper.CompanyName} for {search}....");
                    var a = scraper.Search(search).Result;
                    if (debug) Console.WriteLine(a);

                    Console.WriteLine(" ------------------ ");
                    var b = scraper.GetSearchModel(a).Result;
                    foreach (var item in b)
                    {
                        Console.WriteLine("${0} - {1} x {2}.{3}  {4}", item.Amount, item.Quanity, item.Name, item.Description, item.Brand);
                    }

                    Console.WriteLine("       --        ");
                }
            }


        }

        public static void ConfigureServices()
        {
            var container = new Container();
            container.Configure((x) =>
            {
                x.Scan(scan =>
                {
                    scan.TheCallingAssembly();
                    scan.Assembly(typeof(IScrapeProduct).GetTypeInfo().Assembly);
                    scan.WithDefaultConventions();
                    scan.AddAllTypesOf<IScrapeProduct>();
                    //scan.AddAllTypesOf<IValidator<Address>>();

                    //scan.ConnectImplementationsToTypesClosing(typeof(IScrapeProduct));

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
