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
using Project_s_classes;


namespace Restaurant
{
    public partial class ProfileWindow : Window
    {
        private readonly DataAccess _dataAccess;
        private readonly int _userId;
        private Users _user;

        public ProfileWindow(int userId)
        {
            InitializeComponent();
            _dataAccess = new DataAccess();
            _userId = userId;

            LoadUserData();
        }

        private void LoadUserData()
        {
            string sql = "SELECT UserID, FirstName, LastName, MobileNumber, Email, UserName, Address, Gender FROM dbo.Users WHERE UserID = @UserID";
            _user = _dataAccess.LoadData<Users, dynamic>(sql, new { UserID = _userId }).FirstOrDefault();

            if (_user != null)
            {
                UserNameTextBox.Text = _user.UserName;
                FirstNameTextBox.Text = _user.FirstName;
                LastNameTextBox.Text = _user.LastName;
                EmailTextBox.Text = _user.Email;
                MobileNumberTextBox.Text = _user.MobileNumber;
                AddressTextBox.Text = _user.Address;
                GenderTextBox.Text = _user.Gender;
            }
        }

        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            _user.Email = EmailTextBox.Text;
            _user.Address = AddressTextBox.Text;

            string sql = "UPDATE dbo.Users SET Email = @Email, Address = @Address WHERE UserID = @UserID";
            _dataAccess.SaveData(sql, new { Email = _user.Email, Address = _user.Address, UserID = _user.UserID });

            MessageBox.Show("Changes saved successfully.");
        }
    }
}