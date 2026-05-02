using System;
using Microsoft.Maui.Controls;

namespace Teszt1.Frontend.Statisztika;

public partial class StatisztikaPage : ContentPage
{
    private readonly StatisztikaPageViewModel _viewModel;
    public StatisztikaPage(StatisztikaPageViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private void AdatokFrissitese_Kivalasztva(object sender, EventArgs e)
    {
        if (BindingContext is StatisztikaPageViewModel vm)
        {
            vm.FrissitsAdatokatCommand.Execute(null);
        }
    }
}