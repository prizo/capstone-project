using SQLite;

namespace SmartShop.Model
{
    public class Product
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string DataURL { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Price { get; set; }
        public string Seller { get; set; }
        public string PriceSeller { get; set; }
        public string Link { get; set; }
        public string Details { get; set; }
    }
}
