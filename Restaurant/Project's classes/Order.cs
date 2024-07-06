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
        public int? OrderId { get; set; }
        public int? UserId { get; set; }
        public int? RestaurantId { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }
        public int? Rating { get; set; }
        public string? Comment { get; set; }
        public bool IsReservation { get; set; }

        static DataAccess dataAccess = new DataAccess();

        public Order() {}
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

        public static List<Order> GetPendingReservations()
        {
            string sqlStatement = "SELECT * FROM dbo.Orders WHERE IsReservation = 1 AND Status <> 'canceled'";
            return dataAccess.LoadData<Order, dynamic>(sqlStatement, new { });
        }

        public void Cancel()
        {
            if (Status != "canceled")
            {
                Status = "canceled";
                decimal penalty = (decimal)(TotalAmount * 0.3m);

                string updateOrderSql = "UPDATE dbo.Orders SET Status = 'canceled' WHERE OrderId = @OrderId";
                dataAccess.SaveData(updateOrderSql, new { OrderId });

                string updatePenaltySql = "UPDATE dbo.Restaurants SET PenaltyRevenue = PenaltyRevenue + @Penalty WHERE RestaurantId = @RestaurantId";
                dataAccess.SaveData(updatePenaltySql, new { Penalty = penalty, RestaurantId });
            }
        }
    }
}
