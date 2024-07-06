using DBAccess;
using Project_s_classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace RestaurantPanel
{
    /// <summary>
    /// Interaction logic for RestaurantChangePassword.xaml
    /// </summary>
    public partial class RestaurantChangePassword : Window
    {
        private readonly Restaurants _currentRestaurant;
        private readonly DataAccess _dataAccess;
        public RestaurantChangePassword(Restaurants restaurant)
        {
            InitializeComponent();
            _currentRestaurant = restaurant;
            _dataAccess = new DataAccess();
            LoadPassword(); 
        }

        private void LoadPassword()
        {
            CurrentPasswordTextBox.Text = _currentRestaurant.Password.ToString();
            NewPasswordBox.Password = string.Empty;
            ConfirmPasswordBox.Password = string.Empty;
        }

        private bool ValidatePass(string pass)
        {
            if(pass.Length != 8)
            {
                return false;
            }
            else if(!Regex.IsMatch(pass, @"^\d+$"))
            {
                return false;
            }
            return true;
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string password = NewPasswordBox.Password;
            string repeatPassword = ConfirmPasswordBox.Password;

            if (!string.IsNullOrEmpty(password))
            {
                if (ValidatePass(password))
                {
                    if (password == repeatPassword)
                    {
                        if (password != CurrentPasswordTextBox.Text)
                        {
                            try
                            {
                                int pass = int.Parse(password);
                                _currentRestaurant.Password = pass;
                                string sql = "UPDATE dbo.Restaurants SET Password = @Password WHERE RestaurantID = @RestaurantID";
                                _dataAccess.SaveData(sql, new { Password = _currentRestaurant.Password, RestaurantID = _currentRestaurant.RestaurantID });
                                MessageBox.Show("Password got updated successfully");

                            }
                            catch(FormatException ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("The new password is as same as previous one");
                        }
                    }
                    else
                    {
                        MessageBox.Show("The password and confirm password does not match, try again");
                    }
                }
                else
                {
                    MessageBox.Show("Restaurant's password can only contain 8 digits and nothing more");
                }
            }
            else
            {
                MessageBox.Show("You should enter a new password in the related textbox");
            }

            LoadPassword();
        }
    }
}
