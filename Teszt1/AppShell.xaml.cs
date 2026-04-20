namespace Teszt1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("EtkezesHozzaadasaPage", typeof(EtkezesHozzaadasaPage));
        }
    }
}
