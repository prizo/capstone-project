using Newtonsoft.Json;
using SmartShop.Model;
using SmartShop.ViewModel;
using System;
using System.IO;
using System.Net;
using Xamarin.Forms;
using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.Xaml;

namespace SmartShop.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProductPage : ContentPage
	{
        public ProductPage (Product product, bool isEnabled)
		{
			InitializeComponent ();
            BindingContext = new ProductPageViewModel(product, isEnabled);

            // Hide map for online sellers
            if (product.Seller.Contains(".com") || product.Seller.Contains("eBay"))
            {
                map.IsEnabled = false;
                map.IsVisible = false;
            }
            else
            {
                // Search for nearby stores from product.Seller
                SearchNearby(product.Seller);
            }
        }

        public async void SearchNearby(string query)
        {
            // Build url string to send
            string queryBuilder = "query=" + query;
            string apiKey = "&key=AIzaSyD9m6JQAPfRfiB0xdFLvbYcT10L9tOXo-Q";
            string location = "&location=" + App.Position.Latitude + ", " + App.Position.Longitude;
            string radius = "&radius=16000"; // 10 miles
            string type = "&type=store";
            string googleApiCall = "https://maps.googleapis.com/maps/api/place/textsearch/json?";

            // Create Webrequest with string 
            WebRequest request = WebRequest.Create(googleApiCall + queryBuilder + location + radius + type + apiKey);
            WebResponse response = await request.GetResponseAsync();

            // Read the response stream
            Stream recievedStream = response.GetResponseStream();
            StreamReader read = new StreamReader(recievedStream);
            string responseRead = read.ReadToEnd();
            read.Close();
            response.Close();

            // Deserialize the request
            var result = JsonConvert.DeserializeObject<dynamic>(responseRead);

            if (result["results"].Count == 0)
            {
                map.IsEnabled = false;
                map.IsVisible = false;
            }
            else
            {
                for (int i = 0; i < result["results"].Count; i++)
                {
                    Position pos = new Position((double)result["results"][i]["geometry"]["location"]["lat"], (double)result["results"][i]["geometry"]["location"]["lng"]);
                    string label = result["results"][i]["name"];
                    string address = result["results"][i]["formatted_address"];

                    AddPin(pos, label, address);
                }
            }
        }

        public void AddPin(Position pos, string label, string address)
        {
            var pin = new Pin()
            {
                Type = PinType.Place,
                Label = label,
                Address = address,
                Position = pos
            };
#pragma warning disable CS0618 // Type or member is obsolete
            pin.Clicked += (sender, args) =>
#pragma warning restore CS0618 // Type or member is obsolete
            {
                OpenMap(address);
            };
            map.Pins.Add(pin);
        }

        void OpenMap(string address)
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    Device.OpenUri(
                      new Uri(string.Format("http://maps.apple.com/?q={0}", WebUtility.UrlEncode(address))));
                    break;
                case Device.Android:
                    Device.OpenUri(
                      new Uri(string.Format("geo:0,0?q={0}", WebUtility.UrlEncode(address))));
                    break;
            }
        }
    }
}