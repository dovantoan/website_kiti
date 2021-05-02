// set properties for select tag material
function SetPropComboMaterial(lstMaterial) {
    var select = new CreateCombobox();
    select.DataSource = lstMaterial;
    select.Valuemember = "MaterialCode";
    select.DisplayMember = "MaterialCode";
    select.DisplayMember2 = "Name";
    select.Title = "Nguyên vật liệu";
    //select.Disable = "disabled";

    return select;
}

// set properties for select tag item code
function SetPropComboItemProduce(lstItemCode) {
    var select = new CreateCombobox();
    select.DataSource = lstItemCode;
    select.Valuemember = "IDBOM";
    select.DisplayMember = "ItemCode";
    //select.DisplayMember2 = "Name";
    select.Title = "Sản phẩm";
    //select.Disable = "disabled";

    return select;
}

// set properties for select tag item barcode
function SetPropComboItemBarcode(lstItemBarcode) {
    var select = new CreateCombobox();
    select.DataSource = lstItemBarcode;
    select.Valuemember = "ItemPID";
    select.DisplayMember = "ItemBarcode";
    //select.DisplayMember2 = "Name";
    select.Title = "Chọn Barcode";
    //select.Disable = "disabled";

    return select;
}

// set properties for select tag employee
function SetPropComboEmployee(lstEmployee) {
    var select = new CreateCombobox();
    select.DataSource = lstEmployee;
    select.Valuemember = "EmpCode";
    select.DisplayMember = "EmpCode";
    select.DisplayMember2 = "EmpName";
    select.Title = "Chọn nhân viên";
    //select.Disable = "disabled";

    return select;
}

// set properties for select tag Material Unit
function SetPropComboMaterialUnit(lstMaterialUnit) {
    var select = new CreateCombobox();
    select.DataSource = lstMaterialUnit;
    select.Valuemember = "Symbol";
    select.DisplayMember = "Symbol";
    select.DisplayMember2 = "Description";
    select.Title = "Chọn đơn vị tính";
    //select.Disable = "disabled";

    return select;
}

// set properties for select tag section
function SetPropComboShop(lstShop) {
    var select = new CreateCombobox();
    select.DataSource = lstShop;
    select.Valuemember = "ShopCode";
    select.DisplayMember = "TradeCenterName";
    select.Title = "Chọn cửa hàng";
    //select.Disable = "disabled";

    return select;
}


// set properties for select tag Group Barcode 
function SetPropComboGroupBarcode(lstGroupBarcode) {
    var select = new CreateCombobox();
    select.DataSource = lstGroupBarcode;
    select.Valuemember = "GroupCode";
    select.DisplayMember = "Name";
    select.Title = "Chọn nhóm Barcode";
    //select.Disable = "disabled";

    return select;
}

// set properties for select tag type
function SetPropComboType(lstType) {
    var select = new CreateCombobox();
    select.DataSource = lstType;
    select.Valuemember = "Code";
    select.DisplayMember = "Value";
    select.Title = "Chọn loại";
    //select.Disable = "disabled";

    return select;
}

// set properties for select tag component
function SetPropComboComponentCode(lstComponent) {
    var select = new CreateCombobox();
    select.DataSource = lstComponent;
    select.Valuemember = "CompCode";
    select.DisplayMember = "CompCode";
    select.Title = "Chọn thành phần";
    //select.Disable = "disabled";

    return select;
}

// set properties for select tag social
function SetPropComboSocialCode(lstSocial) {
    var select = new CreateCombobox();
    select.DataSource = lstSocial;
    select.Valuemember = "Code";
    select.DisplayMember = "Value";
    select.Title = "Chọn mạng xã hội";
    //select.Disable = "disabled";

    return select;
}

//tạo text number (áp dụng cho table có số lượng từ - đến) 
function CreateTextNumberFromTo() {
    this.Id = "",
    this.fromId = "",
    this.Value = "",
    this.Event = "OnchangeTextNumber()";
    this.ReadOnly = "",
    this.Type = function () {
        return "TextNumber"
    },
    this.Result = function () {
        var text = "<input type=\"number\" onchange=\"CheckTextNumber('" + this.Id + "', '" + this.FromId + "');" + this.Event + "\" onkeypress=\"return isNumberKey(event)\" id=\"" + this.Id + "\" class=\"form-control-grid\" value=\"" + this.Value + "\" style=\"text-align:right; width:100%\" " + this.ReadOnly + "/>";
        return text;
    }
}

// chỉ cho nhập số trong thẻ input type = number
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    return !(charCode > 31 && (charCode < 48 || charCode > 57));
}

