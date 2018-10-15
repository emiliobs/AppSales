using Xamarin.Forms;
using System;
using Xamarin.Forms.Xaml;
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Sales
{
    using Sales.ViewModels;
    using Sales.Views;

    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new ProductsPage());
            //aqui instacio a la mainviewmodel que acompa;a ;la login page>
            //MainViewModel.GetInstance().Login = new LoginViewModel();
            //MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
