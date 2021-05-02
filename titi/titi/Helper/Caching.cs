using System;
using System.Collections.Generic;

namespace titi.Helper
{
    public class DataSessionManager
    {
        public static T GetData<T>(string entryName, bool removeEntry = false)
        {
            if (ToandvSessionManager.TryGet(entryName,out object ob, removeEntry))
            {
                if(ob is DataWidthKey ret)
                {
                    return (T)ret.Data;
                }
                else
                {
                    return (T)ob;
                }
            }
            return default(T);
        }
        class DataWidthKey
        {
            public DataWidthKey(object key, object data)
            {
                Key = key;
                Data = data; 
            }
            public object Key { get; set; }
            public object Data { get; set; }
        }
        public static bool SetData(string sessionName, object ob, int expirationMinute = 0)
        {
            if(expirationMinute > 0)
            {
                ExpirationSolution.Add(sessionName, expirationMinute);
                
            }
            return ToandvSessionManager.TrySet(sessionName, ob);
        }
        public class ExpirationSolution
        {
            const string KEY = "ExpirationSolution";
            const string KEY2 = "ExpirationSolution2";
            public string SessionName { get; set; }
            public DateTime StarDate { get; set; }
            public int Minutes { get; set; }
            public static void Add(string sessionName, int expiration)
            {
                List<ExpirationSolution> list = DataSessionManager.GetData<List<ExpirationSolution>>(KEY, true);
                bool done = false;
                if(list != null)
                {
                    int cnt = list.Count;
                    for(int i =0; i< cnt; i++)
                    {
                        var it = list[i];
                        if (it.SessionName.Equals(sessionName))
                        {
                            it.StarDate = DateTime.Now;
                            it.Minutes = expiration;
                            done = true;
                            break;
                        }
                    }
                }
                else
                {
                    list = new List<ExpirationSolution>();
                }
                if (!done)
                {
                    list.Add(new ExpirationSolution { StarDate = DateTime.Now, Minutes = expiration, SessionName = sessionName });
                }
                DataSessionManager.SetData(KEY, list);
            }
        }
    }
}