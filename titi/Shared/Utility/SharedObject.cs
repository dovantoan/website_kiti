using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace Shared.Utility
{
    public static class SharedObject
    {
        //public static BOMUserLogin UserInfo = new BOMUserLogin();
    }

    public static class UserLogin
    {
        public static List<string> listUserLogin = new List<string>();
        public static Dictionary<String, String> listUserChat = new Dictionary<string, String>();
        public static int CountUserLogin = 0; 
    }
}
