namespace Util.SearchEntity
{
    public class CategorySearchEntity:BaseSearchEntity
    {
        public long UserId { get; set; }
        public long? CategoryId { get; set; }
        public string CategoryCode { get; set; }
        public int Skip { get { return PageIndex * RowNumber; } }
        public int Task { get { return RowNumber; } }
    }
}
