using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBAccess;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Project_s_classes
{
    public enum ServiceType
    {
        delivery = 1,
        dine_in = 2,
    }
    public class Restaurants
    {

        public int? RestaurantID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public float AverageRating { get; set; }
        public bool IsReservationEnabled { get; set; }
        public string? ServiceType { get; set; }
        public int? AdminID { get; set; }
        public int Password {  get; set; }
        public string UserName {  get; set; }
        public bool haveComplaints { get; set; }
        public int ComplaintsNum { get; set; }
        public decimal PenaltyRevenue { get; set; }
        public string Address { get; set; }

        static DataAccess dataAccess = new DataAccess();

        public Restaurants()
        {

        }
        public Restaurants(int? restaurantId, string name, string city, float averageRating, bool isReservationEnabled, string? serviceType, int? adminID, int password, string userName, string address)
        {
            //RestaurantID = restaurantId;
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
            this.PenaltyRevenue = 0;
            this.Address = address;

            

            // Saving restaurant instance to the database
            string sqlStatement = "INSERT INTO dbo.Restaurants (Name, City, AverageRating, IsReservationEnabled, ServiceType, AdminID, Password, UserName, haveComplaints, ComplaintsNum, PenaltyRevenue, Address )" +
                " VALUES(@Name, @City, @AverageRating, @IsReservationEnabled, @ServiceType, @AdminID, @Password, @UserName, @haveComplaints, @ComplaintsNum, @PenaltyRevenue, @Address)";
            this.RestaurantID = dataAccess.SaveData(sqlStatement, this, true);
        }
    }
}
