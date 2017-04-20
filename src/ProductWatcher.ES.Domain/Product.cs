using NEvilES;

namespace ProductWatcher.ES.Domain
{
    public abstract class Product
    {

        public class ProductAdded : Event
        {
            public ProductAdded(string productCode, Company company)
            {
                ProductCode = productCode;
                Company = company;
            }

            public string ProductCode { get; set; }
            public Company Company { get; set; }
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