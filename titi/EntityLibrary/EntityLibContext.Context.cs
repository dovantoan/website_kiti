﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class EntityLibContext : DbContext
    {
        public EntityLibContext()
            : base("name=EntityLibContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Category> Category { get; set; }
        public virtual DbSet<DefineUI> DefineUI { get; set; }
        public virtual DbSet<DefinitionMultiLanguage> DefinitionMultiLanguage { get; set; }
        public virtual DbSet<Distributor> Distributor { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<GroupImage> GroupImage { get; set; }
        public virtual DbSet<GroupsRole> GroupsRole { get; set; }
        public virtual DbSet<GroupsUI> GroupsUI { get; set; }
        public virtual DbSet<GroupUser> GroupUser { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<Master> Master { get; set; }
        public virtual DbSet<MasterDetail> MasterDetail { get; set; }
        public virtual DbSet<ProductDetail> ProductDetail { get; set; }
        public virtual DbSet<PHANQUYEN> PHANQUYEN { get; set; }
        public virtual DbSet<Session> Session { get; set; }
        public virtual DbSet<Tokens> Tokens { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<Producer> Producer { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<District> District { get; set; }
        public virtual DbSet<Province> Province { get; set; }
        public virtual DbSet<Shipping> Shipping { get; set; }
        public virtual DbSet<Ward> Ward { get; set; }
        public virtual DbSet<Property> Property { get; set; }
        public virtual DbSet<PropertyDetail> PropertyDetail { get; set; }
        public virtual DbSet<TBCategory_TBProperty> TBCategory_TBProperty { get; set; }
        public virtual DbSet<PaymentMethods> PaymentMethods { get; set; }
        public virtual DbSet<OrderDetails> OrderDetails { get; set; }
        public virtual DbSet<Trucking> Trucking { get; set; }
        public virtual DbSet<Order> Order { get; set; }
    
        public virtual ObjectResult<sp_GetDefineUIByUserID_Result> sp_GetDefineUIByUserID(Nullable<long> uSERID)
        {
            var uSERIDParameter = uSERID.HasValue ?
                new ObjectParameter("USERID", uSERID) :
                new ObjectParameter("USERID", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetDefineUIByUserID_Result>("sp_GetDefineUIByUserID", uSERIDParameter);
        }
    
        public virtual ObjectResult<sp_GetPhanQuyenByUserId_Result> sp_GetPhanQuyenByUserId(Nullable<long> uSERID)
        {
            var uSERIDParameter = uSERID.HasValue ?
                new ObjectParameter("USERID", uSERID) :
                new ObjectParameter("USERID", typeof(long));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<sp_GetPhanQuyenByUserId_Result>("sp_GetPhanQuyenByUserId", uSERIDParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> sp_Product_Search(Nullable<long> uSERID, Nullable<long> cATEGORYID, string pRODUCTCODE, string pRODUCTNAME, Nullable<System.DateTime> fROMDATE, Nullable<System.DateTime> tODATE, Nullable<int> sKIP, Nullable<int> tASK)
        {
            var uSERIDParameter = uSERID.HasValue ?
                new ObjectParameter("USERID", uSERID) :
                new ObjectParameter("USERID", typeof(long));
    
            var cATEGORYIDParameter = cATEGORYID.HasValue ?
                new ObjectParameter("CATEGORYID", cATEGORYID) :
                new ObjectParameter("CATEGORYID", typeof(long));
    
            var pRODUCTCODEParameter = pRODUCTCODE != null ?
                new ObjectParameter("PRODUCTCODE", pRODUCTCODE) :
                new ObjectParameter("PRODUCTCODE", typeof(string));
    
            var pRODUCTNAMEParameter = pRODUCTNAME != null ?
                new ObjectParameter("PRODUCTNAME", pRODUCTNAME) :
                new ObjectParameter("PRODUCTNAME", typeof(string));
    
            var fROMDATEParameter = fROMDATE.HasValue ?
                new ObjectParameter("FROMDATE", fROMDATE) :
                new ObjectParameter("FROMDATE", typeof(System.DateTime));
    
            var tODATEParameter = tODATE.HasValue ?
                new ObjectParameter("TODATE", tODATE) :
                new ObjectParameter("TODATE", typeof(System.DateTime));
    
            var sKIPParameter = sKIP.HasValue ?
                new ObjectParameter("SKIP", sKIP) :
                new ObjectParameter("SKIP", typeof(int));
    
            var tASKParameter = tASK.HasValue ?
                new ObjectParameter("TASK", tASK) :
                new ObjectParameter("TASK", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("sp_Product_Search", uSERIDParameter, cATEGORYIDParameter, pRODUCTCODEParameter, pRODUCTNAMEParameter, fROMDATEParameter, tODATEParameter, sKIPParameter, tASKParameter);
        }
    }
}