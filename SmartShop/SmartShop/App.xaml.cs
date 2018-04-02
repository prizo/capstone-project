using SmartShop.Data;
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

		public App ()
		{
			InitializeComponent();

			MainPage = new MainPage();
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
