using EntityLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using titi.Areas.Admin.Models;
using titi.Helper;
using titi.Models.Account;
using titi.Models.Security;
using Util;

namespace titi.Controllers
{
    public class UAccountController : Controller
    {
        private readonly UserService usersv = new UserService();
        private Util.Util util = new Util.Util();
        // GET: Account
        [HttpGet]
        public ActionResult UserLogin()
        {
            return PartialView("~/Views/Account/UserLogin.cshtml");
        }
        public void GoogleLogin(string email, string name, string gender, string lastname, string location)
        {
            //Write your code here to access these paramerters

        }
        [HttpPost]
        public JsonResult UserLogin(string userName, string password)
        {
            SearchResult<User> result = usersv.UserLogin(userName, password);
            if (result.Success)
            {
                UserLoginProfile user = util.Transform<User, UserLoginProfile>(result.Data);
                UserLoginProfiles profile = new UserLoginProfiles
                {
                    UserProfile = user
                };
                SharedContext.Current.UserLoginProfiles = profile;
                return Json(new
                {
                    Success = true,
                    Message = ""
                });
            }
            return Json(new
            {
                Success = false,
                result.Message
            });
        }

        public JsonResult ResgisterUserAccount(UserModel userModel)
        {
            string firsName = userModel.FirstName.Substring(0, userModel.FirstName.IndexOf(' ', 0));
            string lastName = userModel.FirstName.Substring(userModel.FirstName.IndexOf(' ', 0)+1);
            userModel.FirstName = firsName;
            userModel.LastName = lastName;
            userModel.UserName = userModel.Phone;
            PostResult<User> result = usersv.RegisterAccountUser(util.Transform<UserModel, User>(userModel));
            return Json(new
            {
                result.Success,
                result.Message
            });
        }
        
        
    }
}