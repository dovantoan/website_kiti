namespace Util
{
    public class SearchResult<T> where T : class
    {
        public int TotalRows { get; set; }
        public T Data { get; set; }
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class PostResult<T> where T : class
    {
        public bool Success { get; set; }
        public string ErrorCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
}
