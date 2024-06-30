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
        string Name { get; set; }
        string City { get; set; }
        float AverageRating { get; set; }
        bool IsReservationEnabled { get; set; }
        int? AdminID { get; set; }
        static DataAccess dataAccess = new DataAccess();

        public Restaurants(string name, string city, float averageRating, bool isReservationEnabled, int? adminID)
        {
            this.Name = name;
            this.City = city;
            this.AverageRating = averageRating;
            this.IsReservationEnabled = isReservationEnabled;
            this.AdminID = adminID;

            //saving restaurant instance to the data base
            string sqlStatement = "INSERT INTO dbo.Restaurants (Name, City, AverageRating, IsReservationEnabled, AdminID)" +
                "VALUES(@Name, @City, @AverageRating, @IsReservationEnabled, @AdminID)";
            this.RestaurantID = dataAccess.SaveData(sqlStatement, this, true);

        }
    }
}
