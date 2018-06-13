using System;
using NPoco;

namespace ProductWatcher.DbModels
{
    [TableNameAttribute("product"), PrimaryKeyAttribute("_id")]
    public class Product
    {
        [ColumnAttribute("_id")]
        public int _id { get; set; }

        [ColumnAttribute("id")]
        public Guid Id { get; set; }

        [ColumnAttribute("code")]
        public string Code { get; set; }

        [ColumnAttribute("name")]
        public string Name { get; set; }

        [ColumnAttribute("company")]
        public string Company { get; set; }
    }
}