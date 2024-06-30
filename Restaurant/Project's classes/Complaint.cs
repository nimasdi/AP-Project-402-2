using DBAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    internal class Complaint
    {
        public int ComplaintID {  get; set; }
        public int? UserId { get; set; }
        public int? RestaurantId  { get; set; }
        public string? Title {  get; set; }
        public string? Description {  get; set; }
        public string? Status {  get; set; }
        static DataAccess dataAccess = new DataAccess();

        public Complaint(int? userId, int? restaurantId, string? title, string? description, string? status)
        {
            UserId = userId;
            RestaurantId = restaurantId;
            Title = title;
            Description = description;
            Status = status;

            string sqlStatement = "INSERT INTO dbo.Complaints (UserId, RestaurantId, Title, Description, Status)" +
           " VALUES(@UserId, @RestaurantId, @Title, @Description, @Status);";
            this.ComplaintID = dataAccess.SaveData(sqlStatement, this, true);
        }
    }
}
