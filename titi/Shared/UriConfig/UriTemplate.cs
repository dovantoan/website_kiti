using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shared.UriConfig
{
    public static class UriTemplate
    {
        public const string DOMAINWEBAPI = "http://localhost:4221/";
        #region ===== category ======
        public const string CATEGORY_GETALL = "api/category/get";
        #endregion

        #region ===== user =====
        public const string USER_GET = "api/user/get";
        public const string USER_GENERATETOKEN = "api/user/GenerateToken/";
        #endregion

        #region ======== product ==========
        public const string PRODUCT_SEARCH = "api/product/search/";
        public const string PRODUCT_GETBYID = "api/product/getbyid/";
        public const string PRODUCT_GETBYCODE = "api/product/getbycode/";
        #endregion

        #region ========= system ==========
        public const string SYSTEM_GETROLES = "api/system/getRoles/";
        public const string SYSTEM_GETDEFINEUIBYUSERID = "api/system/getDefineUIByUserId/";
        #endregion

        #region ========= Distributor ==============
        public const string DISTRIBUTOR_GETALL = "api/distributor/getall/";
        #endregion
    }
}