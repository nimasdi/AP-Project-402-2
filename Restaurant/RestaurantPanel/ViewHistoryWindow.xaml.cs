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
using System.IO;
using DBAccess;
using Project_s_classes;

namespace RestaurantPanel
{
    /// <summary>
    /// Interaction logic for ViewHistoryWindow.xaml
    /// </summary>
    public partial class ViewHistoryWindow : Window
    {
        private readonly DataAccess _dataAccess;
        private readonly int _restaurantId;
        private List<dynamic> _historyData;

        public ViewHistoryWindow(int restaurantId)
        {
            InitializeComponent();
            _dataAccess = new DataAccess();
            _restaurantId = restaurantId;

            LoadHistory();
        }

        private void LoadHistory()
        {
            string sql = @"
                SELECT 
                    o.OrderID AS ID, 
                    u.Username AS Username, 
                    o.OrderDate AS Date, 
                    'Order' AS Type, 
                    o.Status AS Status, 
                    o.PaymentMethod AS PaymentMethod, 
                    o.TotalAmount AS Price
                FROM dbo.Orders o
                INNER JOIN dbo.Users u ON o.UserID = u.UserID
                WHERE o.RestaurantID = @RestaurantID
                UNION 
                SELECT 
                    r.ReservationID AS ID, 
                    u.Username AS Username, 
                    r.ReservationDate AS Date, 
                    'Reservation' AS Type, 
                    r.Status AS Status, 
                    'N/A' AS PaymentMethod, 
                    0.0 AS Price
                FROM dbo.Reservation r
                INNER JOIN dbo.Users u ON r.UserID = u.UserID
                WHERE r.RestaurantID = @RestaurantID
                ORDER BY Date DESC;";

            _historyData = _dataAccess.LoadData<dynamic, dynamic>(sql, new { RestaurantID = _restaurantId });

            HistoryDataGrid.ItemsSource = _historyData;

            CalculateAndDisplayMetrics(_historyData);
        }

        private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            string filterText = FilterTextBox.Text.Trim().ToLower();

            var filteredHistory = _historyData
                .Where(h =>
                    (h.Username != null && h.Username.ToLower().Contains(filterText)) ||
                    (h.Type != null && h.Type.ToLower().Contains(filterText)) ||
                    (h.Date != null && h.Date.ToString().ToLower().Contains(filterText)))
                .ToList();

            HistoryDataGrid.ItemsSource = filteredHistory;

            CalculateAndDisplayMetrics(filteredHistory);
        }

        private void CalculateAndDisplayMetrics(IEnumerable<dynamic> history)
        {
            int totalOrders = history.Count(h => h.Type == "Order");
            int totalReservations = history.Count(h => h.Type == "Reservation");
            int cancelledReservationsByCustomer = history.Count(h => h.Type == "Reservation" && h.Status == "Cancelled by Customer");
            int cancelledReservations = history.Count(h => h.Type == "Reservation" && (h.Status == "Cancelled by Customer" || h.Status == "Cancelled"));
            decimal totalSales = history.Where(h => h.Price != null).Sum(h => (decimal)h.Price);

            decimal onlinePaymentsPercentage = history.Count(h => h.PaymentMethod == "Online") / (decimal)history.Count() * 100;

            StringBuilder metricsText = new StringBuilder();
            metricsText.AppendLine($"Total Orders: {totalOrders}");
            metricsText.AppendLine($"Total Reservations: {totalReservations}");
            metricsText.AppendLine($"Cancelled Reservations: {cancelledReservations}");
            metricsText.AppendLine($"Cancelled Reservations by Customer: {cancelledReservationsByCustomer}");
            metricsText.AppendLine($"Total Sales: {totalSales:C}");
            metricsText.AppendLine($"Percentage of Online Payments: {onlinePaymentsPercentage:F2}%");

            MetricsTextBlock.Text = metricsText.ToString();
        }

        private void ExportCSVButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine("ID,Username,Date,Type,Status,PaymentMethod,Price");

            foreach (var item in _historyData)
            {
                csvContent.AppendLine($"{item.ID},{item.Username},{item.Date},{item.Type},{item.Status},{item.PaymentMethod},{item.Price}");
            }

            string csvFilePath = "history_report.csv";

            try
            {
                File.WriteAllText(csvFilePath, csvContent.ToString());
                MessageBox.Show($"CSV file successfully exported to {csvFilePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting CSV file: {ex.Message}");
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
