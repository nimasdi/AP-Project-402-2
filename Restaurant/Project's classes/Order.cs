using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    internal class Order
    {
        int OrderId {  get; set; }
        int? UserId {  get; set; }
        int? RestuarantId {  get; set; }
        DateTime? OrderDate {  get; set; }
        decimal? TotalAmount {  get; set; }
        string? PaymentMethod {  get; set; }
        string? status {  get; set; }

        public Order(int? userId, int? restuarantId, DateTime? orderDate, decimal? totalAmount, string? paymentMethod, string? status)
        {
            UserId = userId;
            RestuarantId = restuarantId;
            OrderDate = orderDate;
            TotalAmount = totalAmount;
            PaymentMethod = paymentMethod;
            this.status = status;
        }
    }
}
