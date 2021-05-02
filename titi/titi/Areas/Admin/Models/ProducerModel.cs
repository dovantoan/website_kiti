using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace titi.Areas.Admin.Models
{
    public class ProducerModel
    {
        public long Pid { get; set; }
        public string ProducerCode { get; set; }
        public string ProducerName { get; set; }
        public string TaxCode { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
    }
}