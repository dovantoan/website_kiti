
using EntityLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titi.Areas.Admin.Models;
using Util;

namespace titi.Areas.Admin.Controllers.Category
{
    public class CategoryController : AdminController
    {
        private readonly Util.Util util = new Util.Util();
        private readonly CategoryService catesv = new CategoryService();
        // GET: Category
        public ActionResult Index()
        {
            IList<CategoryModel> listCate = new List<CategoryModel>();
            listCate = util.TransformList<EntityLibrary.Category, CategoryModel>(catesv.GetAllCategory(out int errorCode, out string errorMsg));
            ViewBag.ListCate = listCate.ToList();
            return View();
        }
        public ActionResult SearchCategory()
        {
            return PartialView();
        }

        public ActionResult AddNewCategory()
        {
            List<Models.CategoryModel> listCategory = new List<Models.CategoryModel>();
            IList<EntityLibrary.Category> result = catesv.GetAllCategory(out int errorCode, out string errorMsg);
            listCategory = util.TransformList<EntityLibrary.Category, CategoryModel>(result).ToList();
            ViewBag.listCategory = listCategory.Where(w => w.ParentPid == 0).ToList();
            return PartialView("~/Areas/Admin/Views/Category/AddNewCategory.cshtml",new CategoryModel());
        }
        public ActionResult CategoryDetail(long categoryId)
        {
            SearchResult<EntityLibrary.Category> data = catesv.GetCategoryById(categoryId);
            CategoryModel cate = util.Transform<EntityLibrary.Category, CategoryModel>(data.Data);
            List<CategoryModel> listCategory = new List<CategoryModel>();
            IList<EntityLibrary.Category> result = catesv.GetAllCategory(out int errorCode, out string errorMsg);
            listCategory = util.TransformList<EntityLibrary.Category, CategoryModel>(result).ToList();
            ViewBag.listCategory = listCategory.Where(w => w.ParentPid == 0).ToList();
            return PartialView("~/Areas/Admin/Views/Category/CategoryDetail.cshtml", cate);
        }
        [HttpPost]
        public JsonResult UpdateCategory(CategoryModel category)
        {
            PostResult<EntityLibrary.Category> resut = catesv.UpdateCategory(util.Transform<CategoryModel, EntityLibrary.Category>(category));
            return Json(new {
                resut.Success,
                resut.Message
            });
        }
        [HttpPost]
        public JsonResult InsertCategory(CategoryModel category)
        {
            PostResult<EntityLibrary.Category> resut = catesv.InsertCategory(util.Transform<CategoryModel, EntityLibrary.Category>(category));
            return Json(new
            {
                resut.Success,
                resut.Message
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteCategory(long id)
        {
            int res = catesv.DeleteCategory(id, out int errorCode, out string errorMsg);
            return Json(new
            {
                Success = res == 0,
                Message = errorMsg
            });
        }
    }
}