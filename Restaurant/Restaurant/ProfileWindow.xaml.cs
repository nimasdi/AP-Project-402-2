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


namespace Restaurant
{
    public partial class ProfileWindow : Window
    {
        private readonly DataAccess _dataAccess;
        private Users _user;

        public ProfileWindow(Users user)
        {
            InitializeComponent();
            _dataAccess = new DataAccess();
            _user = user;

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
                GenderTextBox.Text = _user.Gender;

                foreach (ComboBoxItem item in ServiceTierComboBox.Items)
                {
                    if (item.Content.ToString() == _user.ServiceTier)
                    {
                        ServiceTierComboBox.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            string newEmail = EmailTextBox.Text;
            string newAddress = AddressTextBox.Text;

            if (_user.Email != newEmail || _user.Address != newAddress)
            {
                _user.Email = newEmail;
                _user.Address = newAddress;

                string sql = "UPDATE dbo.Users SET Email = @Email, Address = @Address WHERE UserID = @UserID";
                _dataAccess.SaveData(sql, new { Email = _user.Email, Address = _user.Address, UserID = _user.UserID });
            }

            if (ServiceTierComboBox.SelectedItem is ComboBoxItem selectedTier)
            {
                string newServiceTier = selectedTier.Content.ToString();

                if (_user.ServiceTier != newServiceTier)
                {
                    _user.ServiceTier = newServiceTier;
                    SimulatePaymentAndSendEmail();

                    _user.UpdateServiceTier(newServiceTier, DateTime.Now.AddDays(30));
                }
            }

            MessageBox.Show("Changes saved successfully.");
        }

        private void SimulatePaymentAndSendEmail()
        {
            try
            {
                int code = new Random().Next(100000, 999999);
                string message;
                SendEmail(_user.Email, "Payment Instructions for Service Tier Upgrade", code, out message);

                MessageBox.Show(message, "Payment Instructions", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send payment instructions. Please try again.", "Payment Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SendEmail(string email, string subject, int code, out string message)
        {
            message = "";
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("alexcruso84@gmail.com");
                mail.To.Add(email);
                mail.Subject = subject;
                mail.Body = code.ToString();

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new NetworkCredential("alexcruso84@gmail.com", "RandomGmail");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                message = "Mail sent successfully!";
            }
            catch (Exception ex)
            {
                message = "Error: " + ex.Message;
            }
        }
    }
}