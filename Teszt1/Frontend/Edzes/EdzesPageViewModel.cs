using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Teszt1;
using Teszt1.Bakckend.Services; // A te DatabaseService-ed névtere

namespace Teszt1.Frontend.Edzes
{
    public partial class EdzesPageViewModel : ObservableObject
    {

        // Dinamikus listák a napokhoz
        private readonly WorkoutService _workoutService;
        private readonly SessionService _sessionService;
        private bool _isLoaded = false; // Flag a hibrid betöltéshez

        [ObservableProperty] private ObservableCollection<string> hetfoEdzesek = new() { "+ Új edzés hozzáadása" };
        [ObservableProperty] private ObservableCollection<string> keddEdzesek = new() { "+ Új edzés hozzáadása" };
        [ObservableProperty] private ObservableCollection<string> szerdaEdzesek = new() { "+ Új edzés hozzáadása" };
        [ObservableProperty] private ObservableCollection<string> csutortokEdzesek = new() { "+ Új edzés hozzáadása" };
        [ObservableProperty] private ObservableCollection<string> pentekEdzesek = new() { "+ Új edzés hozzáadása" };
        [ObservableProperty] private ObservableCollection<string> szombatEdzesek = new() { "+ Új edzés hozzáadása" };
        [ObservableProperty] private ObservableCollection<string> vasarnapEdzesek = new() { "+ Új edzés hozzáadása" };

        [ObservableProperty] private string kivalasztottHetfo;
        [ObservableProperty] private string kivalasztottKedd;
        [ObservableProperty] private string kivalasztottSzerda;
        [ObservableProperty] private string kivalasztottCsutortok;
        [ObservableProperty] private string kivalasztottPentek;
        [ObservableProperty] private string kivalasztottSzombat;
        [ObservableProperty] private string kivalasztottVasarnap;

        [ObservableProperty] private string hetfoKeret = "Gray";
        [ObservableProperty] private string keddKeret = "Gray";
        [ObservableProperty] private string szerdaKeret = "Gray";
        [ObservableProperty] private string csutortokKeret = "Gray";
        [ObservableProperty] private string pentekKeret = "Gray";
        [ObservableProperty] private string szombatKeret = "Gray";
        [ObservableProperty] private string vasarnapKeret = "Gray";



        public EdzesPageViewModel(WorkoutService workoutService, SessionService session)
        {
            _workoutService = workoutService;
            _sessionService = session;
            JeloldAktualisNapot();

            // MESSENGER: Helyi frissítés, ha máshol módosul egy edzés
            WeakReferenceMessenger.Default.Register<EdzesModositvaUzenet>(this, (r, m) =>
            {
                FrissitListatHelyben(m);
            });
        }

        public void Load()
        {
            // Ha már be van töltve, nem kérdezzük le újra az adatbázist
            if (_isLoaded) return;

            int userid = Convert.ToInt32(_sessionService.UserId);

            var hetiTervek = _workoutService.GetWorkoutPlans(userid); // UserId fixen 1 egyelőre

            var mindenLista = GetMindenNapiLista();
            foreach (var lista in mindenLista)
            {
                lista.Clear();
                lista.Add("+ Új edzés hozzáadása");
                foreach (var terv in hetiTervek)
                {
                    lista.Add(terv);
                }
            }

            _isLoaded = true;
        }

        private void FrissitListatHelyben(EdzesModositvaUzenet m)
        {
            var lista = GetListaNapAlapjan(m.Nap);
            if (lista == null) return;

            int index = lista.IndexOf(m.RegiNev);
            if (index != -1)
            {
                if (m.UjNev == null) lista.RemoveAt(index); // Törlés
                else lista[index] = m.UjNev; // Átnevezés
            }
        }

        private List<ObservableCollection<string>> GetMindenNapiLista() =>
            new() { HetfoEdzesek, KeddEdzesek, SzerdaEdzesek, CsutortokEdzesek, PentekEdzesek, SzombatEdzesek, VasarnapEdzesek };

