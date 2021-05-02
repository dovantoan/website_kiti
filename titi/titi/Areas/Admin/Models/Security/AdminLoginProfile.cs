using System.Collections.Generic;

namespace titi.Areas.Admin.Models.Security
{
    public class AdminLoginProfile
    {
        public UserModel UserProfile { get; set; }
        public List<DefineUIModel> DefineUI { get; set; }
        public TokenModel Token { get; set; }
        public List<RoleViewModel> Roles { get; set; }
        public string ReturnUrl { get; set; }
    }
    public class Impersonatee: AdminLoginProfile
    {
        public long ImpersonatorUserId { get; set; }
    }
}