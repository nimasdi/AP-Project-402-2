using DBAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    public class Order
    {
        public int? OrderId {  get; set; }
        public int? UserId {  get; set; }
        public int? RestaurantId {  get; set; }
        public DateTime? OrderDate {  get; set; }
        public decimal? TotalAmount {  get; set; }
        public string? PaymentMethod {  get; set; }
        public string? status {  get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        static DataAccess dataAccess = new DataAccess();

        public Order(int? orderId,int? userId, int? restaurantId, DateTime? orderDate, decimal? totalAmount, string? paymentMethod, string? status, int? rating = null, string? comment = null)
        {
            OrderId = orderId;
            UserId = userId;
            RestaurantId = restaurantId;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
            PaymentMethod = paymentMethod;
            this.status = status;
            this.Rating = rating;
            this.Comment = comment;

            string sqlStatement = "INSERT INTO dbo.Orders (UserId ,RestaurantId, OrderDate, TotalAmount, PaymentMethod, Status, Rating, Comment)" +
           " VALUES(@UserId , @RestaurantId, @OrderDate, @TotalAmount, @PaymentMethod, @Status, @Rating, @Comment);";
            this.OrderId = dataAccess.SaveData(sqlStatement, this, true);
        }
    }
}
