using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace titi.Models.SearchCriteria
{
    public class ProductSearchCriteria : BaseSearchCriteria
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