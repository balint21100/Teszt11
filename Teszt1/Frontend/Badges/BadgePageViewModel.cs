using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using Microsoft.Maui.Graphics;

namespace Teszt1.Frontend.Badges
{
    public class BadgeItem
    {
        public string Nev { get; set; }
        public string Leiras { get; set; }
        public string KepHivatkozas { get; set; } // Ide jön a PNG fájl neve!
        public bool Megszerzett { get; set; } // Igaz, ha már feloldottad

        // --- AUTOMATIKUS VIZUÁLIS BEÁLLÍTÁSOK A ZÁROLT ÁLLAPOTHOZ ---
        // Ha megszerzett=true, akkor teljesen látszik (1.0). Ha false, akkor halvány (0.3).
        public double KepAtlatszosag => Megszerzett ? 1.0 : 0.3;

        // A szövegek is elszürkülnek, ha még nincs meg a jelvény
        public Color CimSzin => Megszerzett ? Colors.White : Colors.Gray;
        public Color LeirasSzin => Megszerzett ? Colors.LightGray : Color.FromArgb("#666666");
    }

    public partial class BadgePageViewModel : ObservableObject
    {
        public ObservableCollection<BadgeItem> BadgeLista { get; set; } = new();

        public BadgePageViewModel()
        {
            // 1. Jelvény: Ezt már megszerezted (színes, éles lesz)
            BadgeLista.Add(new BadgeItem
            {
                Nev = "Kezdő Lépések (5 nap)",
                Leiras = "Sikeresen teljesítettél egy 5 napos sorozatot. Szép munka!",
                KepHivatkozas = "badge_5.png", // CSERÉLD KI A TE PNG NEVEDRE!
                Megszerzett = true
            });

            // 2. Jelvény: Zárolva (halvány kép, szürke szöveg)
            BadgeLista.Add(new BadgeItem
            {
                Nev = "Lendületben (10 nap)",
                Leiras = "Tarts ki! Érj el egy 10 napos sorozatot a feloldáshoz.",
                KepHivatkozas = "badge_10.png", // CSERÉLD KI A TE PNG NEVEDRE!
                Megszerzett = false
            });

            // 3. Jelvény: Zárolva (halvány kép, szürke szöveg)
            BadgeLista.Add(new BadgeItem
            {
                Nev = "Megállíthatatlan (20 nap)",
                Leiras = "Majdnem egy hónapnyi elkötelezettség. Érd el a 20 napot!",
                KepHivatkozas = "badge_20.png", // CSERÉLD KI A TE PNG NEVEDRE!
                Megszerzett = false
            });
        }
    }
}