//Show image preview before upload------------------------------------------------------------------
var loadFile = function (event) {
    $.ajax({
        url: "/Employee/GetMaxFileSizeUpload",
        method: 'POST'
    }).done(function (data) {
        var maxSize = data.Maxsize;

        var output = document.getElementById('imgpreview');

        var f = event.target.files[0];
        if (f) {
            var r = new FileReader();
            r.onload = function (e) {
                if (f.size > maxSize) {//Check for file size
                    alert('File size Greater than ' + maxSize / 1048576 + ' MB! File will not be uploaded.');
                    output.src = "/Image/Common/avatar.png";
                }
                else {
                    output.src = URL.createObjectURL(event.target.files[0]);
                }
            }
            r.readAsText(f);
        }
        else {
            alert("Failed to load file");
        }
    });
};
//------------------------------------------------------------------

//Show image preview before upload------------------------------------------------------------------
var loadFile1 = function (event) {
    $.ajax({
        url: "/Employee/GetMaxFileSizeUpload",
        method: 'POST'
    }).done(function (data) {
        var maxSize = data.Maxsize;

        var output = document.getElementById('imgpreview1');

        var f = event.target.files[0];
        if (f) {
            var r = new FileReader();
            r.onload = function (e) {
                if (f.size > maxSize) {//Check for file size
                    alert('File size Greater than ' + maxSize / 1048576 + ' MB! File will not be uploaded.');
                    output.src = "/Image/Common/avatar.png";
                }
                else {
                    output.src = URL.createObjectURL(event.target.files[0]);
                }
            }
            r.readAsText(f);
        }
        else {
            alert("Failed to load file");
        }
    });
};
//------------------------------------------------------------------

// Click change password button
function OpenChangePWView() {
    $('#loading-bar').show();
    $('#notification_changepass').show();

    $.ajax({
        url: "/Home/ChangePassWord",
        method: 'GET'
    }).done(function (data) {
        $('#Reponse_modalWindow_changepass').empty();
        $('#Reponse_modalWindow_changepass').html(data);
        $('#editModal_changepass').modal('show');

        $('#loading-bar').hide();
        $('#notification_changepass').hide();
    });
}

