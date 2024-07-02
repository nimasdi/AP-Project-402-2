using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBAccess;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Project_s_classes
{
    public class Restaurants
    {
<<<<<<< HEAD
        public int? RestaurantID { get; set; }
=======
        public int RestaurantID { get; set; }
>>>>>>> bc1d0981a80edd217d22f61a1f3801f598da4862
        public string Name { get; set; }
        public string City { get; set; }
        public float AverageRating { get; set; }
        public bool IsReservationEnabled { get; set; }
<<<<<<< HEAD
        public int? AdminID { get; set; }
        public int Password {  get; set; }
        public string UserName {  get; set; }
        public bool haveComplaints { get; set; }
        public int ComplaintsNum { get; set; }
        
        static DataAccess dataAccess = new DataAccess();

        public Restaurants(int? restaurantId, string name, string city, float averageRating, bool isReservationEnabled, int? adminID, int password, string userName)
=======
        public string ServiceType { get; set; }

        public decimal PenaltyRevenue { get; set; }
        public int? AdminID { get; set; }
        static DataAccess dataAccess = new DataAccess();

        public Restaurants(string name, string city, float averageRating, bool isReservationEnabled, string serviceType, int? adminID, decimal penaltyRevenue)
>>>>>>> bc1d0981a80edd217d22f61a1f3801f598da4862
        {
            RestaurantID = restaurantId;
            this.Name = name;
            this.City = city;
            this.AverageRating = averageRating;
            this.IsReservationEnabled = isReservationEnabled;
            this.ServiceType = serviceType;
            this.AdminID = adminID;
<<<<<<< HEAD
            this.Password=password;
            this.UserName = userName;
            this.haveComplaints = false;
            this.ComplaintsNum = 0;

            //saving restaurant instance to the data base
            string sqlStatement = "INSERT INTO dbo.Restaurants (Name, City, AverageRating, IsReservationEnabled, AdminID, Password, UserName, haveComplaints)" +
                "VALUES(@Name, @City, @AverageRating, @IsReservationEnabled, @AdminID, @Password, @UserName, @haveComplaints)";
            this.RestaurantID = dataAccess.SaveData(sqlStatement, this, true);
            
=======
            this.PenaltyRevenue = penaltyRevenue;

            // Saving restaurant instance to the database
            string sqlStatement = "INSERT INTO dbo.Restaurants (Name, City, AverageRating, IsReservationEnabled, AdminID, ServiceType, PenaltyRevenue)" +
                " VALUES(@Name, @City, @AverageRating, @IsReservationEnabled, @AdminID, @ServiceType, @PenaltyRevenue)";
            this.RestaurantID = dataAccess.SaveData(sqlStatement, this, true);
>>>>>>> bc1d0981a80edd217d22f61a1f3801f598da4862
        }
    }
}
