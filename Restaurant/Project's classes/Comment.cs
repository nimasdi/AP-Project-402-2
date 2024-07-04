using DBAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    public class Comment
    {
        public int? CommentID { get; set; }
        public int? MenuID { get; set; }
        public int? UserID { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public float? Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Edited { get; set; }
        public string AdminResponse { get; set; }
        public DateTime? ResponseDate { get; set; } 

        static DataAccess dataAccess = new DataAccess();


        public Comment(int? menuID, int? userID, string userName, string content, float? rating, DateTime createdAt, bool edited)
        {
            MenuID = menuID;
            UserID = userID;
            UserName = userName;
            Content = content;
            Rating = rating;
            CreatedAt = createdAt;
            Edited = edited;
            AdminResponse = string.Empty;
            ResponseDate = null;


            SaveToDatabase();
        }

        public void SaveToDatabase()
        {
            string sqlStatement = @"INSERT INTO dbo.Comments (MenuID, UserID, UserName, Content, Rating, CreatedAt, Edited, AdminResponse, ResponseDate)
                                    VALUES (@MenuID, @UserID, @UserName, @Content, @Rating, @CreatedAt, @Edited, @AdminResponse, @ResponseDate);
                                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

            var parameters = new
            {
                MenuID,
                UserID,
                UserName,
                Content,
                Rating,
                CreatedAt = DateTime.Now,
                Edited,
                AdminResponse,
                ResponseDate
            };

            CommentID = dataAccess.SaveData(sqlStatement, parameters, true);
        }

        public void Edit(string newContent)
        {
            Content = newContent;
            Edited = true;

            string sqlStatement = @"UPDATE dbo.Comments 
                                    SET Content = @Content, Edited = @Edited 
                                    WHERE CommentID = @CommentID;";

            var parameters = new
            {
                Content,
                Edited,
                CommentID
            };

            dataAccess.SaveData(sqlStatement, parameters, false);
        }

        public void UpdateInDatabase()
        {
            string sql = "UPDATE dbo.Comments " +
                         "SET Content = @Content, Rating = @Rating, Edited = @Edited, AdminResponse = @AdminResponse, ResponseDate = @ResponseDate " +
                         "WHERE CommentID = @CommentID;";
            dataAccess.SaveData(sql, this);
        }

        public void Delete()
        {
            string sqlStatement = "DELETE FROM dbo.Comments WHERE CommentID = @CommentID;";
            dataAccess.SaveData(sqlStatement, new { CommentID });
        }

        public void AddAdminResponse(string response)
        {
            AdminResponse = response;
            ResponseDate = DateTime.Now;

            string sqlStatement = @"UPDATE dbo.Comments 
                                    SET AdminResponse = @AdminResponse, ResponseDate = @ResponseDate 
                                    WHERE CommentID = @CommentID;";

            var parameters = new
            {
                AdminResponse,
                ResponseDate,
                CommentID
            };

            dataAccess.SaveData(sqlStatement, parameters, false);
        }

        public void DeleteAdminResponse()
        {
            AdminResponse = null;
            ResponseDate = null;

            string sqlStatement = @"UPDATE dbo.Comments 
                                    SET AdminResponse = NULL, ResponseDate = NULL 
                                    WHERE CommentID = @CommentID;";

            dataAccess.SaveData(sqlStatement, new { CommentID }, false);
        }
    }
}
