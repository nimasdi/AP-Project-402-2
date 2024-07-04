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

namespace RestaurantPanel
{
    /// <summary>
    /// Interaction logic for UpdateAvailabilityWindow.xaml
    /// </summary>
    public partial class UpdateAvailabilityWindow : Window
    {
        private readonly DataAccess _dataAccess;
        private readonly int _restaurantId;
        private List<Project_s_classes.Menu> _menuItems;

        public UpdateAvailabilityWindow(int restaurantId)
        {
            InitializeComponent();
            _dataAccess = new DataAccess();
            _restaurantId = restaurantId;

            LoadMenuItems();
        }

        private void LoadMenuItems()
        {
            string sql = "SELECT MenuID, RestaurantID, Category, ItemName, Ingredients, Price, ImageURL, AverageRating, QuantityAvailable FROM dbo.Menus WHERE RestaurantID = @RestaurantID";
            _menuItems = _dataAccess.LoadData<Project_s_classes.Menu, dynamic>(sql, new { RestaurantID = _restaurantId });

            FoodItemsListBox.ItemsSource = _menuItems;
        }

        private void FoodItemsListBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (FoodItemsListBox.SelectedItem is Project_s_classes.Menu selectedMenuItem)
            {
                ItemNameTextBox.Text = selectedMenuItem.ItemName;
                CurrentQuantityTextBox.Text = selectedMenuItem.QuantityAvailable.ToString();
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (FoodItemsListBox.SelectedItem is Project_s_classes.Menu selectedMenuItem)
            {
                if (int.TryParse(NewQuantityTextBox.Text, out int newQuantity))
                {
                    selectedMenuItem.QuantityAvailable = newQuantity;

                    string sql = "UPDATE dbo.Menus SET QuantityAvailable = @QuantityAvailable WHERE MenuID = @MenuID";
                    _dataAccess.SaveData(sql, new { QuantityAvailable = selectedMenuItem.QuantityAvailable, MenuID = selectedMenuItem.MenuID });

                    MessageBox.Show("Item availability updated successfully.");
                    this.DialogResult = true;
                }
                else
                {
                    MessageBox.Show("Please enter a valid number.");
                }
            }
        }
    }
}
