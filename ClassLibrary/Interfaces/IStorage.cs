using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary.Interfaces
{
    public interface IStorage
    {
        IRepository<Station> Stations { get; }
        IRepository<Route> Routes { get; }
        IRepository<User> Users { get; }
        void SaveAll();
        void RemoveFavSt(User user, UserStation station);
        void LoadUserPreferences(User user, out User User);
        void AddFavSt(User user, UserStation station);
        void EditFavSt(UserStation station, string Desc);
        void AddUser(User user);
    }

}
