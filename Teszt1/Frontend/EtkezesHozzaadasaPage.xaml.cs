namespace Teszt1.Frontend;

public partial class EtkezesHozzaadasaPage : ContentPage
{
	public EtkezesHozzaadasaPage(EtkezesHozzaadasaPageViewModel viewModel)
	{
		InitializeComponent();
        BindingContext = viewModel;
    }
}