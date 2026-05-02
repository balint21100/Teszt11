using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Bakckend.Calsses
{
    public class Workoutplan
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(User))]
        public int User_Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }

        public virtual ICollection<WorkoutEntry> WorkoutEntries { get; set; }
        public virtual User User { get; set; }
    }
}
