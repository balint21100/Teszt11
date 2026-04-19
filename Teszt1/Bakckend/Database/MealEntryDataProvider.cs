using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Interface;

namespace Teszt1.Bakckend.Database
{
    public class MealEntryDataProvider : IMealEntryDataProvider
    {
        private readonly FitnessDbContext _db;
        public MealEntryDataProvider(FitnessDbContext db) => _db = db;

        public MealEntry AddMealEntry(MealEntry entry)
        {
            var newEntry = _db.MealEntries.Add(entry).Entity;
            _db.SaveChanges();
            return newEntry;
        }

        public void UpdateMealEntry(int id, MealEntry entry)
        {
            var existing = _db.MealEntries.First(x => x.Id == id);
            _db.Entry(existing).CurrentValues.SetValues(entry);
            _db.SaveChanges();
        }

        public void DeleteMealEntry(int id)
        {
            var entry = _db.MealEntries.First(x => x.Id == id);
            _db.MealEntries.Remove(entry);
            _db.SaveChanges();
        }

        public ICollection<MealEntry> GetMealEntries(int mealId) =>
            _db.MealEntries.Where(x => x.Meal_Id == mealId).ToHashSet();
    }
}
