using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Bakckend.Calsses
{
    public class Meal
    {
        // Alapadatok a dim_meal táblából
        [Key]
        public int Id { get; set; }
        public int User_Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; } // Pl. "Reggeli", "Vacsora"

        // NAVIGÁCIÓS TULAJDONSÁGOK (EF Core-hoz)

        // Visszamutat a felhasználóra, aki ette
        public virtual User User { get; set; }

        // Megmutatja az összes ételt, ami ebben az étkezésben szerepelt
        // Ez a dim_mealentry táblával köti össze
        public virtual ICollection<MealEntry> Entries { get; set; }

        public Meal()
        {
            Entries = new HashSet<MealEntry>();
        }
    }
}
