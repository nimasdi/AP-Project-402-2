﻿using Project_s_classes;
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
using DBAccess;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Restaurant_Pages
{
    /// <summary>
    /// Interaction logic for RestaurantManagement.xaml
    /// </summary>
    /// 

    

    public partial class RestaurantRegistration : Window
    {
        public RestaurantRegistration()
        {
            InitializeComponent();
        }
        static DataAccess dataAccess = new DataAccess();

        private int PassGenerator()
        {
            var restaurants = dataAccess.LoadData<Restaurants,dynamic>("SELECT * FROM dbo.Restaurants", new { });
            
            Random random = new Random();
            int pass = 0;
            while (true)
            {
                pass = random.Next(10000000, 99999999);
                bool check = true;
                foreach (var item in restaurants)
                {
                    if (item.Password ==  pass)
                    {
                        check = false;
                        break;
                    }
                }

                if (check)
                {
                    break;
                }
            }

            return pass;
            
        }

        private bool ValidateUsername(string input)
        {
            Regex regex = new Regex("^[a-zA-Z0-9]+$");
            if (!regex.IsMatch(input))
                return false;

            string[] words = input.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 3)
                return false;

            return true;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtName.Clear();
            txtCity.Clear();
            txtAverageRating.Clear();
            txtUserName.Clear();    
            cbIsReservationAvailable.IsChecked = false;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            if (ValidateUsername(txtUserName.Text))
            {
                int pass = PassGenerator();
                //here i should put a service type enum for restaurants and a field or combobox for it
                var restaurant = new Restaurants(null,txtName.Text, txtCity.Text, float.Parse(txtAverageRating.Text), cbIsReservationAvailable.IsChecked ?? false, null, pass, txtUserName.Text, ServiceyTypeComboBox.SelectedIndex.ToString());
                MessageBox.Show("A new restaurant got created by admin");
                this.Close();
            }
            else
            {
                MessageBox.Show("The usernaem is not valid, please try agian");
                txtUserName.Clear();
            }
        }

        private void ServiceyTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}