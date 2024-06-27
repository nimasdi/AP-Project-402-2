using DBAccess;

namespace Project_s_classes
{
    public class Admin
    {
        public int AdminId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        static DataAccess dataAccess = new DataAccess();

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

            //storing to DB
            string sqlStatement = "INSERT INTO dbo.Admins (FirstName, LastName, MobileNumber, Email, UserName, Password, Address, Gender)" +
           " VALUES(@FirstName, @LastName, @MobileNumber, @Email, @UserName, @Password,@Address, @Gender);";
            this.AdminId = dataAccess.SaveData(sqlStatement, this, true);
        }

    }
}