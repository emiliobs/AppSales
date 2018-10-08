namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Sales.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class AddProductViewModel:BaseViewModel
    {
        #region Atributtes
        private string imageSource;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties

        //las suelo colocar todas como string para no perder  los plceholder y mejorpara capturar los datos
        #region Properties bindables
        public string Description { get; set; }
        public string Price { get; set; }
        public string Remarks { get; set; } 
        #endregion

        public string ImageSource
        {
            get => imageSource;

            set
            {
                if (imageSource != value)
                {
                    imageSource = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool IsEnabled
        {
            get => isEnabled;
            set
            {
                if (isEnabled != value)
                {
                    isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsRunning
        {
            get => isRunning;

            set
            {
                if (isRunning != value)
                {
                    isRunning = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Constructor
        public AddProductViewModel()
        {
            ImageSource = "noproduct";

            IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand SaveCommand { get => new RelayCommand(SaveAddProduct); }

        #endregion

        #region Methodos 
        private async void SaveAddProduct()
        {
            if (string.IsNullOrEmpty(this.Description))
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error,Languages.DescriptionError,Languages.Accept);

                return;
            }

            if (string.IsNullOrEmpty(this.Price))
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.PriceError, Languages.Accept);
                return;
            }

            //aqi lo parceo, por que  la propiedad es string:
            var price = decimal.Parse(this.Price);
            if (price <= 0)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.PriceError, Languages.Accept);
                return;
            }

            await Application.Current.MainPage.DisplayAlert(Languages.Error, "Todo Bien.....", Languages.Accept);

        }
        #endregion

    }
}
