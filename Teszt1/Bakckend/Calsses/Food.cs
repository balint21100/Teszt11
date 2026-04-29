using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Bakckend.Calsses
{
    public class Food
    {
        [Key]
        public int Id {  get; set; }
        public string Name { get; set; }
        public int Kcal { get; set; }
        public float Protein { get; set; }
        public float Carbs { get; set; }
        public float Fat { get; set; }

        public string KcalDetail => $"{Kcal} kcal / 100g";


        public override string ToString()
        {
            return $"Name: {Name} - Kcal: {Kcal} - Protein: {Protein} - Carbs: {Carbs} - Fat: {Fat}";
        }
    }
}
