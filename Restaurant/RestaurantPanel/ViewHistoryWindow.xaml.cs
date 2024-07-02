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
            string sql = "SELECT OrderID, CustomerUsername, CustomerMobile, FoodName, OrderDate, OrderType " +
                         "FROM dbo.Orders " +
                         "WHERE RestaurantID = @RestaurantID " +
                         "UNION " +
                         "SELECT ReservationID, CustomerUsername, CustomerMobile, FoodName, ReservationDate, 'Reservation' AS OrderType " +
                         "FROM dbo.Reservations " +
                         "WHERE RestaurantID = @RestaurantID " +
                         "ORDER BY OrderDate DESC;";

            _historyData = _dataAccess.LoadData<dynamic, dynamic>(sql, new { RestaurantID = _restaurantId });

            RefreshHistoryGrid();
        }

        private void RefreshHistoryGrid()
        {
            HistoryDataGrid.ItemsSource = _historyData;
        }

        private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            string filterText = FilterTextBox.Text.Trim().ToLower();

            var filteredHistory = _historyData
                .Where(h =>
                    h.Username.ToLower().Contains(filterText) ||
                    h.Mobile.ToLower().Contains(filterText) ||
                    h.ItemName.ToLower().Contains(filterText) ||
                    h.Type.ToLower().Contains(filterText) ||
                    (h.Date != null && h.Date.ToString().ToLower().Contains(filterText)));

            HistoryDataGrid.ItemsSource = filteredHistory.ToList();

            // Calculate and display report metrics based on filtered data
            CalculateAndDisplayMetrics(filteredHistory);
        }

        private void CalculateAndDisplayMetrics(IEnumerable<dynamic> filteredHistory)
        {
            int totalOrders = filteredHistory.Count(h => h.Type == "Order");
            int totalReservations = filteredHistory.Count(h => h.Type == "Reservation");
            int cancelledReservationsByCustomer = filteredHistory.Count(h => h.Type == "Reservation" && h.Status == "Cancelled by Customer");
            int cancelledReservations = filteredHistory.Count(h => h.Type == "Reservation" && (h.Status == "Cancelled by Customer" || h.Status == "Cancelled"));
            decimal totalSales = filteredHistory.Where(h => h.Price != null).Sum(h => (decimal)h.Price);

            decimal onlinePaymentsPercentage = (decimal)filteredHistory.Count(h => h.PaymentMethod == "Online") / filteredHistory.Count() * 100;

            // Build metrics text
            StringBuilder metricsText = new StringBuilder();
            metricsText.AppendLine($"Total Orders: {totalOrders}");
            metricsText.AppendLine($"Total Reservations: {totalReservations}");
            metricsText.AppendLine($"Cancelled Reservations: {cancelledReservations}");
            metricsText.AppendLine($"Cancelled Reservations by Customer: {cancelledReservationsByCustomer}");
            metricsText.AppendLine($"Total Sales: {totalSales:C}");
            metricsText.AppendLine($"Percentage of Online Payments: {onlinePaymentsPercentage:F2}%");

            // Update TextBlock in UI
            MetricsTextBlock.Text = metricsText.ToString();
        }

        private void ExportCSVButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine("ID,Username,Mobile,ItemName,Date,Type,Status,PaymentMethod,Price");

            foreach (var item in _historyData)
            {
                csvContent.AppendLine($"{item.ID},{item.Username},{item.Mobile},{item.ItemName},{item.Date},{item.Type},{item.Status},{item.PaymentMethod},{item.Price}");
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
    }
}
