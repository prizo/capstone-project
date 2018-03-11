using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShop.Model
{
    class Product
    {
        public string DataUrl { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Offer { get; set; }
        public List<Vendor> OtherOptions { get; set; }
        public string Details { get; set; }
    }
}
