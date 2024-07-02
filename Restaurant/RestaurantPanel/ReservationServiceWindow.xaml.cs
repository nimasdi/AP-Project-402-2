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
    /// Interaction logic for ReservationServiceWindow.xaml
    /// </summary>
    public partial class ReservationServiceWindow : Window
    {
        private readonly DataAccess _dataAccess;
        private readonly int _restaurantId;
        private const float MinRatingForReservation = 4.5f;

        public ReservationServiceWindow(int restaurantId)
        {
            InitializeComponent();
            _dataAccess = new DataAccess();
            _restaurantId = restaurantId;

            LoadRestaurantData();
        }

        private void LoadRestaurantData()
        {
            string sql = "SELECT AverageRating, IsReservationEnabled FROM dbo.Restaurants WHERE RestaurantID = @RestaurantID";
            var restaurant = _dataAccess.LoadData<Restaurants, dynamic>(sql, new { RestaurantID = _restaurantId }).FirstOrDefault();

            if (restaurant != null)
            {
                AverageRatingTextBox.Text = restaurant.AverageRating.ToString();
                DataContext = restaurant; 
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            var restaurant = (Restaurants)DataContext; 

            string sql = "UPDATE dbo.Restaurants SET IsReservationEnabled = @IsReservationEnabled WHERE RestaurantID = @RestaurantID";
            _dataAccess.SaveData(sql, new { IsReservationEnabled = restaurant.IsReservationEnabled, RestaurantID = _restaurantId });

            MessageBox.Show("Reservation service status updated successfully.");
            this.DialogResult = true;
        }
    }
}
}
