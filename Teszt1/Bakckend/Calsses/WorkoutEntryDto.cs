using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Bakckend.Calsses
{
    public class WorkoutEntryDto
    {
        public string ExerciseName { get; set; }
        public float Weight { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
    }
}
