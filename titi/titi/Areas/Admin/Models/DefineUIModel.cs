using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Entity;

namespace titi.Areas.Admin.Models
{
    public class DefineUIModel
    {
        public long Pid { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public string NameSpace { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string OtherInfo { get; set; }
        public Nullable<int> ViewState { get; set; }
        public Nullable<int> WindowState { get; set; }
        public Nullable<long> ParentPid { get; set; }
        public Nullable<int> OrderBy { get; set; }
        public Nullable<int> IsActive { get; set; }
        public string UIParam { get; set; }
        public string Icon { get; set; }
        public Nullable<int> Size { get; set; }
    }
}
