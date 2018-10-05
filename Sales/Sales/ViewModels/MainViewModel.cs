namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Sales.Models;
    using Sales.Views;
    using System;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class MainViewModel
    {
        #region Service

        #endregion

        #region Attributtes

        #endregion

        #region Properties
        public ProductsViewModel Products { get; set; }
       
        #endregion

        #region Contructors
        public MainViewModel()
        {
            Products = new ProductsViewModel();
        }
        #endregion

        #region Commands and Methods
        public ICommand AddProductCommand { get => new RelayCommand(GoToAddProduct); } 
        #endregion

        #region Methods
        private async void GoToAddProduct()
        {
            await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());
        }
        #endregion
    }
}
