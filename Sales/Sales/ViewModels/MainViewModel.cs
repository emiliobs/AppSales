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
        public AddProductPage AddProduct { get; set; }
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
            //aqui instancio la clase, justo en el momento que la necesito para qu pinte los binding en memoria correspondiente:
            this.AddProduct = new AddProductPage();
            await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());
        }
        #endregion
    }
}
