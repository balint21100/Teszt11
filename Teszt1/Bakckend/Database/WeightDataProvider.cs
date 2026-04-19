using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Interface;

namespace Teszt1.Bakckend.Database
{
    public class WeightDataProvider : IWeightDataProvider
    {
        private readonly FitnessDbContext _dbContext;

        public WeightDataProvider(FitnessDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddWeight(Weight weight)
        {
            _dbContext.Set<Weight>().Add(weight); // Ha nincs DbSet, a Set<T>()-vel is elérheted
            _dbContext.SaveChanges();
        }

        public ICollection<Weight> GetUserWeights(int userId)
        {
            return _dbContext.Set<Weight>()
                .Where(x => x.User_Id == userId)
                .OrderByDescending(x => x.Date)
                .ToList();
        }

        public void DeleteWeight(int id)
        {
            var weight = _dbContext.Set<Weight>().FirstOrDefault(x => x.Id == id);
            if (weight != null)
            {
                _dbContext.Set<Weight>().Remove(weight);
                _dbContext.SaveChanges();
            }
        }
    }
}
