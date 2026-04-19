using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Interface;

namespace Teszt1.Bakckend.Database
{
    public class WorkoutDataProvider : IWorkoutDataProvider
    {
        private readonly FitnessDbContext _db;
        public WorkoutDataProvider(FitnessDbContext db) => _db = db;

        public WorkOut AddWorkout(WorkOut workout)
        {
            var entry = _db.Workouts.Add(workout).Entity;
            _db.SaveChanges();
            return entry;
        }

        public void UpdateWorkout(int id, WorkOut workout)
        {
            var existing = _db.Workouts.First(x => x.Id == id);
            _db.Entry(existing).CurrentValues.SetValues(workout);
            _db.SaveChanges();
        }

        public void DeleteWorkout(int id)
        {
            var item = _db.Workouts.First(x => x.Id == id);
            _db.Workouts.Remove(item);
            _db.SaveChanges();
        }

        public ICollection<WorkOut> GetWorkouts(int userId) =>
            _db.Workouts.Where(x => x.User_Id == userId).ToHashSet();
    }
}