//save change password
function UpdatePW() {
    $.ajax({
        url: "/Home/UpdatePassWord",
        method: 'POST',
        data: {
            'Password': $('#Password').val(),
            'NewPassword': $('#NewPassword').val(),
            'ConfirmPassword': $('#ConfirmPassword').val()
        },
    }).done(function (data) {
        if (data.Error) {
            var replacedString = data.MessageErr.split("\r\n").join("</br>");
            $('#notification_edit').show();
            $('#notification_edit').html(replacedString);
            return;
        }

        $('#editModal_changepass').modal('hide');

        toastr.options = {
            "closeButton": true,
            "debug": false,
            "progressBar": true,
            "preventDuplicates": true,
            "positionClass": "toast-top-right",
            "onclick": null,
            "showDuration": "400",
            "hideDuration": "1000",
            "timeOut": "4000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }

        toastr.success(data.MessageErr, "Chú ý!");
    });
}

/*--------------- Định dạng cho tiền tệ------------*/
// format currency
function FormatMoney(id) {
    var money = $('#' + id).val();
    money = money.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    $('#' + id).val(money);
}

//unformat currency
function UnformatMoney(id) {
    var money = $('#' + id).val();
    money = money.toString().split(",").join("");
    $('#' + id).val(money);
}
/*-------------------------------------------------------*/

/*----------- Các sự kiện cho form Import ---------------*/
//Import file CSV
// truyền vào 1 url
function ImportFileCSV(url) {
    var formData = new FormData();
    formData.append("inputFile", $("#inputFile")[0].files[0]);
    $.ajax({
        url: url,
        method: "POST",
        data: formData,
        processData: false,
        contentType: false
    }).done(function (data) {
        if (data.Message) {
            swal("Thông báo", data.Message, "error");
            return;
        }
        $("#table_").html(data);
        InitTableImport();
    })
}

// init table import 
function InitTableImport() {
    var table = $('#tableImport').DataTable({
        "dom": '<"pull-right"f><"pull-left"l>tip',
        "lengthMenu": [25, 50, 100],
        "bLengthChange": true,
        //"scrollY": "300px",
        "scrollCollapse": true,
        "paging": true,
        "aaSorting": [],
        "hover": true,
        "columnDefs": [
            { "orderable": false, "targets": 0 }
        ]
    });

    // load checkbox, default was checked
    function LoadCheckedCheckbox() {
        var check = $("#Active").prop("checked") ? 1 : 0;
        if (check == 1) {
            var cells = table.cells({ filter: 'applied' }).nodes();
            $(cells).find(':checkbox').prop('checked', true);
            $(cells).find(':checkbox').prop('disabled', false);
            table.rows().every(function () {
                var cells = this.nodes();
                var errorDetail = $(cells).find("#ErrorDetail").val();
                $(cells).find("#Ticked:first").text('1');
                if (errorDetail == 1) {
                    $(cells).find(':checkbox').prop('checked', false);
                    $(cells).find(':checkbox').prop('disabled', true);
                    $(cells).find("#Ticked:first").text('0');
                }
            });
        } else {
            var cells = table.cells({ filter: 'applied' }).nodes();
            $(cells).find(':checkbox').prop('checked', false);
            $(cells).find(':checkbox').prop('disabled', true);
            $(cells).find("#Ticked:first").text('0');
        }
    }

    LoadCheckedCheckbox();

    //change checkbox
    $('#Active').change(function () {
        LoadCheckedCheckbox();
    });
}

////on change checkbox
function ChangeCheckbox(rowNumber) {
    var $rows = $('#tableImport').find("tbody tr").each(function (index) {
        if (rowNumber - 1 == index) {
            if ($(this).find("#Ticked:first").text() == 1) {
                $(this).find("#Ticked:first").text('0');
            }
            else {
                $(this).find("#Ticked:first").text('1');
            }
        }
    })
}

//// Sự kiện click vào thẻ input chọn file và button chọn file
function OpenFileImport() {
    $('#inputFile').trigger('click');
    $('#inputFile').change(function () {
        var tmppath = event.target.files[0].name;
        $('#filePath').val(tmppath);
    });
}

// sự kiện save all cho form import
// truyền vào 1 url 
function SaveAllImport(url) {
    var table = $('#tableImport').tableToJSON();

    if (table.length > 0) {
        $.ajax({
            url: url,
            method: "POST",
            data: {
                "Data": JSON.stringify(table)
            }
        }).done(function (data) {
            if (data.Message == "success") {
                swal("Thông báo", "Import dữ liệu thành công.", "success");
                $("#btnImport").trigger("click");
            }
            else {
                swal("Thông báo", "Import dữ liệu thất bại.", "error");
            }
        });
    }
    else {
        swal("Thông báo", "Vui lòng xem lại dữ liệu trước khi lưu\r\n(Nhấn nút hiển thị).", "warning");
    }
}

function SaveAllImport(url, urlExport) {

    var jsonData = MyTableToJson("#tableImport");

    $.ajax({
        url: url,
        method: "POST",
        data: {
            "Data": jsonData
        }
    }).done(function (data) {
        if (data.Message == "success") {
            //swal({
            //    title: "Thông báo",
            //    text: "Import dữ liệu thành công.",
            //    type: "success",
            //    timer: 2000,
            //    showConfirmButton: false
            //});

            $("#btnImport").trigger("click");

            //mở nút export
            //$("#btnExportExcel").removeClass("hidden");

            swal({
                title: "Thông báo",
                text: "Import thành công!\r\nBạn có muốn Xem dữ liệu vừa import thành công?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Export",
                cancelButtonText: "Thoát",
                closeOnConfirm: false
            }, function (isConfirm) {
                if (isConfirm) {
                    ExportExcelImport(urlExport);
                    swal("Thông báo", "Export dữ liệu ...", "success");
                }
            });
        }
        else {
            swal("Thông báo", "Import dữ liệu thất bại.", "error");
        }
    });
}


//Lưu 1 dòng dữ liệu trên lưới
// truyền vào 1 biến row số mấy import và 1 url
//function SaveSingleImport(rowNumber, url) {
//    swal({
//        title: "Thông báo",
//        text: "Bạn chắc chắn muốn lưu dữ liệu?",
//        type: "info",
//        showCancelButton: true,
//        confirmButtonColor: "#4baee0",
//        confirmButtonText: "Lưu",
//        cancelButtonText: "Thoát",
//        closeOnConfirm: false
//    }, function (isConfirm) {
//        if (isConfirm) {
//            $.ajax({
//                url: url,
//                method: "POST",
//                data: {
//                    "Data": tableToJSON('#tableImport', rowNumber)
//                }
//            }).done(function (data) {
//                if (data.Message == "success") {
//                    swal("Thông báo", "Lưu dữ liệu thành công", "success");
//                    $("#btnImport").trigger("click");
//                }
//                else {
//                    swal("Thông báo", "Lưu dữ liệu thất bại", "error");
//                }
//            });
//        }
//    });
//}

// xuất excel dữ liệu vừa import 
function ExportExcelImport(url) {
    location.href = url;
}

// parse table to json
function tableToJSON(tblObj, rowNumber) {
    var data = [];
    var $headers = $(tblObj).find("th");
    var $rows = $(tblObj).find("tbody tr").each(function (index) {
        if (rowNumber == index) {
            $cells = $(this).find("td");
            data[index] = {};
            $cells.each(function (cellIndex) {
                data[index][$($headers[cellIndex]).text().trim().split("\n").join("").split(",").join("")] = $(this).text().trim().split("\n").join("").split(",").join("");
            });
        }
    });
    return (JSON.stringify(data));
}

// hiển thị lỗi
// truyền vào 1 chuỗi lỗi 
function ShowError(message) {
    var replacedString = message.split("</br>").join("\r\n");
    swal("Thông báo", replacedString, "error");
}

//Lưu 1 dòng dữ liệu trên lưới
// truyền vào 1 biến row số mấy import và 1 url
function SaveSingleImport(rowNumber, url) {

    $('#tableImport tbody td').click(function () {
        var rowIndex = $(this).parent().index();
        swal({
            title: "Thông báo",
            text: "Bạn chắc chắn muốn lưu dữ liệu?",
            type: "info",
            showCancelButton: true,
            confirmButtonColor: "#4baee0",
            confirmButtonText: "Lưu",
            cancelButtonText: "Thoát",
            closeOnConfirm: false
        }, function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    url: url,
                    method: "POST",
                    data: {
                        "Data": tableToJSON('#tableImport', rowIndex)
                    }
                }).done(function (data) {
                    if (data.Message == "success") {
                        swal("Thông báo", "Lưu dữ liệu thành công", "success");
                        $("#btnImport").trigger("click");
                    }
                    else {
                        swal("Thông báo", "Lưu dữ liệu thất bại", "error");
                    }
                });
            }
        });
    })

    //swal({
    //    title: "Thông báo",
    //    text: "Bạn chắc chắn muốn lưu dữ liệu?",
    //    type: "info",
    //    showCancelButton: true,
    //    confirmButtonColor: "#4baee0",
    //    confirmButtonText: "Lưu",
    //    cancelButtonText: "Thoát",
    //    closeOnConfirm: false
    //}, function (isConfirm) {
    //    if (isConfirm) {
    //        $.ajax({
    //            url: url,
    //            method: "POST",
    //            data: {
    //                "Data": tableToJSON('#tableImport', rowNumber)
    //            }
    //        }).done(function (data) {
    //            if (data.Message == "success") {
    //                swal("Thông báo", "Lưu dữ liệu thành công", "success");
    //                $("#btnImport").trigger("click");
    //            }
    //            else {
    //                swal("Thông báo", "Lưu dữ liệu thất bại", "error");
    //            }
    //        });
    //    }
    //});
}
/*------------------------------------------------------------------------------------------*/
// Convert table to Json
// truyen vao id cua table( các header của tbale phải có ID)
function ConverTableToJson(tableID) {
    var headers = $("#" + tableID + " thead th");
    var myRows = [];
    var table = $("#" + tableID).DataTable();

    table.rows().every(function (index) {
        var cells = this.nodes();
        myRows[index] = {};
        for (i = 0; i < headers.length; i++) {
            var header = $(headers[i]).attr('id') + "";
            if (header.toString() != "undefined") {
                myRows[index][header] = $(cells).find('#' + header).text().trim().split("\n").join("");
            }
        }
    });
    return (JSON.stringify(myRows));

    //var headers = $("#" + tableID + " thead th");
    //var rows = $("#" + tableID + " tbody tr");
    //var myRows = [];
    //var $rows = rows.each(function (index) {
    //    $cells = $(this).find("td");
    //    myRows[index] = {};
    //    $cells.each(function (cellIndex) {
    //        var header = $(headers[cellIndex]).attr('id') + "";
    //        if (header.toString() != "undefined") {
    //            myRows[index][header] = $(this).text().trim().split("\n").join("");
    //        }
    //    });
    //});
    //return (JSON.stringify(myRows));

}

