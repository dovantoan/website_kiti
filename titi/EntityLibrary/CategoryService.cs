using DatabaseUtility.EntityLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using Util;

namespace EntityLibrary
{
    public class CategoryService
    {
        public IList<Category> GetAllCategory(out int errorCode, out string errorMsg)
        {
            var result = new List<Category>();
            errorCode = 0;
            errorMsg = "";
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    result = entity.Category.Where(w => w.Status == 1).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public PostResult<Category> InsertCategory(Category category)
        {
            PostResult<Category> resutl = new PostResult<Category>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    entity.Category.Add(category);
                    entity.SaveChanges();
                    resutl.Success = true;
                    resutl.ErrorCode = "0";
                    resutl.Message = "Thêm mới loại sản phẩm thành công";
                }
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.ErrorCode = "100";
                resutl.Message = "Thêm mới loại sản phẩm lỗi: " + ex.Message;
            }
            return resutl;
        }

        public PostResult<Category> UpdateCategory(Category category)
        {
            PostResult<Category> resutl = new PostResult<Category>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    Category cate = entity.Category.Where(w => w.Pid == category.Pid).FirstOrDefault();
                    if (cate != null)
                    {
                        cate.CategoryName = category.CategoryName;
                        cate.Image = category.Image;
                        cate.ParentPid = category.ParentPid;
                        cate.Orderby = category.Orderby;
                        cate.Description = category.Description;
                        cate.URL = category.URL;
                        cate.MetaTitle = category.MetaTitle;
                        cate.MetaDescription = category.MetaDescription;
                        cate.MetaKeywords = category.MetaKeywords;
                        cate.Status = category.Status;
                        entity.SaveChanges();
                        resutl.Success = true;
                        resutl.ErrorCode = "0";
                        resutl.Message = "Cập nhật loại sản phẩm thành công";
                    }
                    else
                    {
                        resutl.Success = false;
                        resutl.ErrorCode = "-1";
                        resutl.Message = "Không tìm thấy dữ liệu cập nhật";
                    }
                    resutl.Data = cate;

                }
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.ErrorCode = "100";
                resutl.Message = "Cập nhật loại sản phẩm lỗi: " + ex.Message;
            }
            return resutl;
        }

        public SearchResult<Category> GetCategoryById(long categoryId)
        {
            SearchResult<Category> res = new SearchResult<Category>();
            try
            {
                using (EntityLibContext entities = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    Category p = new Category();
                    p = entities.Category.Where(w => w.Pid == categoryId).FirstOrDefault();
                    if (p != null)
                    {
                        res.Data = p;
                        res.Success = true;
                        res.Message = "lấy thông tin chi tiết loại sản phẩm thành công";
                    }
                    else
                    {
                        res.Data = p;
                        res.Success = false;
                        res.Message = "Data not found!";
                    }
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = "Lỗi lấy thông tin chi tiết loại sản phẩm: " + ex.Message;
            }
            return res;
        }
        
        public int DeleteCategory(long categoryId, out int errorCode, out string errorMsg)
        {
            int result = 0;
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    Category cate = entity.Category.Where(w => w.Pid == categoryId).FirstOrDefault();
                    if (cate != null)
                    {
                        entity.Category.Remove(cate);
                        entity.SaveChanges();
                        errorCode = 0;
                        errorMsg = "Xóa category thành công";
                    }
                    else
                    {
                        result = -1;
                        errorCode = -1;
                        errorMsg = "Không tìm thấy dữ liệu";
                    }

                }
            }
            catch (Exception ex)
            {
                result = 100;
                errorCode = 100;
                errorMsg = "Xóa loại sản phẩm lỗi: " + ex.Message;
            }
            return result;
        }
    }
}
