/// <reference path="jquery-1.7.1-vsdoc.js" />

function getdata() {

    $.ajax({
        type: "POST",
        contentType: "application/json; charset=utf-8",
        url: "../AppCounterService.asmx/GetLandInfoCounts",
        data: "{}",
        dataType: "json",
        success: function (data) {
            $("#tbLandInfoCount").replaceWith(data);
        },
        error: function (jqXHR, textStatus, errorThrown) {
            alert(errorThrown);
        }
    });
    setInterval(getdata, 5000);
}
setInterval(getdata, 5000);