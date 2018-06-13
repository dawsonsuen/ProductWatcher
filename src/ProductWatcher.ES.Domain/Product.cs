using System;
using NEvilES;

namespace ProductWatcher.ES.Domain
{
    public abstract class Product
    {

        public class Added : Event
        {
            public Added(Guid streamId, string productCode, Company company)
            {
                ProductCode = productCode;
                Company = company;
                StreamId = streamId;
            }
            public string ProductCode { get; set; }
            public Company Company { get; set; }
            public Guid StreamId { get; set; }
        }

        public class UpdatedPrice : Event
        {

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