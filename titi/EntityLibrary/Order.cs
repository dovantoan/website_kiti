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
    
    public partial class Order
    {
        public long Id { get; set; }
        public string OrderCode { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public string ProductCode { get; set; }
        public string UnitCode { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<long> UserID { get; set; }
        public string TruckingCode { get; set; }
        public Nullable<long> PaymentMethods { get; set; }
        public int Status { get; set; }
    }
}
