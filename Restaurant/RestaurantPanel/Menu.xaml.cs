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
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Window
    {
        private readonly int _restaurantId;
        private readonly DataAccess _dataAccess;

        public Menu(int restaurantId)
        {
            InitializeComponent();
            _restaurantId = restaurantId;
            _dataAccess = new DataAccess();

            LoadMenuCategories();
        }

        private void LoadMenuCategories()
        {
            string sql = "SELECT DISTINCT Category FROM dbo.Menus WHERE RestaurantID = @RestaurantID";
            var categories = _dataAccess.LoadData<string, dynamic>(sql, new { RestaurantID = _restaurantId });

            MenuCategoriesListBox.ItemsSource = categories;
        }

        private void LoadMenuItems(string category)
        {
            string sql = "SELECT * FROM dbo.Menus WHERE RestaurantID = @RestaurantID AND Category = @Category";
            var menuItems = _dataAccess.LoadData<Project_s_classes.Menu, dynamic>(sql, new { RestaurantID = _restaurantId, Category = category });

            MenuItemsListView.ItemsSource = menuItems;
        }

        private void MenuCategoriesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuCategoriesListBox.SelectedItem is string selectedCategory)
            {
                LoadMenuItems(selectedCategory);
            }
        }

        private void MenuItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuItemsListView.SelectedItem is Project_s_classes.Menu selectedMenu)
            {
                LoadComments((int)selectedMenu.MenuID);
            }
        }

        private void LoadComments(int menuId)
        {
            string sql = "SELECT * FROM dbo.Comments WHERE MenuID = @MenuID";
            var comments = _dataAccess.LoadData<Comment, dynamic>(sql, new { MenuID = menuId });

            CommentsListBox.ItemsSource = comments;
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (MenuCategoriesListBox.SelectedItem is string selectedCategory)
            {
                var dialog = new AddMenuItemDialog(selectedCategory, _restaurantId);
                if (dialog.ShowDialog() == true)
                {
                    try
                    {
                        var newItem = new Project_s_classes.Menu
                        {
                            RestaurantId = _restaurantId,
                            Category = selectedCategory,
                            ItemName = dialog.ItemName,
                            Ingredients = dialog.Ingredients,
                            Price = dialog.Price,
                            QuantityAvailable = dialog.Quantity,
                            ImageURL = dialog.ImageURL,
                            AverageRating = 0
                        };

                        string sql = "INSERT INTO dbo.Menus (RestaurantID, Category, ItemName, Ingredients, Price, QuantityAvailable, ImageURL, AverageRating) " +
                                     "VALUES (@RestaurantID, @Category, @ItemName, @Ingredients, @Price, @QuantityAvailable, @ImageURL, @AverageRating)";

                        _dataAccess.SaveData(sql, new
                        {
                            newItem.RestaurantId,
                            newItem.Category,
                            newItem.ItemName,
                            newItem.Ingredients,
                            newItem.Price,
                            newItem.QuantityAvailable,
                            newItem.ImageURL,
                            newItem.AverageRating
                        });

                        LoadMenuItems(selectedCategory);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error adding menu item: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        private void EditItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (MenuItemsListView.SelectedItem is Project_s_classes.Menu selectedMenu)
            {
                var dialog = new EditMenuItemDialog(selectedMenu);
                if (dialog.ShowDialog() == true)
                {
                    string sql = "UPDATE dbo.Menus SET ItemName = @ItemName, Ingredients = @Ingredients, " +
                                 "Price = @Price, QuantityAvailable = @QuantityAvailable, ImageURL = @ImageURL, AverageRating = @AverageRating " +
                                 "WHERE MenuID = @MenuID";
                    _dataAccess.SaveData(sql, selectedMenu);

                    LoadMenuItems(selectedMenu.Category);
                }
            }
        }

        private void DeleteItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (MenuItemsListView.SelectedItem is Project_s_classes.Menu selectedMenu)
            {
                string sql = "DELETE FROM dbo.Menus WHERE MenuID = @MenuID";
                _dataAccess.SaveData(sql, new { selectedMenu.MenuID });

                LoadMenuItems(selectedMenu.Category);
            }
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new InputDialog("Add Category", "Enter category name:");
            if (dialog.ShowDialog() == true)
            {
                string newCategory = dialog.Input;

                string sql = "INSERT INTO dbo.Menus (RestaurantID, Category) VALUES (@RestaurantID, @Category)";
                _dataAccess.SaveData(sql, new { RestaurantID = _restaurantId, Category = newCategory });

                LoadMenuCategories();
            }
        }

        private void EditCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (MenuCategoriesListBox.SelectedItem is string selectedCategory)
            {
                var dialog = new InputDialog("Edit Category", "Enter new category name:", selectedCategory);
                if (dialog.ShowDialog() == true)
                {
                    string newCategory = dialog.Input;

                    string sql = "UPDATE dbo.Menus SET Category = @NewCategory WHERE RestaurantID = @RestaurantID AND Category = @OldCategory";
                    _dataAccess.SaveData(sql, new { NewCategory = newCategory, RestaurantID = _restaurantId, OldCategory = selectedCategory });

                    LoadMenuCategories();
                }
            }
        }

        private void DeleteCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (MenuCategoriesListBox.SelectedItem is string selectedCategory)
            {
                string sql = "DELETE FROM dbo.Menus WHERE RestaurantID = @RestaurantID AND Category = @Category";
                _dataAccess.SaveData(sql, new { RestaurantID = _restaurantId, Category = selectedCategory });

                LoadMenuCategories();
            }
        }
    }
}
