using System;
using System.Web.Mvc;
using titi.Areas.Admin.Models;
using titi.Areas.Admin.Models.Security;
using titi.Models.Account;
using titi.Models.Security;

namespace titi.Helper
{
    public class SharedContext
    {
        public AdminLoginProfile AdminLogdedProfile
        {
            get {
                return DataSessionManager.GetData<AdminLoginProfile>("AdminLoggedProfile");
            }
            set
            {
                bool solved = false;
                if(value is Impersonatee newValue)
                {
                    if (ToandvSessionManager.TryGet("AdminLoggedProfile", out object ret))
                    {
                        if(!(ret is Impersonatee impersonator))
                        {
                            if(ret is AdminLoginProfile impersonatorFirst)
                            {
                                newValue.ImpersonatorUserId = impersonatorFirst.UserProfile.Pid;
                            }
                        }
                        else
                        {
                            newValue.ImpersonatorUserId = impersonator.ImpersonatorUserId;
                        }
                        //AdminLoggedMenu = null;
                        ToandvSessionManager.TrySet("AdminLoggedProfile", newValue);
                        solved = true;
                    }
                }
                if (!solved)
                {
                    ToandvSessionManager.TrySet("AdminLoggedProfile", value);
                }
                
            }
        }
        public DefineUIModel AdminLoggedMenu
        {
            get
            {
                return DataSessionManager.GetData<DefineUIModel>("AdminLoggedMenu");
            }
            set
            {
                DataSessionManager.SetData("AdminLoggedMenu", value);
            }
        }
        public static SharedContext Current
        {
            get
            {
                SharedContext ret = null;
                try
                {
                    ret = DependencyResolver.Current.GetService<SharedContext>();
                }
                catch (Exception)
                {
                    ret = new SharedContext();
                }return ret;
            }
        }

        public UserLoginProfiles UserLoginProfiles
        {
            get
            {
                return DataSessionManager.GetData<UserLoginProfiles>("UserLoginProfiles");
            }
            set
            {
                bool solved = false;
                if (value is UImpersonatee newValue)
                {
                    if (ToandvSessionManager.TryGet("UserLoginProfiles", out object ret))
                    {
                        if (!(ret is Impersonatee impersonator))
                        {
                            if (ret is AdminLoginProfile impersonatorFirst)
                            {
                                newValue.ImpersonatorUserId = impersonatorFirst.UserProfile.Pid;
                            }
                        }
                        else
                        {
                            newValue.ImpersonatorUserId = impersonator.ImpersonatorUserId;
                        }
                        //AdminLoggedMenu = null;
                        ToandvSessionManager.TrySet("UserLoginProfiles", newValue);
                        solved = true;
                    }
                }
                if (!solved)
                {
                    ToandvSessionManager.TrySet("UserLoginProfiles", value);
                }

            }
        }
    }
}