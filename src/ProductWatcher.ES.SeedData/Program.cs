using System;
using System.Collections.Concurrent;
using System.Reflection;
using NEvilES;
using NEvilES.Pipeline;
using ProductWatcher.ES.Domain;
using Autofac;
using ProductWatcher.ES.Common;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data;
using NEvilES.DataStore;
using Npgsql;
using System.Collections.Generic;

namespace ProductWatcher.ES.SeedData
{
    public static class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        public static void Main(string[] args)
        {
            SetupConfig();

            Console.WriteLine("Seed data.......");
            var builder = new ContainerBuilder();
            var connectionString = Configuration.GetConnectionString("pgsql");

            builder.RegisterInstance(new CommandContext.User(Guid.NewGuid())).Named<CommandContext.IUser>("user");
            //builder.RegisterType<ReadModel.SqlReadModel>().AsImplementedInterfaces();
            builder.RegisterInstance<IEventTypeLookupStrategy>(new EventTypeLookupStrategy());
            builder.RegisterModule(new EventStoreDatabaseModule(connectionString));
            builder.RegisterModule(new EventProcessorModule(typeof(ES.Domain.Product).GetTypeInfo().Assembly, typeof(ES.ReadModel.Product).GetTypeInfo().Assembly));

            var container = builder.Build();
            container.Resolve<IEventTypeLookupStrategy>().ScanAssemblyOfType(typeof(ES.Domain.Product));

            HandleDatabaseDropCreate(connectionString,"product_watcher_test");

            SeedData(container);

            using (var scope = container.BeginLifetimeScope())
            {
                ReplayEvents.Replay(container.Resolve<IFactory>(), scope.Resolve<IAccessDataStore>());
            }
            //var reader = (ReadModel.SqlReadModel)container.Resolve<IReadFromReadModel>();

            //var x = reader.Get<ReadModel.User>(Guid.Empty);

            //Console.WriteLine("Read Model Document Count {0}", reader.Count());
            Console.WriteLine("Done - Hit any key!");
            Console.ReadKey();
        }

        private static void SeedData(IContainer container)
        {
            var id = CombGuid.NewGuid();
            var craigsId = CombGuid.NewGuid();
            var elijahsId = CombGuid.NewGuid();

            using (var scope = container.BeginLifetimeScope())
            {
                scope.Resolve<PipelineTransaction>();
                var processor = scope.Resolve<ICommandProcessor>();

                // var craig = new User.NewUser
                // {
                //     StreamId = craigsId,
                //     Details = new User.Details("craig@test.com", "xxx", "Craig", "Gardiner")
                // };
                // processor.Process(craig);

                //var elijah = new User.NewUser
                //{
                //    StreamId = elijahsId,
                //    Details = new User.Details("elijah@test.com", "xxx", "Elijah", "Bate")
                //};
                //processor.Process(elijah);

            }

            //Events as craig as the context user
            var debtId = CombGuid.NewGuid();
            using (var scope = container.BeginLifetimeScope(x =>
                {
                    x.RegisterInstance(new CommandContext.User(craigsId)).Named<CommandContext.IUser>("user");
                })
            )
            {
                scope.Resolve<PipelineTransaction>();
                var processor = scope.Resolve<ICommandProcessor>();
                //var ourBill = new Bill.Create(id, "voodoo magic bitch", "Sunday arvo fun ;)", 20.35m);

                //processor.Process(ourBill);
                //var youOweMe = new Debt.YouOweMe(debtId, craigsId, elijahsId, ourBill.StreamId, 0.1725m);
                //processor.Process(youOweMe);
            }

            //Events as elijah as the context user
            using (var scope = container.BeginLifetimeScope(x =>
            {
                x.RegisterInstance(new CommandContext.User(elijahsId)).Named<CommandContext.IUser>("user");
            }))
            {
                scope.Resolve<PipelineTransaction>();
                var processor = scope.Resolve<ICommandProcessor>();

                //var elijahComment = new Bill.AddComment(id, CombGuid.NewGuid(), "wooohoo thinkgs and stuuff");
                //processor.Process(elijahComment);
                //processor.Process(new Bill.EditComment(id, elijahComment.CommentId, "wooohoo things and stuff"));
                //processor.Process(new Debt.Accept { StreamId = debtId });

            }
        }

        public static void SetupConfig()
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
               .AddJsonFile($"appsettings.Development.json", optional: true, reloadOnChange: false);

            Configuration = builder.Build();
        }

        public static void HandleDatabaseDropCreate(string connStr, string database)
        {
            var createSql = string.Empty;

            IDbConnection conn;
            var drop = $"drop datatbase {database}";

            conn = new NpgsqlConnection(connStr);
            createSql = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "init.pgsql.sql"));

            conn.Open();

            using (var cmd = conn.CreateCommand())
            {
                try
                {
                    cmd.CommandText = drop;
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                }
                cmd.CommandText = $"Create database {database}";
                cmd.ExecuteNonQuery();
            }

            conn.ChangeDatabase(database);

            using (var t = conn.BeginTransaction())
            {
                var cmd = conn.CreateCommand();
                cmd.Transaction = t;
                cmd.CommandText = createSql;

                cmd.ExecuteNonQuery();
                t.Commit();
            }
        }
    }
    public class InMemoryReadModel : IReadFromReadModel, IWriteReadModel
    {
        private readonly ConcurrentDictionary<Guid, object> data;

        public InMemoryReadModel()
        {
            data = new ConcurrentDictionary<Guid, object>();
        }

        public void Insert<T>(T item) where T : class, IHaveIdentity
        {
            data.TryAdd(item.Id, item);
        }

        public void Update<T>(T item) where T : class, IHaveIdentity
        {
            data[item.Id] = item;
        }

        public T Get<T>(Guid id) where T : IHaveIdentity
        {
            return (T)data[id];
        }

        public void Clear()
        {
            data.Clear();
        }

        public int Count()
        {
            return data.Count;
        }

        public IEnumerable<T> Query<T>(Func<T, bool> p)
        {
            throw new NotImplementedException();
        }
    }

}