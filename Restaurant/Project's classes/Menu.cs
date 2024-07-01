using Microsoft.Data.SqlClient;
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
        public int MenuID {  get; set; }
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



        public Menu(int? restaurantId, string? category, string? itemName, string? ingredients, decimal? price, string? imageURL, float? averageRating, int? quantityAvailable)
        {
            RestaurantId = restaurantId;
            Category = category;
            ItemName = itemName;
            Ingredients = ingredients;
            Price = price;
            ImageURL = imageURL;
            AverageRating = averageRating;
            QuantityAvailable = quantityAvailable;
        }

        public void AddComment(int userId, string userName, string content, float rating)
        {
            var newComment = new Comment(this.MenuID, userId, userName, content, rating, DateTime.Now ,false);

            Comments.Add(newComment);

        }

        public void EditComment(int commentId, string newContent, float newRating)
        {
            var commentToEdit = Comments.Find(c => c.CommentID == commentId);
            if (commentToEdit != null)
            {
                commentToEdit.Content = newContent;
                commentToEdit.Rating = newRating;
                commentToEdit.Edited = true;

                // Update database logic should be implemented here
                // Example: DataAccess.UpdateComment(commentToEdit);
            }
        }
    }
   
}
