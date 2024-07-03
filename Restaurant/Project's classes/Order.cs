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
        public string? Status {  get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public bool IsReservation { get; set; }

        static DataAccess dataAccess = new DataAccess();

        public Order(int? userId, int? restaurantId, DateTime? orderDate, decimal? totalAmount, string? paymentMethod, string? status, bool isReservation, int? rating = null, string? comment = null)
        {
            //OderID
            UserId = userId;
            RestaurantId = restaurantId;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
            PaymentMethod = paymentMethod;
            Status = status;
            Rating = rating;
            Comment = comment;
            IsReservation = isReservation;

            string sqlStatement = "INSERT INTO dbo.Orders (UserId ,RestaurantId, OrderDate, TotalAmount, PaymentMethod, Status, Rating, Comment, IsReservation)" +
                                  " VALUES(@UserId , @RestaurantId, @OrderDate, @TotalAmount, @PaymentMethod, @Status, @Rating, @Comment, @IsReservation);";
            this.OrderId = dataAccess.SaveData(sqlStatement, this, true);
        }
    }
}
