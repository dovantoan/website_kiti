
//tạo ra thẻ td
function CreateTD(id, text, align, hidden) {
    var td;
    if (hidden) {
        td = "<td id=\"" + id + "\" style=\"text-align:" + align + "\" hidden>" + text + "</td>";
    }
    else {
        td = "<td id=\"" + id + "\" style=\"text-align:" + align + "\" >" + text + "</td>";
    }
    return td;
}

//tạo thẻ TR chứa các td(truyền vào 1 list TD theo thứ tự)
function CreateTR(lstTD) {
    var tr = "<tr>";
    for (var i = 0; i < lstTD.length; i++) {
        tr += "" + lstTD[i] + "";
    }
    tr += "</tr>"
    return tr;
}

// xóa một dòng trong table (truyền vào id table)
function DeleteRow(tableID) {
    try {
        var table = document.getElementById(tableID);
        var rowCount = table.rows.length;

        for (var i = 0; i < rowCount; i++) {
            var row = table.rows[i];
            var chkbox = row.cells[0].childNodes[0];
            if (null != chkbox && true == chkbox.checked) {
                table.deleteRow(i);
                rowCount--;
                i--;
            }
        }
    } catch (e) {
        console.log(e);
    }
}

// tạo mới 1 dòng dựa vào table có sẵn
function CreateRow(tableID) {
    var lstTD = [];
    var rows = $("#" + tableID + " tbody tr:first");
    var $rows = rows.each(function (index) {
        $cells = $(this).find("td");
        $cells.each(function (cellIndex) {
            var TD;
            if ($(this).attr('id') == "RowID") {
                TD = this.cloneNode(true);
                $(TD).html("-1");
            }
            else if ($(this).attr('id') != "Modify") {
                TD = this.cloneNode(true);
                $(TD).html("");
            }
            else {
                TD = this.cloneNode(true);
            }
            lstTD.push(TD);
        });
    });
    var tr = "<tr style=\"height:34px\">";
    for (var i = 0; i < lstTD.length; i++) {
        tr += "" + lstTD[i].outerHTML + "";
    }
    tr += "</tr>"
    return tr;
}

// tạo combobox
function CreateCombobox() {
    this.Id = "",
    this.DataSource = [],
    this.Valuemember = "",
    this.DisplayMember = "",
    this.DisplayMember2 = "",
    this.Value = "",
    this.Event = "CellChange()",
    this.Title = "-- Chọn --",
    this.Disable = "",
    this.Type = function () {
        return "Combobox"
    },
    this.Result = function () {
        var select = "<select onchange=\"" + this.Event + "\" class=\"chosen-select\" style=\"width:100%\" id=\"" + this.Id + "\" " + this.Disable + ">";

        //for (var i = 0; i < this.DataSource.length; i++) {
        //    if (this.DataSource[i]["" + this.DisplayMember].trim().length > oldMaxLength) {
        //        oldMaxLength = this.DataSource[i]["" + this.DisplayMember].trim().length;
        //    }
        //}

        select += "<option value=\"\"> -- " + this.Title + " -- </option>";
        for (var i = 0; i < this.DataSource.length; i++) {
            //console.log(this.DataSource[i]["" + this.DisplayMember].trim().length);
            if (this.DisplayMember2 == "") {
                if (this.DataSource[i]["" + this.Valuemember + ""] == this.Value) {
                    select += "<option value=\"" + this.DataSource[i]["" + this.Valuemember + ""] + "\" selected>" + this.DataSource[i]["" + this.DisplayMember] + "</option>";
                }
                else {
                    select += "<option value=\"" + this.DataSource[i]["" + this.Valuemember + ""] + "\">" + this.DataSource[i]["" + this.DisplayMember] + "</option>";
                }
            }
            else {
                if (this.DataSource[i]["" + this.Valuemember + ""] == this.Value) {
                    select += "<option value=\"" + this.DataSource[i]["" + this.Valuemember + ""] + "\" selected>" + this.DataSource[i]["" + this.DisplayMember] + " | " + this.DataSource[i]["" + this.DisplayMember2 + ""] + "</option>"
                }
                else {
                    select += "<option value=\"" + this.DataSource[i]["" + this.Valuemember + ""] + "\">" + this.DataSource[i]["" + this.DisplayMember] + " | " + this.DataSource[i]["" + this.DisplayMember2 + ""] + "</option>"
                }

                //if (this.DataSource[i]["" + this.Valuemember + ""] == this.Value) {
                //    select += "<option value=\"" + this.DataSource[i]["" + this.Valuemember + ""] + "\" selected>"
                //            + AlignedOptionText(oldMaxLength - this.DataSource[i]["" + this.DisplayMember].trim().length,
                //        this.DataSource[i]["" + this.DisplayMember],
                //        this.DataSource[i]["" + this.DisplayMember2 + ""])
                //            + "</option>";
                //}
                //else {
                //    select += "<option value=\"" + this.DataSource[i]["" + this.Valuemember + ""] + "\">"
                //            + AlignedOptionText(oldMaxLength - this.DataSource[i]["" + this.DisplayMember].trim().length,
                //        this.DataSource[i]["" + this.DisplayMember],
                //        this.DataSource[i]["" + this.DisplayMember2 + ""])
                //            + "</option>";
                //}
            }
        }
        //console.log(oldMaxLength);
        select += "</select>";
        //oldMaxLength = 0;
        return select;
    }
}

