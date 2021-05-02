using System;
using System.Collections.Generic;
using Util;
using titi.Areas.Admin.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace titi.Areas.Admin.Models
{
    public class ProductModel : BaseModel
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
        public string FCost { get { return Util.Util.CurrencyFormat(Cost, "VND"); } }
        public string Size { get; set; }
        public long TypeCode { get; set; }
        public decimal Promotion { get; set; }
        public string FPromotion { get { return Util.Util.CurrencyFormat(Promotion, "VND"); } }
        public decimal Price { get; set; }
        public string FPrice { get { return Util.Util.CurrencyFormat(Price, "VND"); } }
        public long? DistributorId { get; set; }
        public decimal? Quantity { get; set; }
        public long? Producer { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? ProductionDate { get; set; }
        public bool IsNew { get; set; }
        public string Video { get; set; }
        public List<ProductDetailModel> ProductDetails { get; set; }
    }

    public class ProductDetailModel
    {
        public long Pid { get; set; }
        public string ProductPid { get; set; }
        public decimal? Quantity { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Description { get; set; }
    }
    public class Test
    {
        public string Name { set; get; }
        public string Location
        {
            set; get;
        }
    }

}
