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
    /// Interaction logic for OrdersWindow.xaml
    /// </summary>
    public partial class OrdersWindow : Window
    {
        private readonly DataAccess _dataAccess;
        private readonly int? _userId;
        private List<Order> _orders;

        public OrdersWindow(int? userId)
        {
            InitializeComponent();
            _dataAccess = new DataAccess();
            _userId = userId;

            LoadOrders();
        }

        private void LoadOrders()
        {
            try
            {
                _orders = GetOrdersByUserId(_userId);
                OrdersDataGrid.ItemsSource = _orders;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load orders: {ex.Message}");
            }
        }

        private void OrdersDataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is Order selectedOrder)
            {
                RatingTextBox.Text = selectedOrder.Rating?.ToString() ?? string.Empty;
                CommentTextBox.Text = selectedOrder.Comment ?? string.Empty;
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersDataGrid.SelectedItem is Order selectedOrder)
            {
                if (int.TryParse(RatingTextBox.Text, out int rating))
                {
                    selectedOrder.Rating = rating;
                }
                else
                {
                    selectedOrder.Rating = null;
                }
                selectedOrder.Comment = CommentTextBox.Text;

                UpdateOrderRatingAndComment((int)selectedOrder.OrderId, selectedOrder.Rating, selectedOrder.Comment);
                UpdateRestaurantAverageRating(selectedOrder.RestaurantId);
                MessageBox.Show("Rating and comment submitted successfully.");
            }
            else
            {
                MessageBox.Show("Please select an order to leave a rating or comment.");
            }
        }
        public List<Order> GetOrdersByUserId(int? userId)
        {
            string sql = "SELECT OrderId, UserId, RestaurantId, OrderDate, TotalAmount, PaymentMethod, Status, Rating, Comment, IsReservation FROM dbo.Orders WHERE UserId = @UserId";
            try
            {
                return _dataAccess.LoadData<Order, dynamic>(sql, new { UserId = userId });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load orders from database: {ex.Message}");
                return new List<Order>();
            }
        }

        public void UpdateOrderRatingAndComment(int orderId, int? rating, string? comment)
        {
            string sql = "UPDATE dbo.Orders SET Rating = @Rating, Comment = @Comment WHERE OrderId = @OrderId";
            try
            {
                _dataAccess.SaveData(sql, new { Rating = rating, Comment = comment, OrderId = orderId });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update order rating and comment: {ex.Message}");
            }
        }

        private void UpdateRestaurantAverageRating(int? restaurantId)
        {
            string sql = "SELECT Rating FROM dbo.Orders WHERE RestaurantId = @RestaurantId AND Rating IS NOT NULL";
            try
            {
                var ratings = _dataAccess.LoadData<int, dynamic>(sql, new { RestaurantId = restaurantId });

                if (ratings.Count > 0)
                {
                    double newAverageRating = ratings.Average();

                    sql = "UPDATE dbo.Restaurants SET AverageRating = @AverageRating WHERE RestaurantID = @RestaurantId";
                    _dataAccess.SaveData(sql, new { AverageRating = newAverageRating, RestaurantId = restaurantId });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update restaurant average rating: {ex.Message}");
            }
        }
    }

}