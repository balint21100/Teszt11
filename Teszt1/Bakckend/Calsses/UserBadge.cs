using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Bakckend.Calsses
{
    public class UserBadge
    {
        [Key]
        public int Id { get; set; }
        public int User_Id { get; set; }
        public int Badge_Id { get; set; }
        public DateTime Date_Awarded { get; set; }



        public virtual User User { get; set; }
        public virtual Badge Badge { get; set; }
    }
}
