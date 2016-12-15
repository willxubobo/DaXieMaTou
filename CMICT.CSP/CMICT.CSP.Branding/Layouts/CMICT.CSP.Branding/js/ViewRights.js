$(document).ready(function () {
    $("#lblPageName").text("页面名称");
    var guid = "0659f2f1-235a-42c9-99b1-3ccd6f7dd752";
    BindData(guid);

});

function BindData(guid) {
    var siteUrl = _spPageContextInfo.siteAbsoluteUrl;
    var serviceUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/GetPageRight";
    var webUrl = "\/Finace";

    $.ajax({
        url: serviceUrl,
        data: '{"siteUrl":"' + siteUrl + '","webUrl":"' + webUrl + '","fileGuid":"' + guid + '" }',
        type: "post",
        contentType: "application/json",
        dataType: "json",
        success:
        function (data) {
            alert(data.GetPageRightResult);
        },
        error:
            function (err) {
                alert(err.d);
            }
    });

}