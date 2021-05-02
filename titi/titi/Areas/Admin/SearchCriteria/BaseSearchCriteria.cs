using System.Runtime.Serialization;

namespace titi.Areas.Admin.SearchCriteria
{
    public class BaseSearchCriteria
    {
        public int PageIndex { get; set; }
        
        public int RowNumber { get; set; }
        
        public bool IsDesc { get; set; }
        
        public string SortName { get; set; }

    }
}