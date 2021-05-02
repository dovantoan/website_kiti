
using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using titi.Helper;

namespace titi.App_Start
{
    public class CustomizeActionFilter : ActionFilterAttribute
    {
        //UIService uiService = new UIService();
        //UserService userService = new UserService();

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserInfo"] == null)
            //if (SharedContext.Current.AdminLogdedProfile == null)
            {
                string returnURL = filterContext.RouteData.Values["controller"].ToString() + @"/" + filterContext.RouteData.Values["action"].ToString();
                //string returnURL = "Admin" + @"/"+filterContext.RouteData.Values["controller"].ToString() + @"/" + filterContext.RouteData.Values["action"].ToString();

                string originalPath = new Uri(HttpContext.Current.Request.Url.AbsoluteUri).OriginalString;
                string method = filterContext.HttpContext.Request.HttpMethod;
                if (method == "GET" && returnURL.Contains("Index"))
                {
                    RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary
                    {
                        { "action", "Login" },
                        { "controller", "Account" },
                        { "returnUrl", returnURL },
                        { "Area", "Admin" }
                    };
                    filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
                }
                else
                {
                    RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary
                    {
                        { "action", "Login" },
                        { "controller", "Account" },
                        { "returnUrl", returnURL },
                        { "Area", "Admin" }
                    };
                    filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);
                }
            }
            else
            {
                string returnURL = filterContext.HttpContext.Session["ReturnUrl"].ToString();
                //string returnURL = SharedContext.Current.AdminLogdedProfile.ReturnUrl.ToString();

                string action = filterContext.RouteData.Values["controller"].ToString() + @"/" + filterContext.RouteData.Values["action"].ToString();
                //string action = "Admin"+@"/"+filterContext.RouteData.Values["controller"].ToString() + @"/" + filterContext.RouteData.Values["action"].ToString();

                if (returnURL.Trim().Length > 0)
                {
                    action = returnURL.Replace("%2", "/");
                }

                if (returnURL.Trim().Length > 0)
                {
                    string returnController = action.Split('/')[0].ToString();
                    string returnAction = action.Split('/')[1].ToString();

                    RouteValueDictionary redirectTargetDictionary = new RouteValueDictionary
                    {
                        { "action", returnAction },
                        { "controller", returnController },
                        { "Area", "Admin" }
                    };
                    filterContext.Result = new RedirectToRouteResult(redirectTargetDictionary);

                    //SharedContext.Current.AdminLogdedProfile.ReturnUrl = "";
                    filterContext.HttpContext.Session["ReturnUrl"] = "";
                }
            }
        }

        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log("OnActionExecuted", filterContext.RouteData);
        }

        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            Log("OnResultExecuting", filterContext.RouteData);
        }

        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            Log("OnResultExecuted", filterContext.RouteData);
        }

        private void Log(string methodName, RouteData routeData)
        {
            var controllerName = routeData.Values["controller"];
            var actionName = routeData.Values["action"];
            var message = String.Format("{0} controller:{1} action:{2}", methodName, controllerName, actionName);
            Debug.WriteLine(message, "Action Filter Log");
        }

    }
}