using Google.Apis.Services;
using Google.Apis.Vision.v1;
using Google.Apis.Vision.v1.Data;
using Plugin.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartShop.ViewModel
{
    class CameraPageViewModel
    {
        public CameraPageViewModel()
        {
            TakePhotoCommand = new Command(HandleTakePhoto);
            SelectPhotoCommand = new Command(HandleSelectPhoto);
        }

        public ICommand TakePhotoCommand { get; private set; }

        private async void HandleTakePhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Camera", "No camera available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                Directory = "Vision",
                Name = "photo.jpg"
            });

            if (file == null)
                return;

            AnnotateAsync(file.Path);
        }

        public ICommand SelectPhotoCommand { get; private set; }

        private async void HandleSelectPhoto()
        {
            await CrossMedia.Current.Initialize();

            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await Application.Current.MainPage.DisplayAlert("No Upload", "Selecting photos not available.", "OK");
                return;
            }

            var file = await CrossMedia.Current.PickPhotoAsync();

            if (file == null)
                return;

            AnnotateAsync(file.Path);
        }

        private async void AnnotateAsync(string path)
        {
            // Create the service
            var service = new VisionService(new BaseClientService.Initializer
            {
                ApiKey = ""
            });

            var bytes = File.ReadAllBytes(path);

            // Create the image request
            var imageRequest = new AnnotateImageRequest
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
                    imageRequest
                }
            };

            // Get the response
            var result = await service.Images.Annotate(request).ExecuteAsync();

            if (result?.Responses?.Count > 0)
            {
                var keywords = result.Responses[0].TextAnnotations.Select(s => s.Description).ToArray();
                var words = String.Join(",", keywords);
                await Application.Current.MainPage.DisplayAlert("Result", words, "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Result", "Nothing found", "OK");
            }
        }
    }
}
