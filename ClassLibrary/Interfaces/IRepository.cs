using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public interface IRepository<T>
    {
        IEnumerable<T> Items { get; }
        void Add(T item);
        void Remove(T item);
        void Save();
        //void GetSchedule(Station station, Route rout, out List<ScheduleItem> result);
        //void Save();
        //List<Route> Routes { get; }
        //List<Station> Stations { get; }
        //List<User> Users { get; set; }
        //List<Route> AllRoutes { get; }
        //List<Station> AllStations { get; }
        //void RemoveFavSt(UserStation Station);
    }
}
