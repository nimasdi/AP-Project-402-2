using Dapper;
using DBAccess;
using Microsoft.Data.SqlClient;
using Project_s_classes;
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
            ResultsDataGrid.ItemsSource = restaurants;
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
                            ResultsDataGrid.ItemsSource = filter;
                            break;
                        case "City":
                            filter = Restaurants.Where(x => x.City == Check);
                            ResultsDataGrid.ItemsSource = filter;
                            break;
                        case "Rating":
                            try
                            {
                                float rating = float.Parse(Check);
                                filter = Restaurants.Where(x => x.AverageRating == rating);
                                ResultsDataGrid.ItemsSource = filter;
                            }
                            catch(FormatException)
                            {
                                MessageBox.Show("The input format for the average rating is wrong, float or int.");
                            }
                            break;
                        case "Have Complaints":
                            filter = Restaurants.Where(x => x.haveComplaints == true);  
                            ResultsDataGrid.ItemsSource = filter;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void ResultsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void ResultsDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.Column.Header.ToString() == "Password")
            {
                TextBox editedElement = e.EditingElement as TextBox;
                if (editedElement != null)
                {
                    string newPassword = editedElement.Text;
                    var selectedRestaurant = (Restaurants)e.Row.Item;

                    bool isValidPassword = newPassword.Length == 8 && Regex.IsMatch(newPassword, @"^\d+$");

                    if (isValidPassword)
                    {
                        bool isUniquePassword = true;
                        var restaurants = dataAccess.LoadData<Restaurants, dynamic>("SELECT * FROM dbo.Restaurants", new { });

                        foreach (var restaurant in restaurants)
                        {
                            if (restaurant.Password.ToString() == newPassword && restaurant.RestaurantID != selectedRestaurant.RestaurantID)
                            {
                                isUniquePassword = false;
                                break;
                            }
                        }

                        if (isUniquePassword)
                        {
                            using (SqlConnection conn = new SqlConnection(dataAccess.ConnectionString))
                            {
                                conn.Open();
                                string query = "UPDATE Restaurants SET Password = @Password WHERE RestaurantID = @RestaurantID";
                                conn.Execute(query, new { Password = newPassword, RestaurantID = selectedRestaurant.RestaurantID });
                            }
                            MessageBox.Show("The password of the restaurant was changed successfully.");
                        }
                        else
                        {
                            MessageBox.Show("This password has already been used by another restaurant.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("The new password does not meet the requirements (8 digits).");
                    }
                }
            }
        }
    }
}
