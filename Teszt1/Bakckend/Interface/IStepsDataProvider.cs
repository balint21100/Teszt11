using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;

namespace Teszt1.Bakckend.Interface
{
    public interface IStepsDataProvider
    {
        void AddSteps(Steps steps);
        ICollection<Steps> GetUserSteps(int userId);
        void UpdateSteps(int id, int newSteps);
    }
}
