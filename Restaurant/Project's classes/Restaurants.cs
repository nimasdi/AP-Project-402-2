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
        public int? AdminID { get; set; }
        public int Password {  get; set; }
        static DataAccess dataAccess = new DataAccess();

        public Restaurants(string name, string city, float averageRating, bool isReservationEnabled, int? adminID, int password)
        {
            this.Name = name;
            this.City = city;
            this.AverageRating = averageRating;
            this.IsReservationEnabled = isReservationEnabled;
            this.AdminID = adminID;
            Password=password;

            //saving restaurant instance to the data base
            string sqlStatement = "INSERT INTO dbo.Restaurants (Name, City, AverageRating, IsReservationEnabled, AdminID, Password)" +
                "VALUES(@Name, @City, @AverageRating, @IsReservationEnabled, @AdminID, @Password)";
            this.RestaurantID = dataAccess.SaveData(sqlStatement, this, true);
            
        }
    }
}
