using titi.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace titi.Models
{
    
    public class CategoryModel: BaseModel
    {

        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }

        public string Image { get; set; }

        public long ParentPid { get; set; }

        public int Orderby { get; set; }

        public string Description { get; set; }

        public string URL { get; set; }

        public string MetaTitle { get; set; }

        public string SeoTitle { get; set; }

        public string MetaKeywords { get; set; }

        public string MetaDescription { get; set; }

        public int Status { get; set; }
    }
}