namespace Sales.ViewModels
{
    using Sales.Models;
    using System.Collections.ObjectModel;
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

        #endregion

        #region Methods

        #endregion
    }
}
