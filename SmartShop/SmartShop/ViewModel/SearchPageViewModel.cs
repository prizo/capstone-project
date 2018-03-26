using SmartShop.Model;
using SmartShop.Utilities;
using SmartShop.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartShop.ViewModel
{
    class SearchPageViewModel : INotifyPropertyChanged
    {
        public SearchPageViewModel()
        {
            SearchCommand = new Command<string>(HandleSearch);
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

        public ObservableCollection<string> SortOptions { get; set; } = 
            new ObservableCollection<string>(new List<string> { "Name", "Price: Low to High", "Price: High to Low", "Seller" });

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

        public ICommand SearchCommand { get; private set; }

        private void HandleSearch(string text)
        {
            string document = "";
            IList<Product> products = null;

            if (text != null && text.Trim() != "")
            {
                string query = Uri.EscapeDataString(text.Trim());
                document = BingWebRequest.SendRequest("/shop?q=" + query);
            }

            if (document != null && document != "")
            {
                products = new ProductExtractor().ExtractProducts(document);
            }

            if (products != null && products.Count > 0) // TODO: Display 'not found' alert if products is empty
            {
                Products = new ObservableCollection<Product>(products);
                SelectedOption = null;
            }
        }

        public ICommand ItemSelectedCommand { get; private set; }

        private async void HandleItemSelected(Product product)
        {
            if (SelectedItem != null)
            {
                SelectedItem = null;

                string document = "";

                if (product.DataURL != null && product.DataURL != "")
                {
                    document = BingWebRequest.SendRequest(product.DataURL);
                }

                if (document != null && document != "")
                {
                    new ProductExtractor().ExtractDetails(product, document);
                }

                await Application.Current.MainPage.Navigation.
                    PushModalAsync(new NavigationPage(new ProductPage(product, true)));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
