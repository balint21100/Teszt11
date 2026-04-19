using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;

namespace Teszt1.Bakckend.Interface
{
    public interface IWorkoutEntryDataProvider
    {
        WorkoutEntry AddWorkoutEntry(WorkoutEntry entry);
        void UpdateWorkoutEntry(int id, WorkoutEntry entry);
        void DeleteWorkoutEntry(int id);
        ICollection<WorkoutEntry> GetWorkoutEntries(int workoutId);
    }
}
