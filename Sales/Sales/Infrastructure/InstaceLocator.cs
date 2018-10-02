namespace Sales.Infrastructure
{
    using Sales.ViewModels;
    public  class InstaceLocator
    {

        #region Properties    
        public MainViewModel Main{ get; set; }

        #endregion

        #region Constructor

        public InstaceLocator()
        {
            Main = new MainViewModel();
        }

        #endregion
    }
}
