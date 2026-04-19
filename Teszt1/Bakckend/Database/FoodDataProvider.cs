using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Interface;

namespace Teszt1.Bakckend.Database
{
    public class FoodDataProvider : IFoodDataProvider
    {
        private readonly FitnessDbContext _db;
        public FoodDataProvider(FitnessDbContext db) => _db = db;

        public Food AddFood(Food food)
        {
            var entry = _db.Foods.Add(food).Entity;
            _db.SaveChanges();
            return entry;
        }

        public void UpdateFood(int id, Food food)
        {
            var existing = _db.Foods.First(x => x.Id == id);
            _db.Entry(existing).CurrentValues.SetValues(food);
            _db.SaveChanges();
        }

        public void DeleteFood(int id)
        {
            var item = _db.Foods.First(x => x.Id == id);
            _db.Foods.Remove(item);
            _db.SaveChanges();
        }

        public ICollection<Food> GetFoods() => _db.Foods.ToHashSet();
    }
}
