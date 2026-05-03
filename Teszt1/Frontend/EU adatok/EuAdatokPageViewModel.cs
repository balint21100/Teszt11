using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Frontend
{
    public partial class EuAdatokPageViewModel : ObservableObject
    {
        // Alapadatok a rajzod alapján (Ideiglenes tesztadatokkal feltöltve)
        [ObservableProperty] private string cel = "Fogyás";
        [ObservableProperty] private string kihivasProgressz = "5 / 28";

        [ObservableProperty] private string magassag = "162";
        [ObservableProperty] private string testsuly = "65";
        [ObservableProperty] private string vernyomas = "120/80";

        [ObservableProperty] private int lepesszam = 4536;
        [ObservableProperty] private int lepesszamCel = 10000;

        [ObservableProperty] private string egyebAdat = "Glutén, laktóz és szója kerülése";

        [RelayCommand]
        public async Task MentesAsync()
        {
            // Ide jön majd az adatbázisba mentés!
            await Shell.Current.DisplayAlert("Mentés", "Egészségügyi adatok sikeresen frissítve!", "OK");
        }
    }
}