// kiểm tra dữ liệu nhập từ giờ vs tới giờ clock picker
function CheckTextNumber(id, fromId) {
    var valueTo = $("#" + id).val();
    var valueFrom = $("#" + fromId).val();
    try {
        if (valueTo < valueFrom) {
            swal("Thông báo", "Số lượng đến phải lớn hơn hoặc bằng số lượng từ! Vui lòng nhập lại", "error");
            $("#" + id).focus();
            $("#" + id).val("");
            return;
        }
    } catch (e) {
        swal("Thông báo", "Số lượng đến hoặc số lượng từ vừa nhập không hợp lệ! Vui lòng nhập lại", "error");
        $("#" + fromId).focus();
        $("#" + fromId).val("");
        return;
    }
}

// tạo textbox nhập %
function CreatePercentTextbox() {
    this.Id = "",
    this.FromId = "",
    this.Event = "",
   this.Value = "",
   this.ReadOnly = "",
   this.Type = function () {
       return "TextBox"
   },
   this.Result = function () {
       var text = "<input type=\"text\" id=\"" + (this.Id) + "\" onchange=\"OnchangeTextPercent('" + (this.Id) + "', '" + (this.FromId) + "');" + this.Event + "\" class=\"form-control-grid\" value=\"" + this.Value + "\" style=\"text-align:right; width:100%\" " + this.ReadOnly + "/>";
       return text;
   }
}

// kiểm tra dữ liệu nhập %
function OnchangeTextPercent(id, fromId) {
    try {
        var value = $("#" + id).val();
        if (value != "") {
            var x = value.split(',');
            if (x[0] > 99 || (x[1] > 99)) {
                swal("Thông báo", "Phần trăm vừa nhập không hợp lệ! Vui lòng nhập lại", "error");
                $("#" + id).focus();
                $("#" + id).val("");
                return;
            } else {
                try {
                    $("#" + fromId).val("");
                } catch (e) { }
            }
        }
    } catch (e) {
        if (value != "") {
            if (isNaN(value) || parseInt(value) < 0 || parseInt(value) > 100) {
                swal("Thông báo", "Phần trăm vừa nhập không hợp lệ! Vui lòng nhập lại", "error");
                $("#" + id).focus();
                $("#" + id).val("");
                return;
            } else {

                try {
                    $("#" + fromId).val("");
                } catch (e) { }
            }
        }
    }
}

// tạo textbox nhập tiền
function CreateAmountTextbox() {
    this.Id = "",
   this.Value = "",
    this.FromId = "",
   this.ReadOnly = "",
   this.Type = function () {
       return "TextBox"
   },
   this.Result = function () {
       var text = "<input onmouseover=\"UnformatMoney('" + (this.Id) + "')\" onmouseout=\"FormatMoney('" + (this.Id) + "')\" type=\"text\" id=\"" + (this.Id) + "\" onchange=\"OnchangeTextAmount('" + (this.Id) + "', '" + (this.FromId) + "')\" class=\"form-control-grid\" value=\"" + this.Value + "\" style=\"text-align:right; width:100%\" " + this.ReadOnly + "/>";
       return text;
   }
}

// kiểm tra dữ liệu nhập tiền
function OnchangeTextAmount(id, fromId) {
    var value = $("#" + id).val().split(',').join('');
    if (value != "") {
        if (isNaN(value) || parseInt(value) < 0) {
            swal("Thông báo", "Số tiền vừa nhập không hợp lệ! Vui lòng nhập lại", "error");
            $("#" + id).focus();
            $("#" + id).val("");
            return;
        } else {
            try {
                $("#" + fromId).val("");
            } catch (e) { }
        }
    }
}

// tạo clock picker (sử dụng cho table có cả từ giờ vs đến giờ)
function CreateClockPicker() {
    this.Id = "",
    this.Value = "",
    this.FromId = "",
    this.ReadOnly = "",
    this.Event = "",
    this.Type = function () {
        return "ClockPicker"
    },
    this.Result = function () {
        var text;
        text = "<div class=\"input-group clockpicker\" data-autoclose=\"true\">";
        text += "<input type=\"text\" class=\"form-control-grid\" onchange=\"CheckClockPicker('" + this.Id + "');OnchangeClockPicker('" + this.Id + "', '" + this.FromId + "');\"" + this.Event + "\"\" value=\"" + this.Value + "\" id=\"" + this.Id + "\">";
        text += "<span class=\"input-group-addon\">";
        text += "<span class=\"fa fa-clock-o\"></span>";
        text += "</span>";
        text += "</div>";
        return text;
    }
}

