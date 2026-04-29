namespace Teszt1.Frontend
{
    public partial class EtkezesHozzaadasaPage : ContentPage
    {
        private readonly EtkezesHozzaadasaPageViewModel _viewModel;
        public EtkezesHozzaadasaPage(EtkezesHozzaadasaPageViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.LoadData();
        }
    }
}

