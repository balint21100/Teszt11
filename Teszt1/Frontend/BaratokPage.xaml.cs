namespace Teszt1.Frontend;

public partial class BaratokPage : ContentPage
{
	public BaratokPage(BaratokPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}