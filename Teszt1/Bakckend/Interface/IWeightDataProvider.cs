using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;

namespace Teszt1.Bakckend.Interface
{
    public interface IWeightDataProvider
    {
        void AddWeight(Weight weight);
        ICollection<Weight> GetUserWeights(int userId);
        void DeleteWeight(int id);
    }
}
