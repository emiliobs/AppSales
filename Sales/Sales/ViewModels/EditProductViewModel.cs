namespace Sales.ViewModels
{
    using Plugin.Media.Abstractions;
    using Sales.Models;
    using Sales.Services;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Xamarin.Forms;

    public class EditProductViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        #endregion

        #region Attributes
        //producto a editar
        private Products products;

        private MediaFile file;
        private ImageSource imageSource;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public ImageSource ImageSource
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
        public Products Products
        {
            get => products;
            set
            {
                if (products != value)
                {
                    products = value;
                    OnPropertyChanged();
                }
            }
        }
        #endregion

        #region Constructors
        public EditProductViewModel(Products products)
        {
            //inyección de dependencia de la clase producto a manipular
            this.products = products;

            apiService = new ApiService();

            //aqui le asigno el valor por defecto que viene con la clase a editar
            ImageSource = products.ImageFullPath;

            IsEnabled = true;

        } 
        #endregion
    }
}
