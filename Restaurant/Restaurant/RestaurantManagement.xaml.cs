using Project_s_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restaurant
{
    /// <summary>
    /// Interaction logic for RestaurantManagement.xaml
    /// </summary>
    /// 



    public class RestaurantManagement : Window
    {
        public RestaurantManagement()
        {
            InitializeComponent();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtName.Clear();
            txtCity.Clear();
            txtAverageRating.Clear();
            cbIsReservationAvailable.IsChecked = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var restaurant = new Restaurants(txtName.Text, txtCity.Text, float.Parse(txtAverageRating.Text), cbIsReservationAvailable.IsChecked ?? false, null);
            MessageBox.Show("A new restaurant got created by admin");
            
        }
    }
}
