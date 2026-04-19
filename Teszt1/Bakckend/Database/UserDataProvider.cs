using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teszt1.Bakckend.Calsses;
using Teszt1.Bakckend.Interface;

namespace Teszt1.Bakckend.Database
{
    public class UserDataProvider : IUserDataProvider
    {
        // Itt a generált DbContext típusát kell használnod
        private readonly FitnessDbContext _dbContext;
        public UserDataProvider(FitnessDbContext fitnessDb)
        {
            _dbContext = fitnessDb;
        }


        public User AddUser(User oneUser)
        {
            User user = _dbContext.Users.Add(oneUser).Entity;
            _dbContext.SaveChanges();
            return user;
        }


        public void UpdateUser(int id, User user)
        {
            User newUser = _dbContext.Users.First(x => x.Id == id);
            newUser = user;
            _dbContext.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            User newUser = _dbContext.Users.First(x => x.Id == id);
            _dbContext.Remove(newUser);
            _dbContext.SaveChanges();
        }

        public ICollection<User> GetUser()
        {
            return _dbContext.Users.ToHashSet();
        }

        
    }
}
