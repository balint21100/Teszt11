

namespace Teszt1.Frontend.Edzes;

public partial class UjEdzesPage : ContentPage
{
	private readonly UjEdzesPageViewModel viewModel;
	public UjEdzesPage(UjEdzesPageViewModel ujEdzesPageViewModel)
	{
		InitializeComponent();
		viewModel = ujEdzesPageViewModel;
		BindingContext = viewModel;
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
}