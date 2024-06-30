using DBAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    internal class Menu
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

            string sqlStatement = "INSERT INTO dbo.Menus (RestaurantId, Category, ItemName, Ingredients, Price, ImageURL, AverageRating, QuanntityAvailable)" +
           " VALUES(@RestaurantId, @Category, @ItemName, @Ingredients, @Price, @ImageURL, @AverageRating, @QuanntityAvailable);";
            this.MenuID = dataAccess.SaveData(sqlStatement, this, true);
        }
    }
}
