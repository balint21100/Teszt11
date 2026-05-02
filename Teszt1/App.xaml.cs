using Teszt1.Frontend.Authe;
using Teszt1.Frontend.Edzes;
using Teszt1.Frontend.Statisztika;
namespace Teszt1
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

        }
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}