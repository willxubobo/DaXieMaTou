var siteUrl;
var subFolderHtml = "";

function GetOrganazition() {
    var serviceUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/GetADFirstLevelUnit";
    var topOrgName = $(".topName").val();

    $.ajax({
        type: "post",
        url: serviceUrl,
        contentType: "application/json",
        dataType: "json",
        success: function (msg) {
            var strReturn = msg.GetADFirstLevelUnitResult;//get the receive json string 
            if (strReturn != "") {
                var obj = $.parseJSON(strReturn);
                var firstUnitHtml = "<li><a name=\"" + topOrgName + "#g\" href=\"javascript:;\" onclick=\"TopAClick(this);\""
                    + " class=\"userGroup topA\"><span class=\"iconRight\">" + topOrgName + "</span></a><ul class=\"userSubList\">";
                $.each(obj, function (key, value) {
                    var loginName = value.LoginName;
                    var isOU = value.TypeId == 1 ? "#g#" : "#p#";

                    var aName = loginName + isOU + value.LDAPPath;

                    firstUnitHtml = firstUnitHtml + "<li><a name=\"" + aName + "\" href=\"javascript:;\""
                        + " onclick=\"aClick(this,'" + value.LDAPPath + "',false);\" class=\"userGroup\"><span class=\"iconRight\">" + value.Name + "</span></a>";

                    firstUnitHtml = firstUnitHtml + GetSubUnitAndUser(value.LDAPPath) + "</li>";
                });
                firstUnitHtml = firstUnitHtml + "</ul></li>";

                $("#ulLeftRootFolder").children().remove();//Remove Current Tree
                //append to the ul
                $("#ulLeftRootFolder").append(firstUnitHtml);
            }
        }
    });
}

function TopAClick(aObj) {
    $(".impowerUserList a").removeClass("clickOn");
    $(".impowerUserList a").removeClass("clickOnColor");

    $(aObj).addClass("clickOn");
    $(aObj).addClass("clickOnColor");

    $(aObj).toggleClass("clickBgOpen");
    $(aObj).next("ul").toggle();

    $("#ulLeftRootFolder li").removeClass("hover");
    CloseChild(aObj);
    $("#ulLeftRootFolder li").removeClass("hover");
    $(aObj).parent("li").addClass("hover");

}

function OpenParent(obj) {

    var parentA = $(obj).parent().parent().prev("a");

    if (parentA.length != 0 && !parentA.hasClass("clickBgOpen")) {
        parentA.removeClass("clickOnColor");
        parentA.click();
        OpenParent(parentA);
        parentA.removeClass("clickOnColor");
    }

}

function CloseChild(obj) {
    $(obj).next("ul").find("a").each(function () {
        if ($(this).hasClass("clickBgOpen") && searchPerson == false) {
            $(this).removeClass("clickBgOpen");
            $(this).next("ul").css("display", "none");
        }
    });
}

function aClick(aObj, parentOULdapPath) {
    $(".impowerUserList a").removeClass("clickOn");
    if (searchPerson == false) {
        $(".impowerUserList a").removeClass("clickOnColor");
    }
    else {
        $(".impowerUserList a").each(function () {
            if ($(this).hasClass("searchOn")) {

            }
            else {
                $(this).removeClass("clickOnColor");
            }
        });
    }

    $(aObj).addClass("clickOn");
    $(aObj).addClass("clickOnColor");

    $(aObj).toggleClass("clickBgOpen");
    $(aObj).next("ul").toggle();

    $("#ulLeftRootFolder li").removeClass("hover");
    CloseChild(aObj);
    $("#ulLeftRootFolder li").removeClass("hover");
    $(aObj).parent("li").addClass("hover");
}

function GetSubUnit(liObj, parentOULdapPath, oldDisplay) {
    var siteUrl = _spPageContextInfo.siteAbsoluteUrl;
    var serverUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/GetADSubUnit";

    $.ajax({
        type: "post",
        url: serverUrl,
        data: '{"parentOULdapPath":"' + parentOULdapPath + '"}',
        contentType: "application/json",
        dataType: "json",
        success: function (msg) {
            var strReturn = msg.GetADSubUnitResult;
            if (strReturn != "") {
                var objReturnValue = $.parseJSON(strReturn);
                //Get the Children ul display
                var origianDisplay = oldDisplay;
                //each the return sub folder info and form the folder html 
                var subFolderHtml = "";

                if ($(liObj).has("ul").length == 0) {
                    subFolderHtml = subFolderHtml + "<ul class=\"userSubList\" style='display:" + origianDisplay + "'>";
                }

                $.each(objReturnValue, function (key, value) {
                    var loginName = value.LoginName;
                    var isOU = value.TypeId == 1 ? "#g#" : "#p#";

                    var aName = loginName + isOU + value.LDAPPath;

                    subFolderHtml = subFolderHtml + "<li><a name=\"" + aName + "\" href=\"javascript:;\""
                        + " onclick=\"aClick(this,'" + value.LDAPPath + "',false);\" class=\"userGroup\"><span class=\"iconRight\">" + value.Name + "</span></a></li>";
                });
                if ($(liObj).has("ul").length == 0) {
                    subFolderHtml = subFolderHtml + "</ul>";
                    $(liObj).append(subFolderHtml);

                    var ulDisplay = $(liObj).children("ul").css("display");//Get the Children Ul display
                    if (ulDisplay == "block") {
                        $(liObj).children("ul").first().css("display", "none");
                    } else {
                        $(liObj).children("ul").first().css("display", "block");
                    }
                }
                else {
                    $(liObj).children("ul").append(subFolderHtml);
                }
            }
        }
    });

}

