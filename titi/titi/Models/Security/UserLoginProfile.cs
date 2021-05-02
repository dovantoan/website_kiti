using System.Collections.Generic;
using titi.Models.Account;

namespace titi.Models.Security
{
    public class UserLoginProfiles
    {
        public UserLoginProfile UserProfile { get; set; }
    }
    public class UImpersonatee : UserLoginProfiles
    {
        public long ImpersonatorUserId { get; set; }
    }
}