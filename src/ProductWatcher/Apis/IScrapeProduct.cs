using System.Threading.Tasks;
using ProductWatcher.Models;

namespace ProductWatcher.Apis
{
    public interface IScrapeProduct
    {
        bool Alcohol { get; }
        string CompanyName { get; }
        Task<string> Search(string searchTerm);
        Task<string> Search(string searchTerm, string storeData);
        Task<Search[]> GetSearchModel(string rawData);
        Task<string> Get(string productCode);
        Task<Models.Product> GetProduct(string rawData);
    }
}