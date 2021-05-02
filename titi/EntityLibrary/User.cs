//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace EntityLibrary
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public long Pid { get; set; }
        public string UserName { get; set; }
        public Nullable<long> Parent_ID { get; set; }
        public Nullable<long> ParentB_ID { get; set; }
        public Nullable<long> Left { get; set; }
        public Nullable<long> Right { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public Nullable<long> Sex { get; set; }
        public string Avatar { get; set; }
        public string TaxCode { get; set; }
        public Nullable<System.DateTime> BirthDate { get; set; }
        public string BirthAddress { get; set; }
        public string Address { get; set; }
        public Nullable<long> Ward { get; set; }
        public string District { get; set; }
        public string Province { get; set; }
        public Nullable<long> Kind { get; set; }
        public string CID { get; set; }
        public Nullable<System.DateTime> CIDDate { get; set; }
        public Nullable<long> CIDPlace { get; set; }
        public string PassportID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string CompanyPhone { get; set; }
        public string Password { get; set; }
        public Nullable<int> Level { get; set; }
        public Nullable<int> Status { get; set; }
        public Nullable<int> PackageID { get; set; }
        public Nullable<int> PreLevel { get; set; }
        public Nullable<System.DateTime> LevelUpdate { get; set; }
        public string AccountBank { get; set; }
        public string Bank { get; set; }
        public string P { get; set; }
        public Nullable<long> DistributorId { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
    }
}
