using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Bakckend.Calsses
{
    public class WorkoutEntry
    {
        [Key]
        public int Id {  get; set; }
        public int Workout_Id { get; set; }
        public int Exercise_Id { get; set; }
        public int Sets {  get; set; }
        public int Reps {  get; set; }
        public float Weight { get; set; }


        public virtual Exercise Exercise { get; set; }
        public virtual WorkOut Workout { get; set; }
    }
}
