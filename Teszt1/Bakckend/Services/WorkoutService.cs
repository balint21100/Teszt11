using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Interface;

namespace Teszt1.Bakckend.Services
{
    public class WorkoutService
    {
        private readonly IWorkoutDataProvider _workoutProvider;
        private readonly IWorkoutEntryDataProvider _entryProvider;
        private readonly IExerciseDataProvider _exerciseProvider;

        public WorkoutService(IWorkoutDataProvider wp, IWorkoutEntryDataProvider ep, IExerciseDataProvider ex)
        {
            _workoutProvider = wp;
            _entryProvider = ep;
            _exerciseProvider = ex;
        }

        public void TeljesEdzesMentese(string gyNev, float suly, int reps, int sets, int userId)
        {
            // 1. Megkeressük/Létrehozzuk a gyakorlatot
            var ex = _exerciseProvider.GetExercises().FirstOrDefault(x => x.Name == gyNev)
                     ?? _exerciseProvider.AddExercise(new Exercise { Name = gyNev });

            // 2. Létrehozzuk a fő edzést
            var wo = _workoutProvider.AddWorkout(new WorkOut { User_Id = userId, Date = DateTime.Now });

            // 3. Mentjük a részleteket
            _entryProvider.AddWorkoutEntry(new WorkoutEntry
            {
                Workout_Id = wo.Id,
                Exercise_Id = ex.Id,
                Weight = suly,
                Reps = reps,
                Sets = sets
            });
        }
    }
}
