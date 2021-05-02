using titi.Areas.Admin.Models;

namespace titi.Areas.Admin.SearchCriteria
{
    public class JsonSearchResultCriteria<T> : JsonResultModel<T> where T : class
    {
        public new int TotalRows { get; set; }
        
        public string TotalPage { get; set; }

    }
}