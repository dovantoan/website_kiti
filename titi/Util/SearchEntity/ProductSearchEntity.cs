using System;
namespace Util.SearchEntity
{
    public class ProductSearchEntity : BaseSearchEntity
    {
        public long UserId { get; set; }
        public long? CategoryId { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int Skip { get { return PageIndex * RowNumber; } }
        public int Task { get { return RowNumber; } }
    }
}
