using titi.Models.BaseModels;
using ExifLib;

namespace titi.Models
{

    public class ProductDetailModel
    {
        public long Pid { get; set; }
        public long ProductId { get; set; }
        public decimal Quantity { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Image { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Description { get; set; }
    }
}