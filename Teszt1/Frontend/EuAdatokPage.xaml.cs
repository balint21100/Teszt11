namespace Teszt1.Frontend;

public partial class EuAdatokPage : ContentPage
{
	public EuAdatokPage(EuAdatokPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}