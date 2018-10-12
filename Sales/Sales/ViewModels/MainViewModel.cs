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
        public AddProductViewModel AddProduct { get; set; }
        public EditProductViewModel EditProduct { get; set; }
        public LoginViewModel Login { get; set; }
        #endregion

        #region Contructors
        public MainViewModel()
        {
            //Singleton
            instance = this;

            //yano lo intacio por la aplicación arranca por la loginpage
            //Products = new ProductsViewModel();
        }
        #endregion

        #region Singlenton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                return new MainViewModel();
            }

            return instance;
        }

        #endregion

        #region Commands and Methods
        public ICommand AddProductCommand { get => new RelayCommand(GoToAddProduct); } 
        #endregion

        #region Methods
        private async void GoToAddProduct()
        {
            //aqui instancio la clase, justo en el momento que la necesito para qu pinte los binding en memoria correspondiente:
            this.AddProduct = new AddProductViewModel();
            await Application.Current.MainPage.Navigation.PushAsync(new AddProductPage());
        }
        #endregion
    }
}
