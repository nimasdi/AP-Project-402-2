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
using DBAccess;
using Microsoft.VisualBasic.ApplicationServices;
using Project_s_classes;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;

namespace Restaurant_pages
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        static DataAccess dataAccess = new DataAccess();
        public Register()
        {
            InitializeComponent();
        }

        private static bool ValidateName(string input)
        {
            Regex regex = new Regex(@"^[a-zA-Z\s]+$");
            if (!regex.IsMatch(input))
            {
                return false;
            }

            string[] words = input.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries); 
            if(words.Length < 3 || words.Length > 32)
            {
                return false;
            }

            

            return true;
        }

        private static bool ValidateEmail(string input)
        {
            string[] parts = input.Split('@');

            if (parts.Length != 2)
                return false;

            string local = parts[0];
            string domain = parts[1];

            string[] localwords = local.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (localwords.Length < 3 || localwords.Length > 32)
                return false;

            Regex regex = new Regex(@"^[a-zA-Z\s]+$");
            if (!regex.IsMatch(local))
                return false;

            string[] domainParts = domain.Split('.');
            if(domainParts.Length != 2)
                return false;

            string back = domainParts[0];   
            string front = domainParts[1];

            string[] frontWords = front.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if(frontWords.Length < 2 || frontWords.Length > 3)
                return false;

            if(!regex.IsMatch(front))
                return false;

            return true;    
        }

        private bool ValidateMobileNumber(string input)
        {
            if(!input.StartsWith("09"))
                return false;

            Regex regex = new Regex("^09[0-9]{9}$");

            return regex.IsMatch(input);
        }

        private bool ValidateUsername(string input)
        {
            Regex regex = new Regex("^[a-zA-Z0-9]+$");
            if(!regex.IsMatch(input))
                return false;

            string[] words = input.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
            if(words.Length < 3) 
                return false;

            return true;
        }

        private bool ValidatePassword(string input)
        {

        }

        private void SendEmail(string email)
        {

        }

        private void VerificationCodeButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string mobileNumber = MobileNumberTextBox.Text;
            string username = UsernameTextBox.Text;
            string email = EmailTextBox.Text;

            string message = string.Empty;

            if(string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(mobileNumber)
                || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
            {
                message = "All the fields should be filled";
                return;
            }

            //getting the users instances
            string sqlStatement = "SELECT * FROM dbo.Users";
            DataAccess dataAccess = new DataAccess();
            List<Users> rows = dataAccess.LoadData<Users, dynamic>(sqlStatement, new { });

            //checking the mobile number uniqueness
            bool check_phone = false;
            foreach(Users row in rows)
            {
                if(row.MobileNumber == mobileNumber)
                {
                    check_phone = true;
                    break;
                }
            }
            if (check_phone)
            {
                message = "Mobile number already exists, try another one";
                return;
            }

            //check the username uniqueness\
            bool check_username = false;
            foreach(Users row in rows)
            {
                if(row.UserName == username)
                {
                    check_username = true;
                    break;
                }
            }
            if (check_username)
            {
                message = "The username already exists, try another one";
                return;
            }

            //check if the inputs are in correct form
            if(!ValidateName(firstName) || !ValidateName(lastName))
            {
                message = "Firstname and lastname should contain no character except alphabetic characteres and should be between 3 to 32 chracters";
                return;
            }
            if(!ValidateEmail(email))
            {
                message = "Your email is not valid, try again";
                return;
            }
            if (!ValidateMobileNumber(mobileNumber))
            {
                message = "Your mobile number is not valid, try again";
                return;
            }
            if (!ValidateUsername(username))
            {
                message = "Your username is not valid, try again";
                return;
            }
            //----------------------------------------------------------------------------



            MessageBox.Show("Registration successful!");

            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FirstNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void LastNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
