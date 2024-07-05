using DBAccess;
using Project_s_classes;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restaurant
{
    /// <summary>
    /// Interaction logic for ChangePasswordUser.xaml
    /// </summary>
    public partial class ChangePasswordUser : Window
    {
        private readonly Users _currentUser;
        private ValidateStrings _validate;
        private readonly DataAccess _dataAccess;
        public ChangePasswordUser(Users user)
        {
            InitializeComponent();
            _currentUser = user;
            _validate = new ValidateStrings();  
            _dataAccess = new DataAccess();
            LoadPassword();
        }

        private void LoadPassword()
        {
            CurrentPasswordTextBox.Text = _currentUser.Password;
            NewPasswordBox.Password = string.Empty;
            ConfirmPasswordBox.Password = string.Empty;
        }

        private void ChangePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            string password = NewPasswordBox.Password;
            string repeatPassword = ConfirmPasswordBox.Password;

            if(!string.IsNullOrEmpty(password))
            {
                if(_validate.ValidatePassword(password))
                {
                    if(password == repeatPassword)
                    {
                        if (password != CurrentPasswordTextBox.Text)
                        {
                            try
                            {
                                _currentUser.Password = password;
                                string sql = "UPDATE dbo.Users SET Password = @Password WHERE UserID = @UserID";
                                _dataAccess.SaveData(sql, new { Password = _currentUser.Password, UserID = _currentUser.UserID });
                                MessageBox.Show("Password got updated successfully");
                               
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
                    MessageBox.Show("The new password do not fulfill the requirements for a password");
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
