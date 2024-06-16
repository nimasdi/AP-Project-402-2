using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    internal class Admin
    {
        int AdminId {  get; set; }
        string FirstName {  get; set; }
        string LastName {  get; set; }
        string MobileNumber {  get; set; }
        string Email {  get; set; }
        string UserName {  get; set; }
        string Password {  get; set; }
        string Address {  get; set; }
        string Gender {  get; set; }

        public Admin(string firstName, string lastName, string mobileNumber, string email, string userName, string password, string address, string gender)
        {
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
