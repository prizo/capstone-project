using Google.Apis.Services;
using Google.Apis.Vision.v1;
using Google.Apis.Vision.v1.Data;
using Plugin.Connectivity;
using Plugin.Media;
using Plugin.Media.Abstractions;
using SmartShop.Model;
using SmartShop.Utilities;
using SmartShop.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartShop.ViewModel
{
    class CameraPageViewModel : INotifyPropertyChanged
    {
        public CameraPageViewModel()
        {
            TakePhotoCommand = new Command(HandleTakePhoto);
            SelectPhotoCommand = new Command(HandleSelectPhoto);
        }

        private Xamarin.Forms.ImageSource _source = "placeholder.png";

        public Xamarin.Forms.ImageSource Source
        {
            get
            {
                return _source;
            }
            set
            {
                _source = value;
                OnPropertyChanged();
            }
        }

        private bool _isRunning = false;

        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                _isRunning = value;
                OnPropertyChanged();
            }
        }

        private bool _isVisible = false;

        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                OnPropertyChanged();
            }
        }

        public ICommand TakePhotoCommand { get; private set; }

        private async void HandleTakePhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("", "No camera available", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
            {
                Directory = "Vision",
                Name = "photo.jpg"
            });

            ProcessImage(file);
        }

        public ICommand SelectPhotoCommand { get; private set; }

        private async void HandleSelectPhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("", "Selecting photos not available", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            ProcessImage(file);
        }

        private async void ProcessImage(MediaFile file)
        {
            // Check for internet connection
            if (!CrossConnectivity.Current.IsConnected)
            {
                await Application.Current.MainPage.DisplayAlert("", "No internet connection", "OK");
                return;
            }

            if (file == null)
                return;

            // Show photo and enable loading animation
            Source = file.Path;
            IsRunning = true;
            IsVisible = true;

            // Annotate image on another thread
            string keywords = "";
            try
            {
                await Task.Run(async () =>
                {
                    keywords = await AnnotateAsync(file.Path);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            // Show placeholder and disable loading animation
            Source = "placeholder.png";
            IsRunning = false;
            IsVisible = false;

            // Search for the item using Bing shop
            if (keywords.Trim() != "")
            {
                string document = "";
                IList<Product> products = null;

                string query = Uri.EscapeDataString(keywords.Trim());
                document = BingWebRequest.SendRequest("/shop?q=" + query);

                if (document != null && document != "")
                {
                    products = new ProductExtractor().ExtractProducts(document);
                }

                if (products != null && products.Count > 0)
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new ResultPage(products)));
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("", "No results", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("", "No results", "OK");
            }
        }

        private async Task<string> AnnotateAsync(string path)
        {
            // Create the service
            var service = new VisionService(new BaseClientService.Initializer
            {
                ApiKey = "AIzaSyA72dkhD9FJZUt29wsFWfwqZGtTds8jsp0"
            });

            var bytes = File.ReadAllBytes(path);

            // Create the image request
            var imgReq = new AnnotateImageRequest
            {
                Image = new Google.Apis.Vision.v1.Data.Image
                {
                    Content = Convert.ToBase64String(bytes)
                },

                Features = new List<Feature>
                {
                    new Feature() { Type = "TEXT_DETECTION" }
                }
            };

            // Create the request
            var request = new BatchAnnotateImagesRequest
            {
                Requests = new List<AnnotateImageRequest>
                {
                    imgReq
                }
            };

            // Get the response
            var result = await service.Images.Annotate(request).ExecuteAsync();

            // Extract the keywords
            string keywords = "";

            if (result?.Responses?.Count > 0 && result.Responses[0].TextAnnotations != null)
            {
                var desc = result.Responses[0].TextAnnotations[0].Description;

                string[] words = desc.Split(
                    new[] { "\r\n", "\r", "\n" },
                    StringSplitOptions.None
                );

                keywords = String.Join(" ", words);
            }

            return keywords;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
