using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Bakckend.Calsses
{
    public class User
    {

        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public int Activity_level { get; set; }
        public int Tdee { get; set; }


        // Kapcsolatok (Navigation properties)
        public virtual ICollection<WorkOut> Workouts { get; set; } = new HashSet<WorkOut>();
        public virtual ICollection<Meal> Meals { get; set; } = new HashSet<Meal>();
        public virtual ICollection<UserBadge> UserBadges { get; set; } = new HashSet<UserBadge>();
        public virtual ICollection<DailyLog> DailyLogs { get; set; } = new HashSet<DailyLog>();
        public virtual ICollection<Weight> Weights { get; set; }
        public virtual ICollection<Steps> Steps { get; set; }
        public virtual ICollection<Workoutplan> Workoutplans { get; set; }
    }
}
