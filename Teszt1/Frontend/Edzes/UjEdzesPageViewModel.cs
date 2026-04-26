using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Teszt1.Frontend.Edzes
{
    public partial class UjEdzesPageViewModel : ObservableObject
    {
        // Ezek a változók vannak a beviteli mezőkben (kicsivel kezdődnek!)
        [ObservableProperty] private string edzesNeve;
        [ObservableProperty] private string ujGyakorlatNeve;
        [ObservableProperty] private string ujSorozat;
        [ObservableProperty] private string ujIsmetles;

        // Ez a lista fogja tartalmazni a felvett gyakorlatokat, mielőtt elmented az adatbázisba
        public ObservableCollection<string> FelvettGyakorlatok { get; set; } = new ObservableCollection<string>();

        public UjEdzesPageViewModel()
        {
            // Inicializálás, ha szükséges
        }

        // 1. Gomb: Amikor hozzáadsz egy gyakorlatot a listához
        [RelayCommand]
        public void GyakorlatHozzaadasa()
        {
            if (!string.IsNullOrWhiteSpace(UjGyakorlatNeve) && !string.IsNullOrWhiteSpace(UjSorozat) && !string.IsNullOrWhiteSpace(UjIsmetles))
            {
                // Összerakjuk a szöveget: "Fekvenyomás - 4x10"
                string gyakorlatSzoveg = $"{UjGyakorlatNeve} - {UjSorozat}x{UjIsmetles}";

                // Hozzáadjuk a listához (ettől azonnal megjelenik a képernyőn!)
                FelvettGyakorlatok.Add(gyakorlatSzoveg);

                // Kiürítjük a beviteli mezőket a következő gyakorlathoz
                UjGyakorlatNeve = string.Empty;
                UjSorozat = string.Empty;
                UjIsmetles = string.Empty;
            }
            else
            {
                App.Current.MainPage.DisplayAlert("Hiba", "Tölts ki minden mezőt a gyakorlathoz!", "OK");
            }
        }

        // 2. Gomb: A végső mentés az adatbázisba
        [RelayCommand]
        public async Task EdzesMentes()
        {
            if (string.IsNullOrWhiteSpace(EdzesNeve) || FelvettGyakorlatok.Count == 0)
            {
                await App.Current.MainPage.DisplayAlert("Hiba", "Adj nevet az edzésnek és vegyél fel legalább egy gyakorlatot!", "OK");
                return;
            }

            // --- IDE JÖN MAJD A MYSQL MENTÉS LOGIKÁJA KÉSŐBB ---
            // Például: _databaseService.UjEdzesMentese(EdzesNeve, FelvettGyakorlatok);

            await App.Current.MainPage.DisplayAlert("Siker", $"A(z) {EdzesNeve} sikeresen mentve {FelvettGyakorlatok.Count} gyakorlattal!", "OK");

            // Visszalépünk az előző oldalra (az Edzéstervhez)
            await Shell.Current.GoToAsync("..");
        }
    }
}