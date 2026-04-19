using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Bakckend.Calsses
{
    public class DailyLog
    {
        [Key]
        public int Id { get; set; }
        public int User_Id { get; set; }
        public DateTime date { get; set; }
        public bool WorkoutDone { get; set; }
        public bool Nutrition_Logged { get; set; }
        public int Streak {  get; set; }






        public virtual User User { get; set; }
    }
}
