using Teszt1.Frontend;
using Teszt1.Frontend.Edzes;
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
        }
    }
}
