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
    /// Interaction logic for ReservationWindow.xaml
    /// </summary>
    public partial class ReservationWindow : Window
    {
        private readonly int _userId;
        private readonly DataAccess _dataAccess;

        public ReservationWindow(int userId)
        {
            InitializeComponent();
            _userId = userId;
            _dataAccess = new DataAccess();

            LoadReservations();
        }

        private void LoadReservations()
        {
            string sql = "SELECT * FROM Orders WHERE UserId = @UserId AND IsReservation = 1 AND Status <> 'Canceled'";
            var reservations = _dataAccess.LoadData<Order, dynamic>(sql, new { UserId = _userId });

            ReservationsListBox.ItemsSource = reservations;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CancelReservationButton_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            int orderId = (int)button.Tag;

            string sqlSelect = "SELECT * FROM Orders WHERE OrderId = @OrderId";
            var order = _dataAccess.LoadData<Order, dynamic>(sqlSelect, new { OrderId = orderId }).FirstOrDefault();

            if (order != null)
            {
                order.Cancel();
                LoadReservations();
                MessageBox.Show("Reservation canceled successfully.");
            }
        }
    }
}


