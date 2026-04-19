using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Interface;

namespace Teszt1.Bakckend.Database
{
    public class ExerciseDataProvider : IExerciseDataProvider
    {
        private readonly FitnessDbContext _db;
        public ExerciseDataProvider(FitnessDbContext db) => _db = db;

        public Exercise AddExercise(Exercise exercise)
        {
            var newEx = _db.Exercises.Add(exercise).Entity;
            _db.SaveChanges();
            return newEx;
        }

        public void UpdateExercise(int id, Exercise exercise)
        {
            var existing = _db.Exercises.First(x => x.Id == id);
            _db.Entry(existing).CurrentValues.SetValues(exercise);
            _db.SaveChanges();
        }

        public void DeleteExercise(int id)
        {
            var ex = _db.Exercises.First(x => x.Id == id);
            _db.Exercises.Remove(ex);
            _db.SaveChanges();
        }

        public ICollection<Exercise> GetExercises() => _db.Exercises.ToHashSet();
    }

}
