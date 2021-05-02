using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Entity
{
    public class ShippingEntity
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public Nullable<long> ProvinceID { get; set; }
        public Nullable<long> DistrictID { get; set; }
        public Nullable<long> WardID { get; set; }
        public string Address { get; set; }
        public string FAddress { get; set; }
        public Nullable<int> AddressType { get; set; }
        public Nullable<long> UserID { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
    }
}
