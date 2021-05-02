using System;

namespace Util.Entity
{
    public class ProductEntity: BaseEntity
    {
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
        public long? CategoryId { get; set; }
        public decimal Cost { get; set; }
        public string Size { get; set; }
        public long TypeCode { get; set; }
        public decimal Promotion { get; set; }
        public decimal Price { get; set; }
        public long? DistributorId { get; set; }
        public decimal? Quantity { get; set; }
        public string Producer { get; set; }
        public DateTime? ProductionDate { get; set; }
    }
}
