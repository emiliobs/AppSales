namespace Sales.ViewModels
{
    using Sales.Models;
    using Sales.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Text;
    using Xamarin.Forms;

    public class ProductsViewModel : BaseViewModel
    {
        #region Service
        private ApiService apiService;
        #endregion

        #region Attributtes
        private ObservableCollection<Products> productsList;
        #endregion

        #region Properties
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

        #endregion

        #region Methods
        private async void LoadProduct()
        {
            //var UrlAPI = App.Current.Resources["UrlAPI"].ToString();
            //var UrlPrefix = App.Current.Resources["UrlPrefix"].ToString();
            //var UrlProductsController = App.Current.Resources["UrlProductsController"].ToString();

            //var response = await apiService.GetList<Products>(UrlAPI, UrlPrefix, UrlProductsController);
            var response = await apiService.GetList<Products>("http://192.168.0.11:555", "/api", "/ProductsAPI");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Error.",response.Message, "Accept.");

                return;
            }

            //aqui debo castiar por que aqui recibo un object tipo lista:
            var list = (List<Products>)response.Result;

            //aqui ya armo la observablecollection con lalista ya castiada:
            productsList = new ObservableCollection<Products>(list);
        }
        #endregion
    }
}
