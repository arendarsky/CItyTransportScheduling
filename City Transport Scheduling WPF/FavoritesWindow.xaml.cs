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
    /// Логика взаимодействия для FavoritesWindow.xaml
    /// </summary>
    public partial class FavoritesWindow : Window
    {
        public Station CurStation { get; set; }
        public MainWindow owner { get; set; }
        public FavoritesWindow(MainWindow owner)
        {
            InitializeComponent();
            this.owner = owner;
            dataGrid.ItemsSource = owner.CurrentUser.FavoriteStations;
        }

        private void ButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            UserStation Station = dataGrid.SelectedItem as UserStation;
            if (Station != null)
            {
                AddFavoriteWindow window = new AddFavoriteWindow(Station, this)
                {
                    Owner = this
                };
                if(window.ShowDialog() == true)
                {
                    dataGrid.ItemsSource = null;
                    dataGrid.ItemsSource = owner.CurrentUser.FavoriteStations;
                }
            }
            else
            {
                MessageBox.Show("Choose station!", "Attention");
            }
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            UserStation Station = dataGrid.SelectedItem as UserStation;
            if (Station != null)
            {
                if(MessageBox.Show("Are you sure you want to DELETE the station?", "Attention",
                    MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                {
                    owner.Db.RemoveFavSt(owner.CurrentUser, Station);
                    owner.CurrentUser.FavoriteStations.Remove(Station);
                    dataGrid.ItemsSource = null;
                    dataGrid.ItemsSource = owner.CurrentUser.FavoriteStations;
                }
            }
            else
            {
                MessageBox.Show("Choose a station!", "Attention");
            }
        }

        private void ButtonSelect_Click(object sender, RoutedEventArgs e)
        {
            UserStation Station = dataGrid.SelectedItem as UserStation;
            if (Station != null)
            {
                CurStation = Station.Station;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Choose a station!", "Attention");
            }
        }
    }
}
