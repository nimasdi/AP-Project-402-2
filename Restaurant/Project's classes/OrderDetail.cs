using DBAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_s_classes
{
    public class OrderDetail
    {
        public int? OrderDetailId { get; set; }
        public int? OrderId {  get; set; }
        public int? MenuId {  get; set; }
        public int? Quantity {  get; set; }
        public decimal? Price {  get; set; }
        static DataAccess dataAccess = new DataAccess();

        public OrderDetail(int? orderId, int? menuId, int? quantity, decimal? price)
        {
            OrderId = orderId;
            MenuId = menuId;
            Quantity = quantity;
            Price = price;

            string sqlStatement = "INSERT INTO dbo.OrderDetails (OrderId ,MenuId, Quantity, Price)" +
           " VALUES(@OrderId ,@MenuId, @Quantity, @Price);";
            this.OrderDetailId = dataAccess.SaveData(sqlStatement, this, true);
        }
    }
}
