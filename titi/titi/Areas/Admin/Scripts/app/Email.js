
// nhận thông báo có email
function UpdateEmail(id) {
    $.ajax({
        url: "/Home/UpdateEmail",
        method: 'POST',
        data: {
            'ID': id
        }
    }).done(function (data) {
        $("#notification_mail").empty();
        $("#notification_mail").html(data);
    });
}

// click vào email
function GetNotificationEmail(toUser) {
    $.ajax({
        url: "/Home/GetNotificationEmail",
        method: 'POST',
        data: { 'ToUser': toUser }
    }).done(function (data) {
        var adapt = $.connection.emailHub;
        $.connection.hub.start().done(function () {
            adapt.server.getEmailTo(data.Result);
        });
    });
}

// khi click vào biểu tượng tin nhắn
function UpdateColEmail(empCode) {
    $.ajax({
        url: "/Home/UpdateColEmail",
        method: 'POST',
        data: { EmpCode: empCode }
    }).done(function (data) {
        $("#notification_mail").empty();
        $("#notification_mail").html(data);
    });
}

$(function () {
    var adaptEmail = $.connection.emailHub;

    $.connection.hub.start().done(function () {
        adaptEmail.server.getMyEmail();
    });

    // lấy tất cả thông báo có email tới
    adaptEmail.client.getAllNotificationEmail = function () {
        ShowNotificationEmail();
    };

    // show notification email
    function ShowNotificationEmail() {
        $.ajax({
            url: "/Home/ShowNotificationEmail",
            method: 'GET'
        }).done(function (data) {
            $("#notification_mail").html(data);
        });
    }

    // hiện thị toast có email tới
    // var adapt = $.connection.emailHub;
    adaptEmail.client.getEmailTo = function (fromUser, id) {
        toastr.options = {
            "closeButton": true,
            "debug": false,
            "progressBar": true,
            "preventDuplicates": true,
            "positionClass": "toast-bottom-right",
            "onclick": null,
            "showDuration": "400",
            "hideDuration": "10000",
            "timeOut": "30000",
            "extendedTimeOut": "1000",
            "showEasing": "swing",
            "hideEasing": "linear",
            "showMethod": "fadeIn",
            "hideMethod": "fadeOut"
        }
        toastr.info(
            "Bạn có một email từ " + fromUser
            , "Đơn nghỉ phép!"
            //,{ onclick: function () { UpdateEmail(id); } }
        );
        ShowNotificationEmail();
    }
});
