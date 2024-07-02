using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    public class Order
    {
        public int OrderId {  get; set; }
        public int? UserId {  get; set; }
        public int? RestaurantId {  get; set; }
        public DateTime? OrderDate {  get; set; }
        public decimal? TotalAmount {  get; set; }
        public string? PaymentMethod {  get; set; }
        public string? Status {  get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }

        public Order(int? userId, int? restaurantId, DateTime? orderDate, decimal? totalAmount, string? paymentMethod, string? status, int? rating = null, string? comment = null)
        {
            UserId = userId;
            RestaurantId = restaurantId;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
            PaymentMethod = paymentMethod;
            Status = status;
            Rating = rating;
            Comment = comment;
        }
    }
}
