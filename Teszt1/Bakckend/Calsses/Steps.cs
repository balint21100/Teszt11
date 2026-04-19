using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Bakckend.Calsses
{
    public class Steps
    {
        [Key]
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int StepCount { get; set; }
        public DateTime Date { get; set; }
        public int Step { get; set; }

        public virtual User User { get; set; }
    }
}
