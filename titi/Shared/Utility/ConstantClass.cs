using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Utility
{
    public class ConstantClass
    {
        public const string FORMAT_DATETIME = "dd/MM/yyyy";
        public const string PATH_LOGO = "\\logo.jpg";
        public const int UserAddmin = 1;
        public const string defaultLang = "vi-VN";
        public const string prefixWHDIssuingType1 = "XK";
        public const string prefixWHDIssuingType2 = "XC";
        public const string prefixWHDIssuingType3 = "XN";
        public const string prefixWHDRequisitionNote = "01REQ";
        public const string prefixPURShopBuyMaterial = "01PUR";
        public const string prefixPURShopUsed = "01SMU";
        public const string prefixWHDAdjustmentIn = "NC";
        public const string prefixWHDAdjustmentOut = "01ADO";
        public const string DataName = "20170823";
        public const long GroupStatusIssuing = 3003;
        public const long GroupStatusRequisition = 3004;                                //trạng thái phiếu xuất kho(WHD)
        public const long GroupKindIssuiForEmployee = 3006;
        public const long GroupTypeIssuingNote = 3008;
        public const long GroupWHDUrgent= 3010;
        public const long GroupReasonADO = 3011;
        public const long GroupStatusShopUsed = 3019;                                   //trạng thái phiếu tiêu hao tại Shop
        public const string ROUND_UNIT = "|sheet|pia|pc|pcs|pair|set|roll|";

        public const string TEAMLEADER = "|2|4|6|10|11|";
        public const string MANAGER = "|13|";
        public const string GroupNameENManager = "MANAGER";
        public const long groupListMaterialVPP = 3014;                                  //nhóm NVL shop có thể yêu cầu văn phòng phẩm
        public const long groupListMaterialShopOpening = 3015;                          //nhóm NVL shop khai trương có thể yêu cầu
        public const string locationCodeVPP = "VPP";
        public const string locationCodeNVL = "NVL";
        public const string locationCodeKCKD = "KCKD";
        public const string empCodeApproverVPP = "DWG0284";                             // người duyệt phiếu yêu cầu văn phòng phẩm trên (ĐẶNG TRẦN HOÀNG OANH)
        // Prefix của chương trình khuyến mãi
        public const string DBName20170508 = "20170508";
        public const string PrefixSHPPromotion = "PROMO"; // Promotion
		public const string HubGroupUserLogin = "USERLOGIN";

        // prefix khách hàng tiềm năng
        public const string PrefixSHPCustomerObject = "POCUS"; //Potential Customer

        // prefix khách hàng
        public const string PrefixSHPCustomer= "CUSTM"; //Customer

        // prefix phiếu quà tặng
        public const string PrefixSHPCustomerVoucherGift = "VOGIF"; //VoucherGift 

        // table name màn hình kỷ luật
        public const string tableNameHRDEmpDiscipline = "HRDEmpDiscipline";

        // table name màn hình phép
        public const string tableNameHRDEmpLeaveRequest = "HRDEmpLeaveRequest";

        // Duong dan luu file template Word
        public const string templateWORD = "\\WordTemplate\\";

        // Duong dan luu file template Word
        public const string templateExcel = "\\ExcelTemplate\\";

        // Duong dan tam download file WORD sau khi export
        public const string tempDownloadOfficeFile = "\\WordTemplate\\Temp\\";

        // Duong dan tam download file Excel sau khi export
        public const string tempDownloadExcelFile = "\\ExcelTemplate\\Temp\\";

        // Duong dan tam download file CSV sau khi export
        public const string tempDownloadCSVFileEmployee = "\\CSVTemplate\\Employee\\";
    }
}
