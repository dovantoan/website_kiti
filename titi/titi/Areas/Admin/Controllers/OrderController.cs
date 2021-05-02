using EntityLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Util.Entity;

namespace titi.Areas.Admin.Controllers
{
    public class OrderController : AdminController
    {
        private readonly OrderService ordersv = new OrderService();
        // GET: Admin/Order
        public ActionResult Index()
        {
            List<OrderViewEntity> list = ordersv.GetAllOrder();
            ViewBag.Data = list;
            return View();
        }
    }
}