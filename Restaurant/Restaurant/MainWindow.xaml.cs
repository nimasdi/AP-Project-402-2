using DBAccess;
using Project_s_classes;
using Restaurant_pages;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restaurant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            string sqlStatement = "SELECT * FROM dbo.Users";
            DataAccess dataAccess = new DataAccess();

            List<Users> users = dataAccess.LoadData<Users, dynamic>(sqlStatement, new { });

            bool user_available = false;
            bool password_available = false;

            foreach (Users user in users)
            {
                if(user.UserName == username)
                {
                    user_available = true;
                    break;
                }
            }

            if (!user_available)
            {
                MessageBox.Show("No such user with the input username is available. You may sign up first");
            }

            foreach(Users user in users)
            {
                if(user.Password == password)
                {
                    password_available = true;
                    break;
                }
            }

            if(!password_available)
            {
                MessageBox.Show("Password is wrong");
            }
            // Todo: implement the login logic
            if (username == "admin" && password == "password")
            {
                MessageBox.Show("Login successful!");
                main_menu mainMenuWindow = new main_menu();
                mainMenuWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private void UsernameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }

}