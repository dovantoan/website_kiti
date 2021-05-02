using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Entity
{
    public class OrderEntity
    {
        public long Id { get; set; }
        public string OrderCode { get; set; }
        public decimal? Amount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ProductCode { get; set; }
        public string UnitCode { get; set; }
        public int? Quantity { get; set; }
        public long? UserID { get; set; }
        public string TruckingCode { get; set; }
        public long? PaymentMethods { get; set; }
        public List<OrderDetailEntity> OrderDetails { get; set; }
        public int Status { get; set; }
    }
    public class OrderDetailEntity
    {
        public long Id { get; set; }
        public long? OrderId { get; set; }
        public string ProductCode { get; set; }
        public long? ProductDettailId { get; set; }
        public string SizeValue { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string ColorValue { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public int? Quantity { get; set; }
        public decimal? Amount { get; set; }
        public string UnitCode { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
