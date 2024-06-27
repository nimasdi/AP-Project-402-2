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
            string sqlStatement2 = "SELECT * FROM dbo.Admins";
            DataAccess dataAccess = new DataAccess();


            List<Users> users = dataAccess.LoadData<Users, dynamic>(sqlStatement, new { });
            List<Admin> admins = dataAccess.LoadData<Admin, dynamic>(sqlStatement2, new { });

            bool admin = false;
            foreach(Admin a in admins)
            {
                if(a.UserName == username)
                {
                    if(a.Password == password)
                    {
                        admin = true;
                        break;
                    }
                }
            }
            if(admin)
            {
                MessageBox.Show("Admin's login successful");
                // admin templamte = new admin template
                this.Close();
                //admintemp.show();
            }

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
                MessageBox.Show("Password is wrong, Try again!!");
            }

            else
            {
                MessageBox.Show("User's Login was succesfull");

                //rederict to the user's template
                //usertemp ut = new usertemp();
                this.Close();
                //ut.show();
            }
            
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            //rederecting to the register panel
            Register register = new Register();
            this.Close();
            register.Show();
        }
    }

}