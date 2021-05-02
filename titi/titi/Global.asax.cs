using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using titi.Infrastructure;

namespace titi
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            MappingUtil.MappingInitialize();
        }
        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            HttpContext context = HttpContext.Current;
            if (context != null && context.Request.Cookies != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies["language"];
                if (cookie != null && cookie.Value != null)
                {
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(cookie.Value);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(cookie.Value);
                }
                else
                {
                    //Request.Cookies["language"].Value = Shared.Utility.ConstantClass.defaultLang;
                    Thread.CurrentThread.CurrentCulture = new CultureInfo(Shared.Utility.ConstantClass.defaultLang);
                    Thread.CurrentThread.CurrentUICulture = new CultureInfo(Shared.Utility.ConstantClass.defaultLang);
                }

                Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".";
            }
        }
    }
}