// tạo khoảng cách 
var oldMaxLength = 0;
function AlignedOptionText(spacesAvailable, beforeTab, afterTab) {
    //console.log(spaceAvailable);
    var spaces = "";
    for (var i = 1; i <= spacesAvailable ; i++)
        spaces += "&nbsp;";
    spaces += "&nbsp;|&nbsp;";
    return beforeTab + spaces + afterTab;
}

// tạo textbox
function CreateTextBox() {
    this.Id = "",
    this.Value = "",
    this.Event = "",
    this.ReadOnly = "",
    this.Type = function () {
        return "TextBox"
    },
    this.Result = function () {
        var text = "<input type=\"text\" onchange=\"" + this.Event + "\" id=\"" + this.Id + "\" class=\"form-control-grid\" value=\"" + this.Value + "\" style=\"text-align:left; width:100%\" " + this.ReadOnly + "/>";
        return text;
    }
}

// tạo checkbox
function CreateCheckBox() {
    this.Id = "",
    this.Value = "",
     this.ReadOnly = "",
    this.Type = function () {
        return "CheckBox"
    },
    this.Result = function () {
        var text;
        if (this.Value == "1") {
            text = "<input type=\"checkbox\" checked id=\"" + this.Id + "\"  value=\"" + this.Value + "\" " + this.ReadOnly + "/>";
        }
        else {
            text = "<input type=\"checkbox\" id=\"" + this.Id + "\" value=\"" + this.Value + "\" " + this.ReadOnly + "/>";
        }

        return text;
    }
}

//tạo text number
function CreateTextNumber() {
    this.Id = "",
    this.Value = "",
    this.ReadOnly = "",
    this.Event = "",
    this.KeyPress = "",
    this.Type = function () {
        return "TextNumber"
    },
    this.Result = function () {
        var text = "<input " + this.KeyPress + " type=\"number\" onchange=\"" + this.Event + "\" id=\"" + this.Id + "\" class=\"form-control-grid\"  value=\"" + this.Value + "\" style=\"text-align:right; width:100%\" " + this.ReadOnly + "/>";
        return text;
    }
}

//tạo text area
function CreateTextArea() {
    this.Id = "",
    this.Value = "",
    this.ReadOnly = "",
    this.Event = "",
    this.Type = function () {
        return "TextArea"
    },
    this.Result = function () {
        var text = "<textarea onchange=\"" + this.Event + "\" id=\"" + this.Id + "\" class=\"form-control-grid\" rows=\"1\" \"" + this.ReadOnly + "\"></textarea>";
        return text;
    }
}

// chỉ cho nhập số trong thẻ input type = number
function isNumberKey(evt) {
    var charCode = (evt.which) ? evt.which : event.keyCode;
    return !(charCode > 31 && (charCode < 48 || charCode > 57));
}

// tạo nút delete
function CreateButtonDelete() {
    this.Id = "",
    this.Value = "",
    this.Event = "Delete()",
    this.Type = function () {
        return "Button"
    },
    this.Result = function () {
        var del = "<a class=\"fa fa-trash-o\" style=\"color:red\" onclick=\"" + this.Event + "\"> Xóa</a>";
        return del;
    }
}

// tạo nút save
function CreateButtonSave() {
    this.Id = "",
    this.Value = "",
    this.Event = "Save()",
    this.Type = function () {
        return "Button"
    },
    this.Result = function () {
        var save = "<a class=\"fa fa-save\" onclick=\"" + this.Event + "\"> Lưu</a>";
        return save;
    }
}

