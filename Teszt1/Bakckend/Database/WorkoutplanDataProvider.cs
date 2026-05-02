using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Interface;

namespace Teszt1.Bakckend.Database
{
    public class WorkoutplanDataProvider : IWorkoutplanDataProvider
    {
        private readonly FitnessDbContext _db;
        public WorkoutplanDataProvider(FitnessDbContext db) => _db = db;


        public Workoutplan AddWorkoutplan(Workoutplan workoutplan)
        {
            var newworkoutplan = _db.Workoutplans.Add(workoutplan).Entity;
            _db.SaveChanges();
            return newworkoutplan;
        }

        public void UpdateWorkoutplan(int id, Workoutplan workoutplan)
        {
            var existing = _db.Workoutplans.First(x => x.Id == id);
            _db.Entry(existing).CurrentValues.SetValues(workoutplan);
            _db.SaveChanges();
        }

        public void DeleteWorkoutplan(int id)
        {
            var b = _db.Workoutplans.First(x => x.Id == id);
            _db.Workoutplans.Remove(b);
            _db.SaveChanges();
        }

        public ICollection<Workoutplan> GetWorkoutplans() => _db.Workoutplans.ToHashSet();
    }
}
