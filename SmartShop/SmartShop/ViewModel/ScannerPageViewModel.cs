﻿using RestSharp;
using SmartShop.Model;
using SmartShop.Utilities;
using SmartShop.View;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

namespace SmartShop.ViewModel
{
    class ScannerPageViewModel : INotifyPropertyChanged
    {
        public ScannerPageViewModel()
        {
            ScanResultCommand = new Command(HandleScanResult);
            FlashCommand = new Command(HandleFlash);
        }

        private bool _isScanning = true;

        public bool IsScanning
        {
            get
            {
                return _isScanning;
            }
            set
            {
                _isScanning = value;
                OnPropertyChanged();
            }
        }

        private bool _isAnalyzing = true;

        public bool IsAnalyzing
        {
            get
            {
                return _isAnalyzing;
            }
            set
            {
                _isAnalyzing = value;
                OnPropertyChanged();
            }
        }

        private bool _isTorchOn;

        public bool IsTorchOn
        {
            get
            {
                return _isTorchOn;
            }
            set
            {
                _isTorchOn = value;
                OnPropertyChanged();
            }
        }

        public ZXing.Result Result { get; set; }

        public ICommand ScanResultCommand { get; private set; }

        private void HandleScanResult()
        {
            IsScanning = false;
            IsAnalyzing = false;

            IList<Product> products = null;
            string query = Result.Text.Trim();

            IRestResponse response = UpcRestRequest.GetResponse(query);

            if (response != null)
            {
                products = UpcProductExtractor.GetProducts(response);
            }

            Device.BeginInvokeOnMainThread(async () =>
            {
                if (products != null && products.Count > 0)
                {
                    await Application.Current.MainPage.Navigation.PushModalAsync(new ResultPage(products));
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("", "Product not found", "OK");
                }
            });
        }

        private string _flashText = "\u26A1 Off";

        public string FlashText
        {
            get
            {
                return _flashText;
            }
            set
            {
                _flashText = value;
                OnPropertyChanged();
            }
        }

        public ICommand FlashCommand { get; private set; }

        private void HandleFlash()
        {
            if (FlashText.Equals("\u26A1 Off"))
            {
                FlashText = "\u26A1 On";
                IsTorchOn = true;
            }
            else
            {
                FlashText = "\u26A1 Off";
                IsTorchOn = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string caller = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(caller));
        }
    }
}
