using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Database;
using Teszt1.Bakckend.Services;

namespace Teszt1.Frontend.Statisztika
{
    public class GrafikonOszlop
    {
        public int Magassag { get; set; }
        public string Cimke { get; set; }
        public string Szin { get; set; }
    }

    public class TopListaElem
    {
        public int Helyezes { get; set; }
        public string Nev { get; set; }
        public string ErtekSzoveg { get; set; }
        public string HelyezesText => $"{Helyezes}.";
        public string HatterSzin => Helyezes == 1 ? "#FFD700" : (Helyezes == 2 ? "#C0C0C0" : (Helyezes == 3 ? "#CD7F32" : "#B0000000"));
        public string SzovegSzin => Helyezes <= 3 ? "Black" : "White";
    }

    public partial class StatisztikaPageViewModel : ObservableObject
    {
        private readonly WorkoutService _workoutService;
        private readonly MealService _mealService;

        // UI Állapotok
        [ObservableProperty] private bool isEdzesAktiv = true;
        [ObservableProperty] private string edzesTabSzin = "#6200EE";
        [ObservableProperty] private string etrendTabSzin = "Transparent";
        [ObservableProperty] private string topListaCim = "Legnagyobb súlyok";
        [ObservableProperty] private string yMaxSzoveg = "0";
        [ObservableProperty] private string kivalasztottIdoszak = "7 nap";

        // Adatlisták a UI-hoz
        [ObservableProperty] private ObservableCollection<GrafikonOszlop> grafikonOszlopok = new();
        [ObservableProperty] private ObservableCollection<TopListaElem> topLista = new();

        public StatisztikaPageViewModel(WorkoutService workoutService, MealService mealService)
        {
            _workoutService = workoutService;
            _mealService = mealService;

            // Első betöltés
            _ = FrissitsAdatokat();
        }
        [RelayCommand]
        public async Task IdoszakValtasa(string napok)
        {
            KivalasztottIdoszak = napok switch
            {
                "7" => "7 nap",
                "30" => "30 nap",
                _ => "Összes"
            };

            int napokSzama = int.Parse(napok);
            await FrissitsAdatokat(napokSzama);
        }
        

        [RelayCommand]
        public async Task TabValtas(string tabNev)
        {
            IsEdzesAktiv = (tabNev == "Edzes");
            EdzesTabSzin = IsEdzesAktiv ? "#6200EE" : "Transparent";
            EtrendTabSzin = !IsEdzesAktiv ? "#27AE60" : "Transparent";

            await FrissitsAdatokat();
        }

        
        [RelayCommand]
        public async Task FrissitsAdatokat(int napok = 7)
        {
            int userId = 1;

            if (IsEdzesAktiv)
            {
                // EDZÉS: Grafikon és a PR-ok (ahogy az előbb írtuk)
                var adatok = _workoutService.GetEdzesStatisztika(userId, napok);
                RajzoldKiGrafikont(adatok, "#6200EE");

                var top = _workoutService.GetLegnehezebbGyakorlatok(userId);
                TopLista = new ObservableCollection<TopListaElem>(top.Select((x, i) => new TopListaElem
                {
                    Helyezes = i + 1,
                    Nev = x.Nev,
                    ErtekSzoveg = $"{x.MaxSuly} kg"
                }));
            }
            else
            {
                // ÉTREND: Kalória grafikon
                var adatok = _mealService.GetKaloriaStatisztika(userId, napok);
                RajzoldKiGrafikont(adatok, "#27AE60");

                // ÉTREND: Makrók lekérése a kördiagramhoz vagy összefoglalóhoz
                var atlagMakro = _mealService.GetAtlagosMakrok(userId, napok);

                TopListaCim = $"Napi átlag makrók ({napok} nap)";
                TopLista = new ObservableCollection<TopListaElem>
        {
            new TopListaElem { Helyezes = 1, Nev = "Fehérje", ErtekSzoveg = $"{(int)atlagMakro.Protein}g" },
            new TopListaElem { Helyezes = 2, Nev = "Szénhidrát", ErtekSzoveg = $"{(int)atlagMakro.Carbs}g" },
            new TopListaElem { Helyezes = 3, Nev = "Zsír", ErtekSzoveg = $"{(int)atlagMakro.Fat}g" }
        };
            }
        }

        private void RajzoldKiGrafikont(List<GrafikonAdatDto> adatok, string szin)
        {
            if (adatok == null || !adatok.Any())
            {
                GrafikonOszlopok = new ObservableCollection<GrafikonOszlop>();
                YMaxSzoveg = "0";
                return;
            }

            float maxErtek = adatok.Max(x => x.Ertek);
            if (maxErtek == 0) maxErtek = 1;

            var ujOszlopok = new List<GrafikonOszlop>();
            foreach (var d in adatok)
            {
                ujOszlopok.Add(new GrafikonOszlop
                {
                    Cimke = d.Cimke,
                    Szin = szin,
                    // Magasság skálázása 0-150px közé
                    Magassag = (int)((d.Ertek / maxErtek) * 150)
                });
            }

            GrafikonOszlopok = new ObservableCollection<GrafikonOszlop>(ujOszlopok);
            YMaxSzoveg = maxErtek >= 1000 ? (maxErtek / 1000.0).ToString("0.0") + "k" : ((int)maxErtek).ToString();
        }
    }
}