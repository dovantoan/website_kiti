using DatabaseUtility.EntityLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace EntityLibrary
{
    public class DistributorService
    {
        private readonly Util.Util util = new Util.Util();
        public SearchResult<IList<Distributor>> GetAllDistributor()
        {
            var result = new SearchResult<IList<Distributor>>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    List<Distributor> ls = entity.Distributor.ToList();
                    result.Data = ls;
                    result.TotalRows = ls.Count;
                    result.Success = ls.Count > 0 ? true : false;
                    result.Message = ls.Count > 0 ? "Lấy danh sách nhà phân phối thành công" : "Data not found!";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Lấy danh sách nhà phân phối lỗi" + ex.Message;
            }
            return result;
        }

        public PostResult<Distributor> InsertDistributor(Distributor newDistributor)
        {
            PostResult<Distributor> resutl = new PostResult<Distributor>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    entity.Distributor.Add(newDistributor);
                    entity.SaveChanges();
                    resutl.Success = true;
                    resutl.ErrorCode = "0";
                    resutl.Message = "Thêm mới nhà phân phối thành công";
                    resutl.Data = newDistributor;
                }
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.ErrorCode = "100";
                resutl.Message = "Thêm mới Distributor lỗi: " + ex.Message;
            }
            return resutl;
        }

        public PostResult<Distributor> UpdateDistributor(Distributor distributor)
        {
            PostResult<Distributor> resutl = new PostResult<Distributor>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    Distributor r = entity.Distributor.Where(w => w.Pid == distributor.Pid).FirstOrDefault();
                    if (r != null)
                    {
                        r.Address = distributor.Address;
                        r.BusinessLicense = distributor.BusinessLicense;
                        r.DistributorCode = distributor.DistributorCode;
                        r.Name = distributor.Name;
                        r.PhoneNumber = distributor.PhoneNumber;
                        r.Status = distributor.Status;
                        entity.SaveChanges();
                        resutl.Success = true;
                        resutl.ErrorCode = "0";
                        resutl.Message = "Cập nhật nhà phân phối thành công";
                    }
                    else
                    {
                        resutl.Success = false;
                        resutl.ErrorCode = "-1";
                        resutl.Message = "Không tìm thấy dữ liệu cập nhật";
                    }
                    resutl.Data = distributor;

                }
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.ErrorCode = "100";
                resutl.Message = "Cập nhật nhà phân phối lỗi: " + ex.Message;
            }
            return resutl;
        }

        public PostResult<Distributor> DeleteDistributor(long id)
        {
            PostResult<Distributor> resutl = new PostResult<Distributor>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    Distributor r = entity.Distributor.Where(w => w.Pid == id).FirstOrDefault();
                    if (r != null)
                    {
                        entity.Distributor.Remove(r);
                        entity.SaveChanges();
                        resutl.Success = true;
                        resutl.ErrorCode = "0";
                        resutl.Message = "Xóa nhà phân phối thành công";
                    }
                    else
                    {
                        resutl.Success = false;
                        resutl.ErrorCode = "-1";
                        resutl.Message = "Không tìm thấy dữ liệu cần xóa";
                    }
                    resutl.Data = r;

                }
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.ErrorCode = "100";
                resutl.Message = "Xóa nhật nhà phân phối lỗi: " + ex.Message;
            }
            return resutl;
        }

        public SearchResult<Distributor> GetDistributorById(long id)
        {
            var result = new SearchResult<Distributor>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    Distributor dis = entity.Distributor.Where(w => w.Pid == id).FirstOrDefault();
                    result.Data = dis;
                    result.TotalRows = dis!=null ? 1 : 0;
                    result.Success = dis!=null ? true : false;
                    result.Message = dis!=null ? "Lấy thông tin nhà phân phối thành công" : "Data not found!";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Lấy thông tin nhà phân phối lỗi" + ex.Message;
            }
            return result;
        }
    }
}
