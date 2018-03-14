namespace SmartShop.Model
{
    public class Product
    {
        public string DataURL { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public Vendor PrimaryOffer { get; set; }
        public string PriceSeller { get; set; }
        public string Details { get; set; }
    }
}
