using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;

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
        public StatisztikaPageViewModel()
        {
            FrissitsAdatokat();
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
            FrissitsAdatokat();
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
            FrissitsAdatokat();
        }

        [RelayCommand]
        public void FrissitsAdatokat()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                GrafikonOszlopok.Clear();
                TopLista.Clear();

                var rnd = new Random();
                Color alapSzin = IsEdzesAktiv ? Color.FromArgb("#00E676") : Color.FromArgb("#FF9800");

                if (!IsEdzesAktiv)
                {
                    AktualisCim = "Kalóriabevitel elemzés";
                    OsszesitoCimke1 = "Összes bevitt kalória";
                    OsszesitoErtek1 = rnd.Next(15000, 25000).ToString("N0") + " kcal";
                    OsszesitoCimke2 = "Napi átlag kalória";
                    OsszesitoErtek2 = rnd.Next(2000, 3500).ToString("N0") + " kcal";
                    TopListaCim = "Top 5 leggyakoribb étel";

                    string[] etelek = { "Csirkemell", "Rizs", "Zabpehely", "Tojás", "Protein", "Banán", "Édesburgonya" };
                    var rendezettEtelek = etelek.Select(e => new { Nev = e, Darab = rnd.Next(5, 45) })
                                                .OrderByDescending(x => x.Darab)
                                                .ToList();

                    for (int i = 0; i < Math.Min(5, rendezettEtelek.Count); i++)
                    {
                        TopLista.Add(new TopListaElem
                        {
                            Helyezes = i + 1,
                            Nev = rendezettEtelek[i].Nev,
                            ErtekSzoveg = $"{rendezettEtelek[i].Darab} alkalommal"
                        });
                    }

                    RajzoldKiGrafikont(new int[] { 2100, 1850, 2400, 2200, 1900, 2600, 2150 }, new[] { "H", "K", "Sze", "Cs", "P", "Szo", "V" }, alapSzin);
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

                    RajzoldKiGrafikont(new int[] { 80, 120, 150, 100, 90, 110, 70 }, new[] { "Fekv", "Gugg", "Felh", "Evez", "Bic", "Tri", "Váll" }, alapSzin);
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