//Convert table to json
function MyTableToJson(tableName) {
    var table = $(tableName).DataTable();
    var $headers = $(tableName).find("th");

    var jsonData = "";

    table.rows().every(function () {
        var cells = this.nodes();
        var data = this.data();

        var cellData = "";
        // cái nào tick thì lưu
        if ($(cells).find(':checkbox').prop('checked') == true) {
            for (i = 0; i < $headers.length; i++) {
                cellData += "\"" + table.column(i).header().innerHTML.split("\"").join("").split("<").join("").split(">").join("")
                    + "\":\"" + data[i].split("\"").join("").split("<").join("").split(">").join("")
                    + "\"" + ",";
            }
            jsonData += "{" + cellData.substring(0, cellData.length - 1) + "}" + ",";
        }
    });

    jsonData = "[" + jsonData.substring(0, jsonData.length - 1) + "]";

    return jsonData;
}

//HIỂN THỊ THÔNG BÁO ĐANG XỬ LÝ
function BeginProcessMessage(title, message) {
    if (title === "") {
        title = "Thông báo";
    }
    if (message === "") {
        message = "Đang xử lý, vui lòng đợi...";
    }

    swal({
        title: title,
        text: message,
        showConfirmButton: false
    });
}


//HIỂN THỊ THÔNG BÁO KẾT THÚC QUÁ TRÌNH
function EndProcessMessage(title, message) {
    if (title === "") {
        title = "Thông báo";
    }
    if (message === "") {
        message = "Kết thúc quá trình";
    }

    swal({
        title: title,
        text: message,
        timer: 1000
    });
}

