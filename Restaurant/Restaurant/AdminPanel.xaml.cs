using Project_s_classes;
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
        private readonly Admin _currentAdmin;
        public AdminPanel(Admin admin)
        {
            InitializeComponent();
            _currentAdmin = admin;
            this.Closing += Window_Closing; 
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RestaurantManagement_Click(object sender, RoutedEventArgs e)
        {
            RestaurantRegistration managementWindow = new RestaurantRegistration(_currentAdmin);
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

        private void ChatButton_Click(object sender, RoutedEventArgs e)
        {
            ChatWindow chatWindow = new ChatWindow(isAdmin: true);
            chatWindow.Show();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                Admin.SetOnlineStatus(_currentAdmin.UserName, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to set admin offline: {ex.Message}");
            }
        }

        private void AnswerCommeentsButton_Click(object sender, RoutedEventArgs e)
        {
            AnswerComments commentTemplate = new AnswerComments();
            commentTemplate.Show();
        }
    }
}