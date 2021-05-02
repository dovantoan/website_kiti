using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SessionManageMVC.Controllers.Shared
{
    public static class ToandvSession
    {
        private const string LogOnSession = "LogOnSession";
        private const string ErrorController = "Error";
        private const string LogOnController = "LogOn";
        private const string LogOnAction = "LogOn";
        
        //public static bool HasSession()
        //{
        //    return Session[LogOnSession] != null;
        //}

        //public TSource GetLogOnSessionModel()
        //{
        //    return (TSource)this.Session[LogOnSession];
        //}

        //public void SetLogOnSessionModel(TSource model)
        //{
        //    Session[LogOnSession] = model;
        //}

        public static void AbandonSession()
        {
            //if (HasSession())
            //{
            //    System.Web.HttpContext.Current.Session.Abandon();
            //}
        }

        public static bool TryGet(string entryName,out object data,bool removeEntry = false)
        {
            object ob = System.Web.HttpContext.Current.Session["entryName"];
            if (ob != null)
            {
                data = ob;
                return true;
            }
            else
            {
                data = null;
                return false;
            }
        }

    }
}
