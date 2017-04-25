using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using ProductWatcher.Apis.Coles;
using Xunit;
using Xunit.Abstractions;

namespace ProductWatcher.Tests
{
    public class WoolworthsScraper : BaseScraperTestFixture<Apis.Woolworths.Scraper>
    {
        private readonly ITestOutputHelper _output;

        public WoolworthsScraper(TestContext context) : base(context)
        {
        }

        //[Fact]
        public void doa()
        {
            //var a = ApiConstants.Woolworths.SearchProducts("coke");
            //_output.WriteLine($"elllo{a}" );

        }

        [Fact]
        public override async Task Integreation_Get_Product_Price_Should_WorkAsync()
        {
            var data = await _scraper.Get("42605");
            var product = await _scraper.GetProduct(data);

            Assert.NotNull(product);
        }

        [Fact]
        public override async Task Integreation_Search_Should_WorkAsync()
        {
            var data = await _scraper.Search("candles");
            var product = await _scraper.GetProduct(data);

            Assert.NotNull(product);
        }

        //[Fact]
        public override async Task Serialize_As_External_Domain_Model()
        {
            //var data = await _scraper.GetProduct()
        }

        public override Task Serialize_As_Internal_Domain_Model()
        {
            throw new NotImplementedException();
        }
    }
}
