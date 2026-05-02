using Teszt1.Frontend;
using Teszt1.Frontend.Edzes;
using Teszt1.Frontend.Statisztika;
using Teszt1.Frontend.Authe;
namespace Teszt1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(EtkezesHozzaadasaPage), typeof(EtkezesHozzaadasaPage));
            Routing.RegisterRoute(nameof(BaratokPage), typeof(BaratokPage));
            Routing.RegisterRoute(nameof(EdzesPage), typeof(EdzesPage));
            Routing.RegisterRoute(nameof(EuAdatokPage), typeof(EuAdatokPage));
            Routing.RegisterRoute(nameof(UjEdzesPage), typeof(UjEdzesPage));
            Routing.RegisterRoute(nameof(EdzesSzerkesztesPage), typeof(EdzesSzerkesztesPage));
            Routing.RegisterRoute(nameof(StatisztikaPage), typeof(StatisztikaPage));
            Routing.RegisterRoute(nameof(AuthPage), typeof(AuthPage));


            
        }
    }
}
