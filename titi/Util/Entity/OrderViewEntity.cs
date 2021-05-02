using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Entity
{
    public class OrderViewEntity
    {
        public long Id { get; set; }
        public string OrderCode { get; set; }
        public decimal? SumAmount { get; set; }
        public string FSumAmount { get { return Util.CurrencyFormat(SumAmount, "VND"); } }
        public DateTime? CreatedDate { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public decimal? TruckingAmount { get; set; }
        public string FTruckingAmount { get { return Util.CurrencyFormat(TruckingAmount, "VND"); } }
        public string PaymentMethods { get; set; }
        public List<OrderDetailViewEntity> OrderDetails { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
    public class OrderDetailViewEntity
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
        public string FAmount { get { return Util.CurrencyFormat(Amount, "VND"); } }
        public string UnitCode { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ProductName { get; set; }
    }
}
