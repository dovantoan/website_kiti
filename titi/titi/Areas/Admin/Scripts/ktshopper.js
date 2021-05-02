function PaggingTemplate(totalPage, currentPage,selectedPage) {
    var template = "";
    var TotalPages = totalPage;
    var CurrentPage = currentPage;
    var PageNumberArray = Array();


    var countIncr = 1;
    for (var i = currentPage; i <= totalPage; i++) {
        PageNumberArray[0] = currentPage;
        if (totalPage != currentPage && PageNumberArray[countIncr - 1] != totalPage) {
            PageNumberArray[countIncr] = i + 1;
        }
        countIncr++;
    };
    PageNumberArray = PageNumberArray.slice(0, 5);
    var FirstPage = 1;
    var LastPage = totalPage;
    if (totalPage != currentPage) {
        var ForwardOne = currentPage + 1;
    }
    var BackwardOne = 1;
    if (currentPage > 1) {
        BackwardOne = currentPage - 1;
    }

    template = "<p>" + CurrentPage + " of " + TotalPages + " pages</p>"
    template = template + '<ul class="pager">' +
        '<li class="previous"><a href="#" onclick="GetPageData(' + FirstPage + ')"><i class="fa fa-fast-backward"></i>&nbsp;First</a></li>' +
        '<li class="float-left"><select ng-model="pageSize" id="selectedId" style="height:29px;border-radius: 8px"><option value="20">20</option><option value="50">50</option><option value="100">100</option><option value="150">150</option></select> </li>' +
        '<li class="float-left"><a href="#" onclick="GetPageData(' + BackwardOne + ')"><i class="glyphicon glyphicon-backward"></i></a>';

    var numberingLoop = "";
    for (var i = 0; i < PageNumberArray.length; i++) {
        numberingLoop = numberingLoop + '<a class="page-number active" onclick="GetPageData(' + PageNumberArray[i] + ')" href="#">' + PageNumberArray[i] + ' &nbsp;&nbsp;</a>';
    }
    template = template + numberingLoop + '<a href="#" onclick="GetPageData(' + ForwardOne + ')" ><i class="glyphicon glyphicon-forward"></i></a></li>' +
        '<li class="next float-left"><a href="#" onclick="GetPageData(' + LastPage + ')">Last&nbsp;<i class="fa fa-fast-forward"></i></a></li></ul>';
    $("#paged").append(template);
    $("#selectedId option[value='" + selectedPage + "']").attr('selected', true);
    $('#selectedId').change(function () {
        GetPageData(1, $(this).val());
    });
}

var highlightActiveMenuItem = function () {
    var url = window.location.pathname;
    $('#side-menu').find("li").removeClass('active');
    $('#side-menu').find("ul").removeClass('in');
    var el = $('a[href="' + url + '"]').parent();
    $(el).addClass('active');
    $(el).parent().addClass('in');
    $(el).parent().parent().addClass('active');
};