namespace Teszt1.Frontend.Authe;

public partial class AuthPage : ContentPage
{
	private readonly AuthenticationViewModel _viewModel;
	public AuthPage(AuthenticationViewModel viewModel)
	{
		InitializeComponent();
		_viewModel = viewModel;
		BindingContext = _viewModel;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}