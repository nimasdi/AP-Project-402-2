using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    internal class Menu
    {
        int MenuID {  get; set; }
        int? RestaurantId {  get; set; }
        string? Category {  get; set; }
        string? ItemName {  get; set; }
        string? Ingredients {  get; set; }
        decimal? Price {  get; set; }
        string? ImageURL {  get; set; }
        float? AverageRating {  get; set; }
        int? QuantityAvailable {  get; set; }

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
    }
}
