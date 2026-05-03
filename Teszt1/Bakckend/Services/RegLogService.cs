using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Interface;

namespace Teszt1.Bakckend.Services
{
    public class RegLogService
    {
        IUserDataProvider _userDataProvider;

        public RegLogService(IUserDataProvider userDataProvider)
        {
            _userDataProvider = userDataProvider;
        }

        public int Login(string email, string password)
        {
            var data = _userDataProvider.GetUser().FirstOrDefault(x => x.Email.Equals(email));
            if (data != null)
            {
                if (data.Email.Equals(email) && data.Password.Equals(password))
                {
                    return 1;
                }
                return 0;
            }
            return -1;
        }

        public void Register(User user)
        {
            _userDataProvider.AddUser(user);
        }
    }
}
