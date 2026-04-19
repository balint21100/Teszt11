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
        int id;
        string email;
        string password;
        string name;
        int age;
        string gender;
        int activity_level;
        int tdee;

        [Key]
        public int Id { get => id; }
        public string Email { get => email; }
        public string Password { get => password;}
        public string Name { get => name;}
        public int Age { get => age;}
        public string Gender { get => gender;}
        public int Activity_level { get => activity_level;}
        public int Tdee { get => tdee;}


        // Kapcsolatok (Navigation properties)
        public virtual ICollection<WorkOut> Workouts { get; set; } = new HashSet<WorkOut>();
        public virtual ICollection<Meal> Meals { get; set; } = new HashSet<Meal>();
        public virtual ICollection<UserBadge> UserBadges { get; set; } = new HashSet<UserBadge>();
        public virtual ICollection<DailyLog> DailyLogs { get; set; } = new HashSet<DailyLog>();
        public virtual ICollection<Weight> Weights { get; set; }
        public virtual ICollection<Steps> Steps { get; set; }
    }
}
