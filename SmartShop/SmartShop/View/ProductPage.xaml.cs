using Plugin.Geolocator;
using SmartShop.Model;
using SmartShop.ViewModel;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace SmartShop.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductPage : ContentPage
	{
        public ProductPage (Product product, bool isEnabled)
		{
			InitializeComponent ();
            BindingContext = new ProductPageViewModel(product, isEnabled);
        }
    }
}