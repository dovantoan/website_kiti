using System;

namespace titi.Models.MasterModels
{
    public class ProvinceModels
    {
        public long ProvinceID { get; set; }
        public string Province1 { get; set; }
        public string ShortName { get; set; }
        public string TelephoneNo { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public partial class DistrictModels
    {
        public long DistrictID { get; set; }
        public string District1 { get; set; }
        public string ShortName { get; set; }
        public long? ProvinceID { get; set; }
        public string TelephoneNo { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public partial class WardModels
    {
        public long WardID { get; set; }
        public string Ward1 { get; set; }
        public string ShortName { get; set; }
        public long? DistrictID { get; set; }
        public string CreateBy { get; set; }
        public DateTime? CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
    public class ShippingModels
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public long? ProvinceID { get; set; }
        public long? DistrictID { get; set; }
        public long? WardID { get; set; }
        public string Address { get; set; }
        public int? AddressType { get; set; }
        public long? UserID { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}