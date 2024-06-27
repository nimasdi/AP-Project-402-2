using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    internal class Complaint
    {
        int ComplaintID {  get; set; }
        int? UserId { get; set; }
        int? RestaurantId  { get; set; }
        string? Title {  get; set; }
        string? Description {  get; set; }
        string? Status {  get; set; }

        public Complaint(int? userId, int? restaurantId, string? title, string? description, string? status)
        {
            UserId = userId;
            RestaurantId = restaurantId;
            Title = title;
            Description = description;
            Status = status;
        }
    }
}
