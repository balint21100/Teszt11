using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;

namespace Teszt1.Bakckend.Interface
{
    public interface IMealEntryDataProvider
    {
        MealEntry AddMealEntry(MealEntry entry);
        void UpdateMealEntry(int id, MealEntry entry);
        void DeleteMealEntry(int id);
        ICollection<MealEntry> GetMealEntries(int mealId);
    }
}
