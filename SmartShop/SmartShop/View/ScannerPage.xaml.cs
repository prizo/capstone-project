using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace SmartShop.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ScannerPage : ContentPage
	{
        ZXingScannerView zxing;
        ZXingDefaultOverlay overlay;

        public ScannerPage () : base ()
		{
            Icon = "ic_crop_free_white_24dp.png";
            Title = "Scanner";

			zxing = new ZXingScannerView
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                AutomationId = "zxingScannerView"
            };
            zxing.OnScanResult += (result) =>
                Device.BeginInvokeOnMainThread(async () =>
                {
                    // Stop analysis until we navigate away so we don't keep reading barcodes
                    zxing.IsAnalyzing = false;

                    // Show an alert
                    await DisplayAlert("Scanned Barcode", result.Text, "OK");

                    // Navigate away
                    await Navigation.PushModalAsync(new NotFoundPage());
                });

            overlay = new ZXingDefaultOverlay
            {
                TopText = "Hold your phone up to the barcode",
                BottomText = "Scanning will happen automatically",
                ShowFlashButton = zxing.HasTorch,
                AutomationId = "zxingDefaultOverlay"
            };
            overlay.FlashButtonClicked += (sender, e) =>
            {
                zxing.IsTorchOn = !zxing.IsTorchOn;
            };

            var grid = new Grid
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };
            grid.Children.Add(zxing);
            grid.Children.Add(overlay);

            // The root page of the application
            Content = grid;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();

            zxing.IsScanning = true;
        }

        protected override void OnDisappearing()
        {
            //zxing.IsScanning = false;

            base.OnDisappearing();
        }
    }
}