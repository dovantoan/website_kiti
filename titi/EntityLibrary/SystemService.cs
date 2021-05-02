using DatabaseUtility.EntityLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using EntityLibrary.ModuleImplement;
using System.Data;
using Util;

namespace EntityLibrary
{
    public class SystemService
    {
        public long Authenticate(string userName, string password, out int errorCode, out string errorMsg)
        {
            errorCode = 0;
            errorMsg = "Success";
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    var user = entity.User.Where(w => w.UserName == userName && w.Password == password).FirstOrDefault();
                    if (user != null && user.Pid > 0)
                        return user.Pid;

                }
            }
            catch (Exception ex)
            {
                errorCode = 100;
                errorMsg = ex.ToString();
            }
            return 0;
        }
        public bool DeleteByUserId(long userId, out string errorCode, out string errorMsg)
        {
            errorCode = "";
            errorMsg = "";
            try
            {
                string _connectionString = DatabaseAccessNonEntity.GetConnectionStringEntity();
                if (_connectionString != "")
                {
                    using (EntityLibContext entity = new EntityLibContext(_connectionString))
                    {
                        var token = entity.Tokens.Where(w => w.UserId == userId).FirstOrDefault();
                        if (token != null)
                        {
                            entity.Tokens.Remove(token);
                            entity.SaveChanges();
                        }
                    }
                }
                else
                {
                    errorCode = "ETT0001";
                    errorMsg = "Authenticate error: connectionString not found!";
                }
                return true;
            }
            catch (Exception ex)
            {
                errorCode = "100";
                errorMsg = "DeleteByUserId error: " + ex.ToString();
                return false;
            }
        }

        public Tokens GenerateToken(long userId,out int errorCode, out string errorMsg)
        {
            errorCode = 0;
            errorMsg = "";
            string _connectionString = DatabaseAccessNonEntity.GetConnectionStringEntity();
            string token = Guid.NewGuid().ToString();
            DateTime issuedOn = DateTime.Now;
            DateTime expiredOn = DateTime.Now.AddSeconds(Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));
            var tokendomain = new Tokens
            {
                UserId = userId,
                AuthToken = token,
                IssuedOn = issuedOn,
                ExpiresOn = expiredOn
            };
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    entity.Tokens.Add(tokendomain);
                    entity.SaveChanges();
                }
                var tokenModel = new Tokens
                {
                    UserId = userId,
                    IssuedOn = issuedOn,
                    ExpiresOn = expiredOn,
                    AuthToken = token
                };
                return tokenModel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public IList<DefineUI> GetDefineUIByUserId(long UserId, out int errorCode, out string errorMsg)
        {
            var result = new List<DefineUI>();
            errorCode = 0;
            errorMsg = "";
            try
            {
                DataTable dt = new DataTable();
                SqlDBParameter[] inputParams = new SqlDBParameter[1];
                inputParams[0] = new SqlDBParameter("@USERID", SqlDbType.BigInt, UserId);
                dt = DatabaseAccessNonEntity.SearchStoreProcedureDataTable("sp_GetDefineUIByUserID", inputParams);
                if (dt!=null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new DefineUI
                        {
                            Pid = long.Parse(row["Pid"].ToString()),
                            URL = row["URL"].ToString(),
                            Name = row["Name"].ToString(),
                            Title = row["Title"].ToString(),
                            Description = row["Description"].ToString(),
                            ParentPid = row["ParentPid"].ToString() != "" ? long.Parse(row["ParentPid"].ToString()) : 0,
                            Icon = row["Icon"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public IList<RoleImplement> GetPhanQuyenByUserId(long UserId, out int errorCode, out string errorMsg)
        {
            errorCode = 0;
            errorMsg = "";
            var result = new List<RoleImplement>();
            string _connectionString = DatabaseAccessNonEntity.GetConnectionString();
            try
            {
                DataTable dt = new DataTable();
                SqlDBParameter[] inputParams = new SqlDBParameter[1];
                inputParams[0] = new SqlDBParameter("@USERID", SqlDbType.BigInt, UserId);
                dt = DatabaseAccessNonEntity.SearchStoreProcedureDataTable(_connectionString, "sp_GetPhanQuyenByUserId", inputParams);
                if (dt!=null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        result.Add(new RoleImplement
                        {
                            RolePid = long.Parse(row["RolePid"].ToString()),
                            RoleName = row["RoleName"].ToString(),
                            Action = row["Action"].ToString(),
                            Description = row["Description"].ToString()
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;
        }

        public bool Kill(string tokenId, string domain, out string errorCode, out string errorMsg)
        {
            errorCode = "";
            errorMsg = "";
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    var token = entity.Tokens.Where(w => w.AuthToken == tokenId).FirstOrDefault();
                    if (token != null)
                    {
                        entity.Tokens.Remove(token);
                        entity.SaveChanges();
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool ValidateToken(string tokenId, string domain, out string errorCode, out string errorMsg)
        {
            errorCode = "";
            errorMsg = "";
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    var token = entity.Tokens.Where(w => w.AuthToken == tokenId && w.ExpiresOn > DateTime.Now).FirstOrDefault();
                    if (token != null && !(DateTime.Now > token.ExpiresOn))
                    {
                        token.ExpiresOn = token.ExpiresOn.AddSeconds(
                                                      Convert.ToDouble(ConfigurationManager.AppSettings["AuthTokenExpiry"]));

                        entity.SaveChanges();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return false;
        }

        #region ======== role ============
        public IList<Role> GetAllRoles(out string errorCode, out string errorMsg)
        {
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    errorCode = "0";
                    errorMsg = "";
                    return entity.Role.ToList();
                }
            }
            catch (Exception ex) {
                errorCode = "100";
                errorMsg = ex.Message;
                return null;
            }
        }

        public PostResult<Role> InsertRole(Role newRole)
        {
            PostResult<Role> resutl = new PostResult<Role>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    entity.Role.Add(newRole);
                    entity.SaveChanges();
                    resutl.Success = true;
                    resutl.ErrorCode = "0";
                    resutl.Message = "Thêm mới role thành công";
                    resutl.Data = newRole;
                }
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.ErrorCode = "100";
                resutl.Message = "Thêm mới role lỗi: "+ex.Message;
            }
            return resutl;
        }

        public PostResult<Role> UpdateRole(Role Role)
        {
            PostResult<Role> resutl = new PostResult<Role>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    Role r = entity.Role.Where(w => w.Pid == Role.Pid).FirstOrDefault();
                    if (r != null)
                    {
                        r.RoleName = Role.RoleName;
                        r.Action = Role.Action;
                        r.Description = Role.Description;
                        entity.SaveChanges();
                        resutl.Success = true;
                        resutl.ErrorCode = "0";
                        resutl.Message = "Cập nhật role thành công";
                    }
                    else
                    {
                        resutl.Success = false;
                        resutl.ErrorCode = "-1";
                        resutl.Message = "Không tìm thấy dữ liệu cập nhật";
                    }
                    resutl.Data = Role;

                }
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.ErrorCode = "100";
                resutl.Message = "Cập nhật role lỗi: " + ex.Message;
            }
            return resutl;
        }

        public PostResult<Role> DeleteRole(long id)
        {
            PostResult<Role> resutl = new PostResult<Role>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    Role r = entity.Role.Where(w => w.Pid == id).FirstOrDefault();
                    if (r != null)
                    {
                        entity.Role.Remove(r);
                        entity.SaveChanges();
                        resutl.Success = true;
                        resutl.ErrorCode = "0";
                        resutl.Message = "Xóa role thành công";
                    }
                    else
                    {
                        resutl.Success = false;
                        resutl.ErrorCode = "-1";
                        resutl.Message = "Không tìm thấy dữ liệu cần xóa";
                    }
                    resutl.Data = r;

                }
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.ErrorCode = "100";
                resutl.Message = "Cập nhật role lỗi: " + ex.Message;
            }
            return resutl;
        }
        #endregion

        #region ========== module ==============
        public IList<DefineUI> GetAllDefineUI()
        {
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    return entity.DefineUI.ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }
        public PostResult<DefineUI> InsertDefineUI(DefineUI newDefineUI)
        {
            PostResult<DefineUI> resutl = new PostResult<DefineUI>();
            try
            {
                newDefineUI.NameSpace = string.IsNullOrEmpty(newDefineUI.NameSpace) ? "Website" : newDefineUI.NameSpace;
                newDefineUI.IsActive = 1;
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    entity.DefineUI.Add(newDefineUI);
                    entity.SaveChanges();
                    resutl.Success = true;
                    resutl.ErrorCode = "0";
                    resutl.Message = "Thêm mới module thành công";
                    resutl.Data = newDefineUI;
                }
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.ErrorCode = "100";
                resutl.Message = "Thêm mới module lỗi: " + ex.Message;
            }
            return resutl;
        }

        public PostResult<DefineUI> UpdateDefineUI(DefineUI defineUI)
        {
            PostResult<DefineUI> resutl = new PostResult<DefineUI>();
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    DefineUI df = entity.DefineUI.Where(w => w.Pid == defineUI.Pid).FirstOrDefault();
                    if (df != null)
                    {
                        df.URL = defineUI.URL;
                        df.Name = defineUI.Name;
                        df.Title = defineUI.Title;
                        df.Description = defineUI.Description;
                        df.ParentPid = defineUI.ParentPid;
                    }
                    entity.SaveChanges();
                    resutl.Success = true;
                    resutl.ErrorCode = "0";
                    resutl.Message = "Cập nhật module thành công";
                    resutl.Data = df;
                }
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.ErrorCode = "100";
                resutl.Message = "Cập nhật module lỗi: " + ex.Message;
            }
            return resutl;
        }

        public bool UpdateGroupUI(List<GroupsUI> listGroupUI, long groupId)
        {
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    entity.GroupsUI.RemoveRange(entity.GroupsUI.Where(w => w.GroupID == groupId).ToList());
                    if(listGroupUI!=null && listGroupUI.Count > 0)
                    {
                        entity.GroupsUI.AddRange(listGroupUI);
                    }
                    entity.SaveChanges();
                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        } 
        #endregion

        #region ========= group ========
        public IList<Group> GetAllGroup()
        {
            try
            {
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    return entity.Group.ToList();
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        public DataTable GetModuleByGroup(long groupId)
        {
            try
            {
                DataSet ds = new DataSet();
                SqlDBParameter[] inputParams = new SqlDBParameter[1];
                inputParams[0] = new SqlDBParameter("@GROUPID", SqlDbType.BigInt, groupId);
                ds = DatabaseAccessNonEntity.SearchStoreProcedure("sp_GetModuleByGroup",inputParams);
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                return dt;
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }

        public DataTable GetAllPhanQuyen()
        {
            try
            {
                DataSet ds = new DataSet();
                ds = DatabaseAccessNonEntity.SearchStoreProcedure("sp_GetAllPhanQuyen");
                DataTable dt = new DataTable();
                dt = ds.Tables[0];
                return dt;
            }
            catch (Exception)
            {
                return new DataTable();
            }
        }
        #endregion
    }
}
