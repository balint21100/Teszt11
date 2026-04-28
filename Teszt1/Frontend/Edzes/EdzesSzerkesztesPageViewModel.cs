using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Teszt1;

namespace Teszt1.Frontend.Edzes
{
    [QueryProperty(nameof(EdzesNev), "EdzesNev")]
    [QueryProperty(nameof(Nap), "Nap")]
    public partial class EdzesSzerkesztesPageViewModel : ObservableObject
    {
        [ObservableProperty] private string nap;
        [ObservableProperty] private string edzesNev;
        [ObservableProperty] private string eredetiNev;

        [ObservableProperty] private bool isSulyemelesAktiv;
        [ObservableProperty] private bool isEgyebAktiv;

        [ObservableProperty] private string sulyGyakorlatNeve;
        [ObservableProperty] private string suly;
        [ObservableProperty] private string ismetles;
        [ObservableProperty] private string sulyMegjegyzes;
        [ObservableProperty] private string egyebTvekenysegNeve;
        [ObservableProperty] private string egyebMegjegyzes;

        public ObservableCollection<string> FelvettGyakorlatok { get; set; } = new();
        private Dictionary<string, int> _gyakorlatSzettek = new();

        partial void OnEdzesNevChanged(string value)
        {
            if (string.IsNullOrEmpty(value)) return;
            if (string.IsNullOrEmpty(EredetiNev)) EredetiNev = value;

            FelvettGyakorlatok.Clear();
            _gyakorlatSzettek.Clear();

            // Később itt egy MySQL lekérdezés lesz: 
            // FelvettGyakorlatok = _databaseService.GyakorlatokLekerdezese(Nap, EredetiNev);

            // Addig is, hogy a felület jól nézzen ki, eldöntjük a fülek állapotát a név alapján
            if (value.ToLower().Contains("mell") || value.ToLower().Contains("bicepsz") || value.ToLower().Contains("súly") || value.ToLower().Contains("láb") || value.ToLower().Contains("hát"))
            {
                IsSulyemelesAktiv = true;
                IsEgyebAktiv = false;
            }
            else
            {
                IsSulyemelesAktiv = false;
                IsEgyebAktiv = true;
            }
        }

        [RelayCommand]
        public void TorolGyakorlat(string gyakorlat)
        {
            if (FelvettGyakorlatok.Contains(gyakorlat))
            {
                FelvettGyakorlatok.Remove(gyakorlat);
            }
        }

        [RelayCommand]
        public void SulyGyakorlatHozzaadasa()
        {
            if (!string.IsNullOrWhiteSpace(SulyGyakorlatNeve) && !string.IsNullOrWhiteSpace(Suly) && !string.IsNullOrWhiteSpace(Ismetles))
            {
                string tiszta = SulyGyakorlatNeve.Trim();
                if (!_gyakorlatSzettek.ContainsKey(tiszta)) _gyakorlatSzettek[tiszta] = 1;
                else _gyakorlatSzettek[tiszta]++;

                FelvettGyakorlatok.Add($"🏋️ {tiszta} | {_gyakorlatSzettek[tiszta]}. szett: {Suly}kg x {Ismetles} ism.");
                Suly = Ismetles = SulyMegjegyzes = string.Empty;
            }
        }

        [RelayCommand]
        public void EgyebGyakorlatHozzaadasa()
        {
            string nev = string.IsNullOrWhiteSpace(EgyebTvekenysegNeve) ? "Gyakorlat" : EgyebTvekenysegNeve.Trim();
            string szoveg = $"🏃 {nev}";
            if (!string.IsNullOrWhiteSpace(EgyebMegjegyzes)) szoveg += $" ({EgyebMegjegyzes})";
            FelvettGyakorlatok.Add(szoveg);
            EgyebTvekenysegNeve = EgyebMegjegyzes = string.Empty;
        }

        [RelayCommand]
        public async Task EdzesMentes()
        {
            WeakReferenceMessenger.Default.Send(new EdzesModositvaUzenet
            {
                Nap = Nap,
                RegiNev = EredetiNev,
                UjNev = EdzesNev
            });

            await App.Current.MainPage.DisplayAlert("Siker", "Módosítások mentve!", "OK");
            await Microsoft.Maui.Controls.Shell.Current.GoToAsync("..");
        }

        [RelayCommand]
        public async Task EdzesTorlese()
        {
            bool valasz = await App.Current.MainPage.DisplayAlert("Törlés", "Biztosan törlöd a teljes edzést?", "Igen", "Nem");
            if (valasz)
            {
                WeakReferenceMessenger.Default.Send(new EdzesModositvaUzenet { Nap = Nap, RegiNev = EredetiNev, UjNev = null });
                await Microsoft.Maui.Controls.Shell.Current.GoToAsync("..");
            }
        }
    }

    // ---> EZ A RÉSZ HIÁNYZOTT AZ ELŐZŐBŐL! <---
    public class EdzesModositvaUzenet
    {
        public string Nap { get; set; }
        public string RegiNev { get; set; }
        public string UjNev { get; set; } // Ha null, akkor törlés történt
    }
}