using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Util.Entity
{
    public class CategoryEntity : BaseEntity
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
