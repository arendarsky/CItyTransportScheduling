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
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        public AuthorizationWindow  owner { get; set; }
        public RegisterWindow(AuthorizationWindow owner)
        {
            InitializeComponent();
            this.owner = owner;
        }

        private void ButtonOk_Click(object sender, RoutedEventArgs e)
        {
            string login = TextBoxLogin.Text;
            string password = User.GetHash(passwordBox.Password);
            string passwordConf = User.GetHash(passwordBoxConf.Password);
            if (string.IsNullOrWhiteSpace(login))
            {
                TextBoxLogin.Focus();
                return;
            }
            if (owner.owner.Db.Users.Items.FirstOrDefault(u => u.Login.ToLower() == login.ToLower()) != null)
            {
                MessageBox.Show("The login has already been taken!");
                return;
            }
            if (string.IsNullOrWhiteSpace(passwordBox.Password))
            {
                passwordBox.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(passwordBoxConf.Password))
            {
                passwordBoxConf.Focus();
                return;
            }
            if (password != passwordConf)
            {
                MessageBox.Show("Passwords are not equal!", "Error");
                return;
            }
            User NewUser = new User
            {
                Login = login,
                Password = password,
                FavoriteStations = new List<UserStation>()
            };
            owner.owner.Db.Users.Add(NewUser);
            owner.owner.Db.AddUser(NewUser);
            DialogResult = true;
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
