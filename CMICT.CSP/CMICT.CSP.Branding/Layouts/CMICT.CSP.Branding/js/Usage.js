
function search(obj, loadTime, templateName, templateID) {
    var serviceUrl = _spPageContextInfo.siteAbsoluteUrl + "/_vti_bin/CMICTServices/CMICT.svc/SaveUsAge";

    var currentUser = currentUserLoginName + ";" + currentUserName;
    currentUser = currentUser.replace("\\", "\\\\\\\\");
    var browserType = navigator.appName;
    var pageUrl = window.location.href;
    var index = pageUrl.lastIndexOf("/");
    var pageName = pageUrl.substring(index + 1);
    var index2 = pageName.lastIndexOf(".");
    pageName = pageName.substring(0, index2);

    //var loadTime = 100;
    //var templateName = "";
    //var templateID = newGuid();

    var created = "";
    var usageID = newGuid();//后台重新赋值

    var usage = "{ID:'" + usageID + "',PageName:'" + pageName + "',Url:'" + pageUrl + "',TemplateID:'" + templateID + "',TemplateName:'" + templateName +
        "',LoadTime:" + loadTime + ",BrowserType:'" + browserType + "',Query:'',Created:'" + created + "',Author:'" + currentUser + "'}";

    var liList = $("." + obj);

    var usageDetail = "";
    var usageDetailPart = "";

    if (liList != null && liList.length > 0) {
        liList.each(function () {
            var queryKey = $(this)[0].name;
            var queryValue = $(this)[0].value;

            //下拉框重新获取值
            if ($(this)[0].type != null && $(this)[0].type != undefined) {
                if ($(this)[0].type.indexOf("select") == 0) {
                    queryValue = $(this).val();
                }
            }
            usageDetailPart = "{ ID: '" + newGuid() + "', UsageID: '" + usageID + "', QueryKey: '" + queryKey + "', Value: '" + queryValue + "' }";
            usageDetail = usageDetail + "," + usageDetailPart;
        });

        usageDetail = "[" + usageDetail.substring(1) + "]";

    }
    else {
        return;
    }
    $.ajax({
        url: serviceUrl,
        data: '{ "usageJson": "' + usage + '", "detailListJson":"' + usageDetail + '" }',
        type: "post",
        contentType: "application/json",
        dataType: "json",
        success:
            function (data) {
                //alert(data.SaveUsAgeResult);
            },
        error:
            function (err) {
                //alert(err.d);
            }
    });
}

function newGuid() {
    //var guid = "";
    //for (var i = 1; i <= 32; i++) {
    //    var n = Math.floor(Math.random() * 16.0).toString(16);
    //    guid += n;
    //    if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
    //        guid += "-";
    //}
    //return guid + "";
    return "00000000-0000-0000-0000-000000000000";
}
