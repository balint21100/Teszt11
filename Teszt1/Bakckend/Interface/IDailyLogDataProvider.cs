using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;

namespace Teszt1.Bakckend.Interface
{
    public interface IDailyLogDataProvider
    {
        DailyLog AddLog(DailyLog log);
        void UpdateLog(int id, DailyLog log);
        ICollection<DailyLog> GetLogs(int userId);
    }
}
