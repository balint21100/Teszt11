using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Interface;

namespace Teszt1.Bakckend.Database
{
    public class BadgeDataProvider : IBadgeDataProvider
    {
        private readonly FitnessDbContext _db;
        public BadgeDataProvider(FitnessDbContext db) => _db = db;

        public Badge AddBadge(Badge badge)
        {
            var newBadge = _db.Badges.Add(badge).Entity;
            _db.SaveChanges();
            return newBadge;
        }

        public void UpdateBadge(int id, Badge badge)
        {
            var existing = _db.Badges.First(x => x.Id == id);
            _db.Entry(existing).CurrentValues.SetValues(badge);
            _db.SaveChanges();
        }

        public void DeleteBadge(int id)
        {
            var b = _db.Badges.First(x => x.Id == id);
            _db.Badges.Remove(b);
            _db.SaveChanges();
        }

        public ICollection<Badge> GetBadges() => _db.Badges.ToHashSet();
    }
}
