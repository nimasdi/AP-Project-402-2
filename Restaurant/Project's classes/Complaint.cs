using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    internal class Complaint
    {
        int ComplaintId;
        int? UserId;
        int? RestaurantId;
        string? Title;
        string? Description;
        string? Status;

        public Complaint(int complaintId, int? userId, int? restaurantId, string? title, string? description, string? status)
        {
            ComplaintId = complaintId;
            UserId = userId;
            RestaurantId = restaurantId;
            Title = title;
            Description = description;
            Status = status;
        }
    }
}
