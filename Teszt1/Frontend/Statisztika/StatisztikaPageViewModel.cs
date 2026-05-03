using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Graphics;
using System.Collections.ObjectModel;
using Teszt1.Bakckend.Services;

namespace Teszt1.Frontend.Statisztika
{
    public class GrafikonOszlop
    {
        public double Magassag { get; set; }
        public string Cimke { get; set; }
        public Color Szin { get; set; }
    }

    public class TopListaElem
    {
        public int Helyezes { get; set; }
        public string Nev { get; set; }
        public string ErtekSzoveg { get; set; }
        public string HelyezesText => $"{Helyezes}.";
        public Color HatterSzin => Helyezes == 1 ? Color.FromArgb("#FFD700") :
                                  (Helyezes == 2 ? Color.FromArgb("#C0C0C0") :
                                  (Helyezes == 3 ? Color.FromArgb("#CD7F32") : Color.FromRgba(255, 255, 255, 0.2)));
        public Color SzovegSzin => Helyezes <= 3 ? Colors.Black : Colors.White;
    }

    public partial class StatisztikaPageViewModel : ObservableObject
    {
        private readonly SessionService _sessionService;
        private readonly MealService _mealService;
        private readonly WorkoutService _workoutService;
        public StatisztikaPageViewModel(SessionService session, MealService mealService, WorkoutService workoutService)
        {
            _sessionService = session;
            _mealService = mealService;
            _workoutService = workoutService;
            FrissitsAdatokat(aktualisIdotav);
        }

        [ObservableProperty] private bool isEdzesAktiv = true;
        [ObservableProperty] private string aktualisIdotav = "Heti";

        [ObservableProperty] private Color edzesTabSzin = Color.FromArgb("#6200EE");
        [ObservableProperty] private Color etkezesTabSzin = Colors.Transparent;

        [ObservableProperty] private Color hetiSzin = Color.FromArgb("#6200EE");
        [ObservableProperty] private Color haviSzin = Colors.Transparent;
        [ObservableProperty] private Color evesSzin = Colors.Transparent;
        [ObservableProperty] private Color egyeniSzin = Colors.Transparent;

        [ObservableProperty] private bool isHetiLathato = true;
        [ObservableProperty] private bool isHaviLathato = false;
        [ObservableProperty] private bool isEvesLathato = false;
        [ObservableProperty] private bool isEgyeniLathato = false;

        public List<string> Hetek { get; set; } = new() { "Aktuális hét", "Előző hét", "2 héttel ezelőtt" };
        public List<string> Honapok { get; set; } = new() { "Január", "Február", "Március", "Április", "Május", "Június", "Július", "Augusztus", "Szeptember", "Október", "November", "December" };
        public List<string> Evek { get; set; } = new() { "2024", "2025", "2026" };

        [ObservableProperty] private string kivalasztottHet = "Aktuális hét";
        [ObservableProperty] private string kivalasztottHonap = "Május";
        [ObservableProperty] private string kivalasztottEv = "2026";
        [ObservableProperty] private DateTime kezdoDatum = DateTime.Now.AddDays(-7);
        [ObservableProperty] private DateTime vegDatum = DateTime.Now;

        [ObservableProperty] private string osszesitoCimke1;
        [ObservableProperty] private string osszesitoErtek1;
        [ObservableProperty] private string osszesitoCimke2;
        [ObservableProperty] private string osszesitoErtek2;

        [ObservableProperty] private string aktualisCim;
        [ObservableProperty] private string yMaxSzoveg;
        [ObservableProperty] private string yMidSzoveg;
        [ObservableProperty] private string topListaCim;

        public ObservableCollection<GrafikonOszlop> GrafikonOszlopok { get; set; } = new();
        public ObservableCollection<TopListaElem> TopLista { get; set; } = new();

        [RelayCommand]
        public void TabValtas(string tab)
        {
            IsEdzesAktiv = (tab == "Edzes");
            EdzesTabSzin = IsEdzesAktiv ? Color.FromArgb("#6200EE") : Colors.Transparent;
            EtkezesTabSzin = !IsEdzesAktiv ? Color.FromArgb("#6200EE") : Colors.Transparent;
            FrissitsAdatokat(aktualisIdotav);
        }

