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
    class ListPageViewModel : INotifyPropertyChanged
    {
        public ListPageViewModel()
        {
            ItemSelectedCommand = new Command<Product>(HandleItemSelected);
            RefreshCommand = new Command(HandleRefresh);
            DeleteCommand = new Command<Product>(HandleDelete);
            PopulateProducts();
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

        private async void PopulateProducts()
        {
            List<Product> products = await App.Database.GetProductsAsync();

            if (products != null && products.Count > 0)
            {
                Products = new ObservableCollection<Product>(products);
            }
        }

        public ICommand ItemSelectedCommand { get; private set; }

        private async void HandleItemSelected(Product product)
        {
            if (SelectedItem != null)
            {
                SelectedItem = null;

                await Application.Current.MainPage.Navigation.
                    PushModalAsync(new NavigationPage(new ProductPage(product, false)));
            }
        }

        public ICommand RefreshCommand { get; private set; }

        private void HandleRefresh()
        {
            IsRefreshing = true;

            PopulateProducts();
            SelectedOption = null;

            IsRefreshing = false;
        }

        public ICommand DeleteCommand { get; private set; }

        private void HandleDelete(Product product)
        {
            // Delete product from database
            App.Database.DeleteProductAsync(product);

            // Delete product from observable collection
            Products.Remove(product);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
