using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;

namespace Teszt1.Bakckend.Interface
{
    internal interface IUserDataProvider
    {
        public User AddUser(User oneUser);
        public void UpdateUser(int id, User user);
        public void DeleteUser(int id);
        public ICollection<User> GetUser();
    }
}
