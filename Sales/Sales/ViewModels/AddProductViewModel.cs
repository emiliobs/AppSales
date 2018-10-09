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
    using System.Text;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class AddProductViewModel : BaseViewModel
    {
        #region services
        private ApiService apiService;
        #endregion

        #region Atributtes
        //utili el plugin de fotos jemasmontemagno: aqui guardo la foto en memoria>
        private MediaFile file;
        private ImageSource imageSource;
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

        public ICommand ChangeImageCommand { get => new RelayCommand(ChangeImage); }

        #endregion

        #region Methodos 


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

        private async void SaveAddProduct()
        {
            if (string.IsNullOrEmpty(this.Description))
            {
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.DescriptionError, Languages.Accept);

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

            byte[] imageArray = null;
            if (this.file != null)
            {
                //casteo a bytes de arreglos:  si hay imagen o tomo una foto(si hay foto)
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            //aqui armo el objeto products:  
            var product = new Products()
            {
                Description = this.Description,
                Price = price,
                Remarks = this.Remarks,
                //aqui ya compio la foto al modelo con si propiedad imagenarray:
                ImageArray = imageArray,
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

                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.Accept);

                return;
            }

            var productViewModel = ProductsViewModel.GetInstance();

            //aqui addciono el nuevro producto al la clse pructos del listview del la clase productviewmodel
            //casteo:
            var newProduct = (Products)response.Result;
            //aqui llamo por singleton a la propiedad de lista myPorducts y le adddiciones el nuevo registro:
            productViewModel.MyPorducts.Add(newProduct);
            //aqui llamo al método con el singleton refreshlist y refresco depues de agregar el nuevo producto;
            productViewModel.RefreshList();
            //aqui llamo el singleton de prodcctsviewmodel:
            //ProductsViewModel.GetInstance().ProductsList.Add(newProduct);
            //ProductsViewModel.GetInstance().ProductsList.Add(new ProductItemViewModel()
            //{
            //    Description = newProduct.Description,
            //    ImageArray = newProduct.ImageArray,
            //    ImagePath = newProduct.ImagePath,
            //    IsAvailable = newProduct.IsAvailable,
            //    Price = newProduct.Price,
            //    ProductId = newProduct.ProductId,
            //    PublishOn = newProduct.PublishOn,
            //    Remarks = newProduct.Remarks,
            //});
            /// productViewModel.ProductsList.Add(new);

            this.IsRunning = false;
            this.IsEnabled = true;


            //productViewModel.LoadProduct();
            //aqui me regreso a la pagina anterior(desapilo):
            await Application.Current.MainPage.Navigation.PopAsync();

        }
        #endregion

    }
}
