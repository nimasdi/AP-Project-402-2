﻿using System;
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
    /// Interaction logic for ComplaintWindow.xaml
    /// </summary>
    public partial class ComplaintWindow : Window
    {
        private readonly DataAccess _dataAccess;
        private readonly int? _userId;
        private List<Restaurants> _restaurants;

        public ComplaintWindow(int? userId)
        {
            InitializeComponent();
            _dataAccess = new DataAccess();
            _userId = userId;

            LoadRestaurants();
        }

        private void LoadRestaurants()
        {
            _restaurants = GetAllRestaurants();
            RestaurantComboBox.ItemsSource = _restaurants;
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleTextBox.Text;
            string description = DescriptionTextBox.Text;
            var selectedRestaurant = (Restaurants)RestaurantComboBox.SelectedItem;

            if (selectedRestaurant == null)
            {
                MessageBox.Show("Please select a valid Restaurant.");
                return;
            }

            var complaint = new Complaint(null,_userId, selectedRestaurant.RestaurantID, title, description);

            /*
             * I should put a place to count the complaints for the restuarant
             */



            MessageBox.Show("Complaint submitted successfully.");
            this.Close();
        }

        public List<Restaurants> GetAllRestaurants()
        {
            string sql = "SELECT RestaurantID, Name FROM dbo.Restaurants";
            return _dataAccess.LoadData<Restaurants, dynamic>(sql, new { });
        }

        private void ShowComplaintsButton_Click(object sender, RoutedEventArgs e)
        {
            if (_userId.HasValue)
            {
                string sql = "SELECT * FROM dbo.Complaints WHERE UserID = @UserID";
                var complaints = _dataAccess.LoadData<Complaint, dynamic>(sql, new { UserID = _userId });

                ComplaintsListBox.ItemsSource = complaints;
            }
            else
            {
                MessageBox.Show("User ID is not available.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}