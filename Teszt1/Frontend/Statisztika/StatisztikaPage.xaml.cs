using System;
using Microsoft.Maui.Controls;

namespace Teszt1.Frontend.Statisztika;

public partial class StatisztikaPage : ContentPage
{
    public StatisztikaPage()
    {
        InitializeComponent();
    }

    private void AdatokFrissitese_Kivalasztva(object sender, EventArgs e)
    {
        if (BindingContext is StatisztikaPageViewModel vm)
        {
            vm.AdatokFrissiteseCommand.Execute(null);
        }
    }
}