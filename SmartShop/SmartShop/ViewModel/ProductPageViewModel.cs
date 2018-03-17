using SmartShop.Model;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartShop.ViewModel
{
    class ProductPageViewModel
    {
        public Product Product { get; set; }

        public ProductPageViewModel(Product product)
        {
            Product = product;
            ShopCommand = new Command(HandleShop);
            SaveCommand = new Command(HandleSave);
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

    }
}
