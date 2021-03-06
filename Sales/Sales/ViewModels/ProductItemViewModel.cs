﻿namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Sales.Models;
    using Sales.Services;
    using Sales.Views;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ProductItemViewModel  : Products
    {
        #region services
        private ApiService apiService;
        #endregion

        #region Attributes

        #endregion

        #region Contructors
        public ProductItemViewModel()
        {
            //singleton
            instance = this;
            //Services
            apiService = new ApiService();
        }
        #endregion

        #region Singlenton
        private static ProductItemViewModel instance;

        public static ProductItemViewModel GetInstance()
        {
            if (instance == null)
            {
                return new ProductItemViewModel();
            }

            return instance;
        }

        #endregion

        #region Commands
        public ICommand DeleteProductCommand { get => new RelayCommand(DeleteProduct); }
        public ICommand EditProductComand { get => new RelayCommand(EditProduct); }

        #endregion

        #region Methods


        private async void EditProduct()
        {
            //aqui instancio la editPorductviewmodel desde el singleton del mainviewmodel
            MainViewModel.GetInstance().EditProduct = new EditProductViewModel(this);
            await Application.Current.MainPage.Navigation.PushAsync(new EditProductPage());
        }

        public async void DeleteProduct()
        {
            var answer = await Application.Current.MainPage.DisplayAlert(
                Languages.Confirm,
                Languages.DeleteConfirmation,
                Languages.Yes,
                Languages.No);

            if (!answer)
            {
                return;
            }

            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                //this.IsRefreshing = false;

                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.Accept);

                return;
            }

            var UrlAPI = App.Current.Resources["UrlAPI"].ToString();
            var UrlPrefix = App.Current.Resources["UrlPrefix"].ToString();
            var UrlProductsController = App.Current.Resources["UrlProductsController"].ToString();

            // this.ProductId sale des contexto de la herencia con products
            var response = await apiService.Delete(UrlAPI, UrlPrefix, UrlProductsController, this.ProductId);
            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error,response.Message, Languages.Accept);
                return;
            }

            var productsViewModel = ProductsViewModel.GetInstance();

            //aqui busco el producto a borrar:
            var deleteProduct = productsViewModel.MyPorducts.Where(p => p.ProductId.Equals(this.ProductId)).FirstOrDefault();
            if (deleteProduct != null)
            {
                productsViewModel.MyPorducts.Remove(deleteProduct);
            }

            productsViewModel.RefreshList();

            await Application.Current.MainPage.Navigation.PopAsync();
        }

        #endregion
    }
}
