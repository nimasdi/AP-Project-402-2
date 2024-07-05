using Project_s_classes;
using DBAccess;
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

namespace RestaurantPanel
{
    /// <summary>
    /// Interaction logic for RestaurantsPanel.xaml
    /// </summary>
    public partial class RestaurantsPanel : Window
    {
        private readonly Restaurants _currentRestaurant;

        public RestaurantsPanel(Restaurants restaurant)
        {
            _currentRestaurant = restaurant;
            InitializeComponent();
        }

        private void ChangeMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Menu menuWindow = new Menu((int)_currentRestaurant.RestaurantID);
                menuWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening menu: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeInventory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentRestaurant != null)
                {
                    UpdateAvailabilityWindow availabilityWindow = new UpdateAvailabilityWindow((int)_currentRestaurant.RestaurantID);
                    availabilityWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening inventory update window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EnableReservationService_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentRestaurant != null)
                {
                    ReservationServiceWindow reservationWindow = new ReservationServiceWindow((int)_currentRestaurant.RestaurantID);
                    reservationWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error enabling reservation service: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OrderReservationHistory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ViewHistoryWindow historyWindow = new ViewHistoryWindow((int)_currentRestaurant.RestaurantID);
                historyWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening history window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}

