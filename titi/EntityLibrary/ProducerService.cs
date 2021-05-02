using DatabaseUtility.EntityLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace EntityLibrary
{
    public class ProducerService
    {
        public IList<Producer> GetAllProducer(out int errorCode, out string errMsg)
        {
            try
            {
                using (EntityLibContext entities = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    errorCode = 0;
                    errMsg = "";
                    return entities.Producer.ToList();
                }
            }
            catch (Exception ex)
            {
                errorCode = 100;
                errMsg = ex.Message;
                return null;
            }
        }

        public PostResult<Producer> UpdateProducer(Producer pro)
        {
            PostResult<Producer> resutl = new PostResult<Producer>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    Producer r = entity.Producer.Where(w => w.Pid == pro.Pid).FirstOrDefault();
                    if (r != null)
                    {
                        r.ProducerCode = pro.ProducerCode;
                        r.ProducerName = pro.ProducerName;
                        r.TaxCode = pro.TaxCode;
                        r.Address = pro.Address;
                        r.PhoneNumber = pro.PhoneNumber;
                        r.Email = pro.Email;
                        entity.SaveChanges();
                        resutl.Success = true;
                        resutl.ErrorCode = "0";
                        resutl.Message = "Cập nhật thông tin nhà sản xuất thành công";
                    }
                    else
                    {
                        resutl.Success = false;
                        resutl.ErrorCode = "-1";
                        resutl.Message = "Không tìm thấy dữ liệu cập nhật";
                    }
                    resutl.Data = pro;

                }
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.ErrorCode = "100";
                resutl.Message = "Cập nhật nhà sản xuất lỗi: " + ex.Message;
            }
            return resutl;
        }

        public PostResult<Producer> InsertProducer(Producer pro)
        {
            PostResult<Producer> resutl = new PostResult<Producer>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    entity.Producer.Add(pro);
                    entity.SaveChanges();
                    resutl.Success = true;
                    resutl.ErrorCode = "0";
                    resutl.Message = "Thêm mới nhà sản xuất thành công";
                    resutl.Data = pro;
                }
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.ErrorCode = "100";
                resutl.Message = "Thêm mới nhà sản xuất lỗi: " + ex.Message;
            }
            return resutl;
        }

        public PostResult<Producer> DeleteProducer(long id)
        {
            PostResult<Producer> resutl = new PostResult<Producer>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    Producer r = entity.Producer.Where(w => w.Pid == id).FirstOrDefault();
                    if (r != null)
                    {
                        entity.Producer.Remove(r);
                        entity.SaveChanges();
                        resutl.Success = true;
                        resutl.ErrorCode = "0";
                        resutl.Message = "Xóa nhà sản xuất thành công thành công";
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
                resutl.Message = "Xóa nhà sản xuất lỗi: " + ex.Message;
            }
            return resutl;
        }
    }
}
