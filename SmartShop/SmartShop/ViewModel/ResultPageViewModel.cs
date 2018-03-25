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

        public ICommand ItemSelectedCommand { get; private set; }

        private void HandleItemSelected(Product product)
        {
            Application.Current.MainPage.Navigation.PushModalAsync(new ProductPage(product, true));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
