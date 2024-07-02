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
<<<<<<< HEAD
        public int? OrderId {  get; set; }
        public int? UserId {  get; set; }
        public int? RestuarantId {  get; set; }
        public DateTime? OrderDate {  get; set; }
        public decimal? TotalAmount {  get; set; }
        public string? PaymentMethod {  get; set; }
        public string? status {  get; set; }
        static DataAccess dataAccess = new DataAccess();

        public Order(int? orderId,int? userId, int? restuarantId, DateTime? orderDate, decimal? totalAmount, string? paymentMethod, string? status)
=======
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
>>>>>>> bc1d0981a80edd217d22f61a1f3801f598da4862
        {
            OrderId = orderId;
            UserId = userId;
            RestaurantId = restaurantId;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
            PaymentMethod = paymentMethod;
<<<<<<< HEAD
            this.status = status;

            string sqlStatement = "INSERT INTO dbo.Orders (UserId ,RestaurantId, OrderDate, TotalAmount, PaymentMethod, Status)" +
           " VALUES(@UserId , @RestaurantId, @OrderDate, @TotalAmount, @PaymentMethod, @Status);";
            this.OrderId = dataAccess.SaveData(sqlStatement, this, true);
=======
            Status = status;
            Rating = rating;
            Comment = comment;
>>>>>>> bc1d0981a80edd217d22f61a1f3801f598da4862
        }
    }
}
