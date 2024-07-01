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
using Microsoft.Data.SqlClient;
using Project_s_classes;
using DBAccess;
using Microsoft.VisualBasic.ApplicationServices;

namespace Restaurant
{
    /// <summary>
    /// Interaction logic for RestaurantDetailsWindow.xaml
    /// </summary>
    public partial class RestaurantDetailsWindow : Window
    {
        private readonly Restaurants _restaurant;
        private readonly DataAccess _dataAccess;
        private readonly Users _currentUser;


        public RestaurantDetailsWindow(Restaurants restaurant, Users currentUser)
        {
            InitializeComponent();
            _restaurant = restaurant;
            _dataAccess = new DataAccess();
            _currentUser = currentUser;
            DataContext = _restaurant;

            List<Project_s_classes.Menu> menuItems = GetMenuItemsForRestaurant(_restaurant.RestaurantID);
            MenuItemListBox.ItemsSource = menuItems;
        }

        private List<Project_s_classes.Menu> GetMenuItemsForRestaurant(int restaurantId)
        {
            string sql = "SELECT MenuID, RestaurantID, Category, ItemName, Ingredients, Price, ImageURL, AverageRating, QuantityAvailable " +
                         "FROM dbo.Menus WHERE RestaurantID = @RestaurantID";
            return _dataAccess.LoadData<Project_s_classes.Menu, dynamic>(sql, new { RestaurantID = restaurantId });
        }

        private void MenuItemListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuItemListBox.SelectedItem is Project_s_classes.Menu selectedMenu)
            {
                var menuItemDetailsWindow = new MenuItemDetailsWindow(selectedMenu,_currentUser);
                menuItemDetailsWindow.Show();
            }
        }
    }
}
