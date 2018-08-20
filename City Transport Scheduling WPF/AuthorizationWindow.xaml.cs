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
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public MainWindow owner { get; set; }
        public AuthorizationWindow(MainWindow owner)
        {
            InitializeComponent();
            this.owner = owner;
            //if (owner.Db.Users.Items == null)
            //    owner.Db.Users.Items = new List<User>();
        }
        private void ButtonLogIn_Click(object sender, RoutedEventArgs e)
        {
            string login = TextBoxLogin.Text;
            string password = ClassLibrary.User.GetHash(passwordBox.Password);
            if (string.IsNullOrWhiteSpace(login))
            {
                TextBoxLogin.Focus();
                return;
            }
            if (string.IsNullOrWhiteSpace(password))
            {
                passwordBox.Focus();
                return;
            }
            var User = owner.Db.Users.Items.FirstOrDefault(u => (
                u.Login.ToLower() == login.ToLower() & u.Password == password));
            if (User != null)
            {
                owner.Db.LoadUserPreferences(User, out User);
                owner.CurrentUser = User;
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Invalid login or password!");
            }
        }

        private void ButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow window = new RegisterWindow(this)
            {
                Owner = this
            };
            window.ShowDialog();
        }
    }
}
