using DBAccess;
using Project_s_classes;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
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
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(txtSearch.Text))
            {
                MessageBox.Show("You should write something on the text box, please try again");
            }
            else
            {
                string sqlStatement = "SELECT * FROM dbo.Restaurants";
                var Restaurants = dataAccess.LoadData<Restaurants, dynamic>(sqlStatement, new { });
                

                string? SelectedAspect = SearchCriteriaComboBox.SelectedItem.ToString();
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
                            float rating = float.Parse(Check);
                            filter = Restaurants.Where(x => x.AverageRating == rating);
                            ResultsDataGrid.ItemsSource = filter;
                            break;
                        case "Unhandled Complaints":
                            //i should contain a complaints part for this
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
}
