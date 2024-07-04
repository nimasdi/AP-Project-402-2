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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Restaurant_pages
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        static DataAccess dataAccess = new DataAccess();
        static int verficationCode = -1;
        private ValidateStrings _validate;


        public Register()
        {
            InitializeComponent();
            _validate = new ValidateStrings();
        }

       

        private void SendEmail(string email, string subject,int code, out string message)
        {
            message = "";
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("alexcruso84@gmail.com");
                mail.To.Add(email);
                mail.Subject = "Email Verification";
                mail.Body = "Your verification code is: " + code.ToString();
                smtpClient.Port = 587;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("alexcruso84@gmail.com", "rycp jqwz hlib qpuk");
                smtpClient.EnableSsl = true;
                smtpClient.Send(mail);

                message = "Verification email sent.";


            }
            catch (Exception ex)
            {
                message = "Error sending verification email: " + ex.Message;
            }
        }

        private void VerificationCodeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string code = VerificationCodeTextBox.Text;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            this.Close();
            mainWindow.Show();
        }

        private void FirstNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void LastNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void rPassword_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string mobileNumber = MobileNumberTextBox.Text;
            string username = UsernameTextBox.Text;
            string email = EmailTextBox.Text;
            string gender = GenderComboBox.SelectedIndex.ToString();
            string address = AddressTextBox.Text;
            string userType = UserTypeComboBox.SelectedIndex.ToString();

            if (string.IsNullOrEmpty(address))
            {
                address = "Unknown";
            }
            if (string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(mobileNumber)
                || string.IsNullOrEmpty(username) || string.IsNullOrEmpty(email))
            {
                MessageBox.Show("All the fields should be filled");
                return;
            }

            //getting the users instances
            string sqlStatement = "SELECT * FROM dbo.Users";
            DataAccess dataAccess = new DataAccess();
            List<Users> rows = dataAccess.LoadData<Users, dynamic>(sqlStatement, new { });

            //checking the mobile number uniqueness
            bool check_phone = false;
            foreach (Users row in rows)
            {
                if (row.MobileNumber == mobileNumber)
                {
                    check_phone = true;
                    break;
                }
            }
            if (check_phone)
            {
                MessageBox.Show("Mobile number already exists, try another one");
                return;
            }

            //check the username uniqueness\
            bool check_username = false;
            foreach (Users row in rows)
            {
                if (row.UserName == username)
                {
                    check_username = true;
                    break;
                }
            }
            if (check_username)
            {
                MessageBox.Show("The username already exists, try another one");
                return;
            }

            //check if the inputs are in correct form
            if (!_validate.ValidateName(firstName) || !_validate.ValidateName(lastName))
            {
                MessageBox.Show("Firstname and lastname should contain no character except alphabetic characteres and should be between 3 to 32 chracters");
                return;
            }
            if (!_validate.ValidateEmail(email))
            {
                MessageBox.Show("Your email is not valid, try again");
                return;
            }
            if (!_validate.ValidateMobileNumber(mobileNumber))
            {
                MessageBox.Show("Your mobile number is not valid, try again");
                return;
            }
            if (!_validate.ValidateUsername(username))
            {
                MessageBox.Show("Your username is not valid, try again");
                return;
            }
            //----------------------------------------------------------------------------sending email
            Random random = new Random();
            verficationCode = random.Next(1000, 10000);
            string message = string.Empty;
            SendEmail(email, "User's verification code", verficationCode, out message);

            MessageBox.Show(message);

            if (message != "Verification email sent.")
            {
                return;
            }
            else
            {
                FirstNameTextBox.IsEnabled = false;
                LastNameTextBox.IsEnabled = false;
                MobileNumberTextBox.IsEnabled = false;
                UsernameTextBox.IsEnabled = false;
                EmailTextBox.IsEnabled = false;
                Verification.IsEnabled = false;
                AddressTextBox.IsEnabled = false;
                UserTypeComboBox.IsEnabled = false;
                GenderComboBox.IsEnabled = false;
                CheckCode.IsEnabled = true;
                VerificationCodeTextBox.IsEnabled = true;

            }
        }

        private void VerifyCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int code = int.Parse(VerificationCodeTextBox.Text);
                if (code == verficationCode)
                {
                    PasswordBox.IsEnabled = true;
                    RepeatPasswordBox.IsEnabled = true;
                    RegisterB.IsEnabled = true;
                    CheckCode.IsEnabled = false;
                    VerificationCodeTextBox.IsEnabled = false;
                    MessageBox.Show("Alright, you can choose your password now");
                    return;
                }
                MessageBox.Show("Wrong code");
                return;
            }
            catch (FormatException)
            {
                MessageBox.Show("wrong input format");
                return;
            }
        }

        private void GenderComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string mobileNumber = MobileNumberTextBox.Text;
            string username = UsernameTextBox.Text;
            string email = EmailTextBox.Text;
            string combo1 = GenderComboBox.SelectedItem.ToString();
            string gender = combo1.Split(':')[1].Trim();
            string address = AddressTextBox.Text;
            string combo2 = UserTypeComboBox.SelectedItem.ToString();
            string userType = combo2.Split(':')[1].Trim();

            if (string.IsNullOrEmpty(address))
            {
                address = "Unknown";
            }
            VerificationCodeTextBox.IsEnabled = false;
            CheckCode.IsEnabled = false;
            PasswordBox.IsEnabled = true;
            RepeatPasswordBox.IsEnabled = true;
            RegisterB.IsEnabled = true;

            if (!_validate.ValidatePassword(PasswordBox.Password)) { MessageBox.Show("Password is wrong format"); }
            else if (PasswordBox.Password != RepeatPasswordBox.Password) { MessageBox.Show("Input password and it's repetance don't match"); }
            else
            {
                Users new_user = new Users(0,firstName, lastName, mobileNumber, email, username, PasswordBox.Password, userType, address, gender, null, null);
                MessageBox.Show("The user has been create");
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
                
            }
        }

        private void UserTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }


    
}
