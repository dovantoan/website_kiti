using EntityLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titi.Areas.Admin.Models;
using Util;

namespace titi.Areas.Admin.Controllers
{
    public class ProducerController : AdminController
    {
        private readonly Util.Util util = new Util.Util();
        private readonly ProducerService producersv = new ProducerService();
        // GET: Admin/Producer
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetAllProducer()
        {
            IList<ProducerModel> listProducer = new List<ProducerModel>();
            listProducer = util.TransformList<Producer, ProducerModel>(producersv.GetAllProducer(out int errorCode, out string errMsg));
            return Json(new { Success = true, Data = listProducer }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateProducer(ProducerModel producer)
        {
            string msg;
            bool success = true;
            try
            {
                PostResult<Producer> result = producersv.UpdateProducer(util.Transform<ProducerModel, Producer>(producer));
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
        public JsonResult InsertProducer([Bind(Exclude = "Pid")] ProducerModel pro)
        {
            string msg;
            bool success = true;
            try
            {
                PostResult<Producer> result = producersv.InsertProducer(util.Transform<ProducerModel, Producer>(pro));
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
        public JsonResult DeleteProducer(long id)
        {
            string msg;
            bool success = true;
            try
            {
                PostResult<Producer> result = producersv.DeleteProducer(id);
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