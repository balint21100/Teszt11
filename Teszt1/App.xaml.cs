namespace Teszt1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Ez a sor kritikus a navigációhoz!
            MainPage = new NavigationPage(new MainPage());
        }
    }
}