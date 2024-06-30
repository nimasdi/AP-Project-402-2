using DBAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    internal class Order
    {
        public int OrderId {  get; set; }
        public int? UserId {  get; set; }
        public int? RestuarantId {  get; set; }
        public DateTime? OrderDate {  get; set; }
        public decimal? TotalAmount {  get; set; }
        public string? PaymentMethod {  get; set; }
        public string? status {  get; set; }
        static DataAccess dataAccess = new DataAccess();

        public Order(int? userId, int? restuarantId, DateTime? orderDate, decimal? totalAmount, string? paymentMethod, string? status)
        {
            UserId = userId;
            RestuarantId = restuarantId;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
            PaymentMethod = paymentMethod;
            this.status = status;

            string sqlStatement = "INSERT INTO dbo.Orders (UserId ,RestaurantId, OrderDate, TotalAmount, PaymentMethod, Status)" +
           " VALUES(@UserId , @RestaurantId, @OrderDate, @TotalAmount, @PaymentMethod, @Status);";
            this.OrderId = dataAccess.SaveData(sqlStatement, this, true);
        }
    }
}
