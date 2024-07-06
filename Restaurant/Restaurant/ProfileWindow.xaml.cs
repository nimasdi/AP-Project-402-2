using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
using System.Windows.Media.Animation;
using Microsoft.EntityFrameworkCore;


namespace Restaurant
{
    public partial class ProfileWindow : Window
    {
        private readonly DataAccess _dataAccess;
        private Users _user;
        private ValidateStrings _validateStrings;

        public ProfileWindow(Users user)
        {
            InitializeComponent();
            _dataAccess = new DataAccess();
            _user = user;
            _validateStrings = new ValidateStrings();

            LoadUserData();
        }

        private void LoadUserData()
        {
            if (_user != null)
            {
                UserNameTextBox.Text = _user.UserName;
                FirstNameTextBox.Text = _user.FirstName;
                LastNameTextBox.Text = _user.LastName;
                EmailTextBox.Text = _user.Email;
                MobileNumberTextBox.Text = _user.MobileNumber;
                AddressTextBox.Text = _user.Address;
                GenderComboBox.SelectedItem = GenderComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == _user.Gender);
                ServiceTierComboBox.SelectedItem = ServiceTierComboBox.Items.Cast<ComboBoxItem>().FirstOrDefault(item => item.Content.ToString() == _user.UserType);
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            string newEmail = EmailTextBox.Text;
            string newAddress = AddressTextBox.Text;

            if (_user.Email != newEmail && _user.Address != newAddress)
            {
                if (_validateStrings.ValidateEmail(newEmail))
                {
                    _user.Email = newEmail;
                    _user.Address = newAddress;
                    string sql = "UPDATE dbo.Users SET Email = @Email, Address = @Address WHERE UserID = @UserID";
                    _dataAccess.SaveData(sql, new { Email = _user.Email, Address = _user.Address, UserID = _user.UserID });
                }
                else
                {
                    EmailTextBox.Text = _user.Email;
                    AddressTextBox.Text = _user.Address;
                    MessageBox.Show("The new email format is not correct");
                    return;
                }
            }
            else if (_user.Email == newEmail && _user.Address != newAddress)
            {
                _user.Address = newAddress;
                string sql = "UPDATE dbo.Users SET Address = @Address WHERE UserID = @UserID";
                _dataAccess.SaveData(sql, new { Address = _user.Address, UserID = _user.UserID });
            }
            else if (_user.Email != newEmail && _user.Address == newAddress)
            {
                if (_validateStrings.ValidateEmail(newEmail))
                {
                    _user.Email = newEmail;
                    string sql = "UPDATE dbo.Users SET Email = @Email WHERE UserID = @UserID";
                    _dataAccess.SaveData(sql, new { Email = _user.Email, UserID = _user.UserID });
                }
                else
                {
                    EmailTextBox.Text = _user.Email;
                    MessageBox.Show("The new email format is not correct");
                    return;
                }
            }

            if (ServiceTierComboBox.SelectedItem is ComboBoxItem selectedTier)
            {
                string newServiceTier = selectedTier.Content.ToString();

                if (_user.UserType != newServiceTier)
                {
                    _user.UserType = newServiceTier;
                    OnlinePayment(newServiceTier);

                    UpdateUserType((int)_user.UserID, newServiceTier);
                }
            }

            MessageBox.Show("Changes saved successfully.");
            LoadUserData();
        }

        private void UpdateUserType(int userId, string newServiceTier)
        {
            string sql = "UPDATE dbo.Users SET UserType = @UserType, ServiceExpiration = @ServiceExpiration WHERE UserID = @UserID";
            try
            {
                _dataAccess.SaveData(sql, new { UserType = newServiceTier, ServiceExpiration = DateTime.Now.AddDays(30), UserID = userId });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update service tier: {ex.Message}");
            }
        }

        private void OnlinePayment(string Tier)
        {
            try
            {
                decimal price = 0;

                if (Tier == "Bronze")
                {
                    price = 100;
                }
                else if (Tier == "Silver")
                {
                    price = 150;
                }
                else
                {
                    price = 300;
                }

                int code = new Random().Next(100000, 999999);
                string message;
                SendEmail(_user.Email, "Payment for Service Tier Upgrade", code, price, out message);

                MessageBox.Show(message, "Payment Instructions", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send payment instructions. Please try again.", "Payment Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SendEmail(string email, string subject, int code, decimal totalAmount, out string message)
        {
            message = "";
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com");
                mail.From = new MailAddress("alexcruso84@gmail.com");
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = $"Thanks you for upgrading your service tier!\nYour verification code is : {code}\n" +
                            $"\nPurchase Detaisls:\n---------------------\n\nTotal Amount:{totalAmount}";

                smtpClient.Port = 587;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("alexcruso84@gmail.com", "rycp jqwz hlib qpuk");
                smtpClient.EnableSsl = true;
                smtpClient.Send(mail);

                message = "Upgrade confirmation email sent.";
            }
            catch (Exception ex)
            {
                message = "Error sending verification email: " + ex.Message;
            }
        }
    }
}