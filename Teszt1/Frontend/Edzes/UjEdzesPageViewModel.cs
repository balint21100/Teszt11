using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Teszt1;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Services;

namespace Teszt1.Frontend.Edzes
{
    // Ezt az üzenetet küldjük át a főoldalnak mentéskor
    // Üzenet osztály a főoldal frissítéséhez
    public class EdzesMentveUzenet
    {
        public string Nap { get; set; }
        public string EdzesNeve { get; set; }
    }

    [QueryProperty(nameof(KivalasztottNap), "Nap")]
    public partial class UjEdzesPageViewModel : ObservableObject
    {
        private readonly WorkoutService _workoutService;
        private readonly SessionService _sessionService;

        [ObservableProperty] private string kivalasztottNap;
        [ObservableProperty] private string edzesNeve;

        // UI megjelenítéshez (szöveges lista)
        public ObservableCollection<string> FelvettGyakorlatok { get; set; } = new ObservableCollection<string>();

        // Backend mentéshez (objektum lista)
        private List<WorkoutEntryDto> _mentendoTetelek = new List<WorkoutEntryDto>();

        // Súlyzós edzés mezők
        [ObservableProperty] private string gyakorlatNeve;
        [ObservableProperty] private string suly;
        [ObservableProperty] private string ismetles;
        [ObservableProperty] private string sulyMegjegyzes;

        // Egyéb edzés mezők
        [ObservableProperty] private string egyebTvekenysegNeve;
        [ObservableProperty] private string egyebMegjegyzes;

        // Tab kezelés
        [ObservableProperty] private bool isSulyemelesAktiv = true;
        [ObservableProperty] private bool isEgyebAktiv = false;
        [ObservableProperty] private string sulyemelesTabSzin = "#6200EE";
        [ObservableProperty] private string egyebTabSzin = "Transparent";

        public UjEdzesPageViewModel(WorkoutService workoutService, SessionService session)
        {
            _workoutService = workoutService;
            _sessionService = session;
        }

        // MAUI default konstruktor, ha a DI nincs beállítva (opcionális)


        [RelayCommand]
        public void TabValtas(string tab)
        {
            if (tab == "Sulyemeles")
            {
                IsSulyemelesAktiv = true;
                IsEgyebAktiv = false;
                SulyemelesTabSzin = "#6200EE";
                EgyebTabSzin = "Transparent";
            }
            else
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
            if (string.IsNullOrWhiteSpace(GyakorlatNeve) || string.IsNullOrWhiteSpace(Suly) || string.IsNullOrWhiteSpace(Ismetles))
                return;

            if (float.TryParse(Suly, out float sulyErtek) && int.TryParse(Ismetles, out int ismetlesErtek))
            {
                // 1. Hozzáadás a belső listához a mentéshez
                _mentendoTetelek.Add(new WorkoutEntryDto
                {
                    ExerciseName = GyakorlatNeve,
                    Weight = sulyErtek,
                    Reps = ismetlesErtek,
                    Sets = 1 // Alapértelmezett 1 szett, vagy bővíthető a UI
                });

                // 2. Megjelenítés a listában
                FelvettGyakorlatok.Add($"💪 {GyakorlatNeve} - {Suly}kg x {Ismetles}");

                // Mezők ürítése
                GyakorlatNeve = string.Empty;
                Suly = string.Empty;
                Ismetles = string.Empty;
            }
        }

        [RelayCommand]
        public void EgyebGyakorlatHozzaadasa()
        {
            if (string.IsNullOrWhiteSpace(EgyebTvekenysegNeve)) return;

            // Egyéb edzésnél (pl. futás) 0 súlyt és 0 ismétlést mentünk, vagy a megjegyzésbe tesszük
            _mentendoTetelek.Add(new WorkoutEntryDto
            {
                ExerciseName = EgyebTvekenysegNeve,
                Weight = 0,
                Reps = 0,
                Sets = 0
            });

            string gyakorlatSzoveg = $"🏃 {EgyebTvekenysegNeve}";
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
                await App.Current.MainPage.DisplayAlert("Hiba", "Kérlek, adj meg egy nevet az edzésnek!", "OK");
                return;
            }

            if (_mentendoTetelek.Count == 0)
            {
                await App.Current.MainPage.DisplayAlert("Hiba", "Még nem adtál hozzá gyakorlatot!", "OK");
                return;
            }

            try
            {
                // TODO: Itt kérd le az aktuális bejelentkezett User Id-ját
                int tempUserId = Convert.ToInt32(_sessionService.UserId);

                // Hívjuk a service-t a mentéshez
                _workoutService.AddWorkoutWithEntries(tempUserId, EdzesNeve, sulyMegjegyzes, _mentendoTetelek);

                // Üzenet a főoldalnak
                WeakReferenceMessenger.Default.Send(new EdzesMentveUzenet
                {
                    Nap = KivalasztottNap,
                    EdzesNeve = EdzesNeve
                });

                await App.Current.MainPage.DisplayAlert("Siker", "Edzés sikeresen mentve!", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Hiba", $"Hiba történt mentés közben: {ex.Message}", "OK");
            }
        }

    }
}