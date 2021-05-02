$(function () {
    $(".main-menu-sidebar").on("click", function (e) {
        sessionStorage.setItem("Exist", "1");

        var old_menu = sessionStorage.getItem('menu');
        var id = $(this).attr("id");

        if (old_menu !== id) {
            $('#' + old_menu).removeClass('active');
            $('#' + old_menu).children('ul').removeClass('in');
        }

        //var old_sub_menu = sessionStorage.getItem('submenu');

        //if (old_sub_menu !== id) {
        //    $('#' + old_sub_menu).removeClass('active');
        //    $('#' + old_sub_menu).children('ul').removeClass('in');
        //}

        sessionStorage.setItem("menu", id);
    });

    $(".menu-sidebar").on("click", function (e) {
        sessionStorage.setItem("Exist", "1");

        var id = $(this).attr("id");
        var parent_id = $(this).parent().parent().attr("id");

        sessionStorage.setItem("menu", parent_id);
        sessionStorage.setItem("submenu", id);
    });

    $(".menu-sidebar-child").on("click", function (e) {
        sessionStorage.setItem("Exist", "1");

        var id = $(this).attr("id");
        var parent_id = $(this).parent().parent().attr("id");
        var grandParent_id = $(this).parent().parent().parent().parent().attr("id");

        sessionStorage.setItem("menu", grandParent_id);
        sessionStorage.setItem("submenu", parent_id);
        sessionStorage.setItem("childmenu", id);
    });

    if (sessionStorage.getItem("Exist") == null) {
        sessionStorage.setItem("menu", null);
        sessionStorage.setItem("submenu", null);
        sessionStorage.setItem("childmenu", null);

        return;
    }

    var menu = sessionStorage.getItem('menu');
    var submenu = sessionStorage.getItem('submenu');
    var childmenu = sessionStorage.getItem('childmenu');

    if (menu != null && submenu != null && childmenu != null) {
        $('#' + menu).children('ul').children('#' + submenu).addClass('active');
        $('#' + menu).addClass('active');
        $('#' + menu).children('ul').addClass('in');

        $('#' + submenu).addClass('active');
        $('#' + submenu).children('ul').children('#' + childmenu).addClass('active');
        $('#' + submenu).addClass('active');
        $('#' + submenu).children('ul').addClass('in');

        $('#' + childmenu).addClass('active');
    }
});