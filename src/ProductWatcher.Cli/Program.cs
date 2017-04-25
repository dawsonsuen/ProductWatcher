using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
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

            while(!shouldExit)
            {

               // System.Threading.Tasks.Task.
            }
        }

        public static void ConfigureServices()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance<IConfigurationRoot>(Configuration);
            builder.RegisterInstance(new CommandContext.User(Guid.NewGuid())).Named<CommandContext.IUser>("user");
            builder.RegisterModule(new EventStoreDatabaseModule(Configuration.GetConnectionString("postgres")));
            builder.RegisterModule(new EventProcessorModule(typeof(ES.Domain.Product).GetTypeInfo().Assembly, typeof(ES.ReadModel.Product).GetTypeInfo().Assembly));
            builder.RegisterType<SqlReadModel>().AsImplementedInterfaces();
            builder.RegisterType<IScrapeProduct>().SingleInstance().AsImplementedInterfaces();

            Container = builder.Build();
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
