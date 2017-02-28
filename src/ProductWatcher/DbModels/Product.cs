using NPoco;

namespace ProductWatcher.DbModels
{
    public class Product
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Company { get; set; }
    }
}