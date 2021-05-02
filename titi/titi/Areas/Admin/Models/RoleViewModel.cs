using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace titi.Areas.Admin.Models
{
    public class RoleViewModel
    {
        public long RolePid { get; set; }
        public string RoleName { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
    }
}