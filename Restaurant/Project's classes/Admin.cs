using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    internal class Admin
    {
        int AdminId;
        string FirstName;
        string LastName;
        string MobileNumber;
        string Email;
        string UserName;
        string Password;
        string Address;
        string Gender;

        public Admin(int adminId, string firstName, string lastName, string mobileNumber, string email, string userName, string password, string address, string gender)
        {
            AdminId = adminId;
            FirstName = firstName;
            LastName = lastName;
            MobileNumber = mobileNumber;
            Email = email;
            UserName = userName;
            Password = password;
            Address = address;
            Gender = gender;
        }

    }
}
