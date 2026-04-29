using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        [ObservableProperty] private bool isEdzesAktiv = true;
        [ObservableProperty] private string edzesTabSzin = "#6200EE";
        [ObservableProperty] private string etkezesTabSzin = "Transparent";

        [ObservableProperty] private string aktualisIdotav = "Heti";
        [ObservableProperty] private string hetiSzin = "#6200EE";
        [ObservableProperty] private string haviSzin = "Transparent";
        [ObservableProperty] private string evesSzin = "Transparent";
        [ObservableProperty] private string egyeniSzin = "Transparent";

        [ObservableProperty] private bool isHetiLathato = true;
        [ObservableProperty] private bool isHaviLathato = false;
        [ObservableProperty] private bool isEvesLathato = false;
        [ObservableProperty] private bool isEgyeniLathato = false;

        public List<string> Hetek { get; set; } = new() { "Aktuális hét", "Előző hét", "2 héttel ezelőtt" };
        public List<string> Honapok { get; set; } = new() { "Január", "Február", "Március", "Április", "Május", "Június", "Július", "Augusztus", "Szeptember", "Október", "November", "December" };
        public List<string> Evek { get; set; } = new() { "2024", "2025", "2026" };

        [ObservableProperty] private string kivalasztottHet = "Aktuális hét";
        [ObservableProperty] private string kivalasztottHonap = "Április";
        [ObservableProperty] private string kivalasztottEv = "2026";
        [ObservableProperty] private DateTime kezdoDatum = DateTime.Now.AddDays(-7);
        [ObservableProperty] private DateTime vegDatum = DateTime.Now;

        [ObservableProperty] private string osszesitoCimke1 = "Összesen";
        [ObservableProperty] private string osszesitoErtek1 = "0";
        [ObservableProperty] private string osszesitoCimke2 = "Átlagosan";
        [ObservableProperty] private string osszesitoErtek2 = "0";

        [ObservableProperty] private string aktualisCim = "";
        [ObservableProperty] private string yMaxSzoveg = "0";
        [ObservableProperty] private string yMidSzoveg = "0";
        [ObservableProperty] private string topListaCim = "Top Lista";

        private readonly MealService _mealService;

        public ObservableCollection<GrafikonOszlop> GrafikonOszlopok { get; set; } = new();
        public ObservableCollection<TopListaElem> TopLista { get; set; } = new();

        public StatisztikaPageViewModel()
        {
            AdatokFrissitese();
        }

        public StatisztikaPageViewModel(MealService mealService)
        {
            _mealService = mealService;
            AdatokFrissitese();
        }

        [RelayCommand]
        public void TabValtas(string tab)
        {
            if (tab == "Edzes")
            {
                IsEdzesAktiv = true;
                EdzesTabSzin = "#6200EE"; EtkezesTabSzin = "Transparent";
            }
            else
            {
                IsEdzesAktiv = false;
                EdzesTabSzin = "Transparent"; EtkezesTabSzin = "#6200EE";
            }
            AdatokFrissitese();
        }

        [RelayCommand]
        public void IdotavValtas(string idotav)
        {
            AktualisIdotav = idotav;
            HetiSzin = HaviSzin = EvesSzin = EgyeniSzin = "Transparent";
            IsHetiLathato = IsHaviLathato = IsEvesLathato = IsEgyeniLathato = false;

            if (idotav == "Heti") { HetiSzin = "#6200EE"; IsHetiLathato = true; }
            else if (idotav == "Havi") { HaviSzin = "#6200EE"; IsHaviLathato = true; }
            else if (idotav == "Eves") { EvesSzin = "#6200EE"; IsEvesLathato = true; }
            else if (idotav == "Egyeni") { EgyeniSzin = "#6200EE"; IsEgyeniLathato = true; }

            AdatokFrissitese();
        }


        [RelayCommand]
        private void AdatokFrissitese()
        {
            GrafikonOszlopok.Clear();
            TopLista.Clear();

            // 1. Grafikon feltöltése az elmúlt 7 nap adataival
            for (int i = 6; i >= 0; i--)
            {
                DateTime datum = DateTime.Today.AddDays(-i);
                // Itt a userId-t (pl. 1) add meg
                var napiMacros = _mealService.GetTodayNutritionSummary(1);
                // Megjegyzés: A GetTodayNutritionSummary-t érdemes kiegészíteni a Service-ben, 
                // hogy kapjon egy DateTime paramétert is, ne csak a mai napot nézze!

                GrafikonOszlopok.Add(new GrafikonOszlop
                {
                    Cimke = datum.ToString("ddd"), // Nap neve (Hé, Ke, stb.)
                    Magassag = (int)napiMacros.Protein,
                    Szin = "#6200EE"
                });
            }

            // 2. Toplista feltöltése a legutóbbi 3 étkezéssel
            var utolsoEtkezesek = _mealService.GetLastThreeMealsFormatted(1);
            int helyezes = 1;
            foreach (var etkezes in utolsoEtkezesek)
            {
                TopLista.Add(new TopListaElem
                {
                    Helyezes = helyezes++,
                    Nev = etkezes.Split('-')[0].Trim(), // Kinyerjük a nevet a formázott stringből
                    ErtekSzoveg = etkezes.Split('-')[1].Trim()
                });
            }
        }


        //[RelayCommand]
        //public void AdatokFrissitese()
        //{
        //    GrafikonOszlopok.Clear();
        //    TopLista.Clear();
        //    var rnd = new Random();
        //    string alapSzin = IsEdzesAktiv ? "#00E676" : "#FF9800";

        //    if (!IsEdzesAktiv)
        //    {
        //        AktualisCim = "Kalóriabevitel elemzés";
        //        OsszesitoCimke1 = "Összes bevitt kalória";
        //        OsszesitoErtek1 = rnd.Next(15000, 25000).ToString("N0") + " kcal";
        //        OsszesitoCimke2 = "Napi átlag kalória";
        //        OsszesitoErtek2 = rnd.Next(2000, 3500).ToString("N0") + " kcal";

        //        TopListaCim = "Top 10 leggyakoribb étel";
        //        List<string> etelek = new() { "Csirkemell", "Rizs", "Zabpehely", "Tojás", "Protein Shake", "Banán", "Édesburgonya", "Túró", "Alma", "Mandula" };
        //        for (int i = 0; i < 10; i++)
        //            TopLista.Add(new TopListaElem { Helyezes = i + 1, Nev = etelek[i], ErtekSzoveg = $"{rnd.Next(400, 4500)} kcal" });

        //        GeneraloMotor(7, new[] { "H", "K", "Sze", "Cs", "P", "Szo", "V" }, 1500, 4000, alapSzin);
        //    }
        //    else
        //    {
        //        AktualisCim = "Gyakorlatok terhelése";
        //        OsszesitoCimke1 = "Összes megmozgatott súly";
        //        OsszesitoErtek1 = rnd.Next(40000, 120000).ToString("N0") + " kg";
        //        OsszesitoCimke2 = "Átlagos terhelés";
        //        OsszesitoErtek2 = rnd.Next(5000, 15000).ToString("N0") + " kg";

        //        TopListaCim = "Top 10 leggyakoribb edzés";
        //        List<string> gyakorlatok = new() { "Fekvenyomás", "Guggolás", "Felhúzás", "Mellről nyomás", "Evezés", "Bicepsz", "Húzódzkodás", "Tricepsz", "Kitörés", "Vádli" };
        //        int[] topSulyok = new int[10];
        //        for (int i = 0; i < 10; i++)
        //        {
        //            int suly = rnd.Next(3000, 25000);
        //            topSulyok[i] = suly;
        //            TopLista.Add(new TopListaElem { Helyezes = i + 1, Nev = gyakorlatok[i], ErtekSzoveg = $"Össz: {suly}kg | Átlag: {suly / 4}kg" });
        //        }
        //        GeneraloMotorSpecialis(10, gyakorlatok.Select(x => x.Substring(0, 3)).ToArray(), topSulyok, alapSzin);
        //    }
        //}

        private void GeneraloMotor(int darab, string[] cimkek, int min, int max, string szin)
        {
            var rnd = new Random();
            int[] ertekek = new int[darab];
            for (int i = 0; i < darab; i++) ertekek[i] = rnd.Next(min, max);
            RajzoldKiGrafikont(ertekek, cimkek, szin);
        }

        private void GeneraloMotorSpecialis(int darab, string[] cimkek, int[] ertekek, string szin)
        {
            RajzoldKiGrafikont(ertekek, cimkek, szin);
        }

        private void RajzoldKiGrafikont(int[] ertekek, string[] cimkek, string szin)
        {
            int maxErtek = ertekek.Max();
            YMaxSzoveg = maxErtek >= 1000 ? (maxErtek / 1000.0).ToString("0.0") + "k" : maxErtek.ToString();
            YMidSzoveg = (maxErtek / 2) >= 1000 ? ((maxErtek / 2) / 1000.0).ToString("0.0") + "k" : (maxErtek / 2).ToString();

            double szorzo = 180.0 / maxErtek;
            for (int i = 0; i < ertekek.Length; i++)
            {
                GrafikonOszlopok.Add(new GrafikonOszlop { Magassag = (int)(ertekek[i] * szorzo), Cimke = i < cimkek.Length ? cimkek[i] : (i + 1).ToString(), Szin = szin });
            }
        }
    }
}