using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Bakckend.Calsses
{
    public class MealEntry
    {
        [Key]
        public int Id { get; set; }
        public int Meal_Id { get; set; }
        public int Food_Id { get; set; }
        public float Quantity { get; set; }

        // Így tudod majd lekérni az étel nevét/kalóriáját a bejegyzésen keresztül
        public virtual Meal Meal { get; set; }
        public virtual Food food { get; set; }

    }
}