        [RelayCommand]
        public void IdotavValtas(string idotav)
        {
            AktualisIdotav = idotav;
            HetiSzin = HaviSzin = EvesSzin = EgyeniSzin = Colors.Transparent;
            IsHetiLathato = IsHaviLathato = IsEvesLathato = IsEgyeniLathato = false;

            switch (idotav)
            {
                case "Heti": HetiSzin = Color.FromArgb("#6200EE"); IsHetiLathato = true; break;
                case "Havi": HaviSzin = Color.FromArgb("#6200EE"); IsHaviLathato = true; break;
                case "Eves": EvesSzin = Color.FromArgb("#6200EE"); IsEvesLathato = true; break;
                case "Egyeni": EgyeniSzin = Color.FromArgb("#6200EE"); IsEgyeniLathato = true; break;
            }
            FrissitsAdatokat(aktualisIdotav);
        }

        [RelayCommand]
        public void FrissitsAdatokat(string mod)
        {
            Random rnd = new Random();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                if (!IsEdzesAktiv) {
                GrafikonOszlopok.Clear();
                TopLista.Clear();

                var adatok = _mealService.GetStatisztika(int.Parse(_sessionService.UserId), mod);

                // Az adatokból kinyerjük a tömböket a grafikonrajzolóhoz
                int[] ertekek = adatok.Select(x => (int)x.Ertek).ToArray();
                string[] cimkek = adatok.Select(x => x.Name).ToArray();

                Color alapSzin = IsEdzesAktiv ? Color.FromArgb("#00E676") : Color.FromArgb("#FF9800");

                // Itt hívod meg a meglévő rajzolódat
                RajzoldKiGrafikont(ertekek, cimkek, alapSzin);

                // Összesítők frissítése a lekrt adatok alapján
                OsszesitoErtek1 = ertekek.Sum().ToString("N0") + " kcal";

                
                }
                else
                {
                    AktualisCim = "Gyakorlatok terhelése";
                    OsszesitoCimke1 = "Összes súly";
                    OsszesitoErtek1 = rnd.Next(40000, 100000).ToString("N0") + " kg";
                    OsszesitoCimke2 = "Átlagos napi";
                    OsszesitoErtek2 = rnd.Next(5000, 12000).ToString("N0") + " kg";
                    TopListaCim = "Top 5 leggyakoribb gyakorlat";

                    string[] gyakorlatok = { "Fekvenyomás", "Guggolás", "Felhúzás", "Evezés", "Bicepsz", "Tricepsz", "Kitörés" };
                    var rendezettGyakorlatok = gyakorlatok.Select(g => new { Nev = g, Darab = rnd.Next(2, 25) })
                                                          .OrderByDescending(x => x.Darab)
                                                          .ToList();

                    for (int i = 0; i < Math.Min(5, rendezettGyakorlatok.Count); i++)
                    {
                        TopLista.Add(new TopListaElem
                        {
                            Helyezes = i + 1,
                            Nev = rendezettGyakorlatok[i].Nev,
                            ErtekSzoveg = $"{rendezettGyakorlatok[i].Darab} alkalommal"
                        });
                    }
                    Color alapszin = Color.FromArgb("#6200EE");
                    RajzoldKiGrafikont(new int[] { 80, 120, 150, 100, 90, 110, 70 }, new[] { "Fekv", "Gugg", "Felh", "Evez", "Bic", "Tri", "Váll" }, alapszin);
                }
            });
        }

        private void RajzoldKiGrafikont(int[] ertekek, string[] cimkek, Color szin)
        {
            if (ertekek.Length == 0) return;
            double maxErtek = ertekek.Max();
            if (maxErtek == 0) maxErtek = 1;

            YMaxSzoveg = maxErtek.ToString("N0");
            YMidSzoveg = (maxErtek / 2).ToString("N0");

            double maxMegjelenithetoMagassag = 150;
            double szorzo = maxMegjelenithetoMagassag / maxErtek;

            for (int i = 0; i < ertekek.Length; i++)
            {
                GrafikonOszlopok.Add(new GrafikonOszlop
                {
                    Magassag = ertekek[i] * szorzo,
                    Cimke = i < cimkek.Length ? cimkek[i] : (i + 1).ToString(),
                    Szin = szin
                });
            }
        }
    }
}