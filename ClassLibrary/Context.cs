using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace ClassLibrary
{
    public class Context: DbContext
    {
        public DbSet<Route> Routes { get; set; }
        public DbSet<Station> Stations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserStation> FavoriteStations { get; set; }
        public Context() : base("ScheduleDb")
        {

        }
    }
}
