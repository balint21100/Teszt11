using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Services;

namespace Teszt1.Frontend.Authe
{
    public partial class AuthenticationViewModel : ObservableObject
    {
        private readonly RegLogService _logService;
        private readonly SessionService _sessionService;

        [ObservableProperty] private string email;
        [ObservableProperty] private string password;
        [ObservableProperty] private string username;
        [ObservableProperty] private bool isRegisterMode;
        [ObservableProperty] private string title = "Bejelentkezés";
        [ObservableProperty] private string buttonText = "Belépés";
        [ObservableProperty] private string switchText = "Nincs még fiókod? Regisztrálj!";
        [ObservableProperty] private int age;
        [ObservableProperty] private string gender; // "Férfi" vagy "Nő"
        [ObservableProperty] private double activityLevel = 1.2; // Alapértelmezett: ülő életmód
        [ObservableProperty] private double height;
        [ObservableProperty] private double weight;
        [ObservableProperty] private double tdee;

        public List<string> Genders => new List<string> { "Férfi", "Nő" };

        public Dictionary<string, double> activityLevels => new Dictionary<string, double>
{
    { "Ülő életmód", 1.2 },
    { "Enyhén aktív", 1.375 },
    { "Mérsékelten aktív", 1.55 },
    { "Nagyon aktív", 1.725 }
};

        // Egy metódus a TDEE kiszámításához (példa súllyal és magassággal kiegészítve)
        private double CalculateTDEE(double weight, double height)
        {
            double bmr;
            if (Gender == "Férfi")
            {
                bmr = (10 * weight) + (6.25 * height) - (5 * Age) + 5;
            }
            else
            {
                bmr = (10 * weight) + (6.25 * height) - (5 * Age) - 161;
            }

            Tdee = Math.Round(bmr * ActivityLevel);
            return Tdee;
        }

        // Váltás a Bejelentkezés és Regisztráció mód között
        [RelayCommand]
        private void SwitchMode()
        {
            IsRegisterMode = !IsRegisterMode;
            Title = IsRegisterMode ? "Fiók létrehozása" : "Bejelentkezés";
            ButtonText = IsRegisterMode ? "Regisztráció" : "Belépés";
            SwitchText = IsRegisterMode ? "Már van fiókod? Jelentkezz be!" : "Nincs még fiókod? Regisztrálj!";
        }

        public AuthenticationViewModel(RegLogService regLogService, SessionService sessionService)
        {
            _logService = regLogService;
            _sessionService = sessionService;
            Logout();
        }
        [RelayCommand]
        private async Task AuthAction()
        {
            if (!_sessionService.IsLoggedIn)
            {


                if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                {
                    await Shell.Current.DisplayAlert("Hiba", "Minden mezőt ki kell tölteni!", "OK");
                    return;
                }

                if (IsRegisterMode && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(Password) && !string.IsNullOrEmpty(Gender) && weight != 0 && height != 0)
                {
                    // Regisztrációs logika helye (pl. API hívás)
                    User user = new User
                    {
                        Email = Email,
                        Name = username,
                        Password = password,
                        Tdee = Convert.ToInt32(tdee),
                        Gender = Gender,
                        Age = Age,
                        Activity_level = Convert.ToInt32(activityLevel)
                    };
                    user = _logService.Register(user);
                    _sessionService.UserId = user.Id.ToString();
                    await Shell.Current.DisplayAlert("Siker", "Regisztráció sikeres!", "OK");

                    await Shell.Current.GoToAsync("//MainPage");
                }
                else if (isRegisterMode)
                {
                    await Shell.Current.DisplayAlert("Hiba", "Minden mezőt ki kell tölteni!", "OK");
                    return;
                }
                if (!IsRegisterMode)
                {
                    int userid = -1;
                    var i = _logService.Login(Email, Password);
                    if (i == 1)
                    {
                        await Shell.Current.DisplayAlert("Siker", "Bejelentkezés sikeres!", "OK");
                        userid = _logService.GetUser(Email, Password);
                        _sessionService.UserId = userid.ToString();
                        await Shell.Current.GoToAsync("//MainPage");
                    }
                    else if (i == 0)
                    {
                        await Shell.Current.DisplayAlert("Hiba", "Hibás felhasználónév vagy jelszó!", "OK");
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Hiba", "A felhasználó nem létezik!", "OK");
                    }
                }
            }
            else
            {
                await Shell.Current.GoToAsync("//MainPage");
            }
        }

        public void Logout()
        {
            _sessionService.Logout();
        }

        public async Task LoggedInAlready()
        {
            if (_sessionService.UserId != "")
            {
                await Shell.Current.GoToAsync("//MainPage");
            }
        }
    }
}
