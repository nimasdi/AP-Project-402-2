using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBAccess;

namespace Project_s_classes
{
    public class Ratings
    {
        int RatingID {  get; set; }
        int? UserID { get; set; }
        int? RestaurantID  { get; set; }
        int? MenuID { get; set; }
        int RatingValue { get; set; }
        string Comment { get; set; }
        DateTime Date { get; set; }
        static DataAccess dataAccess = new DataAccess();

        public Ratings(int? userID, int? restaurantID, int? menuID, int ratingValue, string comment)
        {
            this.UserID = userID;
            this.RestaurantID = restaurantID;
            this.MenuID = menuID;   
            this.RatingValue = ratingValue;
            this.Comment = comment;
            this.Date = DateTime.Now;

            //saving data into database
            string sqlStatement = "INSERT INTO dbo.Ratings(UserID, RestaurantID, MenuID, RatingValue, Comment, Date)" +
                "VALUES(@UserID, @RestaurantID, @MenuID, @RatingValue, @Comment, @Date)";
            this.RatingID = dataAccess.SaveData(sqlStatement, this, true);
        }
    }
}
