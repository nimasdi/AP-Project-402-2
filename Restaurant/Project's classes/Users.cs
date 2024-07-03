using DBAccess;

namespace Project_s_classes
{
    public enum UserType
    {
        Bronze = 1,
        Silver = 2,
        Gold = 3,
    }
    public class Users
    {
        public int? UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNumber { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string? UserType {  get; set; }
        public string Address {  get; set; }
        public string? Gender { get; set; }
        public DateTime? ServiceExpiration { get; set; }
        public int? ReservationsMadeThisMonth { get; set; }

        static DataAccess dataAccess = new DataAccess();


        public Users(int? userID, string firstName, string lastName, string mobileNumber, string email, string userName, string password, string? userType, string address, string? gender, DateTime? serviceExpiration, int? reservationMade)
        {
            UserID = userID;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.MobileNumber = mobileNumber;
            this.Email = email;
            this.UserName = userName;
            this.Password = password;
            this.UserType = userType;
            this.Address = address;
            this.Gender = gender;
            this.ServiceExpiration = null;
            this.ReservationsMadeThisMonth = 0;

            // Saving instance to database
            string sqlStatement = "INSERT INTO dbo.Users (FirstName, LastName, MobileNumber, Email, UserName, Password, UserType, Address, Gender, ServiceExpiration, ReservationsMadeThisMonth)" +
                              " VALUES(@FirstName, @LastName, @MobileNumber, @Email, @UserName, @Password, @UserType, @Address, @Gender, @ServiceExpiration, @ReservationsMadeThisMonth);";
            this.UserID = dataAccess.SaveData(sqlStatement, this, true);
        }

        public void UpdateUserType(string userType, DateTime serviceExpiration)
        {
            this.UserType = userType;
            this.ServiceExpiration = serviceExpiration;
            this.ReservationsMadeThisMonth = 0;

            string sqlStatement = "UPDATE dbo.Users SET UserType = @UserType, ServiceExpiration = @ServiceExpiration, ReservationsMadeThisMonth = @ReservationsMadeThisMonth WHERE UserID = @UserID";
            dataAccess.SaveData(sqlStatement, this);
        }

        public bool CanMakeReservation(out string message)
        {
            if (UserType == null || ServiceExpiration == null || ServiceExpiration < DateTime.Now)
            {
                message = "Your special service has expired or you do not have one.";
                return false;
            }

            int maxReservations;
            switch (UserType)
            {
                case "Bronze":
                    maxReservations = 2;
                    break;
                case "Silver":
                    maxReservations = 5;
                    break;
                case "Gold":
                    maxReservations = 15;
                    break;
                default:
                    message = "Unknown service tier.";
                    return false;
            }

            if (ReservationsMadeThisMonth >= maxReservations)
            {
                message = "You have reached the maximum number of reservations for this month.";
                return false;
            }

            message = "You can make a reservation.";
            return true;
        }

        public void IncrementReservationCount()
        {
            ReservationsMadeThisMonth++;

            string sqlStatement = "UPDATE dbo.Users SET ReservationsMadeThisMonth = @ReservationsMadeThisMonth WHERE UserID = @UserID";
            dataAccess.SaveData(sqlStatement, this);
        }
    }
}
