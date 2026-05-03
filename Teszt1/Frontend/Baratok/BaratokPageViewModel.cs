using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace Teszt1.Frontend
{
    public class Barat
    {
        public string Nev { get; set; }
        public string Cel { get; set; }
        public int CelKaloria { get; set; }
        public string TeljesitettKaloria { get; set; }
        public string TervezettEdzes { get; set; }
        public string TeljesitetteMa { get; set; }
        public string Uzenet { get; set; }
        // Ez a tulajdonság tárolja a kép fájlnevét
        public string KepForras { get; set; }
    }

    public partial class BaratokPageViewModel : ObservableObject
    {
        [ObservableProperty]
        private string ujBaratKereso;

        public ObservableCollection<Barat> BaratokLista { get; set; }

        public BaratokPageViewModel()
        {
            BaratokLista = new ObservableCollection<Barat>
            {
                new Barat
                {
                    Nev = "Csenge",
                    Cel = "Fogyás",
                    CelKaloria = 1800,
                    TeljesitettKaloria = "1356",
                    TervezettEdzes = "Kardió 40 perc",
                    TeljesitetteMa = "Igen",
                    Uzenet = "Kitartást a mai napra is!",
                    KepForras = "cs.png"
                },
                new Barat
                {
                    Nev = "Peti",
                    Cel = "Tömegnövelés",
                    CelKaloria = 3200,
                    TeljesitettKaloria = "0",
                    TervezettEdzes = "Hát + bicepsz",
                    TeljesitetteMa = "Nem",
                    Uzenet = "Holnap lemegyünk együtt edzeni?",
                    KepForras = "p.png"
                }
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