// add class name by Id
function AddClassName(listId, className) {
    var arrId = new Array();
    arrId = listId.split(';');
    for (var i = 0; i < arrId.length - 1; i++) {
        $("#" + arrId[i]).addClass(className);
    }
}

// remove class name by Id
function RemoveClassName(listId, className) {
    var arrId = new Array();
    arrId = listId.split(';');
    for (var i = 0; i < arrId.length; i++) {
        $("#" + arrId[i]).removeClass(className);
    }
}

// refresh input type text
function RefreshTextbox(listId) {
    var arrId = new Array();
    arrId = listId.split(';');
    for (var i = 0; i < arrId.length; i++) {
        $("#" + arrId[i]).val("");
    }
}



//onchange Division
function GetDepartmentByDivCode() {
    $.ajax({
        url: "/HRDSharedAction/GetDepartmentByDivCode",
        method: "POST",
        data: {
            "DivCode": $("#Division").val()
        }
    }).done(function (data) {

        var jsonObj = $.parseJSON(data.JSONresult_);
        var Department = $("#Department").val();

        var listChosenDepartment = "<select class=\"chosen-select\" style=\"width:100%\" name=\"Department\" id=\"Department\" onchange=\"GetSectionByDivDepCode()\"><option value=\"\" selected>-- Phòng ban --</option>";

        for (var i = 0; i < jsonObj.length; i++) {
            if (jsonObj[i].DepartmentPid == Department) {
                listChosenDepartment += "<option value=\"" + jsonObj[i].DepartmentPid + "\" selected> " + jsonObj[i].Department + "</option>";
            }
            else {
                listChosenDepartment += "<option value=\"" + jsonObj[i].DepartmentPid + "\"> " + jsonObj[i].Department + "</option>";
            }
        }

        listChosenDepartment += "</select>";

        $("#Department_Fill").empty();
        $("#Department_Fill").html(listChosenDepartment);

        InitChosenSelect();
    })
}

