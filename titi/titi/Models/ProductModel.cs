using titi.Models.BaseModels;
using ExifLib;
using System.Collections.Generic;

namespace titi.Models
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
    }

    public static class ExifLibExtensions
    {
        public static double? GetLatitude(this ExifReader reader)

        {

            return reader.GetCoordinate(ExifTags.GPSLatitude);

        }
        public static double? GetLongitude(this ExifReader reader)

        {

            return reader.GetCoordinate(ExifTags.GPSLongitude);

        }
        private static double? GetCoordinate(this ExifReader reader, ExifTags type)

        {

            if (reader.GetTagValue(type, out double[] coordinates))

            {

                return ToDoubleCoordinates(coordinates);

            }



            return null;
        }
        private static double ToDoubleCoordinates(double[] coordinates)

        {
            return coordinates[0] + coordinates[1] / 60f + coordinates[2] / 3600f;
        }
    }
    public class PhotoCoordinatesModel

    {
        public double? Lat { get; set; }
        public double? Lon { get; set; }
        public string Error { get; set; }
        public bool HasValidCoordinates()
        {
            return Lat.HasValue && Lon.HasValue;
        }
    }


    public class ProductView : BaseModel
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
        public List<ProductDetailView> ProductDetails { get; set; }
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

    public class PropertyOfProduct
    {
        public string ColorId { get; set; }
        public string ColorValue { get; set; }
        public List<SizeOfProduct> ListSize { get; set; }
    }
    public class SizeOfProduct
    {
        public string SizeId { get; set; }
        public string SizeValue { get; set; }
    }
}