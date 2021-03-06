using System;
using NPoco;

namespace ProductWatcher.DbModels
{
    [TableNameAttribute("data"), PrimaryKeyAttribute("id")]
    public class Data
    {
        [ColumnAttribute("id")]
        public int Id { get; set; }

        [ColumnAttribute("product_id")]
        public Guid ProductId { get; set; }

        [ColumnAttribute("product_model")]
        public string ProductModel { get; set; }

        [ColumnAttribute("raw_data")]
        public string RawData { get; set; }

        [ColumnAttribute("_when")]
        public DateTime When { get; set; }
    }
}