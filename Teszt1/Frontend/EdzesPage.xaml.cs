namespace Teszt1.Frontend;

public partial class EdzesPage : ContentPage
{
	public EdzesPage(EdzesPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel; // Összekötjük a dizájnt a logikával!
    }
}