using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;

namespace Teszt1.Bakckend.Interface
{
    public interface IMealDataProvider
    {
        Meal AddMeal(Meal meal);
        void UpdateMeal(int id, Meal meal);
        void DeleteMeal(int id);
        ICollection<Meal> GetMeals(int userId); // Felhasználó szerint szűrve
    }
}
