using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using titi.Models.BaseModels;

namespace titi.Models.Account
{
    public class UserLoginProfile:BaseModel
    {
        public string UserName { get; set; }
        public long? Parent_ID { get; set; }
        public long? ParentB_ID { get; set; }
        public long? Left { get; set; }
        public long? Right { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public long? Sex { get; set; }
        public string Avatar { get; set; }
        public string TaxCode { get; set; }
        public DateTime? BirthDate { get; set; }
        public string BirthAddress { get; set; }
        public string Address { get; set; }
        public long? Ward { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public long? Kind { get; set; }
        public string CID { get; set; }
        public DateTime? CIDDate { get; set; }
        public long? CIDPlace { get; set; }
        public string PassportID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CompanyPhone { get; set; }
        public string Password { get; set; }
        public int? Level { get; set; }
        public int? Status { get; set; }
        public int? PackageID { get; set; }
        public int? PreLevel { get; set; }
        public DateTime? LevelUpdate { get; set; }
        public string AccountBank { get; set; }
        public string Bank { get; set; }
        public string P { get; set; }
        public long? DistributorId { get; set; }
    }
}