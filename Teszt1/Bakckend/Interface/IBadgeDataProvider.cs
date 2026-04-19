using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;

namespace Teszt1.Bakckend.Interface
{
    public interface IBadgeDataProvider
    {
        Badge AddBadge(Badge badge);
        void UpdateBadge(int id, Badge badge);
        void DeleteBadge(int id);
        ICollection<Badge> GetBadges();
    }
}
