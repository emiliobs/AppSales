﻿namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Helpers;
    using Sales.Models;
    using Sales.Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
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
        private ObservableCollection<ProductItemViewModel> productsList;
        private string filter;
        #endregion

        #region Properties

        public string Filter
        {
            get => filter;
            set
            {
                if (filter != value)
                {
                    filter = value;
                    //OnPropertyChanged();
                    RefreshList();
                }
            }
        }
        public List<Products> MyPorducts { get; set; }
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
        public ObservableCollection<ProductItemViewModel> ProductsList
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
            //singleton
            instance = this;

            //service:
            apiService = new ApiService();

            LoadProduct();
        }


        #endregion

        #region Singlenton
        private static ProductsViewModel instance;

        public static ProductsViewModel GetInstance()
        {
            if (instance == null)
            {
                return  new ProductsViewModel();
            }

            return instance;
        }

        #endregion

        #region Commands and Methods
        public ICommand SearchCommand { get => new RelayCommand(RefreshList); }
        public ICommand RefreshCommand { get => new RelayCommand(LoadProduct); }
        #endregion

        #region Methods
        public async void LoadProduct()
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
            this.MyPorducts = (List<Products>)response.Result;   
            //MétodoRefresList:
            RefreshList();                                                                                     

            this.IsRefreshing = false;
        }

        public void RefreshList()
        {
            //aqui aplico todo el proceso del filtro (search)
            if (string.IsNullOrEmpty(this.Filter))
            {
                //lo mas eficiente en cuando necesitas armar una lista de otra:
                var myProductItemViewModel = this.MyPorducts.Select(p => new ProductItemViewModel()
                {
                    Description = p.Description,
                    ImageArray = p.ImageArray,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    ProductId = p.ProductId,
                    PublishOn = p.PublishOn,
                    Remarks = p.Remarks,

                });

                //aqui ya armo la observablecollection con lalista ya castiada:
                ProductsList = new ObservableCollection<ProductItemViewModel>(myProductItemViewModel.OrderBy(p => p.Description));
            }
            else
            {
                //lo mas eficiente en cuando necesitas armar una lista de otra:
                var myProductItemViewModel = this.MyPorducts.Select(p => new ProductItemViewModel()
                {
                    Description = p.Description,
                    ImageArray = p.ImageArray,
                    ImagePath = p.ImagePath,
                    IsAvailable = p.IsAvailable,
                    Price = p.Price,
                    ProductId = p.ProductId,
                    PublishOn = p.PublishOn,
                    Remarks = p.Remarks,

                    //filtro todo el proceso del filtro con land y linq
                }).Where(p => p.Description.ToLower().Trim().Contains(Filter.ToLower().Trim()));

                //aqui ya armo la observablecollection con lalista ya castiada:
                ProductsList = new ObservableCollection<ProductItemViewModel>(myProductItemViewModel.OrderBy(p => p.Description));
            }

          
        }
        #endregion
    }
}
