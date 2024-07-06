using Dapper;
using DBAccess;
using Microsoft.Data.SqlClient;
using Project_s_classes;
using RestaurantPanel;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restaurant_Pages
{
    /// <summary>
    /// Interaction logic for RestaurantSearch.xaml
    /// </summary>
    public partial class RestaurantSearch : Window
    {
        public RestaurantSearch()
        {
            InitializeComponent();
            LoadRestaurants();  
        }

        DataAccess dataAccess = new DataAccess();

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            LoadRestaurants();
        }

        private void LoadRestaurants()
        {
            var restaurants = dataAccess.LoadData<Restaurants, dynamic>("SELECT * FROM dbo.Restaurants", new { });
            RestaurantListView.ItemsSource = restaurants;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string? SelectedAspect = SearchCriteriaComboBox.SelectedItem.ToString().Split(':')[1].Trim();

            if (string.IsNullOrEmpty(txtSearch.Text) && SelectedAspect != "Have Complaints")
            {
                MessageBox.Show("You should write something on the text box, please try again");
            }
            else
            {
                string sqlStatement = "SELECT * FROM dbo.Restaurants";
                var Restaurants = dataAccess.LoadData<Restaurants, dynamic>(sqlStatement, new { });
                
                string Check = txtSearch.Text;

                if(string.IsNullOrEmpty(SelectedAspect))
                {
                    MessageBox.Show("Choose one of the option in combobox");
                }
                else
                {
                    switch(SelectedAspect)
                    {
                        case "Name":
                            var filter = Restaurants.Where(x => x.Name == Check);
                            RestaurantListView.ItemsSource = filter;
                            break;
                        case "City":
                            filter = Restaurants.Where(x => x.City == Check);
                            RestaurantListView.ItemsSource = filter;
                            break;
                        case "Rating":
                            try
                            {
                                float rating = float.Parse(Check);
                                filter = Restaurants.Where(x => x.AverageRating == rating);
                                RestaurantListView.ItemsSource = filter;
                            }
                            catch(FormatException)
                            {
                                MessageBox.Show("The input format for the average rating is wrong, float or int.");
                            }
                            break;
                        case "Have Complaints":
                            filter = Restaurants.Where(x => x.haveComplaints == true);  
                            RestaurantListView.ItemsSource = filter;
                            break;
                        default:
                            break;
                    }
                }
            }
        }
        private void RestaurantListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if(RestaurantListView.SelectedItem is Restaurants selectedRestaurant)
                {
                    RestaurantsPanel restaurantPanel = new RestaurantsPanel(selectedRestaurant);
                    restaurantPanel.Show();
                    this.Close();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
