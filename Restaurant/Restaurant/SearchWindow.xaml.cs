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
        private readonly Users _currentUser;

        public SearchWindow(Users currentUser)
        {
            InitializeComponent();
            _dataAccess = new DataAccess();
            LoadRestaurants();
            _currentUser = currentUser;
        }

        private void LoadRestaurants()
        {
            string sql = "SELECT * FROM dbo.Restaurants";
            _restaurants = _dataAccess.LoadData<Restaurants, dynamic>(sql, new { }).ToList();
            RestaurantListView.ItemsSource = _restaurants;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var filteredRestaurants = _restaurants.AsEnumerable();
            string selectedOption = GetSelectedServiceType();
            if(selectedOption == "All")
            {
                LoadRestaurants();
            }
            else
            {
                switch (selectedOption)
                {
                    case "Delivery":
                        filteredRestaurants = filteredRestaurants.Where(restaurant => restaurant.ServiceType == "delivery" ||  restaurant.ServiceType == "both");
                        if (filteredRestaurants.Any())
                        {
                            RestaurantListView.ItemsSource = filteredRestaurants;
                            
                        }
                        else
                        {
                            RestaurantListView.ItemsSource = null;
                            MessageBox.Show("There are no restaurant that offer delivery");
                            break;
                        }
                        break;
                    case "Dine-In":
                        filteredRestaurants = filteredRestaurants.Where(restaurant => restaurant.ServiceType == "dine-in" || restaurant.ServiceType == "both");
                        if (filteredRestaurants.Any())
                        {
                            RestaurantListView.ItemsSource= filteredRestaurants;
                            
                        }
                        else
                        {
                            RestaurantListView.ItemsSource = null;
                            MessageBox.Show("There are no restaurant that offer dine-in service");
                            break;
                        }
                        break;
                    case "Restaurant Name":
                        string name = RestaurantNameTextBox.Text;
                        if(string.IsNullOrEmpty(name))
                        {
                            filteredRestaurants = filteredRestaurants.Where(restaurant => restaurant.Name == name);
                            if (!filteredRestaurants.Any())
                            {
                                RestaurantListView.ItemsSource = filteredRestaurants;
                                break;
                            }
                            else
                            {
                                RestaurantListView.ItemsSource = null;
                                MessageBox.Show("There are no restaurant with that name");
                                
                                break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("The restaurant name text box should be filled to search based on that");
                            break;
                        }

                    case "Rating":
                        try
                        {
                            string rating_str = MinRatingTextBox.Text;
                            if (!string.IsNullOrEmpty(rating_str))
                            {
                                float rating = float.Parse(rating_str);
                                if(rating <= 5)
                                {
                                    filteredRestaurants = filteredRestaurants.Where(reataurant => reataurant.AverageRating == rating);
                                    if (filteredRestaurants.Any())
                                    {
                                        RestaurantListView.ItemsSource = filteredRestaurants;
                                        break;
                                    }
                                    else
                                    {
                                        RestaurantListView.ItemsSource = null;
                                        MessageBox.Show("There are no restaurant with that rating available");
                                        
                                        break;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("The rating cannot be bigger than 5");
                                    break;
                                }
                            }
                            else
                            {
                                MessageBox.Show("The rating text box cannot be empty with this optioin");
                                break;
                            }
                        }
                        catch (FormatException)
                        {
                            MessageBox.Show("The input format for rating is not correct please try again");
                            break;
                        }

                    case "City":
                        string city = CityTextBox.Text;
                        if(!string.IsNullOrEmpty(city))
                        {
                            filteredRestaurants = filteredRestaurants.Where(restaurant => restaurant.City == city);
                            if(filteredRestaurants.Any())
                            {
                                RestaurantListView.ItemsSource = filteredRestaurants;
                                break;
                            }
                            else
                            {
                                RestaurantListView.ItemsSource = null;
                                MessageBox.Show("There are no restaurant available in the mentioned city");
                                
                                break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("The city text box should be filled with the option you choosed");
                            break;
                        }
                }
            }

            
        }

        private string GetSelectedServiceType()
        {
            if(ServiceTypeComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                return selectedItem.Content.ToString();
            }
            return string.Empty;
        }

        private void RestaurantListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (RestaurantListView.SelectedItem is Restaurants selectedRestaurant)
                {
                    // Go to restaurant details window
                    var restaurantDetailsWindow = new RestaurantDetailsWindow(selectedRestaurant, _currentUser);
                    restaurantDetailsWindow.Show();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error opening restaurant details: " + ex.Message);
            }
        }
    }
}
