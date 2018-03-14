using SmartShop.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace SmartShop.ViewModel
{
    class ProductPageViewModel
    {
        public Product DetailedProduct { get; set; }

        public ProductPageViewModel(Product product)
        {
            DetailedProduct = product;
        }
    }
}
