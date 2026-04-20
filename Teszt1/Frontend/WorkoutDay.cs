using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Frontend
{
    public partial class WorkoutDay : ObservableObject
    {
        [ObservableProperty] private string dayName;
        [ObservableProperty] private string plannedExercise;
        [ObservableProperty] private bool isCurrentDay;
    }
}
