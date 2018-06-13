using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ProductWatcher.Apis;

namespace Aws.ServerlessApi.Controllers
{

    [Route("api/[controller]")]
    public class SearchController : Controller
    {
        private readonly IEnumerable<ProductWatcher.Apis.IScrapeProduct> scrapers;

        public SearchController(IEnumerable<ProductWatcher.Apis.IScrapeProduct> scrapers)
        {
            this.scrapers = scrapers;
        }

        [HttpGet("{search}")]
        [ResponseCache(VaryByQueryKeys = new []{"search"}, Duration = 86400)]
        public async Task<IActionResult> Get(string search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return NotFound();
            }

            List<ProductWatcher.Models.Search> searchResults = new List<ProductWatcher.Models.Search>();


            Parallel.ForEach(scrapers, (scraper) =>
            {
                bool notFailed = true;
                try
                {
                    var results = TryGetStuff(scraper, search).Result;
                    lock (searchResults)
                    {
                        searchResults.AddRange(results);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    try
                    {
                        var results = TryGetStuff(scraper, search).Result;

                        lock (searchResults)
                        {
                            searchResults.AddRange(results);
                        }
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"couldnt save {scraper.CompanyName}");
                    }
                    notFailed = false;
                }
                finally
                {
                    if (notFailed) Console.WriteLine($"saved {scraper.CompanyName}");
                }
            });

            return Json(searchResults);
        }

        private static async Task<ProductWatcher.Models.Search[]> TryGetStuff(IScrapeProduct scraper, string search)
        {
            var a = await scraper.SearchAsync(search);
            var b = await scraper.GetSearchModelAsync(a);

            return b;
        }
    }
}