//điền dữ liệu vào các cell theo row được chọn.
function FillText(tableID, ID, Text) {
    var obj = $('#' + tableID + ' tbody tr').map(function () {
        if ($(this).find('#Modify_text').prop("checked") == true) {
            $(this).find('#' + ID + '').html(Text);
        }
    });
}

// Dùng Khi thay đổi combobox
var Cell = {
    Name: "", // id
    Value: "", // gia tri valuemember
    OriginalValue: "", // gia tri truoc khi thay doi
    Text: "" // gia tri hien thi tren text
}

// đánh đấu là đã có 1 dòng được chọn.
var RowSelected = false;

function eventclickTable(tableID, lstColumn) {
    $('#' + tableID + ' tbody').on('click', 'tr', function () {
        var $row = $(this);
        var obj = $('#' + tableID + ' tbody tr').map(function () {
            if ($(this).find('#Modify_text').prop("checked") == true
                && $row.find('#Modify_text').prop("checked") == false) {
                $(this).find('#Modify_text').prop("checked", false);
                for (var i = 0; i < lstColumn.length; i++) {
                    $(this).find("#" + lstColumn[i].Key + "").html($("#" + lstColumn[i].Key + "_text").val());
                }
            }
        });
        if ($row.find('#Modify_text').prop("checked") == false) {
            $row.find('#Modify_text').prop("checked", true);
            for (var i = 0; i < lstColumn.length; i++) {
                lstColumn[i].Value.Id = lstColumn[i].Key + "_text";
                lstColumn[i].Value.Value = $row.find('#' + lstColumn[i].Key).text().trim().split("\n").join("");
                $row.find('#' + lstColumn[i].Key).html(lstColumn[i].Value.Result());

                if (lstColumn[i].Value.Type() == "Combobox") {
                    InitChosenSelect();
                    $('#' + lstColumn[i].Value.Id + '_chosen').width("100%");
                    Cell.OriginalValue = $(this).find(":selected").val();
                    $('#' + lstColumn[i].Value.Id).on('change', function () {
                        Cell.Name = $(this).attr("id").split('_text')[0];
                        Cell.Value = $(this).find(":selected").val();
                        Cell.Text = $(this).find(":selected").text();
                    });
                }
                if (lstColumn[i].Value.Type() == "ClockPicker") {
                    $('.clockpicker').clockpicker();
                }
                if (lstColumn[i].Value.Type() == "DatePicker") {
                    InitDatepicker();
                }
            }
        }
        RowSelected = true;
    });
}

//xóa các thẻ control (input, select, ...) trên table (sử dụng cho table có cột không có id tự động tăng)
function RemoveChecked(tableID, lstColumn) {
    //var obj = $('#' + tableID + ' tbody tr').map(function () {
    //    if ($(this).find('#Modify_text').prop("checked") == true) {
    //        for (var i = 0; i < lstColumn.length; i++) {
    //            $(this).find("#" + lstColumn[i] + "").html($("#" + lstColumn[i] + "_text").val());
    //        }
    //        $(this).find('#Modify_text').prop("checked", false);
    //    }
    //});
    //RowSelected = false;

    var table = $('#' + tableID).DataTable();
    table.rows().every(function () {
        var cells = this.nodes();
        if ($(cells).find('#Modify_text').prop('checked') == true) {
            for (var i = 0; i < lstColumn.length; i++) {
                $(cells).find('#' + lstColumn[i]).html($(cells).find('#' + lstColumn[i] + '_text').val());
            }
            $(cells).find('#Modify_text').prop("checked", false);
        }
    });
    RowSelected = false;
}

//load lại các combobox trên row
function LoadCombobox(tableID, lstColumn) {
    var obj = $('#' + tableID + ' tbody tr').map(function () {
        if ($(this).find('#Modify_text').prop("checked") == true) {
            for (var i = 0; i < lstColumn.length; i++) {
                lstColumn[i].Value.Id = lstColumn[i].Key + "_text";
                $(this).find('#' + lstColumn[i].Key).html(lstColumn[i].Value.Result());

                if (lstColumn[i].Value.Type() == "Combobox") {
                    $('#' + lstColumn[i].Value.Id).on('change', function () {
                        Cell.Name = $(this).attr("id").split('_text')[0];
                        Cell.Value = $(this).find(":selected").val();
                    });
                    InitChosenSelect();
                }
            }
        }
    });
}
