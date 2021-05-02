using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace titi.Areas.Admin.Models
{
    public class DistributorModels
    {
        public long Pid { get; set; }
        public string DistributorCode { get; set; }
        public string Name { get; set; }
        public string BusinessLicense { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public int? Status { get; set; }
        public string CreatedBy { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public System.DateTime? ModifiedDate { get; set; }
    }
}