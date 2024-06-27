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
            if (string.IsNullOrEmpty(input))
                return false;

            if (input.Length < 8 || input.Length > 32)
                return false;

            if (!Regex.IsMatch(input, @"[A-Z]"))
                return false;

            if (!Regex.IsMatch(input, @"[a-z]"))
                return false;

            if (!Regex.IsMatch(input, @"\d"))
                return false;
            return true;
        }

        private void SendEmail(string email, string subject,int code, out string message)
        {
            message = ""
;            try
            {
                
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                //RandomGmail
                mail.From = new MailAddress("alexcruso84@gmail.com");
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = code.ToString();

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new NetworkCredential("alexcruso@gmail.com", "RandomGmail");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                message = "Mail sent successfully!";
            }
            catch (Exception ex)
            {
                message = "Error: " + ex.Message;
            }
        }

        private void VerifyButton_Click(object sender, RoutedEventArgs e, string message)
        {
            string firstName = FirstNameTextBox.Text;
            string lastName = LastNameTextBox.Text;
            string mobileNumber = MobileNumberTextBox.Text;
            string username = UsernameTextBox.Text;
            string email = EmailTextBox.Text;
            string gender = GenderComboBox.SelectedIndex.ToString();
            string address = AddressTextBox.Text;
            if(string.IsNullOrEmpty(address))
            {
                address = "Unknown";
            }



            if(string.IsNullOrEmpty(firstName) || string.IsNullOrEmpty(lastName) || string.IsNullOrEmpty(mobileNumber)
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
                MessageBox.Show("Mobile number already exists, try another one");
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
                MessageBox.Show("The username already exists, try another one");
                return;
            }

            //check if the inputs are in correct form
            if(!ValidateName(firstName) || !ValidateName(lastName))
            {
                MessageBox.Show("Firstname and lastname should contain no character except alphabetic characteres and should be between 3 to 32 chracters");
                return;
            }
            if(!ValidateEmail(email))
            {
                MessageBox.Show("Your email is not valid, try again");
                return;
            }
            if (!ValidateMobileNumber(mobileNumber))
            {
                MessageBox.Show("Your mobile number is not valid, try again");
                return;
            }
            if (!ValidateUsername(username))
            {
                MessageBox.Show("Your username is not valid, try again");
                return;
            }
            //----------------------------------------------------------------------------sending email
            Random random = new Random();
            int verification_code = random.Next(1000, 10000);
            message = string.Empty;
            SendEmail(email, "User's verification code", verification_code, out message);

            MessageBox.Show(message);




           
            if (message != "Mail sent successfully!")
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
                VerificationCodeTextBox.IsEnabled = true;
                
                if(VerifyCode_Click(sender, e, verification_code))
                {
                    VerificationCodeTextBox.IsEnabled = false;
                    CheckCode.IsEnabled = false;
                    PasswordTextBox.IsEnabled = true;
                    rPasswordTextBox.IsEnabled = true;
                    RegisterB.IsEnabled = true;

                    if(RegisterButton_Click(sender, e))
                    {
                        MessageBox.Show("The user has been create");
                        Users new_user = new Users(firstName, lastName, mobileNumber, email, username, PasswordTextBox.Text, "Bronze", address ,gender);
                        MainWindow mainWindow = new MainWindow();
                        this.Close();
                        mainWindow.Show();
                    }
                    else
                    {
                        MessageBox.Show("Either the password doesn't pass the requirements or the repeat password field is not the same with password");
                    }
                }
                else
                {
                    MessageBox.Show("The input code is wrong, try again");
                }

            }



            MessageBox.Show("Registration successful!");

            MainWindow loginWindow = new MainWindow();
            loginWindow.Show();
            this.Close();
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

        private bool VerifyCode_Click(object sender, RoutedEventArgs e, int verification_code)
        {
            try
            {
                int code = int.Parse(VerificationCodeTextBox.Text);
                if(code == verification_code )
                {
                    return true;
                }
                return false;
            }
            catch(FormatException)
            {
                MessageBox.Show("wrong input format");
                return false;
            }
        }

        private bool RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            if(!ValidatePassword(PasswordTextBox.Text)) { return false; }
            if(PasswordTextBox.Text != rPasswordTextBox.Text) { return false; }
            return true;
        }


    }


    
}
