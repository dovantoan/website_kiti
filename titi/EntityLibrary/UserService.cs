using DatabaseUtility.EntityLibrary;
using System;
using System.Linq;
using Util;

namespace EntityLibrary
{
    public class UserService
    {
        public User GetUser(string username, string pass, out int errorCode, out string errorMsg)
        {
            using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
            {
                try
                {
                    entity.Configuration.ProxyCreationEnabled = false;
                    var user = entity.User.Where(w => w.UserName.Equals(username) && w.Password.Equals(pass) && w.Status == 1).FirstOrDefault();
                    errorCode = 0;
                    if (user != null)
                        errorMsg = "Success";
                    else
                        errorMsg = "Data not found";
                    return user;
                }
                catch (Exception ex)
                {
                    errorCode = 100;
                    errorMsg = ex.Message;
                    return null;
                }
            }
        }

        public PostResult<User> RegisterAccountUser(User newUser)
        {
            PostResult<User> resutl = new PostResult<User>();
            try
            {
                newUser.Password = Shared.Utility.FunctionUtility.EncodePassword(newUser.P);
                using (EntityLibContext entity = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    string errMsg = entity.User.Where(w => w.Phone.Equals(newUser.Phone)).FirstOrDefault() != null 
                        ? "Số điện thoại đã được đăng ký" 
                        : entity.User.Where(w => w.Phone.Equals(newUser.Email)).FirstOrDefault() != null 
                        ? "Email đã được đăng ký" 
                        : "";
                    if (errMsg == "")
                    {
                        entity.User.Add(newUser);
                        entity.SaveChanges();
                        resutl.Success = true;
                        resutl.ErrorCode = "0";
                        resutl.Message = "Thêm mới user thành công";
                    }
                    else
                    {
                        resutl.ErrorCode = "100";
                        resutl.Success = false;
                        resutl.Message = errMsg;
                    }
                    resutl.Data = newUser;
                }
            }
            catch (Exception ex)
            {
                resutl.Success = false;
                resutl.ErrorCode = "100";
                resutl.Message = "Thêm mới user lỗi: " + ex.Message;
            }
            return resutl;
        }

        public SearchResult<User> UserLogin(string userName, string password)
        {
            SearchResult<User> result = new SearchResult<User>();
            try
            {
                using (EntityLibContext entities = new EntityLibContext(DatabaseAccessNonEntity.GetConnectionStringEntity()))
                {
                    string _password = Shared.Utility.FunctionUtility.EncodePassword(password);
                    User user = entities.User.Where(w => (userName.Contains("@") ? w.Email.Equals(userName) : w.Phone.Equals(userName)) && w.Password.Equals(_password)).FirstOrDefault();
                    if (user != null)
                    {
                        result.Data = user;
                        result.TotalRows = 1;
                        result.Success = true;
                        result.Message = "";
                    }
                    else
                    {
                        result.Data = null;
                        result.TotalRows = 0;
                        result.Success = false;
                        result.Message = "Data not found";
                    }
                    return result;
                }
            }
            catch (Exception ex)
            {
                result.Data = null;
                result.TotalRows = 0;
                result.Success = false;
                result.Message = "Error "+ ex.Message;
                return result;
            }
            
        }
    }
}
