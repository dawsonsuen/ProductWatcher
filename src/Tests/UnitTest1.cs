using System;
using Xunit;

namespace Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //coles search
                        var search = "coke";
            var headDic = new Dictionary<string, string>
            {
                { "X-Coles-API-Key" , "046bc0d4-3854-481f-80dc-85f9e846503d"},
                { "X-Coles-API-Secret", "e6ab96ff-453b-45ba-a2be-ae8d7c12cadf"}
            };

            var url = $"https://api.coles.com.au/customer/v1/coles/products/search?limit=20&q={search}&start=0&storeId=560&type=SKU";
        }

        [Fact]
        public void test2()
        {
            //woolworths search
            var url ="https://www.woolworths.com.au/apis/ui/Search/products?IsSpecial=false&PageNumber=1&PageSize=36&SearchTerm=coke&SortType=Personalised";
        }
    }
}
