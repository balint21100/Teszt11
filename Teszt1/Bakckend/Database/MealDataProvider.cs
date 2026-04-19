
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Interface;

namespace Teszt1.Bakckend.Database
{
    public class MealDataProvider : IMealDataProvider
    {
        private readonly FitnessDbContext _db;
        public MealDataProvider(FitnessDbContext db) => _db = db;

        public Meal AddMeal(Meal meal)
        {
            var entry = _db.Meals.Add(meal).Entity;
            _db.SaveChanges();
            return entry;
        }

        public void UpdateMeal(int id, Meal meal)
        {
            var existing = _db.Meals.First(x => x.Id == id);
            _db.Entry(existing).CurrentValues.SetValues(meal);
            _db.SaveChanges();
        }

        public void DeleteMeal(int id)
        {
            var item = _db.Meals.First(x => x.Id == id);
            _db.Meals.Remove(item);
            _db.SaveChanges();
        }

        public ICollection<Meal> GetMeals(int userId) =>
            _db.Meals.Where(x => x.User_Id == userId).ToHashSet();
    }
}
