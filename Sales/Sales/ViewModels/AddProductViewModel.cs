namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Sales.Models;
    using Sales.Services;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class AddProductViewModel:BaseViewModel
    {
        #region services
        private ApiService apiService;
        #endregion

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
            //services:
            apiService = new ApiService();

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

            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);

                return;
            }

            //aqui armo el objeto products:  
            var product = new Products()
            {
               Description = this.Description,
               Price = price,
               Remarks = this.Remarks,
            };

            var UrlAPI = App.Current.Resources["UrlAPI"].ToString();
            var UrlPrefix = App.Current.Resources["UrlPrefix"].ToString();
            var UrlProductsController = App.Current.Resources["UrlProductsController"].ToString();

            var response = await apiService.Post(UrlAPI, UrlPrefix, UrlProductsController, product);
            //var response = await apiService.GetList<Products>("http://192.168.0.11:555", "/api", "/ProductsAPI");

            //aqui pregunto si grabo el post:
            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(Languages.Error,response.Message, Languages.Accept);

                return;
            }
              

            this.IsRunning = false;
            this.IsEnabled = true;

            //aqui me regreso a la pagina anterior(desapilo):
            await Application.Current.MainPage.Navigation.PopAsync();

        }
        #endregion

    }
}
