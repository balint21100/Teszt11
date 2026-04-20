using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Frontend
{
    public class Barat
    {
        public string Nev { get; set; }
        public string Cel { get; set; }
        public int CelKaloria { get; set; }
        public string TeljesitettEtkezes { get; set; }
        public string TervezettEdzes { get; set; }
        public string TeljesitetteMa { get; set; }
        public string Uzenet { get; set; }
    }
    public partial class BaratokPageViewModel : ObservableObject
    {
        [ObservableProperty] private string ujBaratKereso;
        public ObservableCollection<Barat> BaratokLista { get; set; }

        public BaratokPageViewModel()
        {
            BaratokLista = new ObservableCollection<Barat>
            {
                new Barat { Nev = "Csenge", Cel = "Fogyás", CelKaloria = 1800, TeljesitettEtkezes = "2/3", TervezettEdzes = "Kardió 40 perc", TeljesitetteMa = "Igen", Uzenet = "Hajrá ma is!" },
                new Barat { Nev = "Peti", Cel = "Tömegnövelés", CelKaloria = 3200, TeljesitettEtkezes = "4/5", TervezettEdzes = "Hát + Bicepsz", TeljesitetteMa = "Nem", Uzenet = "Holnap lemegyünk a terembe?" }
            };
        }

        [RelayCommand]
        public async Task HozzaadasAsync()
        {
            if (!string.IsNullOrWhiteSpace(UjBaratKereso))
            {
                await Shell.Current.DisplayAlert("Siker", $"Meghívó küldése ide: {UjBaratKereso}", "OK");
                UjBaratKereso = string.Empty;
            }
        }
    }
}
