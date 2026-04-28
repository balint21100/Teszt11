using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Teszt1;

namespace Teszt1.Frontend.Edzes
{
    // Ezt az üzenetet küldjük át a főoldalnak mentéskor
    public class EdzesMentveUzenet
    {
        public string Nap { get; set; }
        public string EdzesNeve { get; set; }
    }

    // Így vesszük át a navigációból a napot
    [QueryProperty(nameof(KivalasztottNap), "Nap")]
    public partial class UjEdzesPageViewModel : ObservableObject
    {
        [ObservableProperty] private string kivalasztottNap; // Ide kerül pl. hogy "Hétfő"

        [ObservableProperty] private string edzesNeve;
        public ObservableCollection<string> FelvettGyakorlatok { get; set; } = new ObservableCollection<string>();
        private Dictionary<string, int> _gyakorlatSzettek = new Dictionary<string, int>();

        [ObservableProperty] private bool isSulyemelesAktiv = true;
        [ObservableProperty] private bool isEgyebAktiv = false;
        [ObservableProperty] private string sulyemelesTabSzin = "#6200EE";
        [ObservableProperty] private string egyebTabSzin = "Transparent";

        [ObservableProperty] private string sulyGyakorlatNeve;
        [ObservableProperty] private string suly;
        [ObservableProperty] private string ismetles;
        [ObservableProperty] private string sulyMegjegyzes;

        [ObservableProperty] private string egyebTvekenysegNeve;
        [ObservableProperty] private string egyebMegjegyzes;

        [RelayCommand]
        public void TabValtas(string tabNeve)
        {
            if (tabNeve == "Sulyemeles")
            {
                IsSulyemelesAktiv = true;
                IsEgyebAktiv = false;
                SulyemelesTabSzin = "#6200EE";
                EgyebTabSzin = "Transparent";
            }
            else if (tabNeve == "Egyeb")
            {
                IsSulyemelesAktiv = false;
                IsEgyebAktiv = true;
                SulyemelesTabSzin = "Transparent";
                EgyebTabSzin = "#6200EE";
            }
        }

        [RelayCommand]
        public void SulyGyakorlatHozzaadasa()
        {
            if (!string.IsNullOrWhiteSpace(SulyGyakorlatNeve) && !string.IsNullOrWhiteSpace(Suly) && !string.IsNullOrWhiteSpace(Ismetles))
            {
                string tisztaGyakorlatNev = SulyGyakorlatNeve.Trim();

                if (!_gyakorlatSzettek.ContainsKey(tisztaGyakorlatNev))
                    _gyakorlatSzettek[tisztaGyakorlatNev] = 1;
                else
                    _gyakorlatSzettek[tisztaGyakorlatNev]++;

                int aktualisSzettSzam = _gyakorlatSzettek[tisztaGyakorlatNev];
                string gyakorlatSzoveg = $"🏋️ {tisztaGyakorlatNev} | {aktualisSzettSzam}. szett: {Suly}kg x {Ismetles} ism.";

                if (!string.IsNullOrWhiteSpace(SulyMegjegyzes))
                    gyakorlatSzoveg += $" ({SulyMegjegyzes})";

                FelvettGyakorlatok.Add(gyakorlatSzoveg);

                Suly = string.Empty;
                Ismetles = string.Empty;
                SulyMegjegyzes = string.Empty;
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Hiányzó adat", "A gyakorlat neve, a súly és az ismétlés kötelező!", "OK");
            }
        }

        [RelayCommand]
        public void EgyebGyakorlatHozzaadasa()
        {
            if (string.IsNullOrWhiteSpace(EgyebTvekenysegNeve) && string.IsNullOrWhiteSpace(EgyebMegjegyzes)) return;

            string nev = string.IsNullOrWhiteSpace(EgyebTvekenysegNeve) ? "Egyéb gyakorlat" : EgyebTvekenysegNeve.Trim();
            string gyakorlatSzoveg = $"🏃 {nev}";

            if (!string.IsNullOrWhiteSpace(EgyebMegjegyzes))
                gyakorlatSzoveg += $" ({EgyebMegjegyzes})";

            FelvettGyakorlatok.Add(gyakorlatSzoveg);

            EgyebTvekenysegNeve = string.Empty;
            EgyebMegjegyzes = string.Empty;
        }

        [RelayCommand]
        public async Task EdzesMentes()
        {
            if (string.IsNullOrWhiteSpace(EdzesNeve))
            {
                await App.Current.MainPage.DisplayAlert("Hiba", "Kérlek, legalább az edzés nevét add meg a mentéshez!", "OK");
                return;
            }

            // ÜZENET KÜLDÉSE A FŐOLDALNAK! Megmondjuk neki, hogy frissítse a listát!
            WeakReferenceMessenger.Default.Send(new EdzesMentveUzenet
            {
                Nap = KivalasztottNap,
                EdzesNeve = EdzesNeve
            });

            await App.Current.MainPage.DisplayAlert("Sikeres mentés", $"A(z) {EdzesNeve} bekerült a {KivalasztottNap}i listába!", "OK");
            await Microsoft.Maui.Controls.Shell.Current.GoToAsync("..");
        }
    }
}