using Plugin.Connectivity;
using SmartShop.Model;
using SmartShop.Utilities;
using SmartShop.View;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartShop.ViewModel
{
    class ResultPageViewModel : INotifyPropertyChanged
    {
        public ResultPageViewModel(IList<Product> products)
        {
            Products = new ObservableCollection<Product>(products);
            BackCommand = new Command(HandleBack);
            ItemSelectedCommand = new Command<Product>(HandleItemSelected);
        }

        private Product _selectedItem;

        public Product SelectedItem
        {
            get
            {
                return _selectedItem;
            }
            set
            {
                _selectedItem = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<Product> _products;

        public ObservableCollection<Product> Products
        {
            get
            {
                return _products ?? (_products = new ObservableCollection<Product>());
            }
            set
            {
                _products = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> SortOptions { get; set; } = new ObservableCollection<string>(Utilities.SortOptions.options);

        private string _selectedOption;

        public string SelectedOption
        {
            get
            {
                return _selectedOption;
            }
            set
            {
                _selectedOption = value;
                if (value != null)
                {
                    Products = ProductSorter.Sort(value, new List<Product>(Products));
                }
                OnPropertyChanged();
            }
        }

        public ICommand BackCommand { get; private set; }

        private async void HandleBack()
        {
            await Application.Current.MainPage.Navigation.PopModalAsync();
        }

        public ICommand ItemSelectedCommand { get; private set; }

        private async void HandleItemSelected(Product product)
        {
            // Check for internet connection
            if (!CrossConnectivity.Current.IsConnected)
            {
                await Application.Current.MainPage.DisplayAlert("", "No internet connection", "OK");
                return;
            }

            if (SelectedItem != null)
            {
                SelectedItem = null;

                // Bing product needs to have link extracted
                if (product.Link == "")
                {
                    string document = "";

                    if (product.DataURL != null && product.DataURL != "")
                    {
                        document = BingWebRequest.SendRequest(product.DataURL);
                    }

                    if (document != null && document != "")
                    {
                        new ProductExtractor().ExtractDetails(product, document);
                    }
                }

                await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ProductPage(product, true)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
