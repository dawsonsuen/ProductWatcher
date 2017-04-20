using System;

namespace ProductWatcher.ES.ReadModel
{
    public class Product
    {
        public Guid StreamId { get; set; }
        public decimal CurrentPrice { get; set; }
        public string ProductThumbnailLink { get; set; }
        public string ProductImageLink { get; set; }
    }
}