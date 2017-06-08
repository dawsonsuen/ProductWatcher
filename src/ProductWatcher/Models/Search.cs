namespace ProductWatcher.Models
{
    public class Search
    {
        public string ProductCode { get; set; }
        public string Company { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long Quanity { get; set; }
        public decimal Amount { get; set; }
        //public decimal SavingAmount { get; set; }
        public string ImageUrl { get; set; }
        public string Brand { get; internal set; }
        public string CupSting { get; internal set; }
    }
}