function GetUnitUser(liObj, parentOULdapPath, oldDisplay) {
    var siteUrl = _spPageContextInfo.siteAbsoluteUrl;
    var serverUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/GetADSubUsers";

    $.ajax({
        type: "post",
        url: serverUrl,
        data: '{"parentOULdapPath":"' + parentOULdapPath + '"}',
        contentType: "application/json",
        dataType: "json",
        success: function (msg) {
            var strReturn = msg.GetADSubUsersResult;
            if (strReturn != "") {
                var objReturnValue = $.parseJSON(strReturn);
                //Get the Children ul display

                var origianDisplay = oldDisplay;

                //each the return sub folder info and form the folder html 
                var subFolderHtml = "";
                if ($(liObj).has("ul").length == 0) {
                    subFolderHtml = subFolderHtml + "<ul class=\"userSubList\" style='display:" + origianDisplay + "'>";
                }
                if (objReturnValue.length > 0) {
                    $.each(objReturnValue, function (key, value) {
                        var loginName = value.LoginName;
                        var isOU = value.TypeId == 1 ? "#g#" : "#p#";

                        var aName = loginName + isOU;


                        subFolderHtml = subFolderHtml + "<li><a name=\"" + aName + "\" href=\"javascript:;\""
                            + " onclick=\"aClick(this,'',false);\" class=\"userPeople\">" + value.Name + "</a></li>";

                    });
                    if ($(liObj).has("ul").length == 0) {
                        subFolderHtml = subFolderHtml + "</ul>";
                        $(liObj).append(subFolderHtml);

                        var ulDisplay = $(liObj).children("ul").css("display");//Get the Children Ul display
                        if (ulDisplay == "block") {
                            $(liObj).children("ul").first().css("display", "none");
                        } else {
                            $(liObj).children("ul").first().css("display", "block");
                        }
                    }
                    else {
                        $(liObj).children("ul").prepend(subFolderHtml);
                    }
                }
            }
        }
    });
}

function GetSubUnitAndUser(subFolders) {
    var subFolderHtml = "<ul class=\"userSubList\" style='display:\"block\"'>";

    $.each(subFolders, function (key, value) {
        var loginName = value.LoginName;
        var isOU = value.TypeId == 1 ? "#g#" : "#p#";

        var aName = loginName + isOU + value.LDAPPath;

        var aClassName = value.TypeId == 1 ? "userGroup" : "userPeople";

        subFolderHtml = subFolderHtml + "<li><a name=\"" + aName + "\" href=\"javascript:;\""
                           + " onclick=\"aClick(this,'');\" class=\"" + aClassName + "\">";


        if (value.TypeId == 1) {
            //ou
            subFolderHtml = subFolderHtml + "<span class=\"iconRight\">" + value.Name + "</span></a>";
        }
        else {
            //user
            subFolderHtml = subFolderHtml + value.Name + "</a>";
        }

        if (value.SubFolders != null && value.SubFolders.length > 0) {
            subFolderHtml = subFolderHtml + GetSubUnitAndUser(value.SubFolders);
        }

        subFolderHtml = subFolderHtml + "</li>";
    });
    subFolderHtml = subFolderHtml + "</ul>";
    return subFolderHtml;
}