// tạo date picker (dùng cho table chỉ có 1 cột ngày)
function CreateDatePicker() {
    this.Id = "",
    this.FromId = "",
    this.Value = "",
    this.ReadOnly = "",
    this.Event = "OnchangeDatePicker()",
    this.Type = function () {
        return "DatePicker"
    },
    this.Result = function () {
        var text;
        text = "<div class=\"input-group date\">";
        text += "<span class=\"input-group-addon\"><i class=\"fa fa-calendar\"></i></span>";
        text += "<input type=\"text\" class=\"form-control-grid\" onchange=\"" + this.Event + ";CheckDatePicker('" + this.Id + "');CheckGreaterDateNow('" + this.Id + "')\" value=\"" + this.Value + "\" id=\"" + this.Id + "\" placeholder=\"dd/mm/yyyy\">";
        text += "</div>";

        return text;
    }
}

//********************* kiểm tra dữ liệu datetime picker *****************//
function isValidDate(s) {
    try {
        var bits = s.split('/');
        var d = new Date(bits[2], bits[1] - 1, bits[0]);
        return d && (d.getMonth() + 1) == bits[1];
    } catch (e) {
        return false;
    }
}

// kiểm tra dữ liệu datetime picker 
function CheckDatePicker(id) {
    var value = $("#" + id).val();
    try {
        if (value != "") {
            if (!isValidDate(value)) {
                swal("Thông báo", "Ngày vừa chọn không hợp lệ! Vui lòng chọn lại", "error");
                $("#" + id).focus();
                $("#" + id).val("");
                return;
            }
        }
    } catch (e) {

    }
}

// kiểm tra > ngày hien tại
function CheckGreaterDateNow(id) {
    var value = $("#" + id).val();
    var bits = value.split('/');
    var enteredMS = new Date(bits[2], bits[1] - 1, bits[0], 0, 0, 0).getTime();
    var twoMonthMS = new Date();
    var today = new Date(twoMonthMS.getFullYear(), twoMonthMS.getMonth(), twoMonthMS.getDate(), 0, 0, 0).getTime();

    if (enteredMS < today) {
        swal("Thông báo", "Ngày vừa chọn phải lớn hơn hoặc bằng ngày hôm nay! Vui lòng chọn lại", "error");
        $("#" + id).focus();
        $("#" + id).val("");
        return;
    }
}

//********************* kiểm tra dữ liệu clock picker *****************//
// kiểm tra dữ liệu clock picker 
function validTime(inputStr) {
    try {
        if (!inputStr || inputStr.length < 1) { return false; }
        var time = inputStr.split(':');
        return time.length === 2
               && parseInt(time[0], 10) >= 0
               && parseInt(time[0], 10) <= 23
               && parseInt(time[1], 10) >= 0
               && parseInt(time[1], 10) <= 59;
    } catch (e) {
        return false;
    }
}

// kiểm tra nhập hợp lệ clock picker
function CheckClockPicker(id) {
    var inputStr = $("#" + id).val();
    if (inputStr != "") {
        if (!validTime(inputStr)) {
            swal("Thông báo", "Giờ vừa chọn không hợp lệ! Vui lòng chọn lại", "error");
            $("#" + id).focus();
            $("#" + id).val("");
            return;
        }
    }
}

// kiểm tra dữ liệu nhập từ giờ vs tới giờ clock picker
function OnchangeClockPicker(id, fromId) {

    var clockTo = $("#" + id).val();
    var clockFrom = $("#" + fromId).val();

    if (clockFrom == "") {
        swal("Thông báo", "Vui lòng chọn từ giờ", "error");
        $("#" + fromId).focus();
        $("#" + fromId).val("");
        return;
    }

    if (clockTo == "") {
        swal("Thông báo", "Vui lòng chọn đến giờ", "error");
        $("#" + id).focus();
        $("#" + id).val("");
        return;
    }

    if (id.split('_')[0] !== fromId.split('_')[0]) {
        var inputStr = $("#" + id).val();
        var valueFrom = $("#" + fromId).val();
        try {
            var f = valueFrom.split(':');
            var t = inputStr.split(':');
            if ((t[0] < f[0]) || (t[0] == f[0] && t[1] <= f[1])) {
                swal("Thông báo", "Đến giờ phải lớn hơn từ giờ! Vui lòng chọn lại", "error");
                $("#" + id).focus();
                $("#" + id).val("");
                return;
            }
        } catch (e) {
            swal("Thông báo", "Từ giờ hoặc đến giờ vừa chọn không hợp lệ! Vui lòng chọn lại", "error");
            $("#" + fromId).focus();
            $("#" + fromId).val("");
            return;
        }
    }
}