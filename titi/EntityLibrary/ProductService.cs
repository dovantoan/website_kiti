using DatabaseUtility.EntityLibrary;
using EntityLibrary.ModuleImplement;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Util;
using Util.Extension;
using Util.SearchEntity;

namespace EntityLibrary
{
    public class ProductService
    {
        private readonly Util.Util util = new Util.Util();
        public SearchResult<IList<Product>> Search(ProductSearchEntity product)
        {
            SearchResult<IList<Product>> result = new SearchResult<IList<Product>>();
            try
            {
                if (product != null)
                {
                    if (product.ProductCode == null)
                    {
                        product.ProductCode = string.Empty;
                    }
                    using (EntityLibContext entities = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                    {
                        List<Product> data = (from p in entities.Product
                                    where p.ProductCode != null
                                    && p.ProductCode.Contains(product.ProductCode)
                                    //&& psp.FromDate.HasValue ? p.FromDate >= psp.FromDate : 1 == 1
                                    //&& psp.ToDate.HasValue ? p.ToDate <= psp.ToDate : 1 == 1
                                    select p).ToList();
                        if (data != null && data.Any())
                        {
                            IEnumerable<Product> sData = data.CustomSort(product.SortName, product.IsDesc);
                            result.Data = sData.Skip(product.PageIndex * product.RowNumber).Take(product.RowNumber).ToList();
                            result.TotalRows = sData.ToList().Count();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        } 

        public SearchResult<IList<Product>> SearchProduct(ProductSearchEntity product)
        {
            var result = new SearchResult<IList<Product>>();
            try
            {
                if (product != null)
                {
                    SqlDBParameter[] inputParams = new SqlDBParameter[8];
                    inputParams[0] = new SqlDBParameter("@USERID", SqlDbType.BigInt, product.UserId);
                    inputParams[1] = new SqlDBParameter("@CATEGORYID", SqlDbType.BigInt, product.CategoryId);
                    inputParams[2] = new SqlDBParameter("@PRODUCTCODE", SqlDbType.VarChar, 24, product.ProductCode);
                    inputParams[3] = new SqlDBParameter("@PRODUCTNAME", SqlDbType.NVarChar, 250, product.ProductName);
                    inputParams[4] = new SqlDBParameter("@FROMDATE", SqlDbType.DateTime, product.FromDate);
                    inputParams[5] = new SqlDBParameter("@TODATE", SqlDbType.DateTime, product.ToDate);
                    inputParams[6] = new SqlDBParameter("@SKIP", SqlDbType.Int, product.Skip);
                    inputParams[7] = new SqlDBParameter("@TASK", SqlDbType.Int, product.Task);

                    DataSet ds = new DataSet();
                    ds = DatabaseAccessNonEntity.SearchStoreProcedure("sp_Product_Search", inputParams);
                    DataTable dt = new DataTable();
                    var ls = new List<Product>();
                    dt = ds.Tables[1];
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            ls.Add(new Product
                            {
                                Pid = long.Parse(row["Pid"].ToString()),
                                ProductCode = row["ProductCode"].ToString(),
                                ProductName = row["ProductName"].ToString(),
                                Image = row["Image"].ToString(),
                                URL = row["URL"].ToString(),
                                MetaTitle = row["MetaTitle"].ToString(),
                                SeoTitle = row["SeoTitle"].ToString(),
                                MetaKeywords = row["MetaKeywords"].ToString(),
                                MetaDescription = row["MetaDescription"].ToString(),
                                Status = row["Status"].ToString() == string.Empty ? 0 : Int32.Parse(row["Status"].ToString()),
                                Description = row["Description"].ToString(),
                                CategoryId = row["CategoryId"].ToString() == string.Empty ? 0 : long.Parse(row["CategoryId"].ToString()),
                                Cost = row["Cost"].ToString() == string.Empty ? 0 : decimal.Parse(row["Cost"].ToString()),
                                Size = row["Size"].ToString(),
                                TypeCode = row["TypeCode"].ToString() == string.Empty ? 0 : long.Parse(row["TypeCode"].ToString()),
                                CreatedBy = row["CreatedBy"].ToString(),
                                CreatedDate = DateTime.Parse(row["CreatedDate"].ToString()),
                                Promotion = row["Promotion"].ToString() == string.Empty ? 0 : decimal.Parse(row["Promotion"].ToString()),
                                Price = row["Price"].ToString() == string.Empty ? 0 : decimal.Parse(row["Price"].ToString()),
                                DistributorId = row["DistributorId"].ToString() == string.Empty ? 0 : long.Parse(row["DistributorId"].ToString()),
                                Quantity = row["Quantity"].ToString() == string.Empty ? 0 : decimal.Parse(row["Quantity"].ToString()),
                                Producer = row["Producer"].ToString() == string.Empty ? 0: long.Parse(row["Producer"].ToString()),
                                ProductionDate = DateTime.Parse(row["ProductionDate"].ToString())
                            });
                        }
                        result.Data = ls;
                        result.TotalRows = ds.Tables[0].Rows[0].Field<int>(0);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public long InsertProduct(Product product,List<ProductDetail> detail, out string errMsg)
        {
            try
            {
                using (EntityLibContext entities = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    entities.Product.Add(product);
                    entities.SaveChanges();
                    detail = detail.Select(x => new ProductDetail() {
                        ProductId = product.Pid,
                        Color = x.Color,
                        Size = x.Size,
                        Width = x.Width,
                        Height = x.Height,
                        Quantity = x.Quantity,
                        Description = x.Description
                    }).ToList();
                    entities.ProductDetail.AddRange(detail);
                    entities.SaveChanges();
                    errMsg = "";
                    return product.Pid;
                }
            }
            catch(Exception ex)
            {
                errMsg = ex.Message;
                return 100;
            }
        }

        public SearchResult<IList<ProductView>> GetProductViewHome()
        {
            var result = new SearchResult<IList<ProductView>>();
            try
            {
                DataSet ds = new DataSet();
                ds = DatabaseAccessNonEntity.SearchStoreProcedure("sp_Product_showStartPage");
                DataTable dtProduct = new DataTable();
                DataTable dtProductDetail = new DataTable();
                List<Product> ls = new List<Product>();
                List<ProductView> listProduct = new List<ProductView>();
                List<ProductDetailView> listProductDetail = new List<ProductDetailView>();
                dtProduct = ds.Tables[0];
                dtProductDetail = ds.Tables[1];
                if (dtProduct.Rows.Count > 0)
                {
                    if (dtProductDetail.Rows.Count > 0)
                    {
                        foreach (DataRow row in dtProductDetail.Rows)
                        {
                            listProductDetail.Add(new ProductDetailView
                            {
                                Pid = long.Parse(row["Pid"].ToString()),
                                ProductId = long.Parse(row["ProductId"].ToString()),
                                Color = row["Color"].ToString(),
                                ColorValue = row["ColorValue"].ToString(),
                                Size = row["Size"].ToString(),
                                SizeValue = row["SizeValue"].ToString(),
                                Description = row["Description"].ToString(),
                                Height = row["Height"].ToString(),
                                Image = row["Image"].ToString(),
                                Quantity = Convert.ToDecimal(row["Quantity"].ToString()),
                                Width = row["Width"].ToString()
                            });
                        }
                    }
                    foreach (DataRow row in dtProduct.Rows)
                    {
                        listProduct.Add(new ProductView
                        {
                            Pid = long.Parse(row["Pid"].ToString()),
                            ProductCode = row["ProductCode"].ToString(),
                            ProductName = row["ProductName"].ToString(),
                            Image = row["Image"].ToString(),
                            URL = row["URL"].ToString(),
                            MetaTitle = row["MetaTitle"].ToString(),
                            SeoTitle = row["SeoTitle"].ToString(),
                            MetaKeywords = row["MetaKeywords"].ToString(),
                            MetaDescription = row["MetaDescription"].ToString(),
                            Status = row["Status"].ToString() == string.Empty ? 0 : Int32.Parse(row["Status"].ToString()),
                            Description = row["Description"].ToString(),
                            CategoryId = row["CategoryId"].ToString() == string.Empty ? 0 : long.Parse(row["CategoryId"].ToString()),
                            Cost = row["Cost"].ToString() == string.Empty ? 0 : decimal.Parse(row["Cost"].ToString()),
                            Size = row["Size"].ToString(),
                            TypeCode = row["TypeCode"].ToString() == string.Empty ? 0 : long.Parse(row["TypeCode"].ToString()),
                            CreatedBy = row["CreatedBy"].ToString(),
                            CreatedDate = DateTime.Parse(row["CreatedDate"].ToString()),
                            Promotion = row["Promotion"].ToString() == string.Empty ? 0 : decimal.Parse(row["Promotion"].ToString()),
                            Price = row["Price"].ToString() == string.Empty ? 0 : decimal.Parse(row["Price"].ToString()),
                            DistributorId = row["DistributorId"].ToString() == string.Empty ? 0 : long.Parse(row["DistributorId"].ToString()),
                            Quantity = row["Quantity"].ToString() == string.Empty ? 0 : decimal.Parse(row["Quantity"].ToString()),
                            //Producer = row["Producer"].ToString() == string.Empty ? 0 : long.Parse(row["Producer"].ToString()),
                            Producer = row["Producer"].ToString(),
                            ProductionDate = DateTime.Parse(row["ProductionDate"].ToString()),
                            ProductDetails = listProductDetail.Where(w => w.ProductId == long.Parse(row["Pid"].ToString())).ToList()
                        });
                    }
                    result.Data = listProduct;
                    result.TotalRows = ls.Count;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public SearchResult<Product> GetProductById(long productId)
        {
            SearchResult<Product> res = new SearchResult<Product>();
            try
            {
                using (EntityLibContext entities = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    Product p = new Product();
                    p = entities.Product.Where(w => w.Pid == productId).FirstOrDefault();
                    if (p != null)
                    {
                        res.Data = p;
                        res.Success = true;
                        res.Message = "lấy thông tin chi tiết sản phẩm thành công";
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
                res.Message = "Lỗi lấy thông tin chi tiết sản phẩm: "+ex.Message;
            }
            return res;
        }

        public PostResult<Product> UpdateProduct(Product product)
        {
            PostResult<Product> resutl = new PostResult<Product>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    Product pr = entity.Product.Where(w => w.Pid == product.Pid).FirstOrDefault();
                    if (pr != null)
                    {
                        pr.ProductName = product.ProductName;
                        if (!string.IsNullOrEmpty(product.Image))
                        {
                            pr.Image = product.Image;
                        }
                        pr.URL = product.URL;
                        pr.MetaTitle = product.MetaTitle;
                        pr.SeoTitle = product.SeoTitle;
                        pr.MetaKeywords = product.MetaKeywords;
                        pr.MetaDescription = product.MetaDescription;
                        pr.Status = product.Status;
                        pr.Description = product.Description;
                        pr.CategoryId = product.CategoryId;
                        pr.Cost = product.Cost;
                        pr.Size = product.Size;
                        pr.TypeCode = product.TypeCode;
                        pr.Promotion = product.Promotion;
                        pr.Price = product.Price;
                        pr.DistributorId = product.DistributorId;
                        pr.Quantity = product.Quantity;
                        pr.Producer = product.Producer;
                        pr.ProductionDate = product.ProductionDate;
                        pr.IsNew = product.IsNew;
                        entity.SaveChanges();
                        resutl.Success = true;
                        resutl.ErrorCode = "0";
                        resutl.Message = "Cập nhật role thành công";
                    }
                    else
                    {
                        resutl.Success = false;
                        resutl.ErrorCode = "-1";
                        resutl.Message = "Không tìm thấy dữ liệu cập nhật";
                    }
                    resutl.Data = product;

                }
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.ErrorCode = "100";
                resutl.Message = "Cập nhật sản phẩm lỗi: " + ex.Message;
            }
            return resutl;
        }

        public PostResult<Product> DeleteProduct(long productId)
        {
            PostResult<Product> res = new PostResult<Product>();
            try
            {
                using (EntityLibContext entities = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    Product p = new Product();
                    p = entities.Product.Where(w => w.Pid == productId).FirstOrDefault();
                    if (p != null)
                    {
                        entities.Product.Remove(p);
                        entities.SaveChanges();
                        List<ProductDetail> detail = entities.ProductDetail.Where(w => w.ProductId == p.Pid).ToList();
                        if (detail != null)
                        {
                            entities.ProductDetail.RemoveRange(detail);
                            entities.SaveChanges();
                        }
                        res.Data = p;
                        res.Success = true;
                        res.Message = "Xóa sản phẩm thành công";
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
                res.Message = "Lỗi xóa thông tin sản phẩm: " + ex.Message;
            }
            return res;
        }

        public SearchResult<IList<ProductView>> GetProductByCategory(CategorySearchEntity category)
        {
            SearchResult<IList<ProductView>> result = new SearchResult<IList<ProductView>>();
            try
            {
                if (category != null)
                {
                    if (category.CategoryCode == null)
                    {
                        category.CategoryCode = string.Empty;
                    }
                    //using (EntityLibContext entities = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                    //{
                    //    List<Product> data = (from p in entities.Product 
                    //                          join d in entities.ProductDetail on p.Pid equals d.ProductId
                    //                          //join 
                    //                          where p.CategoryId != null
                    //                          && p.CategoryId == category.CategoryId
                    //                          select new Product {
                    //                              Pid = p.Pid,

                    //                          }).ToList();
                    //    if (data != null && data.Any())
                    //    {
                    //        IEnumerable<Product> sData = data.CustomSort(category.SortName, category.IsDesc);
                    //        result.Data = sData.Skip((category.PageIndex -1) * category.RowNumber).Take(category.RowNumber).ToList();
                    //        result.TotalRows = sData.ToList().Count();
                    //    }
                    //}
                    DataSet ds = new DataSet();
                    SqlDBParameter[] inputParams = new SqlDBParameter[3];
                    inputParams[0] = new SqlDBParameter("@CATEGORYID", SqlDbType.BigInt, category.CategoryId);
                    inputParams[1] = new SqlDBParameter("@SKIP", SqlDbType.Int, category.Skip);
                    inputParams[2] = new SqlDBParameter("@TASK", SqlDbType.Int, category.Task);
                    ds = DatabaseAccessNonEntity.SearchStoreProcedure("sp_GetProductsByCategoryId", inputParams);
                    DataTable dtProduct = new DataTable();
                    DataTable dtProductDetail = new DataTable();
                    List<Product> ls = new List<Product>();
                    List<ProductView> listProduct = new List<ProductView>();
                    List<ProductDetailView> listProductDetail = new List<ProductDetailView>();
                    dtProduct = ds.Tables[0];
                    dtProductDetail = ds.Tables[1];
                    if (dtProduct.Rows.Count > 0)
                    {
                        if (dtProductDetail.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtProductDetail.Rows)
                            {
                                listProductDetail.Add(new ProductDetailView
                                {
                                    Pid = long.Parse(row["Pid"].ToString()),
                                    ProductId = long.Parse(row["ProductId"].ToString()),
                                    Color = row["Color"].ToString(),
                                    ColorValue = row["ColorValue"].ToString(),
                                    Size = row["Size"].ToString(),
                                    SizeValue = row["SizeValue"].ToString(),
                                    Description = row["Description"].ToString(),
                                    Height = row["Height"].ToString(),
                                    Image = row["Image"].ToString(),
                                    Quantity = Convert.ToDecimal(row["Quantity"].ToString()),
                                    Width = row["Width"].ToString()
                                });
                            }
                        }
                        foreach (DataRow row in dtProduct.Rows)
                        {
                            listProduct.Add(new ProductView
                            {
                                Pid = long.Parse(row["Pid"].ToString()),
                                ProductCode = row["ProductCode"].ToString(),
                                ProductName = row["ProductName"].ToString(),
                                Image = row["Image"].ToString(),
                                URL = row["URL"].ToString(),
                                MetaTitle = row["MetaTitle"].ToString(),
                                SeoTitle = row["SeoTitle"].ToString(),
                                MetaKeywords = row["MetaKeywords"].ToString(),
                                MetaDescription = row["MetaDescription"].ToString(),
                                Status = row["Status"].ToString() == string.Empty ? 0 : Int32.Parse(row["Status"].ToString()),
                                Description = row["Description"].ToString(),
                                CategoryId = row["CategoryId"].ToString() == string.Empty ? 0 : long.Parse(row["CategoryId"].ToString()),
                                Cost = row["Cost"].ToString() == string.Empty ? 0 : decimal.Parse(row["Cost"].ToString()),
                                Size = row["Size"].ToString(),
                                TypeCode = row["TypeCode"].ToString() == string.Empty ? 0 : long.Parse(row["TypeCode"].ToString()),
                                CreatedBy = row["CreatedBy"].ToString(),
                                CreatedDate = DateTime.Parse(row["CreatedDate"].ToString()),
                                Promotion = row["Promotion"].ToString() == string.Empty ? 0 : decimal.Parse(row["Promotion"].ToString()),
                                Price = row["Price"].ToString() == string.Empty ? 0 : decimal.Parse(row["Price"].ToString()),
                                DistributorId = row["DistributorId"].ToString() == string.Empty ? 0 : long.Parse(row["DistributorId"].ToString()),
                                Quantity = row["Quantity"].ToString() == string.Empty ? 0 : decimal.Parse(row["Quantity"].ToString()),
                                //Producer = row["Producer"].ToString() == string.Empty ? 0 : long.Parse(row["Producer"].ToString()),
                                Producer = row["Producer"].ToString(),
                                ProductionDate = DateTime.Parse(row["ProductionDate"].ToString()),
                                ProductDetails = listProductDetail.Where(w => w.ProductId == long.Parse(row["Pid"].ToString())).ToList()
                            });
                        }
                        result.Data = listProduct;
                        result.TotalRows = ls.Count;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public SearchResult<IList<PropertyDetail>> GetPropertyDetailByPropertyId(long propertyId)
        {
            SearchResult<IList<PropertyDetail>> result = new SearchResult<IList<PropertyDetail>>();
            try
            {
                using (EntityLibContext entities = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    result.Data = entities.PropertyDetail.Where(w => w.PropertyID == propertyId).ToList();
                    if (result.Data != null)
                    {
                        result.Success = true;
                        result.Message = "Lấy thông tin propertyDetail thành công";
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "Data not found!";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Lấy thông tin propertyDetail lỗi: " + ex.Message;
            }
            return result;
        }

        public SearchResult<IList<Property>> GetPropertyOfCategory(long cagetoryId)
        {
            SearchResult<IList<Property>> result = new SearchResult<IList<Property>>();
            try
            {
                using (EntityLibContext entities = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    List<Property> listProperty = (from c in entities.TBCategory_TBProperty
                              join p in entities.Property on c.TBPropertyId equals p.Id
                              where c.TBCategoryId == cagetoryId
                              select p).ToList();
                    if (listProperty != null && listProperty.Any())
                    {
                        result.Data = listProperty;
                        result.Success = true;
                        result.Message = "Lấy thông tin PropertyOfCategory thành công";
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = "Data not found!";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Lấy thông tin PropertyOfCategory lỗi: " + ex.Message;
            }
            return result;
        }

        public SearchResult<IList<ProductDetail>> GetProductDetailByProductId(long productId)
        {
            SearchResult<IList<ProductDetail>> result = new SearchResult<IList<ProductDetail>>();
            try
            {
                using (EntityLibContext entities = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    List<ProductDetail> list = entities.ProductDetail.Where(w => w.ProductId == productId).ToList();
                    result.Data = list;
                    result.Success = true;
                    result.Message = "Lấy thông tin chi tiết sản phẩm thành công";
                }
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.Message = "Lấy thông tin chi tiết sản phẩm lỗi: " + ex.Message;
            }
            return result;
        }

        public SearchResult<ProductView> GetProductView(long productId)
        {
            SearchResult<ProductView> result = new SearchResult<ProductView>();
            try
            {
                using (EntityLibContext entities = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    ProductView product = new ProductView();
                    Product pro = entities.Product.Where(w => w.Pid == productId).FirstOrDefault();
                    if (pro != null)
                    {
                        product.Pid = pro.Pid;
                        product.ProductCode = pro.ProductCode;
                        product.ProductName = pro.ProductName;
                        product.Image = pro.Image;
                        product.URL = pro.URL;
                        product.MetaTitle = pro.MetaTitle;
                        product.SeoTitle = pro.SeoTitle;
                        product.MetaKeywords = pro.MetaKeywords;
                        product.MetaDescription = pro.MetaDescription;
                        product.Status = Convert.ToInt32(pro.Status);
                        product.Description = pro.Description;
                        product.CategoryId = long.Parse(pro.CategoryId.ToString());
                        product.Cost = Convert.ToDecimal(pro.Cost);
                        product.TypeCode = long.Parse(pro.TypeCode.ToString());
                        product.Promotion = Convert.ToDecimal(pro.Promotion);
                        product.Price = Convert.ToDecimal(pro.Price);
                        product.DistributorId = long.Parse(pro.DistributorId.ToString());
                        product.Quantity = long.Parse(pro.DistributorId.ToString());
                        product.Producer = pro.Producer.ToString();
                        DateTime.TryParse(pro.ProductionDate.ToString(), out DateTime ProductionDate);
                        product.ProductionDate = ProductionDate;
                        product.IsNew = pro.IsNew;
                        product.Video = pro.Video;
                        product.ProductDetails = (from c in entities.ProductDetail
                                                  join p in entities.PropertyDetail on c.Size equals p.Id.ToString()
                                                  join p1 in entities.PropertyDetail on c.Color equals p1.Id.ToString()
                                                  where c.ProductId == product.Pid
                                                  select new ProductDetailView
                                                  {
                                                      Pid = c.Pid,
                                                      ProductId = c.ProductId,
                                                      Quantity = c.Quantity,
                                                      Size = p.Id.ToString(),
                                                      SizeValue = p.Vavule,
                                                      Color = p1.Id.ToString(),
                                                      ColorValue = p1.Vavule,
                                                      Image = c.Image,
                                                      Width = c.Width,
                                                      Height = c.Height,
                                                      Description = c.Description
                                                  }).ToList();
                        product.Distributor = (from d in entities.Distributor
                                               where d.Pid == product.DistributorId
                                               select new DistributorView
                                               {
                                                   Pid = d.Pid,
                                                   Address = d.Address,
                                                   BusinessLicense = d.BusinessLicense,
                                                   DistributorCode = d.DistributorCode,
                                                   Name = d.Name,
                                                   PhoneNumber = d.PhoneNumber
                                               }).FirstOrDefault();
                    }
                    //List<ProductDetail> list = entities.ProductDetail.Where(w => w.ProductId == productId).ToList();
                    result.Data = product;
                    result.Success = true;
                    result.Message = "Lấy thông tin chi tiết sản phẩm thành công";
                }
            }
            catch (Exception ex)
            {
                result.Data = new ProductView();
                result.Success = false;
                result.Message = "Lấy thông tin chi tiết sản phẩm lỗi: " + ex.Message;
            }
            return result;
        }
    }
}
