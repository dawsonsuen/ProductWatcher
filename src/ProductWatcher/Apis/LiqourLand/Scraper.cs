using ProductWatcher.Apis.Shared.ColesLiqourGroup;

namespace ProductWatcher.Apis.LiqourLand
{
    public class Scraper : BaseScraper
    {
        public override string CompanyName => "LiqourLand";
        public override string SearchUrl => "https://www.liquorland.com.au/Components/Products/Services/SearchAutoCompleteService.ashx?query={0}";
    }
}