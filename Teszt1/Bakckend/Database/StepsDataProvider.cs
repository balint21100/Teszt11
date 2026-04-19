using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Interface;

namespace Teszt1.Bakckend.Database
{
    public class StepsDataProvider : IStepsDataProvider
    {
        private readonly FitnessDbContext _dbContext;

        public StepsDataProvider(FitnessDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddSteps(Steps steps)
        {
            _dbContext.Set<Steps>().Add(steps);
            _dbContext.SaveChanges();
        }

        public ICollection<Steps> GetUserSteps(int userId)
        {
            return _dbContext.Set<Steps>()
                .Where(x => x.User_Id == userId)
                .OrderByDescending(x => x.Date)
                .ToList();
        }

        public void UpdateSteps(int id, int newSteps)
        {
            var stepsEntry = _dbContext.Set<Steps>().FirstOrDefault(x => x.Id == id);
            if (stepsEntry != null)
            {
                stepsEntry.StepCount = newSteps;
                _dbContext.SaveChanges();
            }
        }
    }
}
