using System.Threading.Tasks;
using ProductWatcher.Models;

namespace ProductWatcher.Apis
{
    public interface IScrapeProduct
    {
        string CompanyName { get; }
        Task<string> Search(string searchTerm);
        Task<string> SearchAsync(string searchTerm, string storeData);
        Task<string> Get(string productCode);
        Task<Product> GetProduct(string rawData);
    }
}