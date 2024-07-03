using Project_s_classes;
using Restaurant;
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

namespace Restaurant_pages
{
    /// <summary>
    /// Interaction logic for main_menu.xaml
    /// </summary>
    public partial class main_menu : Window
    {
        private readonly Users _currentUser;

        public main_menu(Users currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var profileWindow = new ProfileWindow(_currentUser);
            profileWindow.Show();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchWindow = new SearchWindow(_currentUser);
            searchWindow.Show();
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            var ordersWindow = new OrdersWindow(_currentUser.UserID);
            ordersWindow.Show();
        }


        private void ComplaintButton_Click(object sender, RoutedEventArgs e)
        {
            var complaintWindow = new ComplaintWindow(_currentUser.UserID);
            complaintWindow.Show();
        }

        private void ComplaintButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TrackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
