using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SmartShop.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScannerPage : ContentPage
	{
        public ScannerPage ()
		{
            InitializeComponent ();

            if (Device.RuntimePlatform == Device.Android)
            {
                BackgroundColor = Color.DodgerBlue;
                button.TextColor = Color.White;
                label.TextColor = Color.White;
            }
        }
    }
}