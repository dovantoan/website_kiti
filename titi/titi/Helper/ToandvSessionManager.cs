using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace titi.Helper
{
    public class ToandvSessionManager
    {
        public static bool TryGet(string entryName, out object data, bool removeEntry = false)
        {
            object ob = System.Web.HttpContext.Current.Session[entryName];
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
        public static bool TrySet(string entryName,object data)
        {
            System.Web.HttpContext.Current.Session[entryName] = data;
            return System.Web.HttpContext.Current.Session[entryName] != null;
        }
    }
}