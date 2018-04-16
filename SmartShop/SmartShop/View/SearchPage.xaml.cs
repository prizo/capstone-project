using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchPage : ContentPage
	{
		public SearchPage ()
		{
			InitializeComponent ();

            if (Device.RuntimePlatform == Device.Android)
            {
                outerLayout.Padding = 10;
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                innerLayout.Padding = 10;
            }
		}
	}
}