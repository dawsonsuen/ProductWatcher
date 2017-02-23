using NPoco;

namespace ProductWatcher.DbModels
{
    public class Product
    {
        public int Id { get; set; }

        [ColumnAttribute("friendly_name")]
        public string FriendlyName { get; set; }

        [ColumnAttribute("end_point_url")]
        public string EndPointUrl { get; set; }
        public string Company { get; set; }
    }
}