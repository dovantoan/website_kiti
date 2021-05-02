using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace titi.Areas.Admin.Models
{
    public class GroupModule
    {
        public long GroupID { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public int IsActice { get; set; }
    }
}