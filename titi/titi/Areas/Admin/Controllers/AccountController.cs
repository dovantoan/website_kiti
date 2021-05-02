
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using titi.Areas.Admin.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Collections.Generic;
using EntityLibrary;
using EntityLibrary.ModuleImplement;
using titi.Areas.Admin.Models.Security;
using titi.Helper;
using System.Net;

namespace titi.Areas.Admin.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private Util.Util util = new Util.Util();
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }
        //UserService userService = new UserService();
        // GET: Account
        [AllowAnonymous]
        //[ValidateAntiForgeryToken]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                string message = "";
                UserModel user = new UserModel();
                SystemService systemsv = new SystemService();
                UserService usersv = new UserService();
                var us = usersv.GetUser(model.UserName, Shared.Utility.FunctionUtility.EncodePassword(model.Password), out int errorCode, out string errorMsg);
                if (us != null)
                {
                    user = util.Transform<User, UserModel>(us);
                    Session["UserInfo"] = user;
                    Session["ReturnUrl"] = model.ReturnURL ?? "";
                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);
                    try
                    {
                        //string hostName = System.Net.Dns.GetHostEntry(Request.ServerVariables["remote_addr"]).HostName;

                        //string ipAddress = null;

                        //ipAddress = string.IsNullOrEmpty(Request.ServerVariables["HTTP_X_FORWARDED_FOR"])
                        //    ? Request.UserHostAddress
                        //    : Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                        //if (string.IsNullOrEmpty(ipAddress) || ipAddress.Trim() == "::1")
                        //{
                        //    var lan = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).AddressList.FirstOrDefault(r => r.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork);
                        //    ipAddress = lan == null ? string.Empty : lan.ToString();
                        //}
                        //string publicAddress = new System.Net.WebClient().DownloadString(@"http://icanhazip.com").Trim();

                        //Session["HostName"] = hostName;
                        //Session["LocalAddress"] = ipAddress;
                        //Session["PublicAddress"] = publicAddress;
                        AdminLoginProfile profile = new AdminLoginProfile
                        {
                            UserProfile = user
                        };
                        SharedContext.Current.AdminLogdedProfile = profile;
                        //var a = SharedContext.Current.AdminLogdedProfile;
                    }
                    catch (Exception ex)
                    {
                        Session["HostName"] = "";
                        Session["LocalAddress"] = "";
                        Session["PublicAddress"] = "";
                        //throw ex;
                        //ViewBag.messageErr = ex.Message;
                        //return View();
                    }
                    TokenModel token = GenerateToken(user.Pid);
                    GetRoles(user.Pid);
                    GetDefineUIByUserId(user.Pid);
                    return RedirectToAction("Index", "AdminHome", new { Area = "Admin"});
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }
            ViewBag.messageErr = "Account does not exist!";
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }
        private void GetRoles(long userId)
        {
            SystemService sv = new SystemService();
            IList<RoleViewModel> roles = new List<RoleViewModel>();
            var _roles = sv.GetPhanQuyenByUserId(userId, out int errorCode, out string errorMsg);
            if (_roles.Count>0)
            {
                roles = util.TransformList<RoleImplement, RoleViewModel>(_roles);
                SharedContext.Current.AdminLogdedProfile.Roles = roles.ToList();
                //Session["RolesOfUser"] = roles;
            }
        }
        private void GetDefineUIByUserId(long userId)
        {
            SystemService sv = new SystemService();
            IList<DefineUIModel> df = new List<DefineUIModel>();
            var _df = sv.GetDefineUIByUserId(userId, out int errorCode, out string errorMsg);
            if (_df.Count>0)
            {
                df = util.TransformList<DefineUI, DefineUIModel>(_df);
                SharedContext.Current.AdminLogdedProfile.DefineUI = df.ToList();
                //Session["DefineUI"] = df;
            }
        }
        private TokenModel GenerateToken(long userId)
        {
            SystemService systemsv = new SystemService();
            TokenModel token = new TokenModel();
            var tk = systemsv.GenerateToken(userId, out int errorCode, out string errorMsg);
            if (tk != null)
            {
                token = util.Transform<Tokens, TokenModel>(tk);
                SharedContext.Current.AdminLogdedProfile.Token = token;
                //Session["Token"] = token;
            }
            return token;
        }

//        private async Task CreateRoles()
//        {
//                List<RoleViewModel> roles = Session["RolesOfUser"] as List<RoleViewModel>;
////            //adding custom roles
//                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>());
//                IdentityResult roleResult;
////​
//            foreach (var role in roles)
//             {
//                //creating the roles and seeding them to the database
//                var roleExist = await roleManager.RoleExistsAsync(role.RoleName);
//                if (!roleExist)
//                {
//                    roleResult = await roleManager.CreateAsync(new IdentityRole(role.RoleName));
//                }
//            }
//        }
    }
}