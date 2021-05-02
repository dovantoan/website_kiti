using System;
using System.Collections.Generic;
using System.Linq;

namespace Util.Extension
{
    public static class CustomExtension
    {
        public static IEnumerable<T> CustomSort<T>(this IEnumerable<T> list, string sortName, bool isDesc)
        {
            var result = list;
            if (list != null && !string.IsNullOrEmpty(sortName))
            {
                if (isDesc)
                {
                    result = list.OrderByDescending(d => d.GetType().GetProperty(sortName).GetValue(d, null));
                }
                else
                {
                    result = list.OrderBy(d => d.GetType().GetProperty(sortName).GetValue(d, null));
                }
            }
            return result;
        }
    }
}
