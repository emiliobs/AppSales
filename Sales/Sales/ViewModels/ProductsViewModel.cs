namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Sales.Models;
    using Sales.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ProductsViewModel : BaseViewModel
    {
        #region Service
        private ApiService apiService;
        private bool isRefreshing;
        #endregion

        #region Attributtes
        private ObservableCollection<Products> productsList;
        #endregion

        #region Properties
        public bool IsRefreshing
        {
            get=> isRefreshing;
            set
            {
                if (isRefreshing != value)
                {
                    isRefreshing = value;
                    OnPropertyChanged();
                }
            }
        }
        public ObservableCollection<Products> ProductsList
        {
            get => productsList;
            set
            {
                if (productsList != value)
                {
                    productsList = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Contructors
        public ProductsViewModel()
        {
            //service:
            apiService = new ApiService();

            LoadProduct();
        }


        #endregion

        #region Commands and Methods
        public ICommand RefreshCommand { get => new RelayCommand(LoadProduct); }
        #endregion

        #region Methods
        private async void LoadProduct()
        {
            this.IsRefreshing = true;

            var connection = await apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;

                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);

                return;
            }

            var UrlAPI = App.Current.Resources["UrlAPI"].ToString();
            var UrlPrefix = App.Current.Resources["UrlPrefix"].ToString();
            var UrlProductsController = App.Current.Resources["UrlProductsController"].ToString();

            var response = await apiService.GetList<Products>(UrlAPI, UrlPrefix, UrlProductsController);
            //var response = await apiService.GetList<Products>("http://192.168.0.11:555", "/api", "/ProductsAPI");

            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error,response.Message, Languages.Error);

                return;
            }

            //aqui debo castiar por que aqui recibo un object tipo lista:
            var list = (List<Products>)response.Result;

            //aqui ya armo la observablecollection con lalista ya castiada:
            ProductsList = new ObservableCollection<Products>(list);

            this.IsRefreshing = false;
        }
        #endregion
    }
}
