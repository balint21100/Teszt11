using Teszt1.Bakckend.Services;
using Teszt1.Frontend.Authe;
using Teszt1.Frontend.Edzes;
using Teszt1.Frontend.Statisztika;
namespace Teszt1
{
    public partial class App : Application
    {
        public App(SessionService session)
        {
            InitializeComponent();
            MainPage = new AppShell(session);

        }
    }
}