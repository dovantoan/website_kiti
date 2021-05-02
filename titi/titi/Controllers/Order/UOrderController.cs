using EntityLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titi.Helper;
using titi.Models.Security;
using Util;
using Util.Entity;

namespace titi.Controllers.Order
{
    public class UOrderController : Controller
    {
        private readonly OrderService orderSV = new OrderService();
        // GET: UOrder
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AjaxOrder(OrderEntity order)
        {
            UserLoginProfiles user = SharedContext.Current.UserLoginProfiles;
            if (user == null)
            {
                return RedirectToAction("CheckOut", "Products");
            }
            order.UserID = user.UserProfile.Pid;
            order.Status = 0;
            order.OrderCode = Util.Util.RandomString(10, false);
            PostResult<OrderEntity> result = orderSV.InsertUserOrder(order);
            return Json(new
            {
                result.Success,
                result.Message,
                result.Data
            });
        }
    }
}