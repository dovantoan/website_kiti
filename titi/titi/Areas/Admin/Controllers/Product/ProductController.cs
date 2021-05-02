using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using titi.App_Start;
using titi.Models;
using titi.Areas.Admin.Models;
using titi.Areas.Admin.SearchCriteria;
using EntityLibrary;
using Util.SearchEntity;
using System.Web;
using System.IO;
using ExifLib;
using titi.Helper;
using Util;

using Google.YouTube;
using Google.GData.YouTube;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Extensions.MediaRss;
using System.Threading.Tasks;

namespace titi.Areas.Admin.Controllers.Product
{
    public class ProductController : AdminController
    {
        private readonly Util.Util util = new Util.Util();
        private readonly DistributorService distributorsv = new DistributorService();
        private readonly ProductService productsv = new ProductService();
        private readonly CategoryService categorysv = new CategoryService();
        //HttpClient _client;
        //string webapiurl = UriTemplate.PRODUCT_SEARCH;
        // GET: Product

        public ActionResult test()
        {
            double Number = 1234586.01;
            string str = Shared.Utility.FunctionUtility.ConvertToWords(Convert.ToDouble(Number).ToString());
            return View();
        }
        public ActionResult Index()
        {
            bool isInsert = false;
            //List<RoleViewModel> listRolesOfUser = Session["RolesOfUser"] as List<RoleViewModel>;
            List<RoleViewModel> listRolesOfUser = SharedContext.Current.AdminLogdedProfile.Roles;
            if (listRolesOfUser != null)
            {
                if (listRolesOfUser.Where(w => w.RoleName == "addnewproduct").FirstOrDefault() != null)
                    isInsert = true;
            }
            ViewBag.isInsert = isInsert;
            return View();
        }
        [AllowAnonymous]
        public ActionResult SearchProduct(ProductSearchCriteria searchEntity)
        {
            ProductService productsv = new ProductService();
            var result = new JsonSearchResultCriteria<IList<Models.ProductModel>>();

            try
            {
                if (searchEntity != null)
                {
                    searchEntity.UserId = SharedContext.Current.AdminLogdedProfile.UserProfile.Pid;
                    Util.SearchResult<IList<EntityLibrary.Product>> data = productsv.SearchProduct(util.Transform<ProductSearchCriteria, ProductSearchEntity>(searchEntity));
                    if (data != null)
                    {
                        result.Data = util.TransformList<EntityLibrary.Product, Models.ProductModel>(data.Data);
                        result.TotalRows = data.TotalRows;
                        result.Success = true;
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception ex)
            {
                result.ExceptionMessage = ex.Message;
            }
            return PartialView("List");
        }

        [HttpGet]
        public ActionResult AddNewProduct()
        {
            return PartialView("AddNew");
        }

        [AuthorizeCustom("addnewproduct")]
        [HttpGet]
        public ActionResult InsertProduct()
        {
            CategoryService categorysv = new CategoryService();
            ProducerService producersv = new ProducerService();
            List<Models.CategoryModel> listCategory = new List<Models.CategoryModel>();
            var result = categorysv.GetAllCategory(out int errorCode, out string errorMsg);
            listCategory = util.TransformList<EntityLibrary.Category, Models.CategoryModel>(result).ToList();
            ViewBag.listProducer = util.TransformList<Producer, ProducerModel>(producersv.GetAllProducer(out errorCode, out string errMsg));
            ViewBag.listCategory = listCategory;

            List<DistributorModels> listDis = new List<DistributorModels>();
            Util.SearchResult<IList<Distributor>> data = distributorsv.GetAllDistributor();
            listDis = util.TransformList<Distributor, DistributorModels>(data.Data).ToList();
            ViewBag.listDistri = listDis;
            ViewBag.productCode = Util.Util.RandomString(10, false);
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult SaveToTemp(HttpPostedFileBase[] files)
        {
            try
            {
                string filename = "";
                string _filename = "";
                string imgepath = "Null";
                foreach (HttpPostedFileBase file in files)
                {
                    if (file != null)
                    {
                        var InputFileName = Path.GetFileName(file.FileName);
                        imgepath = InputFileName;
                        filename = DateTime.Now.Ticks + InputFileName;
                        _filename += filename + ";";
                        var path = Path.Combine(Server.MapPath("~/Areas/Admin/image/Temp/"), filename);
                        file.SaveAs(path);
                    }
                }
                return Json(_filename, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public JsonResult RemoveFileTemp(string fileUpload)
        {
            try
            {
                string sourcePath = Server.MapPath("~/Areas/Admin/image/Temp/");
                string[] files = System.IO.Directory.GetFiles(sourcePath);
                foreach (string file in files)
                {
                    if (System.IO.File.Exists(System.IO.Path.Combine(sourcePath, file)))
                    {
                        try
                        {
                            System.IO.File.Delete(file);
                        }
                        catch (System.IO.IOException e)
                        {
                            
                        }
                    }
                }
                return Json(new { Success = true, Msg = "Success" });
            }
            catch (Exception ex)
            {
                return Json(new { Success = false, Msg = ex.Message });
            }
        }

        public void RemoveFiles()
        {
            string sourcePath = Server.MapPath("~/Areas/Admin/image/Temp/");
            string[] files = System.IO.Directory.GetFiles(sourcePath);
            foreach (string file in files)
            {
                if (System.IO.File.Exists(System.IO.Path.Combine(sourcePath, file)))
                {
                    try
                    {
                        System.IO.File.Delete(file);
                    }
                    catch (System.IO.IOException e)
                    {
                        return;
                    }
                }
            }
        }
        public JsonResult SaveToMainFolder()
        {
            string fileName = "";
            string destFile = "";
            string sourcePath = Server.MapPath("~/Areas/Admin/image/Temp/");
            string targetPath = Server.MapPath("~/[Your Destination Folder Name]/");
            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);
                // Copy the files. 
                foreach (string file in files)
                {
                    fileName = Path.GetFileName(file);
                    destFile = Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(file, destFile, true);
                }
                RemoveFiles();
            }
            return Json("All Files saved Successfully.", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult InsertProduct(FormCollection frm)
        {
            try
            {
                string productModel = frm["Product"];
                Models.ProductModel model = JsonConvert.DeserializeObject<Models.ProductModel>(productModel);
                string productDetail = frm["ProductDetail"];
                List<Models.ProductDetailModel> detail = JsonConvert.DeserializeObject<List<Models.ProductDetailModel>>(productDetail);
                string errMsg = "";
                bool success = true;
                if (Request.Files.Count > 0 && model != null)
                {
                    HttpPostedFileBase[] attachments = new HttpPostedFileBase[Request.Files.Count];
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        string filename = "";
                        string imgepath = "Null";
                        attachments[i] = Request.Files[i];
                        var InputFileName = Path.GetFileName(attachments[i].FileName);
                        //imgepath = InputFileName.Split('.')[0];
                        imgepath = InputFileName;
                        filename = SharedContext.Current.AdminLogdedProfile.UserProfile.Pid + "_" + model.CategoryId + "_" + DateTime.Now.Ticks+ Path.GetExtension(imgepath);
                        model.Image += filename + ";";
                        //string path = Path.Combine(Server.MapPath("~/Areas/Admin/image/Temp/"), filename);
                        string path = Path.Combine(Server.MapPath("~/Images/Products/"), filename);
                        attachments[i].SaveAs(path);
                    }
                    model.CreatedBy = SharedContext.Current.AdminLogdedProfile.UserProfile.UserName;
                    model.CreatedDate = DateTime.Now;
                    model.ProductCode = model.Producer + model.ProductCode;
                    
                    ProductService productsv = new ProductService();
                    string url = model.ProductName.ToLower();
                    url = Util.Util.ToAscii(Util.Extension.UnicodeStrings.LatinToAscii(model.URL.ToLower())) + "/" +Util.Util.ToAscii(Util.Extension.UnicodeStrings.LatinToAscii(url));

                    model.URL = url;
                    productsv.InsertProduct(util.Transform<Models.ProductModel, EntityLibrary.Product>(model),util.TransformList<Models.ProductDetailModel,EntityLibrary.ProductDetail>(detail).ToList(), out errMsg);
                    //string sourcePath = Server.MapPath("~/Areas/Admin/image/Temp/");
                    //string targetPath = Server.MapPath("~/Images/Products/" + model.CategoryId + "/");
                    //if (errMsg == "")
                    //{
                    //    if (!Directory.Exists(targetPath))
                    //    {
                    //        DirectoryInfo di = Directory.CreateDirectory(targetPath);
                    //    }
                    //    if (Directory.Exists(sourcePath))
                    //    {
                    //        string[] files = Directory.GetFiles(sourcePath);
                    //        string[] temp = model.Image.Split(';');
                    //        foreach (string file in files)
                    //        {
                    //            if (temp.Contains(Path.GetFileName(file)))
                    //            {
                    //                System.IO.File.Copy(file, Path.Combine(targetPath, Path.GetFileName(file)), true);
                    //                System.IO.File.Delete(file);
                    //            }
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    //    success = false;
                    //    if (Directory.Exists(sourcePath))
                    //    {
                    //        string[] files = Directory.GetFiles(sourcePath);
                    //        string[] temp = model.Image.Split(';');
                    //        foreach (string file in files)
                    //        {
                    //            if (temp.Contains(Path.GetFileName(file) + Path.GetExtension(file)))
                    //            {
                    //                System.IO.File.Delete(file);
                    //            }
                    //        }
                    //    }
                    //}
                }
                return Json(new { Success = success, Msg = errMsg });
            }
            catch(Exception ex)
            {
                return Json(new { Success = false, Msg = "Thêm mới sản phẩm lỗi: "+ex.Message });
            }
        }

        //[HttpPost]
        //[ValidateInput(false)]
        //public ActionResult InsertProduct(Models.ProductModel ProductModel, HttpPostedFileBase[] files)
        //{
        //    HttpPostedFileBase attachment = null;
        //    if (Request.Files.Count > 0)
        //    {
        //        attachment = Request.Files[0];
        //    }

        //    return Json(new { Success = true, Msg = "" });
        //}

        public ActionResult Test1()
        {
            var model = new PhotoCoordinatesModel();
            return View(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Test1(FormCollection frm)
        {
            HttpPostedFileBase[] attachments = new HttpPostedFileBase[Request.Files.Count];
            var _model = new PhotoCoordinatesModel();
            try
            {
                NewMethod(attachments, _model);
                return Json(new { Success = true, Data = _model });
            }
            catch (ExifLibException exifex)
            {
                _model.Error = exifex.Message;
                return Json(new { Success = false, Data = _model });
            }
        }

        private void NewMethod(HttpPostedFileBase[] attachments, PhotoCoordinatesModel _model)
        {
            for (int i = 0; i < Request.Files.Count; i++)
            {
                attachments[i] = Request.Files[i];
                string path = @"E:\hinh\100APPLE\";
                using (var reader = new ExifReader(path + attachments[i].FileName))
                {
                    _model.Lat = reader.GetLatitude();
                    _model.Lon = reader.GetLongitude();
                    if (reader.GetTagValue(ExifTags.DateTimeDigitized,
                                                    out DateTime datePictureTaken))
                    {

                    }
                }
                break;
            }
        }
        
        [HttpGet]
        public ActionResult EditProduct(long productId)
        {
            Models.ProductModel model = new Models.ProductModel();
            SearchResult<EntityLibrary.Product> data = productsv.GetProductById(productId);
            if (data != null)
            {
                model = util.Transform<EntityLibrary.Product, Models.ProductModel>(data.Data);
            }
            List<Models.CategoryModel> listCategory = new List<Models.CategoryModel>();
            CategoryService categorysv = new CategoryService();
            var result = categorysv.GetAllCategory(out int errorCode, out string errorMsg);
            listCategory = util.TransformList<EntityLibrary.Category, Models.CategoryModel>(result).ToList();
            ViewBag.listCategory = listCategory;
            List<DistributorModels> listDis = new List<DistributorModels>();
            SearchResult<IList<Distributor>> listDistributor = distributorsv.GetAllDistributor();
            listDis = util.TransformList<Distributor, DistributorModels>(listDistributor.Data).ToList();
            ViewBag.listDistri = listDis;
            ProducerService producersv = new ProducerService();
            ViewBag.listProducer = util.TransformList<Producer, ProducerModel>(producersv.GetAllProducer(out errorCode, out string errMsg));
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public JsonResult UpdateProduct(FormCollection frm)
        {
            string productModel = frm["Product"];
            Models.ProductModel model = JsonConvert.DeserializeObject<Models.ProductModel>(productModel);
            if (model != null)
            {
                SearchResult<EntityLibrary.Product> data = productsv.GetProductById(model.Pid);
                if (data == null)
                {
                    return Json(new
                    {
                        Success = false,
                        ErrMsg = "Sản phẩm không tồn tại"
                    });
                }
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase[] attachments = new HttpPostedFileBase[Request.Files.Count];
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        string filename = "";
                        string imgepath = "Null";
                        attachments[i] = Request.Files[i];
                        var InputFileName = Path.GetFileName(attachments[i].FileName);
                        imgepath = InputFileName;
                        filename = SharedContext.Current.AdminLogdedProfile.UserProfile.Pid + "_" + model.CategoryId + "_" + DateTime.Now.Ticks + Path.GetExtension(imgepath);
                        model.Image += filename + ";";
                        string path = Path.Combine(Server.MapPath("~/Areas/Admin/image/Temp/"), filename);
                        attachments[i].SaveAs(path);
                    }
                }
                model.ModifiedBy = SharedContext.Current.AdminLogdedProfile.UserProfile.UserName;
                model.ModifiedDate = DateTime.Now;
                string url = model.ProductName.ToLower();
                url = Util.Util.ToAscii(Util.Extension.UnicodeStrings.LatinToAscii(model.URL.ToLower())) + "/" + Util.Util.ToAscii(Util.Extension.UnicodeStrings.LatinToAscii(url));

                model.URL = url;
                PostResult<EntityLibrary.Product> result = productsv.UpdateProduct(util.Transform<Models.ProductModel, EntityLibrary.Product>(model));
                string sourcePath = Server.MapPath("~/Areas/Admin/image/Temp/");
                string targetPath = Server.MapPath("~/Images/Products/" + model.CategoryId + "/");
                if (result.Success && Request.Files.Count > 0)
                {
                    if (!Directory.Exists(targetPath))
                    {
                        DirectoryInfo di = Directory.CreateDirectory(targetPath);
                    }
                    if (Directory.Exists(sourcePath))
                    {
                        string[] files = Directory.GetFiles(sourcePath);
                        string[] temp = model.Image?.Split(';');
                        foreach (string file in files)
                        {
                            if (temp.Contains(Path.GetFileName(file)))
                            {
                                System.IO.File.Copy(file, Path.Combine(targetPath, Path.GetFileName(file)), true);
                                System.IO.File.Delete(file);
                            }
                        }
                    }
                }
                if(!result.Success && Request.Files.Count > 0)
                {
                    if (Directory.Exists(sourcePath))
                    {
                        string[] files = Directory.GetFiles(sourcePath);
                        string[] temp = model.Image.Split(';');
                        foreach (string file in files)
                        {
                            if (temp.Contains(Path.GetFileName(file) + Path.GetExtension(file)))
                            {
                                System.IO.File.Delete(file);
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(model.Image) && data.Data.Image != "")
                {
                    Models.ProductModel oldProduct = util.Transform<EntityLibrary.Product, Models.ProductModel>(data.Data);
                    string oldPath = Server.MapPath("~/Images/Products/" + oldProduct.CategoryId + "/");
                    if (Directory.Exists(oldPath))
                    {
                        string[] files = Directory.GetFiles(oldPath);
                        string[] temp = oldProduct.Image?.Split(';');
                        foreach (string file in files)
                        {
                            if (temp.Contains(Path.GetFileName(file)))
                            {
                                System.IO.File.Delete(file);
                            }
                        }
                    }
                }
                return Json(new
                {
                    result.Success,
                    ErrMsg = result.Message
                });
            }
            return Json(new {
                Success = false,
                ErrMsg = "Data not found"
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult DeleteProduct(long productPid)
        {
            PostResult<EntityLibrary.Product> res = productsv.DeleteProduct(productPid);
            if (res.Success)
            {
                Models.ProductModel oldProduct = util.Transform<EntityLibrary.Product, Models.ProductModel>(res.Data);
                string oldPath = Server.MapPath("~/Images/Products/" + oldProduct.CategoryId + "/");
                if (Directory.Exists(oldPath) && !string.IsNullOrEmpty(oldProduct.Image))
                {
                    string[] files = Directory.GetFiles(oldPath);
                    string[] temp = oldProduct.Image?.Split(';');
                    foreach (string file in files)
                    {
                        if (temp.Contains(Path.GetFileName(file)))
                        {
                            System.IO.File.Delete(file);
                        }
                    }
                }
            }
            return Json(new
            {
                res.Success,
                ErrMsg = res.Message
            });
        }

        public ActionResult GetProperty(long categoryId)
        {
            var res = productsv.GetPropertyOfCategory(categoryId).Data?.ToList();
            string result = "<table id='tbProductDetail' class='table table-striped table-bordered table-hover'>"+
                "<thead>"+
                    "<tr>"+
                        "{thSize}"+
                        "{thColor}"+
                        "{thWidth}" +
                        "{thHeight}" +
                        "<th>Description</th>" +
                        "<th>Quantity</th>" +
                        "<th>Action</th>" +
                    "</tr>"+
                "</thead>"+
                "<tbody>"+
                    "<tr class='tr_clone'>" +
                        "{tdSize}"+
                        "{tdColor}"+
                        "{tdWidth}" +
                        "{tdHeight}" +
                        "<td><input type='text' class='form-control' id='detail_Description_1' name='detail_Description' /></td>" +
                        "<td><input type='text' class='form-control' id='detail_Quantity_1' name='detail_Quantity' /></td>" +
                        "<td><input type='button' name='add' value='Thêm dòng' class='tr_clone_add btn btn-info'>&ensp;<input type='button' name='delete' value='Xóa dòng' class='btnDeleteRow btn btn-w-m btn-danger'></td>" +
                    "</tr>"+
                "</tbody>"+
                "</table>";
            if (res != null)
            {
                foreach (var item in res)
                {
                    var searchProperty = productsv.GetPropertyDetailByPropertyId(item.Id).Data.ToList();
                    if (searchProperty != null)
                    {
                        switch (item.Name)
                        {
                            case "Size":
                                string tem = "<td><select id='detail_Size_1' class='form-control' name='detail_Size'><option value=''>---- Chọn Size ----</option>";
                                foreach (var it in searchProperty)
                                {
                                    tem += "<option value='"+it.Id+"'>"+it.TypeName+"</option>";
                                }
                                tem += "</select></td>";
                                result = result.Replace("{thSize}", "<th>Size</th>").Replace("{tdSize}", tem);
                                break;
                            case "Color":
                                string temColor = "<td><select id='detail_Color_1' class='form-control' name='detail_Color'><option value=''>--- chọn màu ---</option>";
                                foreach (var it in searchProperty)
                                {
                                    temColor += "<option value='" + it.Id + "'>" + it.TypeName + "</option>";
                                }
                                temColor += "</select></td>";
                                result = result.Replace("{thColor}","<th>Color</th>").Replace("{tdColor}", temColor);
                                break;
                            case "Width":
                                string temWidth = "<td><input type='text' id='detail_Width_1' name='detail_Width' class='form-control' /></td>";
                                result = result.Replace("{thWidth}","<th>Width</th>").Replace("{tdWidth}", temWidth);
                                break;
                            case "Height":
                                string temHeight = "<td><input type='text' id='detail_Height_1' name='detail_Height' class='form-control' /></td>";
                                result = result.Replace("{thHeight}", "<th>Height</th>").Replace("{tdHeight}", temHeight);
                                break;
                        }
                    }
                    //else
                    //{
                    //    result = "";
                    //}
                }
                result = result.Replace("{thSize}", "").Replace("{tdSize}", "");
                result = result.Replace("{thColor}", "").Replace("{tdColor}", "");
                result = result.Replace("{thWidth}", "").Replace("{tdWidth}", "");
                result = result.Replace("{thHeight}", "").Replace("{tdHeight}", "");
            }
            else
            {
                result = "";
            }
            return Json(new
            {
                Success = result != "" ? true : false,

                Message = result!=""? "":"Data not found",
                Data = result
            });
        }

        [HttpPost]
        [ValidateInput(false)]
        public async Task<ActionResult> UploadVideo()
        //public JsonResult UploadVideo(FormCollection frm)
        {
            if (Request.Files.Count > 0)
            {
                HttpPostedFileBase[] attachments = new HttpPostedFileBase[Request.Files.Count];
                string path = "";
                try
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        string filename = "";
                        string imgepath = "Null";
                        attachments[i] = Request.Files[i];
                        var InputFileName = Path.GetFileName(attachments[i].FileName);
                        imgepath = InputFileName;
                        path = Path.Combine(Server.MapPath("~/Areas/Admin/VideoUpload/"), InputFileName);
                        attachments[i].SaveAs(path);
                        break;
                    }
                    //string videoid = await UploadVideo(path, "Test", "TEST UPLOAD VIDEO PRODUCT");
                    string videoid = await UploadVideo(path, "Test", "TEST UPLOAD VIDEO PRODUCT");
                    return Json(new
                    {
                        Success = true,
                        Message = "Upload video thành công!",
                        Data = videoid
                    });
                }
                catch(Exception ex)
                {
                    return Json(new
                    {
                        Success = false,
                        Message = "Upload video lỗi: "+ex.Message,
                        Data = ""
                    });
                }
            }
            return null;
        }
        public async Task<string> UploadVideo(string FilePath, string Title, string Description)
        {
            //YouTubeRequestSettings settings;
            //YouTubeRequest request;
            //string devkey = "AIzaSyAADYmKhkYCWPlnLwbhp5BupY4to4heeBE";
            //string username = "999070203024-compute@developer.gserviceaccount.com";
            //string password = "116659512181913731661";
            //settings = new YouTubeRequestSettings("TitiUploadVideoProduct", devkey, username, password) { Timeout = -1 };
            ////settings = new YouTubeRequestSettings()
            //request = new YouTubeRequest(settings);

            //Video newVideo = new Video
            //{
            //    Title = Title,
            //    Description = Description,
            //    Private = true
            //};
            //newVideo.YouTubeEntry.Private = false;

            //newVideo.YouTubeEntry.MediaSource = new MediaFileSource(FilePath, "video/mp4");
            //Video createdVideo = request.Upload(newVideo);

            //return createdVideo.VideoId;


            YouTubeRequestSettings ytsettings = new YouTubeRequestSettings("TitiUploadVideoProduct", "AIzaSyAADYmKhkYCWPlnLwbhp5BupY4to4heeBE", "dovantoan86@gmail.com", "Songtoan@123456211868741");
            ytsettings.Timeout = -1;
            YouTubeRequest ytReq = new YouTubeRequest(ytsettings);
            ((GDataRequestFactory)ytReq.Service.RequestFactory).Timeout = 60 * 60 * 1000;
            Video video = new Video
            {
                Title = "the best paint in the world",
                Description = "sonet reach"
            };
            video.Tags.Add(new MediaCategory("Sports", YouTubeNameTable.CategorySchema));
            video.YouTubeEntry.Private = false;
            video.YouTubeEntry.MediaSource = new MediaFileSource(FilePath, "video/mp4");
            Video createdVideo = ytReq.Upload(video);
            string videoID = createdVideo.VideoId;
            return videoID;
        }
    }
}