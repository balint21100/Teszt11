using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;

namespace Teszt1.Bakckend.Interface
{
    public interface IWorkoutplanDataProvider
    {
        Workoutplan AddWorkoutplan(Workoutplan workoutplan);


        void UpdateWorkoutplan(int id, Workoutplan workoutplan);


        void DeleteWorkoutplan(int id);


        ICollection<Workoutplan> GetWorkoutplans();
    }
}
