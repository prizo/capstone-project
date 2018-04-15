using System.Windows.Input;
using Xamarin.Forms;

namespace SmartShop.ViewModel
{
    class LandingPageViewModel
    {
        public LandingPageViewModel()
        {
            StartCommand = new Command(HandleStart);
        }

        public ICommand StartCommand { get; private set; }

        private void HandleStart()
        {
            Application.Current.MainPage = new MainPage();
        }
    }
}
