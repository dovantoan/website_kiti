using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using titi.Areas.Admin.Models;
using titi.Helper;
namespace titi.App_Start
{
    public class AuthorizeCustom : AuthorizeAttribute
    {
        readonly string[] allowedTypes;
        public AuthorizeCustom(params string[] allowedTypes)
        {
            this.allowedTypes = allowedTypes;
        }
        public string[] AllowedTypes
        {
            get { return this.allowedTypes; }
        }
        private string AuthorizeUser(AuthorizationContext filterContext)
        {
            if (filterContext.RequestContext.HttpContext != null)
            {
                var context = filterContext.RequestContext.HttpContext;
                string roleName = Convert.ToString(context.Session["RolesOfUser"]);
                //List<RoleViewModel> roles = context.Session["RolesOfUser"] as List<RoleViewModel>;
                List<RoleViewModel> roles = SharedContext.Current.AdminLogdedProfile?.Roles;
                if (roles != null)
                {
                    foreach (var it in roles)
                    {
                        if (allowedTypes.Contains(it.RoleName))
                            return it.RoleName;
                    }
                }
                return "";
            }
            return "";
        }
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null)
                throw new ArgumentException("filterContext");
            string authUser = AuthorizeUser(filterContext);
            if (!this.AllowedTypes.Any(x => x.Equals(authUser, StringComparison.CurrentCultureIgnoreCase)))
            {
                filterContext.Result = new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary(
                            new
                            {
                                //controller = "Error",
                                //action = "Unauthorised"
                                controller = "Account",
                                action ="Login"
                            })
                        );
            }
        }
    }

    //public class AuthorizeCustom : ActionFilterAttribute, IActionFilter
    //{
    //    readonly string[] allowedTypes;
    //    public AuthorizeCustom(params string[] allowedTypes)
    //    {
    //        this.allowedTypes = allowedTypes;
    //    }
    //    public string[] AllowedTypes
    //    {
    //        get { return this.allowedTypes; }
    //    }
    //    private string AuthorizeUser(AuthorizationContext filterContext)
    //    {
    //        if (filterContext.RequestContext.HttpContext != null)
    //        {
    //            var context = filterContext.RequestContext.HttpContext;
    //            string roleName = Convert.ToString(context.Session["RolesOfUser"]);
    //            List<RoleViewModel> roles = context.Session["RolesOfUser"] as List<RoleViewModel>;
    //            foreach (var it in roles)
    //            {
    //                if (allowedTypes.Contains(it.RoleName))
    //                    return it.RoleName;
    //            }
    //            throw new ArgumentException("filterContext");
    //        }
    //        throw new ArgumentException("filterContext");
    //    }

    //    public override void OnActionExecuting(ActionExecutingContext filterContext)
    //    {
    //        var context = filterContext.RequestContext.HttpContext;
    //        string roleName = Convert.ToString(context.Session["RolesOfUser"]);
    //        List<RoleViewModel> roles = context.Session["RolesOfUser"] as List<RoleViewModel>;
    //        bool check = false;
    //        if (roles != null)
    //        {
    //            foreach (var it in roles)
    //            {
    //                if (allowedTypes.Contains(it.RoleName))
    //                { check = true; break; }
    //            }
    //        }
    //        if (!check)
    //            filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "Controller", "Account" }, { "Action", "Login" } });
    //        base.OnActionExecuting(filterContext);
    //    }
    //}
}