using System.Globalization;
using System.Threading.Tasks;
using Autofac;
using ProductWatcher.Apis;
using Xunit;

namespace ProductWatcher.Tests
{
    public abstract class BaseScraperTestFixture<T> : IClassFixture<TestContext> where T : IScrapeProduct
    {
        protected readonly TestContext _context;
        protected readonly IScrapeProduct _scraper;

        public BaseScraperTestFixture(TestContext context)
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-Au");
            CultureInfo.CurrentUICulture = CultureInfo.CurrentCulture;
            _context = context;
            _scraper = context.Container.Resolve<T>();
        }

        public abstract Task Serialize_As_External_Domain_Model();
        public abstract Task Serialize_As_Internal_Domain_Model();

        //API Intergration tests
        public abstract Task Integreation_Get_Product_Price_Should_WorkAsync();
        public abstract Task Integreation_Search_Should_WorkAsync();
    }
}