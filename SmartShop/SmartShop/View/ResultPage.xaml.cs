using SmartShop.Model;
using SmartShop.ViewModel;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResultPage : ContentPage
	{
		public ResultPage (IList<Product> products)
		{
			InitializeComponent ();
            BindingContext = new ResultPageViewModel(products);
		}
	}
}