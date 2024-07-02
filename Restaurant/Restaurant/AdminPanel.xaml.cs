using Restaurant;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restaurant_Pages
{
    /// <summary>
    /// Interaction logic for AdminPanel.xaml
    /// </summary>
    public partial class AdminPanel : Window
    {
        public AdminPanel()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RestaurantManagement_Click(object sender, RoutedEventArgs e)
        {
            RestaurantRegistration managementWindow = new RestaurantRegistration();
            managementWindow.Show();
        }

        private void RestaurantSearch_Click(object sender, RoutedEventArgs e)
        {
            RestaurantSearch searchRestaurant = new RestaurantSearch();
            searchRestaurant.Show();
        }

        private void ComplaintSearch_Click(object sender, RoutedEventArgs e)
        {
            ComplaintsSearch complaintWindow = new ComplaintsSearch();
            complaintWindow.Show();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AnswerComplaintSearch_Click(object sender, RoutedEventArgs e)
        {
            RespondingComplaintxaml answer = new RespondingComplaintxaml();
            answer.Show();
        }
    }
}
