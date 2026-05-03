using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Teszt1.Bakckend.Services
{
    public class SessionService
    {
        public string UserId
        {
            get => Preferences.Default.Get("UserId", string.Empty);
            set => Preferences.Default.Set("UserId", value);
        }

        public bool IsLoggedIn => !string.IsNullOrEmpty(UserId);

        public void Logout() => Preferences.Default.Clear();
    }
}
