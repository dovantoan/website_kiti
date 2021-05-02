using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.SearchEntity
{
    public class BaseSearchEntity
    {
        public int PageIndex { get; set; }
        public int RowNumber { get; set; }
        public bool IsDesc { get; set; }
        public string SortName { get; set; }
    }
}