//onchange Department
function GetSectionByDivDepCode() {
    $.ajax({
        url: "/HRDSharedAction/GetSectionByDivDepCode",
        method: "POST",
        data: {
            "DepCode": $("#Department").val(),
            "DivCode": $("#Division").val()
        }
    }).done(function (data) {

        var jsonObj = $.parseJSON(data.JSONresult_);
        var Section = $("#Section").val();

        var listChosenSection = "<select class=\"chosen-select\" style=\"width:100%\" name=\"Section\" id=\"Section\"  onchange=\"GetEmployeeByDivDepSectionCode()\"><option value=\"\" selected>-- Nhóm tổ --</option>";

        for (var i = 0; i < jsonObj.length; i++) {
            if (jsonObj[i].SectionPid == Section) {
                listChosenSection += "<option value=\"" + jsonObj[i].SectionPid + "\" selected> " + jsonObj[i].Section + "</option>";
            }
            else {
                listChosenSection += "<option value=\"" + jsonObj[i].SectionPid + "\"> " + jsonObj[i].Section + "</option>";
            }
        }

        listChosenSection += "</select>";

        $("#Section_Fill").empty();
        $("#Section_Fill").html(listChosenSection);

        InitChosenSelect();
    })
}

//onchange Section (tham khảo màn hih xóa nhiều khấu trừ)
function GetEmployeeByDivDepSectionCode() {
    $.ajax({
        url: "/HRDSharedAction/GetEmployeeByDivDepSectionCode",
        method: "POST",
        data: {
            "SectionCode": $("#Section").val(),
            "DepartmentCode": $("#Department").val(),
            "DivisionCode": $("#Division").val()
        }
    }).done(function (data) {
        console.log(data);
        var jsonObj = $.parseJSON(data.JSONresult_);
        var EmpCode = $("#Employee").val();

        var listChosenEmp = "<select class=\"chosen-select\" style=\"width:100%\" name=\"Employee\" id=\"Employee\" ><option value=\"\" selected>-- Nhân viên --</option>";

        for (var i = 0; i < jsonObj.length; i++) {
            if (jsonObj[i].EMPCODE == EmpCode) {
                listChosenEmp += "<option value=\"" + jsonObj[i].EMPCODE + "\" selected> " + jsonObj[i].EMPCODE + " &nbsp;&nbsp;|&nbsp;&nbsp; " + jsonObj[i].EMPNAME + "</option>";
            }
            else {
                listChosenEmp += "<option value=\"" + jsonObj[i].EMPCODE + "\"> " + jsonObj[i].EMPCODE + " &nbsp;&nbsp;|&nbsp;&nbsp; " + jsonObj[i].EMPNAME + "</option>";
            }
        }

        listChosenEmp += "</select>";

        $("#Employee_Fill").empty();
        $("#Employee_Fill").html(listChosenEmp);

        InitChosenSelect();
    })
}

// login (login khi hết session)
// author: toandv
// create date: 08-09-2017
function login() {
    var form = $('#__AjaxAntiForgeryForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();
    $.ajax({
        url: '/Account/AccountLogin',
        type: 'POST',
        data: {
            __RequestVerificationToken: token,
            UserName: $("#username").val(),
            Password: $("#password").val(),
            RememberMe: 0,
            ReturnUrl: $("#ReturnUrl").val()
        },
        success: function (result) {
            if (!result.Error) {
                $('#loginModal').modal('hide');
            }
            else {
                $('#message-input').css({ 'display': 'block' });
                $('#message-input').text(result.messageErr);
            }
        }
    });
}