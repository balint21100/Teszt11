using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;

namespace Teszt1.Bakckend.Database
{
    public class UserBadgeDataProvider
    {
        private readonly FitnessDbContext _db;
        public UserBadgeDataProvider(FitnessDbContext db) => _db = db;

        public void AwardBadge(UserBadge badge)
        {
            _db.UserBadges.Add(badge);
            _db.SaveChanges();
        }

        public ICollection<UserBadge> GetUserBadges(int userId) =>
            _db.UserBadges.Where(x => x.User_Id == userId).ToHashSet();
    }
}
