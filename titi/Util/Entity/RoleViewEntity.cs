using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Entity;

namespace Util.Entity
{
    public class RoleViewEntity
    {
        public long RolePid { get; set; }
        public string RoleName { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
    }
}
