using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Frontend.Authe
{
    public partial class AuthenticationViewModel : ObservableObject
    {
        [ObservableProperty] private string email;
        [ObservableProperty] private string password;
        [ObservableProperty] private string username;
        [ObservableProperty] private bool isRegisterMode;
        [ObservableProperty] private string title = "Bejelentkezés";
        [ObservableProperty] private string buttonText = "Belépés";
        [ObservableProperty] private string switchText = "Nincs még fiókod? Regisztrálj!";

        // Váltás a Bejelentkezés és Regisztráció mód között
        [RelayCommand]
        private void SwitchMode()
        {
            IsRegisterMode = !IsRegisterMode;
            Title = IsRegisterMode ? "Fiók létrehozása" : "Bejelentkezés";
            ButtonText = IsRegisterMode ? "Regisztráció" : "Belépés";
            SwitchText = IsRegisterMode ? "Már van fiókod? Jelentkezz be!" : "Nincs még fiókod? Regisztrálj!";
        }

        public AuthenticationViewModel()
        {
            
        }
        [RelayCommand]
        private async Task AuthAction()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
            {
                await Shell.Current.DisplayAlert("Hiba", "Minden mezőt ki kell tölteni!", "OK");
                return;
            }

            if (IsRegisterMode)
            {
                // Regisztrációs logika helye (pl. API hívás)
                await Shell.Current.DisplayAlert("Siker", "Regisztráció sikeres!", "OK");
            }
            else
            {
                // Bejelentkezési logika helye
                if (Email == "admin@teszt.hu" && Password == "1234")
                {
                    await Shell.Current.GoToAsync("//MainPage");
                }
                else
                {
                    await Shell.Current.DisplayAlert("Hiba", "Hibás adatok!", "OK");
                }
            }
        }
    }
}
