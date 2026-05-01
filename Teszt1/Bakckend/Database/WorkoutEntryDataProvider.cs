using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Interface;

namespace Teszt1.Bakckend.Database
{
    public class WorkoutEntryDataProvider : IWorkoutEntryDataProvider
    {
        private readonly FitnessDbContext _db;
        public WorkoutEntryDataProvider(FitnessDbContext db) => _db = db;

        public WorkoutEntry AddWorkoutEntry(WorkoutEntry entry)
        {
            var newEntry = _db.WorkoutEntries.Add(entry).Entity;
            _db.SaveChanges();
            return newEntry;
        }

        public void UpdateWorkoutEntry(int id, WorkoutEntry entry)
        {
            var existing = _db.WorkoutEntries.First(x => x.Id == id);
            _db.Entry(existing).CurrentValues.SetValues(entry);
            _db.SaveChanges();
        }

        public void DeleteWorkoutEntry(int id)
        {
            var entry = _db.WorkoutEntries.First(x => x.Id == id);
            _db.WorkoutEntries.Remove(entry);
            _db.SaveChanges();
        }

        public ICollection<WorkoutEntry> GetWorkoutEntries(int workoutId) =>
            _db.WorkoutEntries.Where(x => x.Workout_id == workoutId).ToHashSet();
    }
}
