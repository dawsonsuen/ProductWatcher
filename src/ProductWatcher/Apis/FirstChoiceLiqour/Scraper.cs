using ProductWatcher.Apis.Shared.ColesLiqourGroup;

namespace ProductWatcher.Apis.FirstChoiceLiqour
{

    public class Scraper : BaseScraper
    {
        public override string CompanyName => "FirstChoiceLiqour";
        public override string SearchUrl => "https://www.firstchoiceliquor.com.au/Components/Products/Services/SearchAutoCompleteService.ashx?query={0}";
    }

}