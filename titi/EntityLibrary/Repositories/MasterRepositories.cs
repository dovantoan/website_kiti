using DatabaseUtility.EntityLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;
using Util.Entity;

namespace EntityLibrary.Repositories
{
    public class MasterRepositories : IMasterRepositories
    {
        public SearchResult<IList<Province>> GetAllProvince()
        {
            SearchResult<IList<Province>> result = new SearchResult<IList<Province>>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    result.Success = true;
                    result.ErrorCode = "0";
                    result.Message = "lấy danh sách tỉnh thành phố thành công";
                    result.Data = entity.Province.ToList();
                }
            }
            catch(Exception ex)
            {
                result.Success = false;
                result.ErrorCode = "100";
                result.Message = "lấy danh sách tỉnh thành phố thất bại "+ ex.Message;
            }
            return result;
        }

        public SearchResult<IList<District>> GetDistrictByProvinceId(long provinceId)
        {
            SearchResult<IList<District>> result = new SearchResult<IList<District>>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    result.Success = true;
                    result.ErrorCode = "0";
                    result.Message = "lấy danh sách quận/huyện thành công";
                    result.Data = entity.District.Where(w=>w.ProvinceID==(provinceId)).ToList();
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorCode = "100";
                result.Message = "lấy danh sách quận/huyện thất bại " + ex.Message;
            }
            return result;
        }

        public SearchResult<IList<Ward>> GetWardByDistrictId(long districtId)
        {
            SearchResult<IList<Ward>> result = new SearchResult<IList<Ward>>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    result.Success = true;
                    result.ErrorCode = "0";
                    result.Message = "lấy danh sách phường/xã thành công";
                    result.Data = entity.Ward.Where(w=>w.DistrictID==districtId).ToList();
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorCode = "100";
                result.Message = "lấy danh sách phường/xã thất bại " + ex.Message;
            }
            return result;
        }

        public PostResult<Shipping> InserShipping(Shipping shipping)
        {
            PostResult<Shipping> resutl = new PostResult<Shipping>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    entity.Shipping.Add(shipping);
                    entity.SaveChanges();
                    resutl.Success = true;
                    resutl.ErrorCode = "0";
                    resutl.Message = "Thêm mới địa chỉ giao hàng thành công";
                }
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.ErrorCode = "100";
                resutl.Message = "Thêm mới địa chỉ giao hàng lỗi: " + ex.Message;
            }
            return resutl;
        }

        public SearchResult<ShippingEntity> GetShippingByUserID(long UserId)
        {
            SearchResult<ShippingEntity> result = new SearchResult<ShippingEntity>();
            ShippingEntity shipping = new ShippingEntity();
            try
            {
                DataTable dt = new DataTable();
                SqlDBParameter[] inputParams = new SqlDBParameter[1];
                inputParams[0] = new SqlDBParameter("@USERID", SqlDbType.BigInt, UserId);
                dt = DatabaseAccessNonEntity.SearchStoreProcedureDataTable("sp_GetShippingByUserID", inputParams);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        shipping.Id = long.Parse(row["Id"].ToString());
                        shipping.FullName = row["FullName"].ToString();
                        shipping.ProvinceID = long.Parse(row["ProvinceID"].ToString());
                        shipping.DistrictID = long.Parse(row["DistrictID"].ToString());
                        shipping.Address = row["Address"].ToString();
                        shipping.FAddress = row["FAddress"].ToString();
                        shipping.WardID = long.Parse(row["WardID"].ToString());
                        shipping.UserID = long.Parse(row["UserID"].ToString());
                        shipping.AddressType = Convert.ToInt32(row["AddressType"].ToString());
                        shipping.CreatedDate = DateTime.Parse(row["CreatedDate"].ToString());
                        shipping.PhoneNumber = row["PhoneNumber"].ToString();
                        break;
                    }
                    result.Data = shipping;
                    result.Success = true;
                    result.Message = "Lấy thông tin địa chỉ giao hàng thành công";
                }
                else
                {
                    result.Data = shipping;
                    result.Success = false;
                    result.Message = "Data not found";
                }
            }
            catch (Exception ex)
            {
                result.Data = shipping;
                result.Success = false;
                result.Message = "Lấy thông tin giao hàng lỗi: "+ex.Message;
            }
            return result;
        }

        public PostResult<Shipping> UpdateShipping(Shipping shipping)
        {
            PostResult<Shipping> resutl = new PostResult<Shipping>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    Shipping _shipping = entity.Shipping.Where(w => w.Id == shipping.Id).FirstOrDefault();
                    if (_shipping == null)
                    {
                        resutl.Success = false;
                        resutl.ErrorCode = "-1";
                        resutl.Message = "Không tìm thấy dữ liệu cập nhật";
                    }
                    else
                    {
                        _shipping.PhoneNumber = shipping.PhoneNumber;
                        _shipping.FullName = shipping.FullName;
                        _shipping.ProvinceID = shipping.ProvinceID;
                        _shipping.DistrictID = shipping.DistrictID;
                        _shipping.WardID = shipping.WardID;
                        _shipping.Address = shipping.Address;
                        _shipping.AddressType = shipping.AddressType;
                        entity.SaveChanges();
                        resutl.Success = true;
                        resutl.ErrorCode = "0";
                        resutl.Message = "Cập nhật địa chỉ giao hàng thành công";
                    }
                }
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.ErrorCode = "100";
                resutl.Message = "Thêm mới địa chỉ giao hàng lỗi: " + ex.Message;
            }
            return resutl;
        }
    }
}
