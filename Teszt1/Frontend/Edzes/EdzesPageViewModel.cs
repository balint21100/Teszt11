using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Teszt1; // A te DatabaseService-ed névtere

namespace Teszt1.Frontend.Edzes
{
    public partial class EdzesPageViewModel : ObservableObject
    {
        private readonly DatabaseService _databaseService = new DatabaseService();

        // Dinamikus listák a napokhoz
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

        public EdzesPageViewModel()
        {
            var maiNap = DateTime.Now.DayOfWeek;
            if (maiNap == DayOfWeek.Monday) HetfoKeret = "HotPink";
            else if (maiNap == DayOfWeek.Tuesday) KeddKeret = "HotPink";
            else if (maiNap == DayOfWeek.Wednesday) SzerdaKeret = "HotPink";
            else if (maiNap == DayOfWeek.Thursday) CsutortokKeret = "HotPink";
            else if (maiNap == DayOfWeek.Friday) PentekKeret = "HotPink";
            else if (maiNap == DayOfWeek.Saturday) SzombatKeret = "HotPink";
            else if (maiNap == DayOfWeek.Sunday) VasarnapKeret = "HotPink";

            // --- ADATBÁZIS BETÖLTÉSE INDULÁSKOR ---
            // Ez biztosítja, hogy elnavigálás után is megmaradjanak az adatok!
            Task.Run(async () => await BetoltMindenEdzestAsync());

            WeakReferenceMessenger.Default.Register<EdzesMentveUzenet>(this, (r, m) =>
            {
                if (m.Nap == "Hétfő" && !HetfoEdzesek.Contains(m.EdzesNeve)) { HetfoEdzesek.Insert(HetfoEdzesek.Count - 1, m.EdzesNeve); KivalasztottHetfo = m.EdzesNeve; }
                else if (m.Nap == "Kedd" && !KeddEdzesek.Contains(m.EdzesNeve)) { KeddEdzesek.Insert(KeddEdzesek.Count - 1, m.EdzesNeve); KivalasztottKedd = m.EdzesNeve; }
                else if (m.Nap == "Szerda" && !SzerdaEdzesek.Contains(m.EdzesNeve)) { SzerdaEdzesek.Insert(SzerdaEdzesek.Count - 1, m.EdzesNeve); KivalasztottSzerda = m.EdzesNeve; }
                else if (m.Nap == "Csütörtök" && !CsutortokEdzesek.Contains(m.EdzesNeve)) { CsutortokEdzesek.Insert(CsutortokEdzesek.Count - 1, m.EdzesNeve); KivalasztottCsutortok = m.EdzesNeve; }
                else if (m.Nap == "Péntek" && !PentekEdzesek.Contains(m.EdzesNeve)) { PentekEdzesek.Insert(PentekEdzesek.Count - 1, m.EdzesNeve); KivalasztottPentek = m.EdzesNeve; }
                else if (m.Nap == "Szombat" && !SzombatEdzesek.Contains(m.EdzesNeve)) { SzombatEdzesek.Insert(SzombatEdzesek.Count - 1, m.EdzesNeve); KivalasztottSzombat = m.EdzesNeve; }
                else if (m.Nap == "Vasárnap" && !VasarnapEdzesek.Contains(m.EdzesNeve)) { VasarnapEdzesek.Insert(VasarnapEdzesek.Count - 1, m.EdzesNeve); KivalasztottVasarnap = m.EdzesNeve; }
            });

            WeakReferenceMessenger.Default.Register<EdzesModositvaUzenet>(this, (r, m) =>
            {
                ObservableCollection<string> lista = null;
                if (m.Nap == "Hétfő") lista = HetfoEdzesek;
                else if (m.Nap == "Kedd") lista = KeddEdzesek;
                else if (m.Nap == "Szerda") lista = SzerdaEdzesek;
                else if (m.Nap == "Csütörtök") lista = CsutortokEdzesek;
                else if (m.Nap == "Péntek") lista = PentekEdzesek;
                else if (m.Nap == "Szombat") lista = SzombatEdzesek;
                else if (m.Nap == "Vasárnap") lista = VasarnapEdzesek;

                if (lista != null)
                {
                    int index = lista.IndexOf(m.RegiNev);
                    if (index != -1)
                    {
                        if (m.UjNev == null)
                        {
                            lista.RemoveAt(index);
                        }
                        else
                        {
                            lista[index] = m.UjNev;
                        }
                    }
                }
            });
        }

        // FÜGGVÉNY: Lekéri az összes napot a MySQL-ből
        private async Task BetoltMindenEdzestAsync()
        {
            int userId = 1; // Ezt a te bejelentkezett felhasználódra kell cserélni később!

            var hetfoLista = await _databaseService.GetNapiEdzesekAsync(userId, "Hétfő");
            var keddLista = await _databaseService.GetNapiEdzesekAsync(userId, "Kedd");
            var szerdaLista = await _databaseService.GetNapiEdzesekAsync(userId, "Szerda");
            var csutortokLista = await _databaseService.GetNapiEdzesekAsync(userId, "Csütörtök");
            var pentekLista = await _databaseService.GetNapiEdzesekAsync(userId, "Péntek");
            var szombatLista = await _databaseService.GetNapiEdzesekAsync(userId, "Szombat");
            var vasarnapLista = await _databaseService.GetNapiEdzesekAsync(userId, "Vasárnap");

            MainThread.BeginInvokeOnMainThread(() =>
            {
                foreach (var e in hetfoLista) if (!HetfoEdzesek.Contains(e)) HetfoEdzesek.Insert(HetfoEdzesek.Count - 1, e);
                foreach (var e in keddLista) if (!KeddEdzesek.Contains(e)) KeddEdzesek.Insert(KeddEdzesek.Count - 1, e);
                foreach (var e in szerdaLista) if (!SzerdaEdzesek.Contains(e)) SzerdaEdzesek.Insert(SzerdaEdzesek.Count - 1, e);
                foreach (var e in csutortokLista) if (!CsutortokEdzesek.Contains(e)) CsutortokEdzesek.Insert(CsutortokEdzesek.Count - 1, e);
                foreach (var e in pentekLista) if (!PentekEdzesek.Contains(e)) PentekEdzesek.Insert(PentekEdzesek.Count - 1, e);
                foreach (var e in szombatLista) if (!SzombatEdzesek.Contains(e)) SzombatEdzesek.Insert(SzombatEdzesek.Count - 1, e);
                foreach (var e in vasarnapLista) if (!VasarnapEdzesek.Contains(e)) VasarnapEdzesek.Insert(VasarnapEdzesek.Count - 1, e);
            });
        }
    }
}