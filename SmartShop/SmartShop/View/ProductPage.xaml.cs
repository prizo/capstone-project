using SmartShop.Model;
using SmartShop.ViewModel;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductPage : ContentPage
	{
        List<string> sellers = new List<string>()
        {
            "Ace Hardware", "Apple Store", "Best Buy", "Boost Mobile", "Dollar General",
            "DICK'S Sporting Goods", "GameStop", "GNC", "JCPenney", "Kmart", "Kohl's", "Lowe's",
            "Macy's", "Nordstrom", "Office Depot", "OfficeMax", "Sam's Club", "Sears", "Sprint",
            "Staples", "Target", "T-Mobile", "Ulta", "Verizon Wireless", "Walgreens", "Walmart",
            "Wal-Mart.com"
        };

        public ProductPage (Product product, bool isEnabled)
		{
			InitializeComponent ();
            BindingContext = new ProductPageViewModel(product, isEnabled);

            if (!sellers.Contains(product.Seller))
            {
                map.IsEnabled = false;
                map.IsVisible = false;
            }
            else
            {
                // Add a pin to the map
            }
        }
    }
}