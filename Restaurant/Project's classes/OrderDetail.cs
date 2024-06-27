using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    internal class OrderDetail
    {
        int OrderDetailId { get; set; }
        int? OrderId {  get; set; }
        int? MenuId {  get; set; }
        int? Quantity {  get; set; }
        decimal? Price {  get; set; }

        public OrderDetail(int? orderId, int? menuId, int? quantity, decimal? price)
        {
            OrderId = orderId;
            MenuId = menuId;
            Quantity = quantity;
            Price = price;
        }
    }
}
