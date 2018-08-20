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
using System.Windows.Shapes;
using ClassLibrary;

namespace City_Transport_Scheduling_WPF
{
    /// <summary>
    /// Логика взаимодействия для AddFavoriteWindow.xaml
    /// </summary>
    public partial class AddFavoriteWindow : Window
    {
        public UserStation CurUserStation { get; set; }
        public FavoritesWindow owner { get; set; }
        public MainWindow mainowner { get; set; }
        public bool AddSt { get; set; }
        public AddFavoriteWindow(MainWindow owner)
        {
            InitializeComponent();
            mainowner = owner;
            TextBoxName.Text = mainowner.CurStation.Name;
            ButtonAdd.Content = "Add Station";
            AddSt = true;
        }
        public AddFavoriteWindow(UserStation station, FavoritesWindow owner)
        {
            InitializeComponent();
            CurUserStation = station;
            this.owner = owner;
            TextBoxName.Text = CurUserStation.Station.Name;
            TextBoxDesc.Text = CurUserStation.Description;
            ButtonAdd.Content = "Edit Station";
            AddSt = false;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            string Desc = TextBoxDesc.Text;
            if (AddSt)
            {
                var station = new UserStation
                {
                    StationId = mainowner.CurStation.Id,
                    Station = mainowner.CurStation,
                    Description = Desc
                };
                mainowner.CurrentUser.FavoriteStations.Add(station);
                mainowner.Db.AddFavSt(mainowner.CurrentUser, station);
            }
            else
            {
                owner.owner.Db.EditFavSt(CurUserStation, Desc);
                owner.owner.CurrentUser.FavoriteStations.Find(st => st.Id == CurUserStation.Id).
                    Description = Desc;
                
            }
            DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
