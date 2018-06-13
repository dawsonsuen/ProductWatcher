using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.DependencyInjection;
using ProductWatcher.Apis;

namespace ProductWatcher.Web.Pages
{
    public class IndexModel : PageModel
    {
        //private readonly IServiceCollection services;
        private readonly IScrapeProduct[] scrapers;
        public IndexModel(IScrapeProduct[] scrapers)
        {
            this.scrapers = scrapers;


        }

        public void OnGet()
        {

            var a = HttpContext.RequestServices.GetService(typeof(IScrapeProduct));
            var b = "  ";
        }

        [BindProperty(SupportsGet = true)]
        public InputModel Input { get; set; }

        public class ViewModel
        {
            //public List<Products>
        }

        public class InputModel
        {
            public string Search { get; set; }
        }
    }
}
