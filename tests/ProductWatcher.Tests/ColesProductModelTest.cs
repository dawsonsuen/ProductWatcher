using Newtonsoft.Json;
using System;
using Xunit;
using Flurl.Http;
using System.Collections.Generic;
using ProductWatcher.Models;

namespace ProductWatcher.Tests
{
    public class ColesProductModelTests
    {
        public const string CokeTestDataOnSpecial = "{\"catalogEntryView\": [{\"a\": {\"A4\": [\"1.0\"],\"B\": [\"9300675047067\", \"9300675045674\", \"9300675012089\", \"10291\"],\"E1\": [\"false\"],\"F3\": [\"S\"],\"H1\": [\"false\"],\"H2\": [\"false\"],\"I1\": [\"true\"],\"L2\": [\"false\"],\"O\": [\"L\"],\"O1\": [\"1.0\"],\"O3\": [\"30 pack\"],\"P\": [\"1.0\"],\"P8\": [\"Drinks\"],\"P9\": [\"6\"],\"S\": [\"11.25\"],\"S5\": [\"30 Pack\"],\"S9\": [\"931859 COCA COLA AMATIL AUST P/L (GHPL)\", \"931858 COCA COLA AMATIL AUST P/L (DIREC\"],\"T\": [\"10.0\"],\"T1\": [\"false\"],\"T2\": [\"Cola\"],\"U\": [\"Ea\"],\"W1\": [\"false\"],\"WEIGHTEDBYEACH\": [\"false\"]},\"fi\": \"/wcsstore/Coles-CAS/images/8/4/6/8464796.jpg\",\"m\": \"Coca-Cola\",\"n\": \"Coke 375mL Cans\",\"p\": \"8464796P\",\"p1\": {\"l4\": \"37.07\",\"o\": \"18.0\"},\"pl\": \"6\",\"s\": \"coca-cola-soft-drink-coke-375ml-cans\",\"s9\": \"38147\",\"t\": \"/wcsstore/Coles-CAS/images/8/4/6/8464796-th.jpg\",\"t1\": \"S\",\"u\": \"11434\",\"u2\": \"$1.60 per 1L\"}],\"m4\": {\"p1\": \"1\"},\"recordSetCount\": 1,\"recordSetStartNumber\": 0,\"recordSetTotal\": 1}";
        public const string CokeTestDataNotOnSpecial = "{\"catalogEntryView\": [{\"a\": {\"A4\": [\"1.0\"],\"B\": [\"10355\", \"9300675055109\", \"9300675056441\", \"400001456354\", \"9300675000628\"],\"E1\": [\"false\"],\"F3\": [\"S\"],\"H1\": [\"false\"],\"H2\": [\"false\"],\"L2\": [\"false\"],\"O\": [\"L\"],\"O1\": [\"1.0\"],\"O3\": [\"24 pack\"],\"P\": [\"1.0\"],\"P8\": [\"Drinks\"],\"P9\": [\"6\"],\"S\": [\"9.0\"],\"S9\": [\"931858 COCA COLA AMATIL AUST P/L (DIREC\", \"931859 COCA COLA AMATIL AUST P/L (GHPL)\"],\"T\": [\"10.0\"],\"T1\": [\"false\"],\"T2\": [\"Cola\"],\"U\": [\"ea\"],\"W1\": [\"false\"],\"WEIGHTEDBYEACH\": [\"false\"]},\"fi\": \"/wcsstore/Coles-CAS/images/7/3/6/7365777.jpg\",\"m\": \"Coca-Cola\",\"n\": \"Coke Can 375mL\",\"p\": \"7365777P\",\"p1\": {\"o\": \"30.25\"},\"pl\": \"99\",\"s\": \"coca-cola-soft-drink-coke-375ml-cans-7365777p\",\"s9\": \"29406\",\"t\": \"/wcsstore/Coles-CAS/images/7/3/6/7365777-th.jpg\",\"u\": \"20143\",\"u2\": \"$3.36 per 1L\"}],\"m4\": {\"p1\": \"1\"},\"recordSetCount\": 1,\"recordSetStartNumber\": 0,\"recordSetTotal\": 1}";

        [Fact]
        public void DoesTheGarbageParseIsShitFuckedWhenIsOnSpecial()
        {
            var a = new Apis.Coles.Scraper();
            var expectedModel = new Product()
            {
                Id = "8464796",
                Company = a.CompanyName,
                Description = "Coke 375mL Cans 30 pack",
                CupPrice = 1.60M,
                ImgUrl = "/wcsstore/Coles-CAS/images/8/4/6/8464796.jpg",
                Name = "Coke 375mL Cans",
                Price = 37.07M,
                //This is for test data as parse to one decimal place if .10 or .0 etc
                SpecialPrice = 18.0M
            };

            //Do this coz deep equalz be ballin up
            var expectedModelStringfiled = JsonConvert.SerializeObject(expectedModel);

            //var parsedProduct = JsonConvert.DeserializeObject<Apis.Coles.Models.ShitAsProductModel>(CokeTestDataOnSpecial);
            var product = a.GetProduct(CokeTestDataOnSpecial);
            var productStringified = JsonConvert.SerializeObject(product);

            Assert.Equal(expectedModelStringfiled,productStringified);
        }

        [Fact]
        public void DoesTheGarbageParseIsShitFuckedWhenIsNotOnSpecial()
        {
             var a = new Apis.Coles.Scraper();
            var expectedModel = new Product()
            {
                Id = "7365777",
                Company = a.CompanyName,
                Description = "Coke Can 375mL 24 pack",
                CupPrice = 3.36M,
                ImgUrl = "/wcsstore/Coles-CAS/images/7/3/6/7365777.jpg",
                Name = "Coke Can 375mL",
                Price = 30.25M
            };

            //Do this coz deep equalz be ballin up
            var expectedModelStringfiled = JsonConvert.SerializeObject(expectedModel);

            //var parsedProduct = JsonConvert.DeserializeObject<Apis.Coles.Models.ShitAsProductModel>(CokeTestDataNotOnSpecial);
            var product = a.GetProduct(CokeTestDataNotOnSpecial);
            var productStringified = JsonConvert.SerializeObject(product);

            Assert.Equal(expectedModelStringfiled, productStringified);
        }

        [Fact]
        public void TestPriceApiEndpointIsCorrect()
        {
            var endpoint = "https://shop.coles.com.au/search/resources/store/20601/productview/bySearchTerm/8464796";

            var result = endpoint.GetJsonAsync().Result;

            Assert.NotNull(result);
        }

        [Fact]
        public void TestSearchApiEndpointIsCorrect()
        {
            //var result = ApiConstants.Coles.SearchProducts("candles");

            //Assert.NotNull(result);
            Assert.True(false);
        }
    }
}
