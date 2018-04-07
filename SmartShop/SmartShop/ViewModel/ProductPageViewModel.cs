using SmartShop.Model;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;

namespace SmartShop.ViewModel
{
    class ProductPageViewModel
    {
        public ProductPageViewModel(Product product, bool isEnabled)
        {
            Product = product;
            IsEnabled = isEnabled;
            BackCommand = new Command(HandleBack);
            ShopCommand = new Command(HandleShop);
            SaveCommand = new Command(HandleSave);
        }

        public Product Product { get; set; }

        public bool IsEnabled { get; set; }

        private string _saveText;

        public string SaveText
        {
            get
            {
                return IsEnabled ? "Save To List" : "Saved";
            }
            set
            {
                _saveText = value;
            }
        }

        public ICommand BackCommand { get; private set; }

        private async void HandleBack()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        public ICommand ShopCommand { get; private set; }

        private void HandleShop()
        {
            Device.OpenUri(new Uri(Product.Link, UriKind.Absolute));
        }

        public ICommand SaveCommand { get; private set; }

        private void HandleSave()
        {
            App.Database.SaveProductAsync(Product);

            Application.Current.MainPage.DisplayAlert("", "Product saved!", "OK");
        }

        public CameraUpdate InitialCameraUpdate { get; set; } =
            CameraUpdateFactory.NewPositionZoom(new Position(App.Position.Latitude, App.Position.Longitude), 10d);
    }
}
