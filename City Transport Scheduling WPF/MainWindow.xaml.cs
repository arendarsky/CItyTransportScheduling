using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ClassLibrary;
using ClassLibrary.Helpers;
using ClassLibrary.Interfaces;

namespace City_Transport_Scheduling_WPF
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public IStorage Db { get; set; }
        List<Route> AllRoutes { get; set; }
        List<Station> AllStations { get; set; }
        bool Log { get; set; }
        bool FavStOn { get; set; }
        public Station CurStation { get; set; }
        public Route CurRoute { get; set; }
        public User CurrentUser { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            SetSelectItems();
            if (MessageBox.Show("Do you want to load from database?","Loading priority",
                    MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                Db = Factory.Instance.GetDatabaseStorage();
            else
                Db = Factory.Instance.GetFileStorage(false);

            CurRoute = AllRoutes.First();
            CurStation = AllStations.First();
            FillStationSource();
            FillRouteSource();
        }
        private void FillStationSource()
        {
            if (CurRoute.Name == AllRoutes.First().Name)
            {
                ComboBoxStations.ItemsSource = AllStations.Concat(Db.Stations.Items);
            }
            else
            {
                ComboBoxStations.ItemsSource = AllStations.Concat(Db.Stations.Items.Where(
                    s => CurRoute.Stations.FirstOrDefault(st => st.StationId == s.Id) != null));
            }
            
        }
        private void FillRouteSource()
        {
            if (CurStation.Name == AllStations.First().Name)
            {
                ComboBoxRoutes.ItemsSource = AllRoutes.Concat(Db.Routes.Items);
            }
            else
            {
                StationSelect();
            }
            
        }
        private void ComboBoxRoutes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurRoute = ComboBoxRoutes.SelectedItem as Route;
            FillStationSource();
            GetResult();
        }

        private void ComboBoxStations_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CurStation = ComboBoxStations.SelectedItem as Station;
            FillRouteSource();
            GetResult();
        }
        private void StationSelect()
        {
            ComboBoxRoutes.ItemsSource = AllRoutes.Concat(Db.Routes.Items.Where(r =>
                 r.Stations.FirstOrDefault(s => s.StationId == CurStation.Id) != null));
        }
        private void GetResult()
        {
            ScheduleBuilder SchBld = new ScheduleBuilder(Db.Routes.Items.ToList(), CurStation, CurRoute);
            SchBld.GetSchedule(out List<ScheduleItem> result);
            dataGrid.ItemsSource = result;
        }

        private void ButtonLog_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentUser == null)
            {
                AuthorizationWindow window = new AuthorizationWindow(this)
                {
                    Owner = this
                };
                if (window.ShowDialog() == true)
                {
                    ButtonLog.Content = "Log Out";
                    ButtonAddFavorite.IsEnabled = true;
                    ButtonFavorites.IsEnabled = true;

                }
            }
            else
            {
                CurrentUser = null;
                ButtonLog.Content = "Log In";
                ButtonAddFavorite.IsEnabled = false;
                ButtonFavorites.IsEnabled = false;
                Db.SaveAll();
            }
        }
        private void ButtonFavorites_Click(object sender, RoutedEventArgs e)
        {
            FavoritesWindow window = new FavoritesWindow(this)
            {
                Owner = this
            };
            if (window.ShowDialog() == true)
            {
                CurStation = Db.Stations.Items.FirstOrDefault(s => s.Id == window.CurStation.Id);
                CurRoute = AllRoutes.First();
                ComboBoxRoutes.SelectedItem = AllRoutes.First();
                ComboBoxStations.SelectedItem = CurStation;
                StationSelect();
            }
        }

        private void ButtonAddFavorite_Click(object sender, RoutedEventArgs e)
        {
            if (CurStation.Name != AllStations.First().Name)
            {
                if (CurrentUser.FavoriteStations.Find(st => st.StationId == CurStation.Id) != null)
                {
                    MessageBox.Show("The station has already been in favorites!");
                }
                else
                {
                    AddFavoriteWindow window = new AddFavoriteWindow(this)
                    {
                        Owner = this
                    };
                    window.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Choose a station!", "Attention");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (Context context = new Context())
            {          
                Db.SaveAll();
            }
        }
        private void SetSelectItems()
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
        }
    }
}
