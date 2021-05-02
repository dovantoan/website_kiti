using EntityLibrary;
using EntityLibrary.ModuleImplement;
using EntityLibrary.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using titi.Areas.Admin.Models;
using titi.Areas.Admin.SearchCriteria;
using titi.Helper;
using titi.Models;
using titi.Models.MasterModels;
using titi.Models.Security;
using Util;
using Util.Entity;

namespace choquekimthuy.com.Controllers.Products
{
    public class ProductsController : Controller
    {
        private readonly Util.Util util = new Util.Util();
        //private readonly MasterService masSV = new MasterService();
        private readonly MasterService masSV = new MasterService();

        //public ProductsController(IMasterService _masSV)
        //{
        //    masSV = _masSV;
        //}
        // GET: Products
        public ActionResult ProductDetails(long productId)
        {
            ProductService productsv = new ProductService();
            SearchResult<EntityLibrary.ModuleImplement.ProductView> res = productsv.GetProductView(productId);
            List<PropertyOfProduct> listProperty = new List<PropertyOfProduct>();
            if (res.Data != null && res.Data.ProductDetails.Count > 0)
            {
                //var listDetail = res.Data.ProductDetails;
                //for (int i = 0; i<listDetail.Count; i++)
                //{
                //    PropertyOfProduct pro = new PropertyOfProduct
                //    {
                //        SizeId = listDetail[i].Size,
                //        SizeValue = listDetail[i].SizeValue
                //    };
                //    var col = (from c in res.Data.ProductDetails where c.Size.Equals(listDetail[i].Size) select new ColorOfProduct { ColorId = c.Color, ColorValue = c.ColorValue }).ToList();
                //    pro.Colors = col;
                //    listProperty.Add(pro);
                //    listDetail.RemoveAll(listDetail.Where(w => w.Size.Equals(pro.SizeId)).ToList());
                //}
                foreach (var item in res.Data.ProductDetails)
                {
                    if (listProperty.Where(w => w.ColorId.Equals(item.Color)).FirstOrDefault() == null)
                    {
                        PropertyOfProduct pro = new PropertyOfProduct
                        {
                            ColorId = item.Color,
                            ColorValue = item.ColorValue
                        };
                        var siz = (from c in res.Data.ProductDetails where c.Color.Equals(item.Color) select new SizeOfProduct { SizeId = c.Size, SizeValue = c.SizeValue }).ToList();
                        pro.ListSize = siz;
                        listProperty.Add(pro);
                    }
                }
                ViewBag.ListSize = listProperty;
            }
            return View(res.Data);
        }
        public ActionResult CartDetails()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ListProduct()
        {
            try
            {
                CategoryService cate = new CategoryService();
                IList<titi.Models.CategoryModel> listCa = new List<titi.Models.CategoryModel>();
                var listCate = cate.GetAllCategory(out int errorCode, out string errorMsg);
                listCa = util.TransformList<Category, titi.Models.CategoryModel>(listCate);
                ViewBag.ListCategory = listCa.ToList();
                ProductService productsv = new ProductService();
                SearchResult<IList<EntityLibrary.ModuleImplement.ProductView>> data = productsv.GetProductViewHome();
                JsonSearchResultCriteria<IList<EntityLibrary.ModuleImplement.ProductView>> result = new JsonSearchResultCriteria<IList<EntityLibrary.ModuleImplement.ProductView>>();
                if (data.Data != null)
                {
                    result.Data = data.Data;
                    result.TotalRows = data.TotalRows;
                    result.Success = true;
                    List<EntityLibrary.ModuleImplement.ProductView> listProd = result.Data?.ToList();
                    ViewBag.ListProduct = listProd;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return PartialView("Partial_ListProduct");
        }

        public ActionResult CheckOut()
        {
            UserLoginProfiles user =  SharedContext.Current.UserLoginProfiles;
            if (user?.UserProfile != null)
            {
                return RedirectToAction("Shipping", "Products");
            }
            return View();
        }
        //[NonAction]
        //public SelectList ToSelectList(List<object> list1, string valueField, string textField)
        //{
        //    List<SelectListItem> list = new List<SelectListItem>();

        //    foreach (DataRow row in table.Rows)
        //    {
        //        list.Add(new SelectListItem()
        //        {
        //            Text = row[textField].ToString(),
        //            Value = row[valueField].ToString()
        //        });
        //    }

        //    return new SelectList(list, "Value", "Text");
        //}
        [HttpGet]
        public ActionResult Shipping()
        {
            UserLoginProfiles user = SharedContext.Current.UserLoginProfiles;
            SearchResult<IList<Province>> data = masSV.GetAllProvince();
            IList<ProvinceModels> lsProvince = util.TransformList<Province, ProvinceModels>(data.Data);
            List<SelectListItem> listProv = new List<SelectListItem>();
            foreach (var item in lsProvince)
            {
                listProv.Add(new SelectListItem()
                {
                    Text = item.Province1,
                    Value = item.ProvinceID.ToString()
                });
            }
            ViewBag.Prov = new SelectList(listProv, "Value", "Text");
            //ViewBag.ListProvince = lsProvince.ToList();
            if (user?.UserProfile == null)
            {
                return RedirectToAction("CheckOut", "Products");
            }
            ShippingEntity shiping = new ShippingEntity();
            SearchResult<ShippingEntity> res = masSV.GetShippingByUserID(user.UserProfile.Pid);
            if (res.Success)
            {
                //shiping = util.Transform<Shipping, ShippingModels>(res.Data);
                shiping = res.Data;
                ViewBag.Shipping = shiping;
                if (res.Data != null)
                {
                    SearchResult<IList<District>> DataDistrict = masSV.GetDistrictByProvinceId(long.Parse(shiping.ProvinceID.ToString()));
                    IList<DistrictModels> lsDistrict = util.TransformList<District, DistrictModels>(DataDistrict.Data);
                    if (lsDistrict != null)
                    {
                        List<SelectListItem> listDis = new List<SelectListItem>();
                        foreach (var item in lsDistrict)
                        {
                            listDis.Add(new SelectListItem()
                            {
                                Text = item.District1,
                                Value = item.DistrictID.ToString()
                            });
                        }
                        ViewBag.Dis = new SelectList(listDis, "Value", "Text");
                    }
                    SearchResult<IList<Ward>> dataWard = masSV.GetWardByDistrictId(long.Parse(shiping.DistrictID.ToString()));
                    IList<WardModels> lsWard = util.TransformList<Ward, WardModels>(dataWard.Data);
                    if (lsWard != null)
                    {
                        List<SelectListItem> listWard = new List<SelectListItem>();
                        foreach (var item in lsWard)
                        {
                            listWard.Add(new SelectListItem()
                            {
                                Text = item.Ward1,
                                Value = item.WardID.ToString()
                            });
                        }
                        ViewBag.Ward = new SelectList(listWard, "Value", "Text");
                    }
                }
            }
            
            return View(shiping);
        }

        public ActionResult AjaxGetDistrict(long provinceID)
        {
            SearchResult<IList<District>> data = masSV.GetDistrictByProvinceId(provinceID);
            IList<DistrictModels> lsDistrict = util.TransformList<District, DistrictModels>(data.Data);
            string res = "<option value=0>----- chọn Quận/Huyện ----</option>";
            if (lsDistrict != null)
            {
                foreach(var item in lsDistrict)
                {
                    res += "<option value='" + item.DistrictID + "'>" + item.District1 + "</option>";
                }
                return Json(new
                {
                    Success = true,
                    Message = "",
                    Data = res
                });
            }
            return Json(new
            {
                Success = false,
                Message = "Data not found"
            });
        }

        public ActionResult AjaxGetDWard(long districtID)
        {
            Util.SearchResult<IList<Ward>> data = masSV.GetWardByDistrictId(districtID);
            IList<WardModels> lsWard = util.TransformList<Ward, WardModels>(data.Data);
            string res = "<option value=0>----- chọn phường/xã ----</option>";
            if (lsWard != null)
            {
                foreach (var item in lsWard)
                {
                    res += "<option value='" + item.WardID + "'>" + item.Ward1 + "</option>";
                }
                return Json(new
                {
                    Success = true,
                    Message = "",
                    Data = res
                });
            }
            return Json(new
            {
                Success = false,
                Message = "Data not found"
            });
        }

        [HttpPost]
        public JsonResult AjaxInsertShipping(ShippingModels shipping)
        {
            UserLoginProfiles user = SharedContext.Current.UserLoginProfiles;
            shipping.UserID = user.UserProfile.Pid;
            shipping.CreatedDate = DateTime.Now;
            Shipping _shipping = util.Transform<ShippingModels, Shipping>(shipping);
            PostResult<Shipping> data = masSV.InserShipping(_shipping);
            return Json(new
            {
                data.Success,
                data.Message,
                Data = util.Transform<Shipping, ShippingModels>(data.Data)
            });
        }

        public ActionResult Payment()
        {
            UserLoginProfiles user = SharedContext.Current.UserLoginProfiles;
            if (user != null)
            {
                Util.SearchResult<ShippingEntity> res = masSV.GetShippingByUserID(user.UserProfile.Pid);
                if (res.Success)
                {
                    //ShippingModels shiping = util.Transform<Shipping, ShippingModels>(res.Data);
                    ShippingEntity shiping = res.Data;
                    ViewBag.Shipping = shiping;
                }
            }
            if (user?.UserProfile == null)
            {
                return RedirectToAction("CheckOut", "Products");
            }
            return View();
        }

        [HttpPost]
        public JsonResult AjaxUpdateShipping(ShippingModels shipping)
        {
            UserLoginProfiles user = SharedContext.Current.UserLoginProfiles;
            shipping.UserID = user.UserProfile.Pid;
            shipping.CreatedDate = DateTime.Now;
            Shipping _shipping = util.Transform<ShippingModels, Shipping>(shipping);
            PostResult<Shipping> data = masSV.UpdateShipping(_shipping);
            return Json(new
            {
                data.Success,
                data.Message,
                Data = util.Transform<Shipping, ShippingModels>(data.Data)
            });
        }
    }
}