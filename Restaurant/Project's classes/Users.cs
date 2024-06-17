using DBAccess;

namespace Project_s_classes
{
    public class Users
    {
        int UserID { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string MobileNumber { get; set; }
        string Email { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        string UserType {  get; set; }
        string Address {  get; set; }
        string Gender { get; set; }
        static DataAccess dataAccess = new DataAccess();
        

        public Users(string firstName, string lastName, string mobileNumber, string email, string userName, string password, string userType, string address, string gender)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.MobileNumber = mobileNumber;
            this.Email = email;
            this.UserName = userName;
            this.Password = password;
            this.UserType = userType;
            this.Address = address;
            this.Gender = gender;

            //saving instance to data base
            string sqlStatement = "INSERT INTO dbo.Users (FirstName, LastName, MobileNumber, Email, UserName, Password, UserType,Address, Gender)" +
            " VALUES(@FirstName, @LastName, @MobileNumber, @Email, @UserName, @Password, @UserType,@Address, @Gender);";
            this.UserID = dataAccess.SaveData(sqlStatement, this, true);

        }
    }
}
