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
        public int? RatingID {  get; set; }
        public int? UserID { get; set; }
        public int? RestaurantID  { get; set; }
        public int? MenuID { get; set; }
        public int RatingValue { get; set; }
        public string Comment { get; set; }
        DateTime Date { get; set; }
        static DataAccess dataAccess = new DataAccess();

        public Ratings(int? ratingID, int? userID, int? restaurantID, int? menuID, int ratingValue, string comment)
        {
            RatingID = ratingID;
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
