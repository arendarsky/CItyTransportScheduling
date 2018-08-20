using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClassLibrary.Interfaces;
using System.Data.Entity;

namespace ClassLibrary
{
    internal class FileStorage: IStorage
    {
        IRepository<Station> _stationRepository;
        IRepository<Route> _routeRepository;
        IRepository<User> _userRepository;
        bool _loaded;
        public bool ForDb;
        public void AddFavSt( User user, UserStation station)
        {

        }
        public void EditFavSt(UserStation station, string Desc)
        {

        }
        public void LoadUserPreferences(User user, out User User)
        {
            User = user;
        }
        private void Load()
        {
            if (_loaded)
                return;
            if (ForDb)
            {
                _stationRepository = new FileRepository<Station>("./ClassLibrary/Data/Stations.json");
                _routeRepository = new FileRepository<Route>("./ClassLibrary/Data/Routes.json");
                _userRepository = new FileRepository<User>("./ClassLibrary/Data/Users.json");
            }
            else
            {
                _stationRepository = new FileRepository<Station>("../../../ClassLibrary/Data/Stations.json");
                _routeRepository = new FileRepository<Route>("../../../ClassLibrary/Data/Routes.json");
                _userRepository = new FileRepository<User>("../../../ClassLibrary/Data/Users.json");
            }
            foreach (var r in _routeRepository.Items)
                foreach (var st in r.Stations)
                    st.Station = _stationRepository.Items.First(c => c.Id == st.StationId);
            foreach (var u in _userRepository.Items)
                foreach (var st in u.FavoriteStations)
                    st.Station = _stationRepository.Items.First(c => c.Id == st.StationId);
            _loaded = true;
        }
        public void SaveAll()
        {
            if (!_loaded)
                return;
            _stationRepository.Save();
            _routeRepository.Save();
            _userRepository.Save();
        }
        public void AddUser(User user)
        {

        }
        public IRepository<Station> Stations
        {
            get
            {
                Load();
                return _stationRepository;
            }
        }
        public IRepository<Route> Routes
        {
            get
            {
                Load();
                return _routeRepository;
            }
        }
        public IRepository<User> Users
        {
            get
            {
                Load();
                return _userRepository;
            }
        }
        public void RemoveFavSt(User user, UserStation station)
        {
            user.FavoriteStations.Remove(station);
        }
    }
    internal class DatabaseStorage : IStorage
    {
        IRepository<Station> _stationRepository;
        IRepository<Route> _routeRepository;
        IRepository<User> _userRepository;
        Context context;
        bool _loaded;
        private void Load()
        {
            if (_loaded)
                return;
            using(context = new Context())
            {
                _stationRepository = new DatabaseRepository<Station>(context.Stations.ToList());
                _routeRepository = new DatabaseRepository<Route>(
                    context.Routes.Include(r => r.Stations).ToList()); ;
                _userRepository = new DatabaseRepository<User>(
                    context.Users.ToList()); ;
            }
            _loaded = true;
        }
        public void SaveAll()
        {
 
        }
        public void AddUser(User user)
        {
            using (context = new Context())
            {
                context.Users.Add(user);
                context.SaveChanges();
            }
        }
        public IRepository<Station> Stations
        {
            get
            {
                Load();
                return _stationRepository;
            }
        }
        public IRepository<Route> Routes
        {
            get
            {
                Load();
                return _routeRepository;
            }
        }
        public IRepository<User> Users
        {
            get
            {
                Load();
                return _userRepository;
            }
        }
        public void RemoveFavSt(User user, UserStation station)
        {
            using (context = new Context())
            {
                station = context.FavoriteStations.First(st => st.Id == station.Id);
                context.FavoriteStations.Remove(station);
                context.SaveChanges();
            }
        }
        public void AddFavSt(User user, UserStation station)
        {
            using(context = new Context())
            {
                station.Station = context.Stations.First(s => s.Id == station.StationId);
                context.Users.First(u => u.Login == user.Login).FavoriteStations.Add(station);
                context.SaveChanges();
            }
            
        }
        public void EditFavSt(UserStation station, string Desc)
        {
            using (context = new Context())
            {
                context.FavoriteStations.First(s => s.Id == station.Id).Description = Desc;
                context.SaveChanges();
            }
        }
        public void LoadUserPreferences(User user, out User User)
        {
            using (context = new Context())
            {
                User = context.Users.Include(u => u.FavoriteStations).SingleOrDefault(u => u.Id ==user.Id);
                foreach (var st in User.FavoriteStations)
                    st.Station = context.Stations.First(s => s.Id == st.StationId);
            }
        }
    }
}
