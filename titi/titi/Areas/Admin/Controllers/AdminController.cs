using EntityLibrary;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using titi.App_Start;
using titi.Areas.Admin.Models;
using Util;

namespace titi.Areas.Admin.Controllers
{
    [CustomizeActionFilter]
    public class AdminController : Controller
    {
        private Util.Util util = new Util.Util();
        private readonly SystemService systemsv = new SystemService();
        [HttpGet]
        public ActionResult AddUser()
        {
            return View();
        }
        public ActionResult SearchUser()
        {
            return View();
        }
        public ActionResult ListUser()
        {
            return PartialView();
        }
        [AuthorizeCustom("addnewuser")]
        public ActionResult InsertUser()
        {
            return View();
        }

        #region ======== roles =========
        public ActionResult Roles()
        {
            IList<RoleModel> listRoles = new List<RoleModel>();
            listRoles = util.TransformList<Role, RoleModel>(systemsv.GetAllRoles(out string errorCode, out string errorMsg));
            ViewBag.listRoles = listRoles;
            return View();
        }
        
        public JsonResult GetAllRoles()
        {
            IList<RoleModel> listRoles = new List<RoleModel>();
            listRoles = util.TransformList<Role, RoleModel>(systemsv.GetAllRoles(out string errorCode, out string errorMsg));
            return Json(new { Success = true, Data = listRoles}, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateRole(RoleModel role)
        {
            string msg;
            bool success = true;
            try
            {
                PostResult<Role> result = systemsv.UpdateRole(util.Transform<RoleModel, Role>(role));
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
        public JsonResult InsertRole([Bind(Exclude = "Pid")] RoleModel role)
        {
            string msg;
            bool success = true;
            try
            {
                PostResult<Role> result = systemsv.InsertRole(util.Transform<RoleModel, Role>(role));
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
        public JsonResult DeleteRole(long id)
        {
            string msg;
            bool success = true;
            try
            {
                PostResult<Role> result = systemsv.DeleteRole(id);
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
        #endregion

        #region =========== module ===========
        public ActionResult Modules()
        {
            return View();
        }
        public JsonResult GetAllModules()
        {
            IList<DefineUIModel> listModules = new List<DefineUIModel>();
            listModules = util.TransformList<DefineUI, DefineUIModel>(systemsv.GetAllDefineUI());
            return Json(new { Success = true, Data = listModules }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult UpdateModule(DefineUIModel defineUI)
        {
            string msg;
            bool success = true;
            try
            {
                PostResult<DefineUI> result = systemsv.UpdateDefineUI(util.Transform<DefineUIModel, DefineUI>(defineUI));
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
        public JsonResult InsertModule([Bind(Exclude = "Pid")] DefineUIModel module)
        {
            string msg;
            bool success = true;
            try
            {
                PostResult<DefineUI> result = systemsv.InsertDefineUI(util.Transform<Models.DefineUIModel, DefineUI>(module));
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

        // phân quyền module
        public ActionResult PermissionModule()
        {
            List<Models.DefineUIModel> listModules = new List<Models.DefineUIModel>();
            listModules = util.TransformList<DefineUI, Models.DefineUIModel>(systemsv.GetAllDefineUI()).ToList();
            ViewBag.listModules = listModules;
            List<Models.GroupModule> listGroup = new List<Models.GroupModule>();
            listGroup = util.TransformList<Group, Models.GroupModule>(systemsv.GetAllGroup()).ToList();
            ViewBag.listGroup = listGroup;
            return View();
        }
        public JsonResult GetMouduleByGroup(long groupID)
        {
            return Json(new { Success = true, Data = GetTreeData(groupID) }, JsonRequestBehavior.AllowGet);
        }

        //public string GetTreeData(long groupID)
        //{
        //    List<GroupModule> listGroup = new List<GroupModule>();
        //    listGroup = util.TransformList<Group, GroupModule>(systemsv.GetAllGroup()).ToList();

        //    DataTable dt = systemsv.GetModuleByGroup(groupID);
        //    List<FlatObject> flatObjects = new List<FlatObject>();
        //    if (dt.Rows.Count > 0)
        //    {
        //        foreach(DataRow row in dt.Rows)
        //        {
        //            FlatObject obj = new FlatObject(row["Name"].ToString(), long.Parse(row["UIPid"].ToString()), row["ParentPid"].ToString() != "" ? long.Parse(row["ParentPid"].ToString()) : 0);
        //            flatObjects.Add(obj);
        //        }
        //    }
        //    var recursiveObjects = FillRecursive(flatObjects, 0);
        //    string myjsonmodel = new JavaScriptSerializer().Serialize(recursiveObjects);

        //    return myjsonmodel;
        //}

        //private static List<RecursiveObject> FillRecursive(List<FlatObject> flatObjects, Int64 parentId)
        //{
        //    List<RecursiveObject> recursiveObjects = new List<RecursiveObject>();

        //    foreach (var item in flatObjects.Where(x => x.ParentId.Equals(parentId)))
        //    {
        //        recursiveObjects.Add(new RecursiveObject
        //        {
        //            data = item.data,
        //            id = item.Id,
        //            attr = new FlatTreeAttribute { id = item.Id, selected = true,opened = true },
        //            children = FillRecursive(flatObjects, item.Id)
        //        });
        //    }
        //    return recursiveObjects;
        //}

        public string GetTreeData(long groupID)
        {
            List<DefineUIModel> listModules = new List<DefineUIModel>();
            listModules = util.TransformList<DefineUI, DefineUIModel>(systemsv.GetAllDefineUI()).ToList();

            DataTable dt = systemsv.GetModuleByGroup(groupID);
            List<FlatObject> flatObjects = new List<FlatObject>();
            if(listModules != null && listModules.Count > 0)
            {
                foreach(var it in listModules)
                {
                    FlatObject obj = new FlatObject(it.Name, it.Pid, it.ParentPid != null ? long.Parse(it.ParentPid.ToString()) : 0);
                    flatObjects.Add(obj);
                }
            }
            List<DefineUIModel> lsChild = new List<DefineUIModel>();

            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    lsChild.Add(new DefineUIModel { Pid = long.Parse(row["UIPid"].ToString()), Name = row["Name"].ToString() });
                }
            }
            var recursiveObjects = FillRecursive(flatObjects, 0,lsChild);
            string myjsonmodel = new JavaScriptSerializer().Serialize(recursiveObjects);

            return myjsonmodel;
        }

        private static List<RecursiveObject> FillRecursive(List<FlatObject> flatObjects, Int64 parentId,List<DefineUIModel> lsChild)
        {
            List<RecursiveObject> recursiveObjects = new List<RecursiveObject>();

            foreach (FlatObject item in flatObjects.Where(x => x.ParentId.Equals(parentId)))
            {
                recursiveObjects.Add(new RecursiveObject
                {
                    data = item.data,
                    id = item.Id,
                    attr = new FlatTreeAttribute { id = item.Id, selected = (lsChild.Where(w=>w.Pid==item.Id).FirstOrDefault()!=null ? true : false), opened = true },
                    children = FillRecursive(flatObjects, item.Id, lsChild)
                });
            }
            return recursiveObjects;
        }

        public ActionResult UpdateGroupUI(List<long> listUIid, long groupId )
        {
            List<GroupsUI> ls = new List<GroupsUI>();
            bool res = false;
            if(listUIid!=null && listUIid.Count > 0)
            {
                foreach(long it in listUIid)
                {
                    ls.Add(new GroupsUI { UIPid = it, GroupID = groupId });
                }
                res = systemsv.UpdateGroupUI(ls, groupId);
            }
            return Json(new {Success = res });
        }

        public ActionResult PhanQuyen()
        {
            IList<RoleModel> listRoles = new List<RoleModel>();
            listRoles = util.TransformList<Role, RoleModel>(systemsv.GetAllRoles(out string errorCode, out string errorMsg));
            ViewBag.listRoles = listRoles;
            return View();
        }

        public JsonResult GetPhanQuyen()
        {
            DataTable dt = systemsv.GetAllPhanQuyen();
            List<PhanQuyenViewModel> lsPhanQuyen = new List<PhanQuyenViewModel>();
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow r in dt.Rows)
                {
                    lsPhanQuyen.Add(new PhanQuyenViewModel
                    {
                        Pid = long.Parse(r["Pid"].ToString()),
                        UserPid = long.Parse(!String.IsNullOrEmpty(r["UserPid"].ToString()) ? r["UserPid"].ToString() : "0"),
                        UserName = r["UserName"].ToString(),
                        GroupPid = long.Parse(!String.IsNullOrEmpty(r["GroupPid"].ToString()) ? r["GroupPid"].ToString() : "0"),
                        GroupName = r["GroupName"].ToString(),
                        RolePid = long.Parse(r["RolePid"].ToString()),
                        RoleName = r["RoleName"].ToString(),
                        Description = r["Description"].ToString()
                    });
                }
            }
            return Json(new { Success = true, Data = lsPhanQuyen }, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}