using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace titi.Areas.Admin.Models
{
    public class JsonResultModel<T> where T : class
    {
        public string ExceptionMessage { get; set; }
        
        public IList<string> ValidateMessage { get; set; }
        
        public bool Success { get; set; }
        public int TotalRows { get; set; }

        public T Data { get; set; }

    }
}