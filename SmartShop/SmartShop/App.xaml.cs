using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using SmartShop.Data;
using SmartShop.Utilities;
using SmartShop.View;
using Xamarin.Forms;

namespace SmartShop
{
    public partial class App : Application
	{
        static ProductDatabase database;

        public static ProductDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new ProductDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("ProductSQLite.db3"));
                }
                return database;
            }
        }

        static Position position;

        public static Position Position
        {
            get
            {
                if (position == null)
                {
                    GetCurrentPosition();
                }
                return position;
            }
        }

        public static async void GetCurrentPosition()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            position = await locator.GetPositionAsync();
        }

        public string IsFirstTime
        {
            get
            {
                return Settings.GeneralSettings;
            }
            set
            {
                if (Settings.GeneralSettings == value)
                    return;

                Settings.GeneralSettings = value;
            }
        }

        public App ()
		{
			InitializeComponent();

            // If the app is running for the first time, show a landing page
            if (IsFirstTime == "yes")
            {
                IsFirstTime = "no";
                MainPage = new LandingPage();
            }
            else
            {
                MainPage = new MainPage();
            }

            GetCurrentPosition();
		}

		protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
