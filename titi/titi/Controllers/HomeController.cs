using titi.Models;
using System;
using System.Collections.Generic;
using EntityLibrary.ModuleImplement;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using EntityLibrary;
using System.Linq;
using Util;
using Util.SearchEntity;
using System.IO;
using NPOI.HSSF.UserModel;

namespace titi.Controllers
{
    public class HomeController : Controller
    {
        private Util.Util util = new Util.Util();
        private readonly ProductService prodsv = new ProductService();
        private readonly CategoryService catesv = new CategoryService();
        public HomeController()
        {
            CategoryService cate = new CategoryService();
            IList<CategoryModel> result = new List<CategoryModel>();
            var listCate = cate.GetAllCategory(out int errorCode, out string errorMsg);
            result = util.TransformList<Category, CategoryModel>(listCate);
            System.Web.HttpContext.Current.Session["Category"] = result;
        }
        
        public ActionResult Index()
        {
            //IList<CategoryModel> listCategory = new List<CategoryModel>();
            //listCategory = Session["Category"] as List<CategoryModel>;
            //ViewBag.ListCategory = listCategory.ToList();
            //ProductService productsv = new ProductService();
            //Util.SearchResult<IList<Product>> data = productsv.GetProductViewHome();
            //JsonSearchResultCriteria<IList<ProductModel>> result = new JsonSearchResultCriteria<IList<ProductModel>>();
            //if (data != null)
            //{
            //    result.Data = util.TransformList<Product, ProductModel>(data.Data);
            //    result.TotalRows = data.TotalRows;
            //    result.Success = true;
            //}
            //List<ProductModel> listProd = result.Data.ToList();
            //ViewBag.ListProduct = listProd;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
        public ActionResult Change(string languageAbbrevation)
        {
            if (languageAbbrevation != null)
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(languageAbbrevation);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageAbbrevation);
            }
            HttpCookie cookie = new HttpCookie("Language")
            {
                Value = languageAbbrevation
            };
            Response.Cookies.Add(cookie);
            string filePath = AppDomain.CurrentDomain.BaseDirectory.ToString();
            
            string pathResourceFile;

            if (cookie.Value == Shared.Utility.ConstantClass.defaultLang)
            {
                pathResourceFile = @"Language\language.resx";
            }
            else
            {
                pathResourceFile = @"Language\language." + cookie.Value + ".resx";
            }
            return Redirect(Request.UrlReferrer.AbsoluteUri.ToString());
        }

        public ActionResult GetCategory()
        {
            CategoryService cate = new CategoryService();
            int errorCode = 0;
            string errorMsg = "";
            IList<CategoryModel> result = new List<CategoryModel>();
            if (Session["Category"] == null)
            {
                var listCate = cate.GetAllCategory(out errorCode, out errorMsg);
                result = util.TransformList<Category, CategoryModel>(listCate);
                Session["Category"] = result;
            }
            else
            {
                result = Session["Category"] as List<CategoryModel>;
            }
            ViewBag.listCategory = result;
            return PartialView("_MenuLeftPartial");
        }

        public ActionResult CategoryDetailts(long categotyId, int pageIndex = 1, int rowNumber=20)
        {
            SearchResult<Category> reCate = new SearchResult<Category>();
            reCate = catesv.GetCategoryById(categotyId);
            CategoryModel cate = new CategoryModel();
            if (reCate.Success)
            {
                cate = util.Transform<Category,CategoryModel>(reCate.Data);
            }
            ViewBag.CateDetail = cate;
            SearchResult<IList<EntityLibrary.ModuleImplement.ProductView>> rs = new SearchResult<IList<EntityLibrary.ModuleImplement.ProductView>>();
            CategorySearchEntity searchEntity = new CategorySearchEntity
            {
                CategoryId = categotyId,
                PageIndex = pageIndex - 1,
                RowNumber = rowNumber
            };
            rs = prodsv.GetProductByCategory(searchEntity);
            List<EntityLibrary.ModuleImplement.ProductView> list = rs.Data?.ToList();
            ViewBag.ListProduct = list;
            return View();
        }

        public ActionResult NPOICreate()
        {
            try
            {
                // Opening the Excel template...
                FileStream fs =
                    new FileStream(Server.MapPath(@"\TemplateReport\Template.xls"), FileMode.Open, FileAccess.Read);

                // Getting the complete workbook...
                HSSFWorkbook templateWorkbook = new HSSFWorkbook(fs, true);

                // Getting the worksheet by its name...
                //HSSFSheet sheet = templateWorkbook.GetSheet("Sheet1");
                HSSFSheet sheet = (HSSFSheet)templateWorkbook.GetSheet("Sheet1");

                // Getting the row... 0 is the first row.
                HSSFRow dataRow = (HSSFRow)sheet.GetRow(2);

                // Setting the value 77 at row 5 column 1
                //dataRow.GetCell(2).SetCellValue("dsdsd");
                HSSFCell cell1 = (HSSFCell)dataRow.CreateCell(2);
                cell1.SetCellValue("Header 1");
                // Forcing formula recalculation...
                sheet.ForceFormulaRecalculation = true;

                MemoryStream ms = new MemoryStream();

                // Writing the workbook content to the FileStream...
                templateWorkbook.Write(ms);

                TempData["Message"] = "Excel report created successfully!";

                // Sending the server processed data back to the user computer...
                return File(ms.ToArray(), "application/vnd.ms-excel", "NPOINewFile.xlsx");
            }
            catch (Exception ex)
            {
                TempData["Message"] = "Oops! Something went wrong.";

                return RedirectToAction("NPOI");
            }
        }
    }
}