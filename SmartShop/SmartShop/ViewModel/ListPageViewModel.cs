using SmartShop.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SmartShop.ViewModel
{
    class ListPageViewModel : INotifyPropertyChanged
    {
        public ListPageViewModel()
        {
            PopulateProducts();
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
        
        private async void PopulateProducts()
        {
            List<Product> products = await App.Database.GetProductsAsync();

            if (products != null && products.Count > 0)
            {
                Products = new ObservableCollection<Product>(products);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
