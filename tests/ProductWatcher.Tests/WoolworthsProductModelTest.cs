using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace ProductWatcher.Tests
{
    public class WoolworthsProductModelTest
    {
        private readonly ITestOutputHelper _output;

        public WoolworthsProductModelTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void doa()
        {
            //var a = ApiConstants.Woolworths.SearchProducts("coke");
            //_output.WriteLine($"elllo{a}" );

        }
    }
}
