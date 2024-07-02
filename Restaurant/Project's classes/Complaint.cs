using DBAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    public class Complaint
    {
        public int? ComplaintID {  get; set; }
        public int? UserId { get; set; }
        public int? RestaurantId  { get; set; }
        public string? Title {  get; set; }
        public string? Description {  get; set; }
        public string? Status {  get; set; }
        public DateTime CreateDate { get; set; }
        public string Response {  get; set; }
        static DataAccess dataAccess = new DataAccess();

        public Complaint(int? complaintID,int? userId, int? restaurantId, string? title, string? description) 
        {
            ComplaintID = complaintID;
            UserId = userId;
            RestaurantId = restaurantId;
            Title = title;
            Description = description;
            Status = "Not Checked";
            CreateDate = DateTime.Now;
            Response = string.Empty;

            string sqlStatement = "INSERT INTO dbo.Complaints (UserId, RestaurantId, Title, Description, Status, CreateDate, Response)" +
           " VALUES(@UserId, @RestaurantId, @Title, @Description, @Status, @CreateDate, @Response);";
            this.ComplaintID = dataAccess.SaveData(sqlStatement, this, true);
        }
    }
}
