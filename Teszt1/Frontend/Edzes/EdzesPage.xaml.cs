using Microsoft.Maui.Controls;
using System;

namespace Teszt1.Frontend.Edzes;

public partial class EdzesPage : ContentPage
{
    private readonly EdzesPageViewModel _viewModel;
    public EdzesPage(EdzesPageViewModel edzesPageViewModel)
    {
        InitializeComponent();
        _viewModel = edzesPageViewModel;
        BindingContext = _viewModel;
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Load();
    }

    private void OnShowPopupClicked(object sender, EventArgs e)
    {
        PopUpCha.IsVisible = true;
        MyChallengePopup.IsVisible = true;
    }

    private void OnChallengeResponded(object sender, bool accepted)
    {
        if (accepted)
        {
            PopUpCha.IsVisible = false;
        }
    }
    private async void OnEdzesKivalasztva(object sender, EventArgs e)
    {
        var picker = (Picker)sender;
        if (picker.SelectedItem != null && picker.SelectedItem.ToString() == "+ Új edzés hozzáadása")
        {
            string nap = picker.ClassId;
            MainThread.BeginInvokeOnMainThread(() => picker.SelectedIndex = -1);

            try
            {
                await Shell.Current.GoToAsync($"{nameof(UjEdzesPage)}?Nap={nap}");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Hiba", $"Navigációs hiba: {ex.Message}", "OK");
            }
        }
    }

    private async void OnSzerkesztesKattintva(object sender, EventArgs e)
    {
        var gomb = (Button)sender;
        // Megkeressük a gomb melletti Pickert, hogy tudjuk, melyik edzés van kiválasztva
        var grid = (Grid)gomb.Parent;
        var picker = (Picker)grid.Children[1];

        if (picker.SelectedItem == null || picker.SelectedItem.ToString() == "+ Új edzés hozzáadása")
        {
            await DisplayAlert("Figyelem", "Elõbb válassz ki egy szerkeszthetõ edzést!", "OK");
            return;
        }

        string edzesNeve = picker.SelectedItem.ToString();
        string nap = picker.ClassId;

        try
        {
            // Átadjuk a nevet és a napot a szerkesztõnek
            await Shell.Current.GoToAsync($"{nameof(EdzesSzerkesztesPage)}?EdzesNev={edzesNeve}&Nap={nap}");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Hiba", $"Navigációs hiba: {ex.Message}", "OK");
        }
    }
}