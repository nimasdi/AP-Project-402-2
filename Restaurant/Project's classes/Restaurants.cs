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

        public int? RestaurantID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public float AverageRating { get; set; }
        public bool IsReservationEnabled { get; set; }
        public int? AdminID { get; set; }
        public int Password {  get; set; }
        public string UserName {  get; set; }
        public bool haveComplaints { get; set; }
        public int ComplaintsNum { get; set; }
        public decimal PenaltyRevenue { get; set; }
        public string ServiceType { get; set; }

        static DataAccess dataAccess = new DataAccess();

        public Restaurants(int? restaurantId, string name, string city, float averageRating, bool isReservationEnabled, int? adminID, int password, string userName, decimal penaltyRevenue, string serviceType)
        {
            RestaurantID = restaurantId;
            this.Name = name;
            this.City = city;
            this.AverageRating = averageRating;
            this.IsReservationEnabled = isReservationEnabled;
            this.ServiceType = serviceType;
            this.AdminID = adminID;
            this.Password=password;
            this.UserName = userName;
            this.haveComplaints = false;
            this.ComplaintsNum = 0;
            this.PenaltyRevenue = penaltyRevenue;

            // Saving restaurant instance to the database
            string sqlStatement = "INSERT INTO dbo.Restaurants (Name, City, AverageRating, IsReservationEnabled, AdminID, ServiceType, PenaltyRevenue, haveComplaints)" +
                " VALUES(@Name, @City, @AverageRating, @IsReservationEnabled, @AdminID, @ServiceType, @PenaltyRevenue, @haveComplaints)";
            this.RestaurantID = dataAccess.SaveData(sqlStatement, this, true);
        }
    }
}
