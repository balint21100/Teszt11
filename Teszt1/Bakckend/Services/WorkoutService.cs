using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public void AddWorkoutWithEntries(int userId, string workoutName, List<WorkoutEntryDto> entries)
        {
            // 1. Fő edzés (WorkOut) létrehozása és mentése
            var newWorkout = new WorkOut
            {
                User_Id = userId,
                Date = DateTime.Now,
                // Megjegyzés: Ha a WorkOut osztályodba szeretnél nevet is menteni, 
                // ahhoz bővíteni kell a WorkOut.cs-t egy Name mezővel.
            };
            var savedWorkout = _workoutProvider.AddWorkout(newWorkout);

            // 2. Gyakorlatok mentése egyenként
            foreach (var item in entries)
             {
                // Megkeressük a gyakorlatot név alapján, ha nincs, létrehozzuk (Exercise tábla)
                    var exercise = _exerciseProvider.GetExercises()
                    .FirstOrDefault(x => x.Name.Equals(item.ExerciseName));
                

                if (exercise == null)
                {
                    exercise = _exerciseProvider.AddExercise(new Exercise
                    {
                        Name = item.ExerciseName,
                        Category = "Súlyzós" // Alapértelmezett kategória
                    });
                }

                // Létrehozzuk a bejegyzést (WorkoutEntry tábla)
                var newEntry = new WorkoutEntry
                {
                    Workout_id = savedWorkout.Id,
                    Exercise_id = exercise.Id,
                    Weight = item.Weight,
                    Reps = item.Reps,
                    Sets = item.Sets
                };

                _entryProvider.AddWorkoutEntry(newEntry);
            }
        }

        //public ObservableCollection<string> GetNapiEdzesek(int userid)
        //{
        //    ObservableCollection<string> edzesek = new ObservableCollection<string>();
        //    var edzes = _workoutProvider.GetWorkouts(userid);
        //    foreach (var item in edzes)
        //    {
        //        var edzesentry = _entryProvider.GetWorkoutEntries(edzes)
        //    }
            
        //}

        //public void TeljesEdzesMentese(string gyNev, float suly, int reps, int sets, int userId)
        //{
        //    // 1. Megkeressük/Létrehozzuk a gyakorlatot
        //    var ex = _exerciseProvider.GetExercises().FirstOrDefault(x => x.Name == gyNev)
        //             ?? _exerciseProvider.AddExercise(new Exercise { Name = gyNev });

        //    // 2. Létrehozzuk a fő edzést
        //    var wo = _workoutProvider.AddWorkout(new WorkOut { User_Id = userId, Date = DateTime.Now });

        //    // 3. Mentjük a részleteket
        //    _entryProvider.AddWorkoutEntry(new WorkoutEntry
        //    {
        //        Workout_Id = wo.Id,
        //        Exercise_Id = ex.Id,
        //        Weight = suly,
        //        Reps = reps,
        //        Sets = sets
        //    });
        //}
    }
}