        private ObservableCollection<string> GetListaNapAlapjan(string nap) => nap switch
        {
            "Hétfő" => HetfoEdzesek,
            "Kedd" => KeddEdzesek,
            "Szerda" => SzerdaEdzesek,
            "Csütörtök" => CsutortokEdzesek,
            "Péntek" => PentekEdzesek,
            "Szombat" => SzombatEdzesek,
            "Vasárnap" => VasarnapEdzesek,
            _ => null
        };

        private void JeloldAktualisNapot()
        {
            var maiNap = DateTime.Now.DayOfWeek;
            if (maiNap == DayOfWeek.Monday) HetfoKeret = "HotPink";
            else if (maiNap == DayOfWeek.Tuesday) KeddKeret = "HotPink";
            else if (maiNap == DayOfWeek.Wednesday) SzerdaKeret = "HotPink";
            else if (maiNap == DayOfWeek.Thursday) CsutortokKeret = "HotPink";
            else if (maiNap == DayOfWeek.Friday) PentekKeret = "HotPink";
            else if (maiNap == DayOfWeek.Saturday) SzombatKeret = "HotPink";
            else if (maiNap == DayOfWeek.Sunday) VasarnapKeret = "HotPink";
        }

        public string AktualisHet
        {
            get
            {
                DateTime date = DateTime.Now;
                int diff = (7 + (date.DayOfWeek - DayOfWeek.Monday)) % 7;
                DateTime startOfWeek = date.AddDays(-1 * diff).Date;
                DateTime endOfWeek = startOfWeek.AddDays(6);
                var culture = new System.Globalization.CultureInfo("hu-HU");
                return $"{startOfWeek.ToString("yyyy. MMMM dd.", culture)} - {endOfWeek.ToString("yyyy. MMMM dd.", culture)}".ToLower();
            }
        }
    

    //// FÜGGVÉNY: Lekéri az összes napot a MySQL-ből
    //private async Task BetoltMindenEdzestAsync()
    //{
    //    int userId = 1; // Ezt a te bejelentkezett felhasználódra kell cserélni később!

    //    var hetfoLista = await _databaseService.GetNapiEdzesekAsync(userId, "Hétfő");
    //    var keddLista = await _databaseService.GetNapiEdzesekAsync(userId, "Kedd");
    //    var szerdaLista = await _databaseService.GetNapiEdzesekAsync(userId, "Szerda");
    //    var csutortokLista = await _databaseService.GetNapiEdzesekAsync(userId, "Csütörtök");
    //    var pentekLista = await _databaseService.GetNapiEdzesekAsync(userId, "Péntek");
    //    var szombatLista = await _databaseService.GetNapiEdzesekAsync(userId, "Szombat");
    //    var vasarnapLista = await _databaseService.GetNapiEdzesekAsync(userId, "Vasárnap");

    //    MainThread.BeginInvokeOnMainThread(() =>
    //    {
    //        foreach (var e in hetfoLista) if (!HetfoEdzesek.Contains(e)) HetfoEdzesek.Insert(HetfoEdzesek.Count - 1, e);
    //        foreach (var e in keddLista) if (!KeddEdzesek.Contains(e)) KeddEdzesek.Insert(KeddEdzesek.Count - 1, e);
    //        foreach (var e in szerdaLista) if (!SzerdaEdzesek.Contains(e)) SzerdaEdzesek.Insert(SzerdaEdzesek.Count - 1, e);
    //        foreach (var e in csutortokLista) if (!CsutortokEdzesek.Contains(e)) CsutortokEdzesek.Insert(CsutortokEdzesek.Count - 1, e);
    //        foreach (var e in pentekLista) if (!PentekEdzesek.Contains(e)) PentekEdzesek.Insert(PentekEdzesek.Count - 1, e);
    //        foreach (var e in szombatLista) if (!SzombatEdzesek.Contains(e)) SzombatEdzesek.Insert(SzombatEdzesek.Count - 1, e);
    //        foreach (var e in vasarnapLista) if (!VasarnapEdzesek.Contains(e)) VasarnapEdzesek.Insert(VasarnapEdzesek.Count - 1, e);
    //    });
    //}

}
}