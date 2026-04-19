using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;

namespace Teszt1.Bakckend.Interface
{
    public interface IFoodDataProvider
    {
        Food AddFood(Food food);
        void UpdateFood(int id, Food food);
        void DeleteFood(int id);
        ICollection<Food> GetFoods();
    }
}
