using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Bakckend.Calsses
{
    public class WorkOut
    {
        [Key]
        public int Id { get; set; }
        public int User_Id { get; set; }
        public DateTime Date { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<WorkoutEntry> Entries { get; set; } = new HashSet<WorkoutEntry>();
        public virtual ICollection<Exercise> Exercises { get; set; } = new HashSet<Exercise>();
    }
}