function GetTheAllOrganization() {
    $("#txtSearchName").val("");

    $(".impowerUserBox").addClass("loadOrg");

    var serviceUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/GetTheAllOrganization";
    var topOrgName = $(".topName").val();

    $.ajax({
        type: "post",
        url: serviceUrl,
        contentType: "application/json",
        dataType: "json",
        success: function (msg) {
            $(".impowerUserBox").removeClass("loadOrg");

            var strReturn = msg.GetTheAllOrganizationResult;//get the receive json string 
            if (strReturn != "") {
                var obj = $.parseJSON(strReturn);
                var firstUnitHtml = "<li><a name=\"" + topOrgName + "#g\" href=\"javascript:;\" onclick=\"TopAClick(this);\""
                    + " class=\"userGroup topA\"><span class=\"iconRight\">" + topOrgName + "</span></a><ul class=\"userSubList\">";
                $.each(obj.Folders, function (key, value) {
                    var loginName = value.LoginName;
                    var isOU = value.TypeId == 1 ? "#g#" : "#p#";

                    var aName = loginName + isOU + value.LDAPPath;

                    firstUnitHtml = firstUnitHtml + "<li><a name=\"" + aName + "\" href=\"javascript:;\""
                        + " onclick=\"aClick(this,'" + value.LDAPPath + "');\" class=\"userGroup\"><span class=\"iconRight\">" + value.Name + "</span></a>";

                    firstUnitHtml = firstUnitHtml + GetSubUnitAndUser(value.SubFolders) + "</li>";
                });
                firstUnitHtml = firstUnitHtml + "</ul></li>";

                $("#ulLeftRootFolder").children().remove();//Remove Current Tree
                //append to the ul
                $("#ulLeftRootFolder").append(firstUnitHtml);
            }
        }
    });
}

function GetFilterADInfo(userNames) {
    $(".impowerUserBox").addClass("loadOrg");

    var siteUrl = _spPageContextInfo.siteAbsoluteUrl;
    var serviceUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/GetFilterADInfo";
    var topOrgName = $(".topName").val();

    $.ajax({
        type: "post",
        url: serviceUrl,
        data: '{"loginNames":"' + userNames + '"}',
        contentType: "application/json",
        dataType: "json",
        success: function (msg) {
            $(".impowerUserBox").removeClass("loadOrg");

            var strReturn = msg.GetFilterADInfoResult;//get the receive json string 
            if (strReturn != "") {
                var obj = $.parseJSON(strReturn);
                var firstUnitHtml = "<li><a name=\"" + topOrgName + "#g\" href=\"javascript:;\" onclick=\"TopAClick(this);\""
                    + " class=\"userGroup topA\"><span class=\"iconRight\">" + topOrgName + "</span></a><ul class=\"userSubList\">";
                $.each(obj.Folders, function (key, value) {
                    var loginName = value.LoginName;
                    var isOU = value.TypeId == 1 ? "#g#" : "#p#";

                    var aName = loginName + isOU + value.LDAPPath;

                    firstUnitHtml = firstUnitHtml + "<li><a name=\"" + aName + "\" href=\"javascript:;\""
                        + " onclick=\"aClick(this,'" + value.LDAPPath + "');\" class=\"userGroup\"><span class=\"iconRight\">" + value.Name + "</span></a>";

                    firstUnitHtml = firstUnitHtml + GetSubUnitAndUserByFilter(value.SubFolders) + "</li>";
                });
                firstUnitHtml = firstUnitHtml + "</ul></li>";

                $("#ulLeftRootFolder").children().remove();//Remove Current Tree
                //append to the ul
                $("#ulLeftRootFolder").append(firstUnitHtml);

                $("#ulLeftRootFolder").find("a").each(function () {

                    var aHtml = $(this).html();
                    aHtml = aHtml.toLowerCase();
                    if (aHtml.indexOf(userNames.toLowerCase()) > -1) {

                        OpenParent(this);
                        $(this).addClass("clickOnColor");
                        $(this).addClass("searchOn");
                    }
                });

                searchPerson = false;

                $("#ulLeftRootFolder").find(".userGroup").each(function () {

                    if ($(this).hasClass("clickBgOpen"))
                    { }
                    else
                    {
                        $(this).css("display", "none");
                    }
                });
            }
        }
    });
}

function GetSubUnitAndUserByFilter(subFolders) {
    var subFolderHtml = "<ul class=\"userSubList\" style='display:\"block\"'>";

    $.each(subFolders, function (key, value) {
        var loginName = value.LoginName;
        var isOU = value.TypeId == 1 ? "#g#" : "#p#";

        var aName = loginName + isOU + value.LDAPPath;

        var aClassName = value.TypeId == 1 ? "userGroup" : "userPeople clickOnColor";

        subFolderHtml = subFolderHtml + "<li><a name=\"" + aName + "\" href=\"javascript:;\""
                           + " onclick=\"aClick(this,'');\" class=\"" + aClassName + "\">";


        if (value.TypeId == 1) {
            //ou
            subFolderHtml = subFolderHtml + "<span class=\"iconRight\">" + value.Name + "</span></a>";
        }
        else {
            //user
            subFolderHtml = subFolderHtml + value.Name + "</a>";
        }

        if (value.SubFolders != null && value.SubFolders.length > 0) {
            subFolderHtml = subFolderHtml + GetSubUnitAndUserByFilter(value.SubFolders);
        }

        subFolderHtml = subFolderHtml + "</li>";
    });
    subFolderHtml = subFolderHtml + "</ul>";
    return subFolderHtml;
}
