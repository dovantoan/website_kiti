using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLibrary.ModuleImplement
{
    public class ProductView
    {
        public long Pid { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string Image { get; set; }
        public string URL { get; set; }
        public string MetaTitle { get; set; }
        public string SeoTitle { get; set; }
        public string MetaKeywords { get; set; }
        public string MetaDescription { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }
        public decimal Cost { get; set; }
        public string FCost { get { return Util.Util.CurrencyFormat(Cost, "VND"); } }
        public string Size { get; set; }
        public long TypeCode { get; set; }
        public decimal Promotion { get; set; }
        public decimal Price { get; set; }
        public string FPrice { get { return Util.Util.CurrencyFormat(Price, "VND"); } }
        public long DistributorId { get; set; }
        public decimal Quantity { get; set; }
        public string Producer { get; set; }
        public System.DateTime ProductionDate { get; set; }
        public bool? IsNew { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public List<ProductDetailView> ProductDetails { get; set; }
        public DistributorView Distributor { get; set; }
        public string Video { get; set; }
    }
    public class ProductDetailView
    {
        public long Pid { get; set; }
        public long ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string Size { get; set; }
        public string SizeValue { get; set; }
        public string Color { get; set; }
        public string ColorValue { get; set; }
        public string Image { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Description { get; set; }
    }
    public class DistributorView
    {
        public long Pid { get; set; }
        public string DistributorCode { get; set; }
        public string Name { get; set; }
        public string BusinessLicense { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int? Status { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
    }
}
