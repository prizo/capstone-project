using SmartShop.Model;
using SmartShop.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductPage : ContentPage
	{
		public ProductPage (Product product)
		{
			InitializeComponent ();
            BindingContext = new ProductPageViewModel(product);
		}
	}
}