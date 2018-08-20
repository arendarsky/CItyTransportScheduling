using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;

namespace ClassLibrary
{
    public class JsRepository: IRepository
    {
        public List<Route> Routes { get; set; }
        public List<Station> Stations { get; set; }
        public List<User> Users { get; set; }
        private const string UsersDb = "ClassLibrary/Data/Users";
        private const string RoutesDb = "ClassLibrary/Data/Routes";
        private const string StationsDb = "ClassLibrary/Data/Stations";
        public 
        public JsRepository()
        {
            
            Routes = ReadList<Route>(RoutesDb);
            Stations = ReadList<Station>(StationsDb);
            foreach (var r in Routes)
            {
                foreach (var st in r.Stations)
                {
                    st.Station = Stations.First(c => c.Id == st.StationId);
                }
            }
            Users = ReadList<User>(UsersDb);
            foreach (var u in Users)
            {
                foreach (var st in u.FavoriteStations)
                {
                    st.Station = Stations.First(c => c.Id == st.StationId);
                }
            }
        }
        public void Save()
        {
            SaveList<Route>(Routes, RoutesDb);
            SaveList<Station>(Stations, StationsDb);
            SaveList<User>(Users, UsersDb);
        }
        public void RemoveFavSt(UserStation Station)
        {

        }
        private List<T> ReadList<T>(string DbName)
        {
            using (StreamReader sr = new StreamReader($"{DbName}.json"))
            {
               using (JsonTextReader jtr = new JsonTextReader(sr))
               {
                   JsonSerializer js = new JsonSerializer();
                   return js.Deserialize<List<T>>(jtr);
               }
            }
        }
        private void SaveList<T>(List<T> db, string DbName)
        {
            using (StreamWriter sw = new StreamWriter($"{DbName}.json"))
            {
                using (JsonTextWriter jtw = new JsonTextWriter(sw))
                {
                    jtw.Formatting = Formatting.Indented;
                    JsonSerializer js = new JsonSerializer();
                    js.Serialize(jtw, db);
                }
            }
        }
        public void GetSchedule(Station station, Route rout, out List<ScheduleItem> result)
        {
            result = new List<ScheduleItem>();
            DateTime currentDt = DateTime.Now;
            List<Route> routes;
            if (rout.Name == "Select Route")
            {
                routes = Routes;
            }
            else
            {
                routes = new List<Route>() {rout };
            }
            foreach (var route in routes)
            {
                var routeStation = route.Stations
                    .FirstOrDefault(st => st.Station.Id == station.Id);
                if (routeStation != null)
                {
                    if (routeStation != route.Stations.Last())
                    {
                        int left = route.TimeToNextDepartureFromOrigin(routeStation, currentDt);
                        result.Add(new ScheduleItem
                        {
                            RouteName = route.Name,
                            Destination = route.Stations.Last().Station.Name,
                            MinutesLeft = left
                        });
                    }
                    if (routeStation != route.Stations.First())
                    {
                        int left = route.TimeToNextDepartureFromDest(routeStation, currentDt);
                        result.Add(new ScheduleItem
                        {
                            RouteName = route.Name,
                            Destination = route.Stations.First().Station.Name,
                            MinutesLeft = left
                        });
                    }
                }
            }
        }
    }
}
