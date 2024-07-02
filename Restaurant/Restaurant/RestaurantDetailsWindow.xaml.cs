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
using System.Net.Mail;

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
        public Dictionary<string, List<Project_s_classes.Menu>> MenuItemsByCategory { get; set; }
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();

        public bool IsReservationButtonVisible => _restaurant.AverageRating >= 4.5;

        public RestaurantDetailsWindow(Restaurants restaurant, Users currentUser)
        {
            InitializeComponent();
            _restaurant = restaurant;
            _dataAccess = new DataAccess();
            _currentUser = currentUser;
            DataContext = this;

            MenuItemsByCategory = GetMenuItemsForRestaurant(_restaurant.RestaurantID);
            MenuCategoriesListBox.ItemsSource = MenuItemsByCategory.Keys;

            CheckReservationEligibility();
        }

        private void CheckReservationEligibility()
        {
            if (_restaurant.AverageRating < 4.5)
            {
                _restaurant.IsReservationEnabled = false;
            }
        }

        private Dictionary<string, List<Project_s_classes.Menu>> GetMenuItemsForRestaurant(int restaurantId)
        {
            string sql = "SELECT MenuID, RestaurantID, Category, ItemName, Ingredients, Price, ImageURL, AverageRating, QuantityAvailable " +
                         "FROM dbo.Menus WHERE RestaurantID = @RestaurantID";
            var menuItems = _dataAccess.LoadData<Project_s_classes.Menu, dynamic>(sql, new { RestaurantID = restaurantId });

            var groupedMenuItems = menuItems.GroupBy(m => m.Category ?? "Uncategorized")
                                            .ToDictionary(g => g.Key, g => g.ToList());

            return groupedMenuItems;
        }

        private void MenuCategoriesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuCategoriesListBox.SelectedItem is string selectedCategory)
            {
                MenuItemsListView.ItemsSource = MenuItemsByCategory[selectedCategory];
            }
        }

        private void MenuItemsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MenuItemsListView.SelectedItem is Project_s_classes.Menu selectedMenu)
            {
                if (selectedMenu.QuantityAvailable > 0)
                {
                    var existingCartItem = CartItems.FirstOrDefault(ci => ci.MenuID == selectedMenu.MenuID);
                    if (existingCartItem != null)
                    {
                        if (existingCartItem.Quantity < selectedMenu.QuantityAvailable)
                        {
                            existingCartItem.Quantity++;
                        }
                    }
                    else
                    {
                        CartItems.Add(new CartItem { MenuID = selectedMenu.MenuID, ItemName = selectedMenu.ItemName, Quantity = 1 });
                    }

                    CartListBox.ItemsSource = null;
                    CartListBox.ItemsSource = CartItems;
                }
                else
                {
                    MessageBox.Show("This item is out of stock.", "Out of Stock", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }

        private void CheckoutButton_Click(object sender, RoutedEventArgs e)
        {
            if (CartItems.Count == 0)
            {
                MessageBox.Show("Your cart is empty.", "Empty Cart", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var selectedPaymentMethod = (PaymentMethodComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            if (selectedPaymentMethod == null)
            {
                MessageBox.Show("Please select a payment method.", "Payment Method", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (selectedPaymentMethod == "Cash")
            {
                MessageBox.Show("Order placed successfully. Please pay in cash upon delivery.", "Order Placed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (selectedPaymentMethod == "Online")
            {
                OnlinePayment();
            }

            // Clear cart after checkout
            CartItems.Clear();
            CartListBox.ItemsSource = null;
        }

        private void OnlinePayment()
        {
            try
            {
                // TODO: Implement online payment logic

                MessageBox.Show("Order placed successfully. Payment instructions have been sent to your email.", "Order Placed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send payment instructions. Please try again.", "Payment Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MakeReservationButton_Click(object sender, RoutedEventArgs e)
        {
            if (_restaurant.IsReservationEnabled && IsUserEligibleForReservation())
            {
                var reservationWindow = new ReservationWindow(_restaurant, _currentUser, this);
                reservationWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("This restaurant does not offer reservations or you are not eligible.", "Reservation Not Available", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void HandleReservationCancellation(decimal penaltyAmount)
        {
            _restaurant.PenaltyRevenue += penaltyAmount;

            // Update penalty revenue in the database
            string updateSql = "UPDATE dbo.Restaurants SET PenaltyRevenue = @PenaltyRevenue WHERE RestaurantID = @RestaurantID";
            _dataAccess.SaveData(updateSql, new { PenaltyRevenue = _restaurant.PenaltyRevenue, RestaurantID = _restaurant.RestaurantID });
        }

        private bool IsUserEligibleForReservation()
        {
            var today = DateTime.Today;

            if (_currentUser.ServiceExpiration == null || _currentUser.ServiceExpiration < today)
            {
                return false;
            }

            switch (_currentUser.ServiceTier)
            {
                case "Bronze":
                    return _currentUser.ReservationsMadeThisMonth < 2;
                case "Silver":
                    return _currentUser.ReservationsMadeThisMonth < 5;
                case "Gold":
                    return _currentUser.ReservationsMadeThisMonth < 15;
                default:
                    return false;
            }
        }
    }
    public class CartItem
    {
        public int MenuID { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }
}
