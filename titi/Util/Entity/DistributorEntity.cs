using System;

namespace Util.Entity
{
    public class DistributorEntity : BaseEntity
    {
        public string DistributorCode { get; set; }
        public string Name { get; set; }
        public string BusinessLicense { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int Status { get; set; }
    }
}
