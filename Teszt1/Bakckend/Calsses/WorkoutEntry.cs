using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Bakckend.Calsses
{
    public class WorkoutEntry
    {
        [Key]
        public int Id {  get; set; }
        [ForeignKey(nameof(Workout))]
        public int Workout_id { get; set; }
        [ForeignKey(nameof(Workoutplan))]
        public int Workoutplan_id { get; set; }
        [ForeignKey(nameof(Exercise))]
        public int Exercise_id { get; set; }
        public int Sets {  get; set; }
        public int Reps {  get; set; }
        public float Weight { get; set; }


        public virtual Workoutplan Workoutplan { get; set; }
        public virtual Exercise Exercise { get; set; }
        public virtual WorkOut Workout { get; set; }
        
    }
}
