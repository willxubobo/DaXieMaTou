$(document).ready(function () {
    wcfTest();
});

function wcfTest() {
    var serviceUrl = _spPageContextInfo.siteAbsoluteUrl + "/_vti_bin/CMICTServices/CMICT.svc/Dowork";
    $.ajax({
        url: serviceUrl,
        type: "post",
        contentType: "application/json",
        dataType: "json",
        success:
            function (data) {
                alert(data.DoWorkResult);
            },
        error:
            function (err) {
                alert(err.d);
            }
    });
}