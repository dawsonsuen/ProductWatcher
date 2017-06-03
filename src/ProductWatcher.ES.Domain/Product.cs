using System;
using NEvilES;

namespace ProductWatcher.ES.Domain
{
    public abstract class Product
    {

        public class ProductSearched : ICommand
        {
            public ProductSearched(Guid streamId, string searchTerm)
            {
                this.StreamId = streamId;
                this.SearchTerm = searchTerm;

            }

            public Guid StreamId { get; set; }
            public string SearchTerm { get; set; }
        }



        public class ProductAdded : ICommand
        {
            public ProductAdded(Guid streamId, string productCode, Company company)
            {
                ProductCode = productCode;
                Company = company;
                StreamId = streamId;
            }
            public string ProductCode { get; set; }
            public Company Company { get; set; }
            public Guid StreamId { get; set; }
        }


        public enum Company
        {
            Woolworths,
            Coles,
            IGA,
            Aldi,
            Dan
        }
    }
}