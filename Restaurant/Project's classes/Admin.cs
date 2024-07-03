using DBAccess;

namespace Project_s_classes
{
    public class Admin
    {
        public int? AdminId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public bool IsOnline { get; set; }

        static DataAccess dataAccess = new DataAccess();

        public Admin()
        {

        }
        public Admin(int? adminID, string firstName, string lastName, string mobileNumber, string email, string userName, string password, string address, string gender, bool isOnline = false)
        {
            //AdminId = adminID;
            FirstName = firstName;
            LastName = lastName;
            MobileNumber = mobileNumber;
            Email = email;
            UserName = userName;
            Password = password;
            Address = address;
            Gender = gender;
            IsOnline = isOnline;

            // Storing to DB
            string sqlStatement = "INSERT INTO dbo.Admins (FirstName, LastName, MobileNumber, Email, UserName, Password, Address, Gender, IsOnline)" +
                                  " VALUES(@FirstName, @LastName, @MobileNumber, @Email, @UserName, @Password, @Address, @Gender, @IsOnline);" +
                                  " SELECT CAST(SCOPE_IDENTITY() as int)";
            this.AdminId = dataAccess.SaveData(sqlStatement, this, true);
        }

        public static void SetOnlineStatus(string username, bool isOnline)
        {
            string sqlStatement = "UPDATE dbo.Admins SET IsOnline = @IsOnline WHERE UserName = @UserName";
            dataAccess.SaveData(sqlStatement, new { IsOnline = isOnline, UserName = username });
        }

        public static bool IsAnyAdminOnline()
        {
            string sqlStatement = "SELECT COUNT(*) FROM dbo.Admins WHERE IsOnline = 1";
            int count = dataAccess.LoadData<int, dynamic>(sqlStatement, new { }).FirstOrDefault();
            return count > 0;
        }

    }
}