namespace ProductWatcher.Tests
{
    public class TestEndpoints
    {

        public const string LiquorLandSearchUrl = "https://www.liquorland.com.au/Components/Products/Services/SearchAutoCompleteService.ashx?query={0}";

        //Normally cheaper then liquorland
        public const string FirstChoiceLiquorSerachUrl = "https://www.firstchoiceliquor.com.au/Components/Products/Services/SearchAutoCompleteService.ashx?query={0}";

        public const string DanMurphysSearchUrl = "https://www.danmurphys.com.au/predictive?_=1492137999223&search={0}&scope=//catalog01/en_AU";

        public const string BWSSearchUrl = "https://api.bws.com.au/apis/ui/Search/Suggestion?Key={0}";


        public const string BWSProductUrl = "https://api.bws.com.au/apis/ui/Product/{productCode}";

        public const string BoozeBudSearchUrl = "https://www.boozebud.com/a/products?asc=false&o=RN&p=1&q=four&q=pines&s=16&wn=false";
    }
}