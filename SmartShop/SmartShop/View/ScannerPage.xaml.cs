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

            BackgroundColor = Color.Default;
        }
    }
}