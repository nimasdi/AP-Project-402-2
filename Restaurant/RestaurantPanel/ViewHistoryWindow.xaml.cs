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
            LoadAdditionalMetrics();
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
            ORDER BY Date DESC;";

            _historyData = _dataAccess.LoadData<dynamic, dynamic>(sql, new { RestaurantID = _restaurantId });

            HistoryDataGrid.ItemsSource = _historyData;

            CalculateAndDisplayMetrics(_historyData);
        }

        private void LoadAdditionalMetrics()
        {
            dynamic minPricedOrder = GetMinPricedOrder();
            MinPricedOrderTextBlock.Text = minPricedOrder != null ? $"{minPricedOrder.ID} - {minPricedOrder.Price:C}" : "N/A";

            dynamic maxPricedOrder = GetMaxPricedOrder();
            MaxPricedOrderTextBlock.Text = maxPricedOrder != null ? $"{maxPricedOrder.ID} - {maxPricedOrder.Price:C}" : "N/A";
        }

        private int GetUserCount()
        {
            string sql = "SELECT COUNT(DISTINCT UserID) FROM dbo.Users WHERE RestaurantID = @RestaurantID";
            int userCount = _dataAccess.LoadData<int, dynamic>(sql, new { RestaurantID = _restaurantId }).FirstOrDefault();
            return userCount;
        }

        private dynamic GetMinPricedOrder()
        {
            string sql = "SELECT TOP 1 OrderID, TotalAmount AS Price FROM dbo.Orders WHERE RestaurantID = @RestaurantID ORDER BY TotalAmount ASC";
            dynamic minOrder = _dataAccess.LoadData<dynamic, dynamic>(sql, new { RestaurantID = _restaurantId }).FirstOrDefault();
            return minOrder;
        }

        private dynamic GetMaxPricedOrder()
        {
            string sql = "SELECT TOP 1 OrderID, TotalAmount AS Price FROM dbo.Orders WHERE RestaurantID = @RestaurantID ORDER BY TotalAmount DESC";
            dynamic maxOrder = _dataAccess.LoadData<dynamic, dynamic>(sql, new { RestaurantID = _restaurantId }).FirstOrDefault();
            return maxOrder;
        }

    
        private void CalculateAndDisplayMetrics(IEnumerable<dynamic> history)
        {
            int totalOrders = OrderCount();
            int totalReservations = ReservationCount();
            int cancelledReservations = CanceledOrderCount();
            decimal totalSales = history.Where(h => h.Price != null).Sum(h => (decimal)h.Price);

            decimal onlinePaymentsPercentage = history.Count() > 0 ?
                history.Count(h => h.PaymentMethod == "Online") / (decimal)history.Count() * 100 : 0;

            StringBuilder metricsText = new StringBuilder();
            metricsText.AppendLine($"Total Orders: {totalOrders}");
            metricsText.AppendLine($"Total Reservations: {totalReservations}");
            metricsText.AppendLine($"Cancelled Reservations: {cancelledReservations}");
            metricsText.AppendLine($"Total Sales: {totalSales:C}");
            metricsText.AppendLine($"Percentage of Online Payments: {onlinePaymentsPercentage:F2}%");

            MetricsTextBlock.Text = metricsText.ToString();
        }

        private void ApplyFilterButton_Click(object sender, RoutedEventArgs e)
        {
            string usernameFilter = UsernameFilterTextBox.Text.Trim().ToLower();
            string typeFilter = ((ComboBoxItem)TypeFilterComboBox.SelectedItem)?.Content.ToString();
            DateTime? startDate = StartDatePicker.SelectedDate;
            DateTime? endDate = EndDatePicker.SelectedDate;

            var filteredHistory = _historyData
                .Where(h =>
                    (string.IsNullOrEmpty(usernameFilter) || h.Username.ToLower().Contains(usernameFilter)) &&
                    (typeFilter == "All" || string.IsNullOrEmpty(typeFilter) || h.Type.ToLower() == typeFilter.ToLower()) &&
                    (!startDate.HasValue || h.Date >= startDate.Value) &&
                    (!endDate.HasValue || h.Date <= endDate.Value))
                .ToList();

            HistoryDataGrid.ItemsSource = filteredHistory;

            CalculateAndDisplayMetrics(filteredHistory);
        }

        private void ExportCSVButton_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine("ID,Username,Date,Type,Status,PaymentMethod,Price");

            foreach (var item in _historyData)
            {
                csvContent.AppendLine($"{item.ID},{item.Username},{item.Date},{item.Type},{item.Status},{item.PaymentMethod},{item.Price}");
            }

            string csvFilePath = "C:\\Users\\s\\Desktop\\tet\\AP-Project-402-2\\history_report.csv";

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

        private int OrderCount()
        {
            string sqlOrderCount = "SELECT COUNT(*) FROM dbo.Orders WHERE RestaurantId = @RestaurantId";
            return _dataAccess.LoadData<int, dynamic>(sqlOrderCount, new { RestaurantId = _restaurantId }).FirstOrDefault();
        }

        private int CanceledOrderCount()
        {
            string sqlStatement = "SELECT COUNT(*) FROM dbo.Orders WHERE Status = 'canceled' AND RestaurantId = @RestaurantId";
            return _dataAccess.LoadData<int, dynamic>(sqlStatement, new { RestaurantId = _restaurantId }).FirstOrDefault();
        }


        private int ReservationCount()
        {
            string sqlStatement = "SELECT COUNT(*) FROM dbo.Orders WHERE IsReservation = 1 AND RestaurantId = @RestaurantId";
            return _dataAccess.LoadData<int, dynamic>(sqlStatement, new { RestaurantId = _restaurantId }).FirstOrDefault();
        }
    }
}

