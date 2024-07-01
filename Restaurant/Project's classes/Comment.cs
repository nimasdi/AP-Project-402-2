using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    public class Comment
    {
        public int CommentID { get; set; }
        public int MenuID { get; set; }
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public float Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Edited { get; set; }


        public Comment(int menuID, int userID, string userName, string content, float rating, DateTime createdAt, bool edited)
        {
            MenuID = menuID;
            UserID = userID;
            UserName = userName;
            Content = content;
            Rating = rating;
            CreatedAt = createdAt;
            Edited = edited;
        }
    }
}
