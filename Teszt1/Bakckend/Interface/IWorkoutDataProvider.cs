using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;

namespace Teszt1.Bakckend.Interface
{
    public interface IWorkoutDataProvider
    {
        WorkOut AddWorkout(WorkOut workout);
        void UpdateWorkout(int id, WorkOut workout);
        void DeleteWorkout(int id);
        ICollection<WorkOut> GetWorkouts(int userId);
    }
}
