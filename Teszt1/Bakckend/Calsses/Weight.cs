using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Bakckend.Calsses
{
    public class Weight
    {
        [Key]
        public int Id { get; set; }
        public int User_Id { get; set; }
        public DateTime Date { get; set; }
        public float Weights { get; set; }

        public virtual User User { get; set; }
    }
}
