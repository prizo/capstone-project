using SmartShop.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SmartShop.Utilities
{
    class ProductSorter
    {
        public static ObservableCollection<Product> Sort(string option, List<Product> products)
        {
            IOrderedEnumerable<Product> sortedList = null;

            if (option == "Name")
            {
                sortedList = products.OrderBy(x => x.Name);
            }
            else if (option == "Price: Low to High")
            {
                sortedList = products.OrderBy(x => x.Price);
            }
            else if (option == "Price: High to Low")
            {
                sortedList = products.OrderByDescending(x => x.Price);
            }
            else if (option == "Seller")
            {
                sortedList = products.OrderBy(x => x.Seller);
            }

            if (sortedList == null)
            {
                return new ObservableCollection<Product>(products);
            }
            else
            {
                return new ObservableCollection<Product>(sortedList);
            }
        }
    }
}
