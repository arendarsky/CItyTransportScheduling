using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace ClassLibrary
{
    public class DbRepository : IRepository
    {
        public List<Route> Routes { get; set; }
        public List<Station> Stations { get; set; }
        public List<User> Users { get; set; }
        private Context context { get; set; }
        public DbRepository()
        {
            AllRoutes = new List<Route>()
            {
                new Route()
                {
                    Name = "Select Route"
                }
            };
            AllStations = new List<Station>
            {
                new Station
                {
                    Name = "Select Station"
                }
            };
            context = new Context();
            Routes = context.Routes.ToList();
            Stations = context.Stations.ToList();
            Users = context.Users.ToList();
        }
        public void Save()
        {
            foreach (var u in Users)
            {
                context.Users.AddOrUpdate(c => c.Login, u);
            }
            context.SaveChanges();
            context.Dispose();
        }
        public void RemoveFavSt(UserStation Station)
        {
            context.FavoriteStations.Remove(Station);
            context.SaveChanges();
        }
        
    }
}
