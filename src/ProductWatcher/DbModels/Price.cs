using NPoco;
using System;

namespace ProductWatcher
{
    public class Price
    {
        public int Id { get; set; }

        [Column("original_data")]
        public string OriginalData { get; set; }

        [Column("original_price")]
        public decimal OriginalPrice { get; set; }

        [Column("on_sale_price")]
        public decimal? OnSalePrice { get; set; }
        public string Company { get; set; }
        public string Description { get; set; }
        public DateTime When { get; set; }
    }

}