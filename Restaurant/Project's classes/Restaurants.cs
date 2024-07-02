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
        public int RestaurantID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public float AverageRating { get; set; }
        public bool IsReservationEnabled { get; set; }
        public string ServiceType { get; set; }

        public decimal PenaltyRevenue { get; set; }
        public int? AdminID { get; set; }
        static DataAccess dataAccess = new DataAccess();

        public Restaurants(string name, string city, float averageRating, bool isReservationEnabled, string serviceType, int? adminID, decimal penaltyRevenue)
        {
            this.Name = name;
            this.City = city;
            this.AverageRating = averageRating;
            this.IsReservationEnabled = isReservationEnabled;
            this.ServiceType = serviceType;
            this.AdminID = adminID;
            this.PenaltyRevenue = penaltyRevenue;

            // Saving restaurant instance to the database
            string sqlStatement = "INSERT INTO dbo.Restaurants (Name, City, AverageRating, IsReservationEnabled, AdminID, ServiceType, PenaltyRevenue)" +
                " VALUES(@Name, @City, @AverageRating, @IsReservationEnabled, @AdminID, @ServiceType, @PenaltyRevenue)";
            this.RestaurantID = dataAccess.SaveData(sqlStatement, this, true);
        }
    }
}
