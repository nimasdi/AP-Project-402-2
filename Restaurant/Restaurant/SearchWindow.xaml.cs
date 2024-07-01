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
using DBAccess;
using Project_s_classes;

namespace Restaurant
{
    /// <summary>
    /// Interaction logic for SearchWindow.xaml
    /// </summary>
    public partial class SearchWindow : Window
    {
        private readonly DataAccess _dataAccess;
        private List<Restaurants> _restaurants;

        public SearchWindow()
        {
            InitializeComponent();
            _dataAccess = new DataAccess();
            LoadRestaurants();
        }

        private void LoadRestaurants()
        {
            string sql = "SELECT RestaurantID, Name, City, Address, AverageRating, IsReservationEnabled, AdminID, ServiceType FROM dbo.Restaurants";
            _restaurants = _dataAccess.LoadData<Restaurants, dynamic>(sql, new { }).ToList();
            RestaurantListView.ItemsSource = _restaurants;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var filteredRestaurants = _restaurants.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(CityTextBox.Text))
            {
                filteredRestaurants = filteredRestaurants.Where(r => r.City.Contains(CityTextBox.Text));
            }

            if (!string.IsNullOrWhiteSpace(RestaurantNameTextBox.Text))
            {
                filteredRestaurants = filteredRestaurants.Where(r => r.Name.Contains(RestaurantNameTextBox.Text));
            }

            if (decimal.TryParse(MinRatingTextBox.Text, out decimal minRating))
            {
                filteredRestaurants = filteredRestaurants.Where(r => r.AverageRating >= (float)minRating);
            }

            if (ServiceTypeComboBox.SelectedItem is ComboBoxItem selectedItem && selectedItem.Content.ToString() != "All")
            {
                string serviceType = selectedItem.Content.ToString();
                filteredRestaurants = filteredRestaurants.Where(r => r.ServiceType.Contains(serviceType));
            }

            RestaurantListView.ItemsSource = filteredRestaurants.ToList();
        }

        private void RestaurantListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RestaurantListView.SelectedItem is Restaurants selectedRestaurant)
            {
                // go to restaurant details window
                var restaurantDetailsWindow = new RestaurantDetailsWindow(selectedRestaurant.RestaurantID);
                restaurantDetailsWindow.Show();
                this.Close();
            }
        }
    }
}
