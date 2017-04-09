using NPoco;
using System;
using System.Collections.Generic;

namespace ProductWatcher.DbModels
{
    [TableNameAttribute("price"), PrimaryKeyAttribute("id")]
    public class Price
    {
        [ColumnAttribute("id")]
        public int Id { get; set; }

        [ColumnAttribute("data_id")]
        public int DataId { get; set; }

        [ColumnAttribute("product_id")]
        public Guid ProductId { get; set; }

        [Column("original_price")]
        public decimal OriginalPrice { get; set; }

        [Column("on_sale_price")]
        public decimal? OnSalePrice { get; set; }

        [ColumnAttribute("company")]
        public string Company { get; set; }

        [ColumnAttribute("description")]
        public string Description { get; set; }

        [ColumnAttribute("_when")]
        public DateTime When { get; set; }

        [ColumnAttribute("additional_data"), SerializedColumnAttribute]
        public Dictionary<string, object> AdditionalData { get; set; }
    }

}