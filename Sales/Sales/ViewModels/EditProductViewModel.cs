namespace Sales.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Sales.Helpers;
    using Sales.Models;
    using Sales.Services;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Windows.Input;
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

        #region Commnads

        public ICommand DeleteEditProductCommand { get => new RelayCommand(DeleteEditProduct); }
      
        public ICommand SaveEditCommand { get => new RelayCommand(SaveEdit); }

        public ICommand ChangeImageCommand { get => new RelayCommand(ChangeImage); }

        #endregion

        #region Methods


        private async  void DeleteEditProduct()
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

            var UrlAPI = App.Current.Resources["UrlAPI"].ToString();
            var UrlPrefix = App.Current.Resources["UrlPrefix"].ToString();
            var UrlProductsController = App.Current.Resources["UrlProductsController"].ToString();

            // this.ProductId sale des contexto de la herencia con products
            var response = await apiService.Delete(UrlAPI, UrlPrefix, UrlProductsController, this.Products.ProductId);
            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);
                return;
            }

            var productsViewModel = ProductsViewModel.GetInstance();

            //aqui busco el producto a borrar:
            var deleteProduct = productsViewModel.MyPorducts.Where(p => p.ProductId.Equals(this.Products.ProductId)).FirstOrDefault();
            if (deleteProduct != null)
            {
                productsViewModel.MyPorducts.Remove(deleteProduct);
            }

            productsViewModel.RefreshList();


            this.IsRunning = false;
            this.IsEnabled = true;

            await Application.Current.MainPage.Navigation.PopAsync();

        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var souce = await Application.Current.MainPage.DisplayActionSheet(
                 Languages.ImageSource,
                 Languages.Cancel,
                 null,
                 Languages.FromGallery,
                 Languages.NewPicture
                );

            if (souce == Languages.Cancel)
            {
                this.file = null;

                return;
            }

            if (souce == Languages.NewPicture)
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(

                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test,jpg",
                        PhotoSize = PhotoSize.Small,
                    });
            }
            else
            {
                //aqui selecciono la foto de la galeria:
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                //aqui leo todo el achivo de foro y lo cargo en memoria:
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = this.file.GetStream();

                    return stream;
                });
            }
        }
        private async void SaveEdit()
        {
            if (string.IsNullOrEmpty(this.Products.Description))
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.DescriptionError, Languages.Accept);

                return;
            }             

            //aqi lo parceo, por que  la propiedad es string:
            if (this.Products.Price <= 0)
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

            byte[] imageArray = null;
            if (this.file != null)
            {
                //casteo a bytes de arreglos:  si hay imagen o tomo una foto(si hay foto)
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
                //aqui se le envio el array a la propiedad ImageArray dela clase product
                this.Products.ImageArray = imageArray;
            }

            var UrlAPI = App.Current.Resources["UrlAPI"].ToString();
            var UrlPrefix = App.Current.Resources["UrlPrefix"].ToString();
            var UrlProductsController = App.Current.Resources["UrlProductsController"].ToString();

            var response = await apiService.Put<Products>(UrlAPI, UrlPrefix, UrlProductsController, this.Products, this.Products.ProductId);
            //var response = await apiService.GetList<Products>("http://192.168.0.11:555", "/api", "/ProductsAPI");

            //aqui pregunto si grabo el post:
            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);

                return;
            }

            var productViewModel = ProductsViewModel.GetInstance();

            //aqui addciono el nuevro producto al la clse pructos del listview del la clase productviewmodel
            //casteo:
            var newProduct = (Products)response.Result;

            //aqui busco el producto viejo a editar y los borro antes de addicionar el bnuevo producto editado:
            //var oldPorduct = productViewModel.MyPorducts.Where(p=>p.ProductId.Equals(this.Products.ProductId)).FirstOrDefault();
            ////aqui valido que se encuentre el producto a eliminar:
            //if (oldPorduct != null)
            //{
            //    productViewModel.MyPorducts.Remove(oldPorduct); 
            //}

            //aqui llamo por singleton a la propiedad de lista myPorducts y le adddiciones el nuevo registro:
            productViewModel.MyPorducts.Add(newProduct);
            //aqui llamo al método con el singleton refreshlist y refresco depues de agregar el nuevo producto;
            //productViewModel.RefreshList();
            productViewModel.LoadProduct();
            
            this.IsRunning = false;
            this.IsEnabled = true;   

            //productViewModel.LoadProduct();
            //aqui me regreso a la pagina anterior(desapilo):
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        #endregion
    }
}
