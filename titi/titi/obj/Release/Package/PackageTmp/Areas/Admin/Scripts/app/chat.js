var position = 270;
var hash = {};
var count = 0;

function Chat_user(sender, receiver) {
    if (hash[receiver] == null) {
        if (receiver != sender) {
            if (position > 800) {
                alert("Bạn được Chat tối đa là 3 người.");
                return;
            }
            if (position == 270) {
                hash[receiver] = 1;
            }
            else if (position == 520) {
                hash[receiver] = 2;
            }
            else if (position == 770) {
                hash[receiver] = 3;
            }
            ShowBoxChat(receiver, sender);
        }
    }
    else {
        return;
    }
}

// hiển thị box chat
function ShowBoxChat(receiver, sender) {
    $.ajax({
        url: "/Home/OpenBoxChat",
        method: 'POST',
        data: {
            'Sender': sender,
            'Receiver': receiver,
            'Position': position
        }
    }).done(function (data) {
        if (data.messageErr) {
            return;
        }
        $("#chatboxarea").append(data);
        $('#txtMessageSend_' + receiver).focus();
        $('#Scroll_Chat_' + receiver).scrollTop($('#Scroll_Chat_' + receiver)[0].scrollHeight);
        InitSendEventChat(sender, receiver);
        position += 250;
        ShowMessageChat();
    });
}

//khi click gửi tin nhắn
function InitSendEventChat(sender, receiver) {

    // Click send
    $("#BtnSend_" + receiver).click(function (e) {
        e.preventDefault();

        // Send message to client
        var message = $("#txtMessageSend_" + receiver).val();
        if (message.length > 0) {
            var sendToServer = $.connection.chatHub;
            $.connection.hub.start().done(function () {
                sendToServer.server.sendMessage(sender, receiver, message);
            });
        }
        $("#txtMessageSend_" + receiver).val('').focus();
    });

    // Enter trong txtMessage
    $("#txtMessageSend_" + receiver).keyup(function (e) {
        if (e.which == 13) {
            $("#BtnSend_" + receiver).click();
        }
    });

    // Remove Button
    $("#RemoveBoxChat_" + receiver).click(function (e) {
        var lastposition = 1;
        count = hash[receiver];
        delete hash[receiver];
        $("#BoxChat_" + receiver).remove();

        for (var key in hash) {
            if (hash[key] > count) {
                lastposition = 0;
                if (hash[key] == 2) {
                    position = 270;
                } else {
                    position = 520;
                }
                hash[key] = hash[key] - 1;
                $("#BoxChat_" + key).css('right', position + 'px');
                position += 250;
            }
        }
        if (lastposition == 1) {
            position -= 250;
        }
        count = 0;
    });
}

// sự kiện khi đọc tin nhắn
function UpdateNotification_Chat(Receiver) {
    $.ajax({
        url: "/Home/removeNotificationChat",
        method: 'POST',
        data: {
            'Receiver': Receiver
        }
    }).done(function (data) {
        $("#notification_chat").empty();
        $("#notification_chat").html(data);
    });
}

// hiển thị thông báo có tin nhắn tới
function ShowMessageChat() {
    $.ajax({
        url: "/Home/ShowMessageChat",
        method: 'POST'
    }).done(function (data) {
        $("#notification_chat").empty();
        $("#notification_chat").html(data);
    });
}

// khi click vào biểu tượng tin nhắn
function UpdateColChat(empCode) {
    //alert(empCode);
    $.ajax({
        url: "/Home/UpdateChat",
        method: 'POST',
        data: { EmpCode: empCode }
    }).done(function (data) {
        $("#notification_chat").empty();
        $("#notification_chat").html(data);
    });
}


function SearchUserOnline() {
    var empName = $("#SearchUserOnline").val();
    if (empName != '') {
        $.ajax({
            url: "/Home/SearchUser",
            method: 'POST',
            data: { 'EmpName': $("#SearchUserOnline").val() }
        }).done(function (data) {
            if (data.messageErr) {
                return;
            }
            $("#useronline").html(data);
        });
    }
}

$(function () {
    //var count = 1;
    var adapt = $.connection.chatHub;
    $.connection.hub.start().done(function () {
        adapt.server.getAllUserLogin();
    });

    adapt.client.getAllUserLogin = function (username, date) {
        ShowUserOnline(username, date);
        //ShowMessageChat();

        //alert(count);
        //count++;
    };

    adapt.client.sendNotify = function () {
        if (confirm("Phiên hoạt động đã hết hạn. Bạn có muốn đăng nhập để tiếp tục sử dụng!") == true) {
            window.open(window.location.origin + "\\Account\\Login", "_blank", "comma,delimited,list,of,window,features");
        }
    };

    //--------------------------------------------------
    function ShowUserOnline(username, date) {
        $.ajax({
            url: "/Home/CreateUserOnline",
            method: 'POST',
            data: {
                'listname': username,
                'listdate': date
            }
        }).done(function (data) {
            if (data.messageErr) {
                return;
            }
            $("#useronline").html(data);
        });
    }

    // Show notification have a message 
    adapt.client.getNotificationMesage = function () {
        ShowMessageChat();
    }

    // Show box chat
    adapt.client.getMessage = function (sender, receiver, msg) {
        if (sender != receiver) {
            if ($("#BoxChat_" + sender).length == 0) {
                Chat_user(receiver, sender);
            }
            setTimeout(displayMessage(sender, 'left', msg, 'active'), 1000);
        }
        else {
            if ($("#BoxChat_" + receiver).length == 0) {
                Chat_user(sender, receiver);
            }
            setTimeout(displayMessage(receiver, 'right', msg, ''), 1000);
        }
        ShowMessageChat();
    }

    // show message
    function displayMessage(receiver, classDisplay, message, active) {
        $('#Messages_' + receiver).append(
            '<li>' +
                 '<div class="' + classDisplay + '">' +
                     '<div class="author-name">' +
                         '<small class="chat-date">' +
                              formatAMPM(new Date()) +
                         '</small>' +
                     '</div>' +
                     '<div class="chat-message ' + active + '">' +
                           message +
                     '</div>' +
                 '</div>' +
             '</li> '
         );
        $('#Scroll_Chat_' + receiver).scrollTop($('#Scroll_Chat_' + receiver)[0].scrollHeight);
    }

    function formatAMPM(date) {
        var hours = date.getHours();
        var minutes = date.getMinutes();
        var ampm = hours >= 12 ? 'pm' : 'am';
        hours = hours % 12;
        hours = hours ? hours : 12;
        hours = hours < 10 ? '0' + hours : hours;
        minutes = minutes < 10 ? '0' + minutes : minutes;
        var strTime = hours + ':' + minutes + ' ' + ampm;
        return strTime;
    }
});