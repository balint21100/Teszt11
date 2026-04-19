using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Interface;

namespace Teszt1.Bakckend.Database
{
    public class DailyLogDataProvider : IDailyLogDataProvider
    {
        private readonly FitnessDbContext _db;
        public DailyLogDataProvider(FitnessDbContext db) => _db = db;

        public DailyLog AddLog(DailyLog log)
        {
            var entry = _db.DailyLogs.Add(log).Entity;
            _db.SaveChanges();
            return entry;
        }

        public void UpdateLog(int id, DailyLog log)
        {
            var existing = _db.DailyLogs.First(x => x.Id == id);
            _db.Entry(existing).CurrentValues.SetValues(log);
            _db.SaveChanges();
        }

        public ICollection<DailyLog> GetLogs(int userId) =>
            _db.DailyLogs.Where(x => x.User_Id == userId).OrderByDescending(x => x.date).ToHashSet();
    }
}
