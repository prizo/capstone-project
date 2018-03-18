using SmartShop.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartShop.ViewModel
{
    class ListPageViewModel : INotifyPropertyChanged
    {
        public ListPageViewModel()
        {
            PopulateProducts();
            RefreshCommand = new Command(HandleRefresh);
        }

        private bool _isRefreshing = false;
        
        public bool IsRefreshing
        {
            get
            {
                return _isRefreshing;
            }
            set
            {
                _isRefreshing = value;
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
            new ObservableCollection<string>(new List<string> { "Name", "Price", "Seller" });

        private async void PopulateProducts()
        {
            List<Product> products = await App.Database.GetProductsAsync();

            if (products != null && products.Count > 0)
            {
                Products = new ObservableCollection<Product>(products);
            }
        }

        public ICommand RefreshCommand { get; private set; }

        private void HandleRefresh()
        {
            IsRefreshing = true;

            PopulateProducts();

            IsRefreshing = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
