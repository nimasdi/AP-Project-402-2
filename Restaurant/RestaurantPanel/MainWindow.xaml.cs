using DBAccess;
using Project_s_classes;
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

namespace RestaurantPanel
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

            if (!int.TryParse(password, out int passwordAsInt))
            {
                MessageBox.Show("Password must be numeric. Please try again.", "Invalid Password", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            DataAccess dataAccess = new DataAccess();

            string sqlAdmin = "SELECT * FROM dbo.Restaurants WHERE UserName = @UserName AND Password = @Password";
            List<Restaurants> restaurants = dataAccess.LoadData<Restaurants, dynamic>(sqlAdmin, new { UserName = username, Password = passwordAsInt });

            if (restaurants.Count > 0)
            {
                Restaurants loginRestaurant = restaurants[0];

                RestaurantsPanel panel = new RestaurantsPanel(loginRestaurant);
                panel.Show();
                this.Close();
                return;
            }

            MessageBox.Show("Invalid username or password. Please try again.");
        }
    }
}