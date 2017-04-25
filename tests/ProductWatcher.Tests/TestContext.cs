using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Options;
using System.Reflection;
using NPoco;
using Autofac;
using NEvilES.Pipeline;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using System.Globalization;
using System.Threading;
using System.Linq;
using ProductWatcher.Apis;

namespace ProductWatcher.Tests
{
    public class TestContext
    {
        public string CurrentPath { get; private set; }
        public TestContext()
        {
            CurrentPath = Path.GetDirectoryName(typeof(TestContext).GetTypeInfo().Assembly.Location);
            // var builder = new ConfigurationBuilder()
            //     .SetBasePath(CurrentPath)
            //     .AddJsonFile("appsettings.json", optional:false)
            //     .AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: false);

            // Configuration = builder.Build();

            var services = new ServiceCollection()
                .AddOptions();

            var builder = new ContainerBuilder();

            //builder.RegisterInstance(new CommandContext.User(Guid.NewGuid())).Named<CommandContext.IUser>("user");
            //builder.RegisterType<ReadModel.SqlReadModel>().AsImplementedInterfaces();
            //builder.RegisterInstance<IEventTypeLookupStrategy>(new EventTypeLookupStrategy());
            //builder.RegisterModule(new EventStoreDatabaseModule(Configuration.GetConnectionString(connStrName), databaseType));
            //builder.RegisterModule(new EventProcessorModule(typeof(Domain.User).GetTypeInfo().Assembly, typeof(ReadModel.User).GetTypeInfo().Assembly));
            var apiTypes = typeof(IScrapeProduct).GetTypeInfo().Assembly
                         .GetTypes()
                         .Where(x => x.Namespace.StartsWith(typeof(IScrapeProduct).Namespace))
                         .ToArray();

            builder.RegisterTypes(apiTypes).SingleInstance();
            builder.Populate(services);

            Container = builder.Build();
        }

        public IContainer Container { get; private set; }

        public IConfigurationRoot Configuration { get; private set; }
    }
}
