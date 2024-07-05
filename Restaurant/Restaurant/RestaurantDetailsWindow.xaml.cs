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
using Microsoft.VisualBasic.ApplicationServices;
using System.Net.Mail;
using System.Net;

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

            MenuItemsByCategory = GetMenuItemsForRestaurant((int)_restaurant.RestaurantID);
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
                        CartItems.Add(new CartItem { MenuID = (int)selectedMenu.MenuID, ItemName = selectedMenu.ItemName, Quantity = 1 });
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

        private void MenuItemsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (MenuItemsListView.SelectedItem is Project_s_classes.Menu selectedMenu)
            {
                var menuItemDetailsWindow = new MenuItemDetailsWindow(selectedMenu, _currentUser);
                menuItemDetailsWindow.ShowDialog();
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

            decimal totalAmount = CartItems.Sum(item => item.Quantity * GetItemPrice(item.MenuID));

            Order newOrder = new Order(
                _currentUser.UserID,
                _restaurant.RestaurantID,
                DateTime.Now,
                totalAmount,
                selectedPaymentMethod,
                "Pending",
                false
            );

            foreach (var cartItem in CartItems)
            {
                new OrderDetail(newOrder.OrderId, cartItem.MenuID, cartItem.Quantity, GetItemPrice(cartItem.MenuID));
                UpdateMenuItemQuantity(cartItem.MenuID, cartItem.Quantity);
            }

            if (selectedPaymentMethod == "Cash")
            {
                MessageBox.Show("Order placed successfully. Please pay in cash upon delivery.", "Order Placed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (selectedPaymentMethod == "Online")
            {
                OnlinePayment(newOrder);
            }

            // Clear cart after checkout
            CartItems.Clear();
            CartListBox.ItemsSource = null;
        }

        private void UpdateMenuItemQuantity(int menuID, int quantityPurchased)
        {
            var menuItem = MenuItemsByCategory.Values.SelectMany(list => list).FirstOrDefault(m => m.MenuID == menuID);
            if (menuItem != null)
            {
                menuItem.QuantityAvailable -= quantityPurchased;

                string updateSql = "UPDATE dbo.Menus SET QuantityAvailable = @QuantityAvailable WHERE MenuID = @MenuID";
                _dataAccess.SaveData(updateSql, new { QuantityAvailable = menuItem.QuantityAvailable, MenuID = menuItem.MenuID });
            }
        }

        private void MakeReservationButton_Click(object sender, RoutedEventArgs e)
        {
            if (_restaurant.IsReservationEnabled && IsUserEligibleForReservation())
            {
                if (CartItems.Count == 0)
                {
                    MessageBox.Show("Your cart is empty.", "Empty Cart", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var selectedPaymentMethod = "Cash";

                decimal totalAmount = CartItems.Sum(item => item.Quantity * GetItemPrice(item.MenuID));

                Order newOrder = new Order(
                    _currentUser.UserID,
                    _restaurant.RestaurantID,
                    DateTime.Now,
                    totalAmount,
                    selectedPaymentMethod,
                    "Pending",
                    true
                );

                foreach (var cartItem in CartItems)
                {
                    new OrderDetail(newOrder.OrderId, cartItem.MenuID, cartItem.Quantity, GetItemPrice(cartItem.MenuID));
                    UpdateMenuItemQuantity(cartItem.MenuID, cartItem.Quantity);
                }

                MessageBox.Show("Order placed successfully. Please pay in cash upon delivery.", "Order Placed", MessageBoxButton.OK, MessageBoxImage.Information);

                // Clear cart after checkout
                CartItems.Clear();
                CartListBox.ItemsSource = null;

                _currentUser.IncrementReservationCount();

            }
            else
            {
                MessageBox.Show("This restaurant does not offer reservations or you are not eligible.", "Reservation Not Available", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private decimal GetItemPrice(int menuID)
        {
            foreach (var category in MenuItemsByCategory.Values)
            {
                var item = category.FirstOrDefault(menu => menu.MenuID == menuID);
                if (item != null)
                {
                    return item.Price ?? 0;
                }
            }
            return 0;
        }

        private void OnlinePayment(Order order)
        {
            try
            {
                string verificationCode = GenerateOrderCode(_currentUser.UserID, _restaurant.RestaurantID);
                SendEmail(_currentUser.Email, "Order Confirmation", verificationCode, order.TotalAmount, CartItems, out string emailMessage);
                MessageBox.Show(emailMessage, "Order Placed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send payment instructions. Please try again.", "Payment Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddToCartButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var stackPanel = button.Parent as StackPanel;
            var quantityTextBox = stackPanel.Children.OfType<TextBox>().FirstOrDefault();
            if (quantityTextBox != null && int.TryParse(quantityTextBox.Text, out int quantityToAdd) && quantityToAdd > 0)
            {
                var menuItem = (MenuItemsListView.SelectedItem as Project_s_classes.Menu);
                if (menuItem != null)
                {
                    if (menuItem.QuantityAvailable >= quantityToAdd)
                    {
                        var existingCartItem = CartItems.FirstOrDefault(ci => ci.MenuID == menuItem.MenuID);
                        if (existingCartItem != null)
                        {
                            if (existingCartItem.Quantity + quantityToAdd <= menuItem.QuantityAvailable)
                            {
                                existingCartItem.Quantity += quantityToAdd;
                            }
                            else
                            {
                                MessageBox.Show("Not enough stock available.", "Out of Stock", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                        else
                        {
                            CartItems.Add(new CartItem { MenuID = (int)menuItem.MenuID, ItemName = menuItem.ItemName, Quantity = quantityToAdd });
                        }

                        CartListBox.ItemsSource = null;
                        CartListBox.ItemsSource = CartItems;
                    }
                    else
                    {
                        MessageBox.Show("Not enough stock available.", "Out of Stock", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please enter a valid quantity.", "Invalid Quantity", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteCartItemButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is CartItem cartItem)
            {
                CartItems.Remove(cartItem);
                CartListBox.ItemsSource = null;
                CartListBox.ItemsSource = CartItems; // Refresh the ListBox
            }
        }

        private string GenerateOrderCode(int? userId, int? restaurantId)
        {
            var random = new Random();
            var timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            return $"{userId}-{restaurantId}-{timestamp}-{random.Next(1000, 9999)}";
        }

        private void SendEmail(string email, string subject, string code, decimal? totalAmount, List<CartItem> cartItems, out string message)
        {
            message = "";
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("alexcruso84@gmail.com");
                mail.To.Add(email);
                mail.Subject = subject;

                StringBuilder body = new StringBuilder();
                body.AppendLine("Thank you for your order!");
                body.AppendLine("Your order code is: " + code);
                body.AppendLine();
                body.AppendLine("Purchase Details:");
                body.AppendLine("----------------------------");

                foreach (var item in cartItems)
                {
                    body.AppendLine($"Item: {item.ItemName}, Quantity: {item.Quantity}, Price: {GetItemPrice(item.MenuID):C}");
                }

                body.AppendLine();
                body.AppendLine($"Total Amount: {totalAmount:C}");

                smtpClient.Port = 587;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("alexcruso84@gmail.com", "rycp jqwz hlib qpuk");
                smtpClient.EnableSsl = true;
                smtpClient.Send(mail);

                message = "Order confirmation email sent.";
            }
            catch (Exception ex)
            {
                message = "Error sending verification email: " + ex.Message;
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

            switch (_currentUser.UserType)
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
