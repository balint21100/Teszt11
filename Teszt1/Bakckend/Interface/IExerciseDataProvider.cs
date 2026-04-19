using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;

namespace Teszt1.Bakckend.Interface
{
    public interface IExerciseDataProvider
    {
        Exercise AddExercise(Exercise exercise);
        void UpdateExercise(int id, Exercise exercise);
        void DeleteExercise(int id);
        ICollection<Exercise> GetExercises();
    }
}
