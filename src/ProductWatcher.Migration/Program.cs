using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using NPoco;

namespace ProductWatcher.Migration
{
    class Program
    {
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
        static void Main(string[] args)
        {
            using (var sql = new Database("Server={server};User Id=sa;Password={password}$;Database={database}", DatabaseType.SqlServer2008, System.Data.SqlClient.SqlClientFactory.Instance))
            using (var pg = new Database("Server={server};Port=5432;Database={database};User Id=postgres;Password={password};", DatabaseType.PostgreSQL, Npgsql.NpgsqlFactory.Instance))
            {
                //using(var tran = pg.GetTransaction())
                using (var tran = sql.GetTransaction())
                {
                    tryMigrate(sql);
                    //getDataFromPostgres(pg, sql);
                    //strans.Complete();
                    //insertMigratedData(sql, pg);


                    tran.Complete();
                }
            }

        }

        public static void insertMigratedData(Database sql, Database pg)
        {
            var data = sql.Query<DbModels.Data>().ToList();
            var products = sql.Query<DbModels.Product>().ToList();

            Parallel.ForEach(data, x =>
            {
                var productType = products.Where(p => p.Id == x.ProductId).FirstOrDefault();
                var productModel = JsonConvert.DeserializeObject<Models.Product>(x.ProductModel);

                var newData = new DbModels.Data()
                {
                    ProductId = x.ProductId,
                    ProductModel = x.ProductModel,
                    RawData = x.RawData,
                    When = x.When
                };

                LockInsert(pg, newData);


                var price = new DbModels.Price
                {
                    DataId = newData.Id,
                    ProductId = productType.Id,
                    OriginalPrice = productModel.Price,
                    OnSalePrice = productModel.SpecialPrice,
                    Company = productModel.Company,
                    Description = productModel.Description.Trim(),
                    AdditionalData = new Dictionary<string, object>(){
                                 {
                                     "$/L", productModel.DollarPerLitre
                                 },
                                 {
                                     "imgUrl",
                                     productModel.ImgUrl
                                 },

                    },
                    When = newData.When
                };
                LockInsert(pg, price);

            });
        }

        public static void getDataFromPostgres(Database pg, Database sql)
        {
            var data = pg.Query<DbModels.Data>().ToList();
            var products = sql.Query<DbModels.Product>().ToList();

            Parallel.ForEach(data, x =>
            {
                var productType = products.Where(p => p.Id == x.ProductId).FirstOrDefault();
                var productModel = JsonConvert.DeserializeObject<Models.Product>(x.ProductModel);

                var newData = new DbModels.Data()
                {
                    ProductId = x.ProductId,
                    ProductModel = x.ProductModel,
                    RawData = x.RawData,
                    When = x.When
                };

                LockInsert(sql, newData);


                var price = new DbModels.Price
                {
                    DataId = newData.Id,
                    ProductId = productType.Id,
                    OriginalPrice = productModel.Price,
                    OnSalePrice = productModel.SpecialPrice,
                    Company = productModel.Company,
                    Description = productModel.Description.Trim(),
                    AdditionalData = new Dictionary<string, object>(){
                                 {
                                     "$/L", productModel.DollarPerLitre
                                 },
                                 {
                                     "imgUrl",
                                     productModel.ImgUrl
                                 },

                    },
                    When = newData.When
                };
                LockInsert(sql, price);

            });




        }


        public static void tryMigrate(Database sql)
        {
            var items = sql.Query<OldPrice>().ToList();
            var products = sql.Query<DbModels.Product>().ToList();

            Parallel.ForEach(items, x =>
            {
                try
                {
                    Models.Product product = null;
                    var data = new DbModels.Data();

                    data.When = x.When.AddHours(-11);

                    if (ApiConstants.Coles.Is(x.Company))
                    {
                        data.RawData = x.OriginalData;

                        product = ApiConstants.Coles.GetProductFromJson(data.RawData);
                    }
                    else if (ApiConstants.Woolworths.Is(x.Company))
                    {
                        data.RawData = x.OriginalData;

                        product = ApiConstants.Woolworths.GetProductFromJson(data.RawData);
                    }
                    data.ProductModel = JsonConvert.SerializeObject(product);
                    var productType = products.Where(p => p.Code == product.Id).FirstOrDefault();
                    data.ProductId = productType.Id;
                    LockInsert(sql, data);
                    LockUpdate(sql, data);


                    var price = new DbModels.Price
                    {
                        DataId = data.Id,
                        ProductId = productType.Id,
                        OriginalPrice = product.Price,
                        OnSalePrice = product.SpecialPrice,
                        Company = product.Company,
                        Description = product.Description.Trim(),
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

                    LockInsert(sql, price);



                }
                catch (Exception ex)
                {
                    Console.WriteLine(x.Id);
                }
            });
        }
    }


    [TableNameAttribute("price_old")]
    public class OldPrice
    {
        [ColumnAttribute("id")]
        public int Id { get; set; }

        [ColumnAttribute("original_data")]
        public string OriginalData { get; set; }

        [ColumnAttribute("original_price")]
        public decimal OriginalPrice { get; set; }

        [ColumnAttribute("on_sale_price")]
        public decimal? OnSalePrice { get; set; }

        [ColumnAttribute("company")]
        public string Company { get; set; }

        [ColumnAttribute("description")]
        public string Description { get; set; }

        [ColumnAttribute("when")]
        public DateTime When { get; set; }

    }


}

