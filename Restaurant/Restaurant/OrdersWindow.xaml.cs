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
        private readonly int _userId;
        private List<Order> _orders;

        public OrdersWindow(int userId)
        {
            InitializeComponent();
            _dataAccess = new DataAccess();
            _userId = userId;

            LoadOrders();
        }

        private void LoadOrders()
        {
            _orders = GetOrdersByUserId(_userId);
            OrdersDataGrid.ItemsSource = _orders;
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

                UpdateOrderRatingAndComment(selectedOrder.OrderId, selectedOrder.Rating, selectedOrder.Comment);
                MessageBox.Show("Rating and comment submitted successfully.");
            }
            else
            {
                MessageBox.Show("Please select an order to leave a rating or comment.");
            }
        }
        public List<Order> GetOrdersByUserId(int userId)
        {
            string sql = "SELECT OrderId, UserId, RestaurantId, OrderDate, TotalAmount, PaymentMethod, Status, Rating, Comment FROM dbo.Orders WHERE UserId = @UserId";
            return _dataAccess.LoadData<Order, dynamic>(sql, new { UserId = userId });
        }

        public void UpdateOrderRatingAndComment(int orderId, int? rating, string? comment)
        {
            string sql = "UPDATE dbo.Orders SET Rating = @Rating, Comment = @Comment WHERE OrderId = @OrderId";
            _dataAccess.SaveData(sql, new { Rating = rating, Comment = comment, OrderId = orderId });
        }
    }

}