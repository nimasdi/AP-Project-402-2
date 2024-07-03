using DBAccess;
﻿using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBAccess;

namespace Project_s_classes
{
    public class Menu
    {

        public int? MenuID {  get; set; }

        public int? RestaurantId {  get; set; }
        public string? Category {  get; set; }
        public string? ItemName {  get; set; }
        public string? Ingredients {  get; set; }
        public decimal? Price {  get; set; }
        public string? ImageURL {  get; set; }
        public float? AverageRating {  get; set; }
        public int? QuantityAvailable {  get; set; }

        public List<Comment> Comments { get; set; } = new List<Comment>(); 

        static DataAccess dataAccess = new DataAccess();


        public Menu(){        }


        public Menu(int? restaurantId, string? category, string? itemName, string? ingredients, decimal? price, string? imageURL, float? averageRating, int? quantityAvailable)
        {
            this.MenuID = menuId;
            RestaurantId = restaurantId;
            Category = category;
            ItemName = itemName;
            Ingredients = ingredients;
            Price = price;
            ImageURL = imageURL;
            AverageRating = averageRating;
            QuantityAvailable = quantityAvailable;


            string sqlStatement = "INSERT INTO dbo.Menus (RestaurantId, Category, ItemName, Ingredients, Price, ImageURL, AverageRating, QuanntityAvailable)" +
           " VALUES(@RestaurantId, @Category, @ItemName, @Ingredients, @Price, @ImageURL, @AverageRating, @QuanntityAvailable);";
            this.MenuID = dataAccess.SaveData(sqlStatement, this, true);
            LoadComments();
        }

        private void LoadComments()
        {
            string sql = "SELECT CommentID, UserID, UserName, Content, Rating, CreatedAt, Edited " +
                         "FROM dbo.Comments " +
                         "WHERE MenuID = @MenuID;";

            var parameters = new { MenuID };

            var commentsFromDb = dataAccess.LoadData<Comment, dynamic>(sql, parameters);

            Comments.AddRange(commentsFromDb);
        }
        public void AddComment(int userId, string userName, string content, float rating)
        {
            var newComment = new Comment(this.MenuID, userId, userName, content, rating, DateTime.Now, false);

            newComment.SaveToDatabase();
            Comments.Add(newComment);
        }

        public void EditComment(int commentId, string newContent)
        {
            var commentToEdit = Comments.FirstOrDefault(c => c.CommentID == commentId);
            if (commentToEdit != null)
            {
                commentToEdit.Edit(newContent);
            }
        }

        public void DeleteComment(int commentId)
        {
            var commentToDelete = Comments.FirstOrDefault(c => c.CommentID == commentId);
            if (commentToDelete != null)
            {
                commentToDelete.Delete();
                Comments.Remove(commentToDelete);
            }
        }

    }
   
}
