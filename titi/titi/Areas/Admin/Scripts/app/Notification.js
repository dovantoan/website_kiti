
// nhận thông báo 
//function GetNotification(toUser) {
//    $.ajax({
//        url: "/Home/GetNotification",
//        method: 'POST',
//        data: { 'ToUser': toUser }
//    }).done(function (data) {
//        var adapt = $.connection.notificationHub;
//        $.connection.hub.start().done(function () {
//            adapt.server.getNotificationTo(data.Result);
//        });
//    });
//}

//// khi click vào biểu tượng thông báo
//function UpdateColNotification(empCode) {
//    //alert(empCode);
//    $.ajax({
//        url: "/Home/UpdateColNotification",
//        method: 'POST',
//        data: { EmpCode: empCode }
//    }).done(function (data) {
//        $("#notification_noti").empty();
//        $("#notification_noti").html(data);
//    });
//}
//// click vào thông báo
//function UpdateNotification(ID) {
//    $.ajax({
//        url: "/Home/UpdateNotification",
//        method: 'POST',
//        data: {
//            'ID': ID
//        }
//    }).done(function (data) {
//        $("#notification_noti").empty();
//        $("#notification_noti").html(data);
//        $(location).attr("href", "/TimerMachine/ShowAttendance");
//    });
//}

//$(function () {
//    var adapt = $.connection.notificationHub;

//    $.connection.hub.start().done(function () {
//        adapt.server.getMyNotification();
//    });

//    // lấy tất cả thông báo tới
//    adapt.client.getAllNotification = function () {
//        ShowNotification();
//    };

//    // ShowOnline--------------------------------------------------------------------
//    adapt.client.getUserLogin = function (fromUser, receiver) {
//            toastr.options = {
//                "closeButton": true,
//                "debug": false,
//                "progressBar": true,
//                "preventDuplicates": true,
//                "positionClass": "toast-bottom-right",
//                "showDuration": "400",
//                "hideDuration": "10000",
//                "timeOut": "30000",
//                "extendedTimeOut": "1000",
//                "showEasing": "swing",
//                "hideEasing": "linear",
//                "showMethod": "fadeIn",
//                "hideMethod": "fadeOut",
//                onclick: function () { Chat_user($('#Sender').val(), receiver); }
//            }
//            toastr.info(
//                 fromUser + "\nVừa Online"

//             );
  
//    }
    
    
//    //show off------------------------------------------------------
//    adapt.client.GetUserOffline = function (fromUser) {
//        toastr.options = {
//            "closeButton": true,
//            "debug": false,
//            "progressBar": true,
//            "preventDuplicates": true,
//            "positionClass": "toast-bottom-right",
//            "onclick": null,
//            "showDuration": "400",
//            "hideDuration": "10000",
//            "timeOut": "30000",
//            "extendedTimeOut": "1000",
//            "showEasing": "swing",
//            "hideEasing": "linear",
//            "showMethod": "fadeIn",
//            "hideMethod": "fadeOut"
//        }
//        toastr.info(
//             fromUser + "\nVừa Offline"
//            //,{ onclick: function () { UpdateNotification(id); } }
//         );
//    }

//    // show notification
//    function ShowNotification() {
//        $.ajax({
//            url: "/Home/ShowNotification",
//            method: 'POST'
//        }).done(function (data) {
//            $("#notification_noti").html(data);
//        });
//    }

//    // hiện thị toast có thông báo tới
//    adapt.client.getNotificationTo = function (fromUser, id) {
//        toastr.options = {
//            "closeButton": true,
//            "debug": false,
//            "progressBar": true,
//            "preventDuplicates": true,
//            "positionClass": "toast-bottom-right",
//            "onclick": null,
//            "showDuration": "400",
//            "hideDuration": "10000",
//            "timeOut": "30000",
//            "extendedTimeOut": "1000",
//            "showEasing": "swing",
//            "hideEasing": "linear",
//            "showMethod": "fadeIn",
//            "hideMethod": "fadeOut"
//        }
//        toastr.info(
//            "Bạn có một thông báo từ " + fromUser
//            , "Duyệt dữ liệu chấm công!"
//         );
//    }
//});
