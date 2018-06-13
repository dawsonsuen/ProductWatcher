using System.Threading.Tasks;
using ProductWatcher.Models;

namespace ProductWatcher.Apis
{
    public interface IScrapeProduct
    {
        bool Alcohol { get; }
        string CompanyName { get; }
        Task<string> SearchAsync(string searchTerm);
        Task<string> SearchAsync(string searchTerm, string storeData);
        Task<Search[]> GetSearchModelAsync(string rawData);
        Task<string> GetAsync(string productCode);
        Task<Models.Product> GetProductAsync(string rawData);
    }
}