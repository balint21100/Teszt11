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
        private readonly IWorkoutplanDataProvider _workoutplanProvider;
        private readonly IExerciseDataProvider _exerciseProvider;

        public WorkoutService(IWorkoutDataProvider wp, IWorkoutEntryDataProvider ep, 
            IWorkoutplanDataProvider workoutplan, IExerciseDataProvider ex)
        {
            _workoutProvider = wp;
            _entryProvider = ep;
            _workoutplanProvider = workoutplan;
            _exerciseProvider = ex;
        }
        public void AddWorkoutWithEntries(int userId, string Plan, string description, List<WorkoutEntryDto> entries)
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

            var workoutplan = _workoutplanProvider.GetWorkoutplans().FirstOrDefault(x => x.Name.Equals(Plan));
            if (workoutplan == null)
            {
                workoutplan = _workoutplanProvider.AddWorkoutplan(new Workoutplan
                {
                    User_Id = userId,
                    Name = Plan,
                    Description = description
                });
            }

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

                        Category = item.Weight != null? "Súlyzós" : "Egyéb" // Alapértelmezett kategória
                    });
                }

                // Létrehozzuk a bejegyzést (WorkoutEntry tábla)
                var newEntry = new WorkoutEntry
                {
                    Workout_id = savedWorkout.Id,
                    Workoutplan_id = workoutplan.Id,
                    Exercise_id = exercise.Id,
                    Weight = item.Weight,
                    Reps = item.Reps,
                    Sets = item.Sets
                };

                _entryProvider.AddWorkoutEntry(newEntry);
            }
        }

        public List<string> GetWorkoutPlans(int user_id)
        {
            List<string> workoutnames = new List<string>();
            var workoutplans = _workoutplanProvider.GetWorkoutplans().Where(x => x.User_Id.Equals(user_id));
            foreach (var item in workoutplans)
            {
                workoutnames.Add(item.Name);
            }
            return workoutnames;
        }

        public List<GrafikonAdatDto> GetEdzesStatisztika(int userId)
        {
            List<GrafikonAdatDto> lista = new List<GrafikonAdatDto>();
            var workout = _workoutProvider.GetWorkouts(userId);
            foreach (var item in workout)
            {

                var entries = _entryProvider.GetWorkoutEntries(item.Id);
                var excersises = _exerciseProvider.GetExercises();
                foreach (var item2 in entries)
                {
                    var gyakorlat = excersises.Where(x => x.Id == item2.Exercise_id).FirstOrDefault();
                    if (gyakorlat != null)
                    {
                        lista.Add(new GrafikonAdatDto
                        {
                            Datum = item.Date,
                            Name = gyakorlat.Name,
                            Ertek = item2.Weight
                        });
                    }
                }
                
                

            }
            return lista;
        }

        public List<TopListaElemDto> GetLegnehezebbGyakorlatok(int userId)
        {
            var workouts = _workoutProvider.GetWorkouts(userId);
            var entries = new List<WorkoutEntry>();

            foreach (var w in workouts)
            {
                entries.AddRange(_entryProvider.GetWorkoutEntries(w.Id));
            }

            return entries
                .GroupBy(e => e.Exercise_id)
                .Select(g => new TopListaElemDto
                {
                    Nev = _exerciseProvider.GetExercises().FirstOrDefault(ex => ex.Id == g.Key)?.Name ?? "Ismeretlen",
                    MaxSuly = g.Max(e => e.Weight)
                })
                .OrderByDescending(x => x.MaxSuly)
                .Take(5)
                .ToList();
        }
        public Dictionary<string, int> GetUserExerciseFrequency(int userId)
        {
            // 1. Lekérjük a felhasználó összes edzését (WorkOut)
            var userWorkouts = _workoutProvider.GetWorkouts(userId).ToList();

            // 2. Összegyűjtjük az összes gyakorlat-bejegyzést (WorkoutEntry) ezekből az edzésekből
            var allEntries = new List<WorkoutEntry>();
            foreach (var workout in userWorkouts)
            {
                var entries = _entryProvider.GetWorkoutEntries(workout.Id);
                allEntries.AddRange(entries);
            }

            // 3. Lekérjük az összes alapgyakorlatot a nevek párosításához
            var allExercises = _exerciseProvider.GetExercises();

            // 4. LINQ csoportosítás gyakorlat azonosító alapján és gyakoriság számolása
            var frequency = allEntries
                .GroupBy(e => e.Exercise_id)
                .Select(group => new
                {
                    ExerciseName = allExercises.FirstOrDefault(ex => ex.Id == group.Key)?.Name ?? "Ismeretlen gyakorlat",
                    Count = group.Count()
                })
                .OrderByDescending(x => x.Count) // A leggyakrabban végzettek lesznek elöl
                .ToDictionary(x => x.ExerciseName, x => x.Count);

            return frequency;
        }
        public List<GrafikonAdatDto> GetEdzesVolumeStatisztika(int userId, string idotav)
        {
            DateTime kezdodatum = DateTime.Now;

            // Időtáv beállítása
            if (idotav == "Heti") kezdodatum = DateTime.Now.AddDays(-7);
            else if (idotav == "Havi") kezdodatum = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            else if (idotav == "Éves") kezdodatum = new DateTime(DateTime.Now.Year, 1, 1);

            // 1. Lekérjük az edzéseket az időszakban
            var workouts = _workoutProvider.GetWorkouts(userId)
                .Where(w => w.Date >= kezdodatum)
                .ToList();

            var ideiglenesLista = new List<WorkoutEntry>();
            foreach (var w in workouts)
            {
                ideiglenesLista.AddRange(_entryProvider.GetWorkoutEntries(w.Id));
            }

            // 2. Csoportosítás gyakorlatok szerint és összsúly számítás
            var statisztika = ideiglenesLista
                .GroupBy(e => e.Exercise_id)
                .Select(g => {
                    var exercise = _exerciseProvider.GetExercises().FirstOrDefault(ex => ex.Id == g.Key);

                    // Összsúly számítása: Súly * Ismétlés * Széria (ha valamelyik null, vegyük 1-nek vagy 0-nak)
                    float osszSuly = g.Sum(e => e.Weight * e.Reps * e.Sets);

                    return new GrafikonAdatDto
                    {
                        Name = exercise?.Name ?? "Ismeretlen",
                        Ertek = osszSuly,
                        Datum = kezdodatum // Itt most a név a fontosabb
                    };
                })
                .OrderByDescending(x => x.Ertek) // A legtöbbet mozgatott legyen elöl
                .Take(7) // Csak a top 7 gyakorlatot mutatjuk, hogy kiférjen a grafikonon
                .ToList();

            return statisztika;
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
