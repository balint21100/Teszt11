using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Teszt1.Frontend;
using Teszt1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Frontend
{
    public partial class EdzesPageViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService;

        public ObservableCollection<WorkoutDay> WeekDays { get; set; }

        public EdzesPageViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;

            // Generáljuk a 7 napot
            WeekDays = new ObservableCollection<WorkoutDay>
            {
                new WorkoutDay { DayName = "Hétfő", PlannedExercise = "" },
                new WorkoutDay { DayName = "Kedd", PlannedExercise = "" },
                new WorkoutDay { DayName = "Szerda", PlannedExercise = "" },
                new WorkoutDay { DayName = "Csütörtök", PlannedExercise = "" },
                new WorkoutDay { DayName = "Péntek", PlannedExercise = "" },
                new WorkoutDay { DayName = "Szombat", PlannedExercise = "" },
                new WorkoutDay { DayName = "Vasárnap", PlannedExercise = "" }
            };

            // Melyik nap van ma? (Kiemeléshez)
            int todayIndex = (int)DateTime.Today.DayOfWeek;
            int myIndex = todayIndex == 0 ? 6 : todayIndex - 1;
            WeekDays[myIndex].IsCurrentDay = true;
        }

        [RelayCommand]
        public void ClearDay(WorkoutDay day)
        {
            if (day != null)
            {
                day.PlannedExercise = string.Empty; // Törli a szövegdobozt
            }
        }

        [RelayCommand]
        public async Task SavePlanAsync()
        {
            var kitoltottNapok = WeekDays.Where(n => !string.IsNullOrWhiteSpace(n.PlannedExercise)).ToList();

            if (kitoltottNapok.Count == 0)
            {
                await Shell.Current.DisplayAlert("Hiba", "Legalább egy napra írj be valamit!", "OK");
                return;
            }

            string planName = await Shell.Current.DisplayPromptAsync("Mentés", "Milyen néven mentsük el a heti tervet?", "Mentés", "Mégsem", "pl. Kezdő terv");

            if (!string.IsNullOrWhiteSpace(planName))
            {
                try
                {
                    _databaseService.SaveWorkoutPlan(1, planName, kitoltottNapok);
                    await Shell.Current.DisplayAlert("Siker", $"A '{planName}' terv sikeresen elmentve!", "OK");
                }
                catch (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Hiba", $"Mentési hiba: {ex.Message}", "OK");
                }
            }
        }

        // Ezt hívja meg a Zöld Pipa gomb egy adott napnál
        [RelayCommand]
        public async Task SaveSingleDayAsync(WorkoutDay day)
        {
            if (day != null && !string.IsNullOrWhiteSpace(day.PlannedExercise))
            {
                // Később ide jöhet egy külön adatbázis mentés, egyelőre adunk egy vizuális visszajelzést!
                await Shell.Current.DisplayAlert("Szuper!", $"{day.DayName} napi terv rögzítése megtörtént.", "OK");
            }
        }
    }
}
