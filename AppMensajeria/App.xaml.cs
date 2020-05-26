using AppMensajeria.Contexts;
using AppMensajeria.Interfaces;
using AppMensajeria.Views;
using Xamarin.Forms;

namespace AppMensajeria
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();
            GetContext().Database.EnsureCreated();
            MainPage = new MainPage();
        }

        public static AppDbContext GetContext() {
            string DbPath = DependencyService.Get<IConfigDataBase>().GetFullPath("dbMensajeria1.db");
            return new AppDbContext(DbPath);
        }


        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
