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
using System.Windows.Shapes;
using Microsoft.AspNetCore.SignalR.Client;


namespace Restaurant_pages
{
    /// <summary>
    /// Interaction logic for main_menu.xaml
    /// </summary>
    public partial class main_menu : Window
    {
        private readonly Users _currentUser;

        public main_menu(Users currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
        }

        private void ProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var profileWindow = new ProfileWindow(_currentUser);
            profileWindow.Show();
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            var searchWindow = new SearchWindow(_currentUser);
            searchWindow.Show();
        }

        private void OrdersButton_Click(object sender, RoutedEventArgs e)
        {
            var ordersWindow = new OrdersWindow(_currentUser.UserID);
            ordersWindow.Show();
        }


        private void ComplaintButton_Click(object sender, RoutedEventArgs e)
        {
            var complaintWindow = new ComplaintWindow(_currentUser.UserID);
            complaintWindow.Show();
        }


        private async void OnlineSupportButton_Click(object sender, RoutedEventArgs e)
        {
            var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5290/chathub")
                .Build();

            await connection.StartAsync();
            bool isAdminOnline = await connection.InvokeAsync<bool>("IsAdminOnline");

            if (isAdminOnline)
            {
                ChatWindow chatWindow = new ChatWindow(false);
                chatWindow.Show();
            }
            else
            {
                MessageBox.Show("No admin is currently online. Please try again later.", "Admin Offline", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            await connection.StopAsync();
        }

        private void TrackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordUser changePasswordUser = new ChangePasswordUser(_currentUser);
            changePasswordUser.Show();
        }

        private void ReservationsButton_Click(object sender, RoutedEventArgs e)
        {
            ReservationWindow reservationWindow = new ReservationWindow((int)_currentUser.UserID);
            reservationWindow.Show();
        }
    
    }
    
}

