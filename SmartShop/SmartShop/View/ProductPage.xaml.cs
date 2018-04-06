using SmartShop.Model;
using SmartShop.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.GoogleMaps;
using Plugin.Geolocator;
using System.Net;
using System;
using System.IO;
using Newtonsoft.Json;
using RestSharp;

namespace SmartShop.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProductPage : ContentPage
    {

        private double _Lat;
        public double Lat {
            get { return _Lat; }
            set { _Lat = value; }
        }
        private double _Lng;
        public double Lng {
            get { return _Lng; }
            set { _Lng = value; }
        }

        public ProductPage(Product product, bool isEnabled)
        {
            InitializeComponent();
            BindingContext = new ProductPageViewModel(product, isEnabled);


            map.MyLocationEnabled = true;


            map.InitialCameraUpdate = CameraUpdateFactory.NewPositionZoom(new Position(41.759851d, -88.321833d), 12d);


            //Searching for nearby stores from product.PriceSeller
            SearchNearby(product.PriceSeller);
        }

        public async void CurrentLocation()
        {

            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;
            
            var current = await locator.GetLastKnownLocationAsync();
           
            Lat = current.Latitude;
            Lng = current.Longitude;
          
            await map.MoveCamera(CameraUpdateFactory.NewPosition(new Position(current.Latitude, current.Longitude)));

            
           
        }

        public async void SearchNearby(string query)
        {
            //Building url string to send
            string queryBuilder = "query=" + query;
            string apiKey = "&key=AIzaSyD9m6JQAPfRfiB0xdFLvbYcT10L9tOXo-Q";
            string type = "&type=store";
            string location = "&location=41.759851, -88.321833";
            string googleApiCall = "https://maps.googleapis.com/maps/api/place/textsearch/json?";

            //Creating Webquest with string 
            WebRequest request = WebRequest.Create(googleApiCall + queryBuilder + location + type + apiKey);
            WebResponse response = await request.GetResponseAsync();
           
            
            //Reading the response stream
            Stream recievedStream = response.GetResponseStream();
            StreamReader read = new StreamReader(recievedStream);
            string responseRead = read.ReadToEnd();
            read.Close();
            response.Close();

            //Deserializing Request
            var result = JsonConvert.DeserializeObject<dynamic>(responseRead);
           
                if (result["results"].Count == 0)
                    Console.WriteLine("");
                else
                {
                    for (int i = 0; i < result["results"].Count; i++)
                    {
                        Console.WriteLine("\n\n\n\n" + result["results"].Count + "\n\n\n\n\n");
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
            map.Pins.Add(pin);
        }

        public async void AddPinByAddress(string address, string label)
        {
            var gc = new Geocoder();
            var positions = await gc.GetPositionsForAddressAsync(address);
            if (positions.Count() > 0)
            {
                var pos = positions.First();
                Position pinPosition = new Position(pos.Latitude, pos.Longitude);

                var pin = new Pin()
                {
                    Type = PinType.Place,
                    Label = label,
                    Address = address,
                    Position = pinPosition
                };

                map.Pins.Add(pin);

            }
            else
            {
                await this.DisplayAlert("Not found", "Geocoder return no results", "Close");
            }
        }
    }

    
}