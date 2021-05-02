namespace titi.Areas.Admin.Models
{
    public class PhanQuyenViewModel
    {
        public long Pid { get; set; }
        public long UserPid { get; set; }
        public string UserName { get; set; }
        public long GroupPid { get; set; }
        public string GroupName { get; set; }
        public long RolePid { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
    }
}