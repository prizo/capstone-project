using SmartShop.Model;
using SmartShop.View;
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

        public ICommand SearchCommand { get; private set; }

        private void HandleSearch(string query)
        {
            string document = "";
            IList<Product> products = null;

            if (query != null && query.Trim() != "")
            {
                document = new BingWebRequest().SendRequest(query.Trim());
            }

            if (document != null && document != "")
            {
                products = new ProductExtractor().ExtractData(document);
            }

            if (products != null && products.Count > 0)
            {
                Products = new ObservableCollection<Product>(products);
            }
        }

        public ICommand ItemSelectedCommand { get; private set; }

        private void HandleItemSelected(Product product)
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new VendorPage());
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
