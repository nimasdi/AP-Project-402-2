using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    internal class Menu
    {
        int MenuId;
        int? RestaurantId;
        string? Category;
        string? ItemName;
        string? Ingredients;
        decimal? Price;
        string? ImageURL;
        float? AverageRating;
        int? QuantityAvailable;

        public Menu(int menuId, int? restaurantId, string? category, string? itemName, string? ingredients, decimal? price, string? imageURL, float? averageRating, int? quantityAvailable)
        {
            MenuId = menuId;
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
