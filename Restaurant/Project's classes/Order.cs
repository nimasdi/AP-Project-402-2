using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    internal class Order
    {
        int OrderId;
        int? UserId;
        int? RestuarantId;
        DateTime? OrderDate;
        decimal? TotalAmount;
        string? PaymentMethod;
        string? status;

        public Order(int orderId, int? userId, int? restuarantId, DateTime? orderDate, decimal? totalAmount, string? paymentMethod, string? status)
        {
            OrderId = orderId;
            UserId = userId;
            RestuarantId = restuarantId;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
            PaymentMethod = paymentMethod;
            this.status = status;
        }
    }
}
