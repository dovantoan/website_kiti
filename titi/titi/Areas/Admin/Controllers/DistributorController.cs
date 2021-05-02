using EntityLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titi.App_Start;
using titi.Areas.Admin.Models;
using Util;

namespace titi.Areas.Admin.Controllers
{
    [CustomizeActionFilter]
    public class DistributorController : Controller
    {
        private readonly DistributorService sv = new DistributorService();
        private Util.Util util = new Util.Util();
        // GET: Admin/Distributor
        public ActionResult Distributor()
        {
            return View();
        }
        public JsonResult GetAllDistributor()
        {
            IList<DistributorModels> listDis = new List<DistributorModels>();
            SearchResult<IList<Distributor>> res = sv.GetAllDistributor();
            if (res.Success)
            {
                listDis = util.TransformList<Distributor, DistributorModels>(res.Data);
            }
            return Json(new { Success = true, Data = listDis }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateDis(DistributorModels dis)
        {
            string msg;
            bool success = true;
            try
            {
                PostResult<Distributor> result = sv.UpdateDistributor(util.Transform<DistributorModels, Distributor>(dis));
                msg = result.Message;
                success = result.Success;
            }
            catch (Exception ex)
            {
                msg = "Error occured:" + ex.Message;
                success = false;
            }
            return Json(new { Success = success, Msg = msg });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult InsertDis([Bind(Exclude = "Pid")] DistributorModels dis)
        {
            string msg;
            bool success = true;
            try
            {
                PostResult<Distributor> result = sv.InsertDistributor(util.Transform<DistributorModels, Distributor>(dis));
                msg = result.Message;
                success = result.Success;
            }
            catch (Exception ex)
            {
                msg = "Error occured:" + ex.Message;
                success = false;
            }
            return Json(new { Success = success, Msg = msg });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public JsonResult DeleteDis(long id)
        {
            string msg;
            bool success = true;
            try
            {
                PostResult<Distributor> result = sv.DeleteDistributor(id);
                msg = result.Message;
                success = result.Success;
            }
            catch (Exception ex)
            {
                msg = "Error occured:" + ex.Message;
                success = false;
            }
            return Json(new { Success = success, Msg = msg });
        }
    }
}