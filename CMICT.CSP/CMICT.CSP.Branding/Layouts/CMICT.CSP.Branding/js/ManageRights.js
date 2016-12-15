var isCheckIn = "已签入";
var checkInFail = "签入失败，请联系管理员！";
var serachFail = "查询失败，请联系管理员。";
var pageManage = "管理";
var pageRead = "查看";
var saveFail = "保存失败！";
var saveSuccess = "保存成功！";
var fileGuid;
var webUrl;
var person = "用户";
var group = "用户组";
var deleteFail = "删除失败";
var saveLoad = "保存中,请稍后...";
var deleteLoad = "删除中,请稍后...";
var serachLoad = "查询中,请稍后...";
var NoRightPerson = "";
var viewRightPageName = "";
var viewRightPageGuid = "";
var viewRightWebUrl = "";
var searchPerson = false;

$(document).ready(function () {
    BindSelectAllEvent();
    BindEachCboEvent();
});

/*-------------授权页面-----------------*/
function ForbitEnter() {
    if (event.keyCode == 13) {
        event.keyCode = 0;
        event.returnValue = false;
    }

}

function GetSerach() {
    ForbitEnter();
}

function ShowPermissionPage() {
    //GetOrganazition();
    GetTheAllOrganization();
    var newTableBody;
    var selectInfos = GetAllSelectInfo();

    if (selectInfos != "") {
        var arrSelectInfo = selectInfos.split(";");
        for (var index = 0; index < arrSelectInfo.length; index++) {
            if (arrSelectInfo[index] != "") {
                var pageName = arrSelectInfo[index].split("@")[0];
                var pageUrl = arrSelectInfo[index].split("@")[1];
                var fileGuid = arrSelectInfo[index].split("@")[2];
                var webUrl = arrSelectInfo[index].split("@")[3];
                var TempleatName = arrSelectInfo[index].split("@")[4];
                var CreatedTime = arrSelectInfo[index].split("@")[5];
                var UseUnit = arrSelectInfo[index].split("@")[6];
                var Author = arrSelectInfo[index].split("@")[7];

                var newTr = "<tr><td><input type=\"checkbox\"/></td><td class=\"adminColor\">"
                                + "<a class=\"aPermission\" name=\"" + fileGuid + ";" + webUrl + "\" href=\"" + pageUrl + "\" target=\"_blank\">"
                                + pageName + "</a></td><td>" + TempleatName + "</td>"
                                + "<td>" + CreatedTime + "</td><td>" + UseUnit + "</td><td>" + Author + "</td></tr>";

                newTableBody = newTableBody + newTr;
            }

        }

        var permissionHtml = $("#tablePermissionInfo").html();
        permissionHtml = permissionHtml.replace("<tbody>", "").replace("</tbody>", "");
        newTableBody = newTableBody.replace("undefined", "")

        $("#tablePermissionInfo").html(permissionHtml + newTableBody);

        BindPermissionSelectAllEvent();
        BindPermissionEachCboEvent();

        $("#cboPermissionCheckAll").click();

        var layerHeight = document.documentElement.clientHeight;
        var isFix = true;
        var offsetX = "";

        if (layerHeight > 632) {
        }
        else {
            isFix = false;
            offsetX = "-80px";
        }

        $.layer({
            type: 1, //0-4的选择,
            title: false,
            border: [0],
            closeBtn: [0, false],
            shadeClose: false,
            area: ['920px', '620px'],
            offset: [offsetX, ''],
            fix: isFix,
            shade: [0.2, '#000'],
            page: {
                dom: '#divPermissionPage'
            }
        });

        $("#txtSearchName").focus();
    }
    else {
        //var index = layer.alert("请至少选择一个页面进行授权", 9, function () {
        //    layer.close(index);
        //});
        var index = $.layer({
            area: ['auto', 'auto'],
            dialog: {
                msg: '请至少选择一个页面进行授权',
                type: 8

            },
            btns: 1,
            btn: ['确定'],
            closeBtn: false,
            yes: function () {
                layer.close(index);
            }
        });

    }


}

function SearchNameInPermissionPage() {
    searchPerson = true;
    $("#ulLeftRootFolder li").removeClass("hover");
    $("#ulLeftRootFolder a").removeClass("clickOnColor");
    var name = $("#txtSearchName").val();

    if (name == "") {
        var index = $.layer({
            area: ['auto', 'auto'],
            dialog: {
                msg: '请输入要查询的名称。',
                type: 8

            },
            btns: 1,
            btn: ['确定'],
            closeBtn: false,
            yes: function () {
                layer.close(index);
            }
        });
        return;
    }

    $("#ulLeftRootFolder").html("");
    GetFilterADInfo(name.toLowerCase());

    //$("#ulLeftRootFolder").find("a").each(function () {

    //    var aHtml = $(this).html();
    //    aHtml = aHtml.toLowerCase();
    //    if (aHtml.indexOf(name.toLowerCase()) > -1) {

    //        OpenParent(this);
    //        $(this).addClass("clickOnColor");
    //        $(this).addClass("searchOn");
    //    }
    //});

    //searchPerson = false;
}

//对选中的内容进行授权
function SetPermission() {
    var siteUrl = _spPageContextInfo.siteAbsoluteUrl;
    var serviceUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/SetPermission";

    //获取选中的要授权的页面
    var setPermissionPages = GetSelectPagesToSetRight();

    if (setPermissionPages == "") {
        //var index = layer.alert("请选中要授权的页面", 9, function () {
        //    layer.close(index);
        //});
        var index = $.layer({
            area: ['auto', 'auto'],
            dialog: {
                msg: '请选中要授权的页面',
                type: 8

            },
            btns: 1,
            btn: ['确定'],
            closeBtn: false,
            yes: function () {
                layer.close(index);
            }
        });

        return;
    }

    //获取选中的权限信息
    var userRight = GetSetPermissionInfo();

    if (userRight == "") {
        //var index = layer.alert("请选中要给该页面授予的权限", 9, function () {
        //    layer.close(index);
        //});
        var index = $.layer({
            area: ['auto', 'auto'],
            dialog: {
                msg: '请选中要给该页面授予的权限',
                type: 8

            },
            btns: 1,
            btn: ['确定'],
            closeBtn: false,
            yes: function () {
                layer.close(index);
            }
        });

        return;
    }

    if (NoRightPerson != "") {
        var confirmMsg = "您没有给" + NoRightPerson + "选择权限，继续将会删除这些角色的权限，是否继续?";

        $.layer({
            area: ['auto', 'auto'],
            dialog: {
                msg: confirmMsg,
                type: 4,
                btns: 2
            },
            btns: 2,
            btn: ['确定', '取消'],
            closeBtn: false,
            yes: function () {
                layer.load(saveLoad);

                $.ajax({
                    url: serviceUrl,
                    data: '{"siteUrl":"' + siteUrl + '","pageInfos":"' + setPermissionPages + '","userRight":"' + userRight + '"  }',
                    type: "post",
                    contentType: "application/json",
                    dataType: "json",
                    success:
                    function (data) {
                        var result = data.SetPermissionResult;
                        if (result == "success") {
                            var index = $.layer({
                                area: ['auto', 'auto'],
                                dialog: {
                                    msg: saveSuccess,
                                    type: 9

                                },
                                btns: 1,
                                btn: ['确定'],
                                closeBtn: false,
                                yes: function () {
                                    ClosePermissionPage();
                                }
                            });
                        }
                        else {
                            var index = $.layer({
                                area: ['auto', 'auto'],
                                dialog: {
                                    msg: saveFail,
                                    type: 8

                                },
                                btns: 1,
                                btn: ['确定'],
                                closeBtn: false,
                                yes: function () {
                                    layer.close(index);
                                }
                            });
                        }

                    },
                    error:
                    function (err) {
                        var index = $.layer({
                            area: ['auto', 'auto'],
                            dialog: {
                                msg: saveFail,
                                type: 8

                            },
                            btns: 1,
                            btn: ['确定'],
                            closeBtn: false,
                            yes: function () {
                                layer.close(index);
                            }
                        });
                    }
                });


            }
        });
    }
    else {
        layer.load(saveLoad);
        $.ajax({
            url: serviceUrl,
            data: '{"siteUrl":"' + siteUrl + '","pageInfos":"' + setPermissionPages + '","userRight":"' + userRight + '"  }',
            type: "post",
            contentType: "application/json",
            dataType: "json",
            success:
            function (data) {

                var result = data.SetPermissionResult;
                if (result == "success") {
                    var index = $.layer({
                        area: ['auto', 'auto'],
                        dialog: {
                            msg: saveSuccess,
                            type: 9

                        },
                        btns: 1,
                        btn: ['确定'],
                        closeBtn: false,
                        yes: function () {
                            ClosePermissionPage();
                        }
                    });
                }
                else {
                    var index = $.layer({
                        area: ['auto', 'auto'],
                        dialog: {
                            msg: saveFail,
                            type: 8

                        },
                        btns: 1,
                        btn: ['确定'],
                        closeBtn: false,
                        yes: function () {
                            layer.close(index);
                        }
                    });
                }

            },
            error:
            function (err) {
                var index = $.layer({
                    area: ['auto', 'auto'],
                    dialog: {
                        msg: saveFail,
                        type: 8

                    },
                    btns: 1,
                    btn: ['确定'],
                    closeBtn: false,
                    yes: function () {
                        layer.close(index);
                    }
                });
            }
        });
    }
}

//获取要授权的用户信息
function GetSetPermissionInfo() {
    var permissionInfo = "";
    NoRightPerson = "";
    $("#ulRightUserPermission li").each(function () {
        var userLoginName = $(this).find("p").attr("name");
        var userName = $(this).find("p").find(".userNameOverflow").html();
        var userPermission = $(this).find("input[type='checkbox'].Manage")[0].checked ? pageManage : $(this).find("input[type='checkbox'].Read")[0].checked ? pageRead : "";

        if (userPermission == "") {
            NoRightPerson = NoRightPerson + "," + userName;
        }

        permissionInfo = permissionInfo + userLoginName + "@" + userPermission + "?";
    });

    if (NoRightPerson.length > 1) {
        NoRightPerson = "<span class=\"mustIcon\" >" + NoRightPerson.substr(1) + "</span>";
    }

    return permissionInfo;
}

//获取授权子页面选中的页面信息
function GetSelectPagesToSetRight() {
    var selectStr = "";

    $("#tablePermissionInfo tr:gt(0) input[type='checkbox']:checked").each(function () {
        var aName = $(this).parent().next().find("a").attr("name");

        selectStr = selectStr + aName + "@";
    });

    return selectStr;
}

//获取授权首页选中的页面信息
function GetAllSelectInfo() {
    var selectStr = "";

    $("#tableContent tr:gt(0) input[type='checkbox']:checked").each(function () {
        var aName = $(this).parent().next().find("a").attr("name");

        //selectValue=pagename+fileUrl+fileguid+webUrl+TempleatName+CreatedTime+UseUnit+Author
        selectStr = selectStr + aName.split(';')[0] + "@" + aName.split(';')[1] + "@"
            + aName.split(';')[2] + "@" + aName.split(';')[3] + "@";
        selectStr = selectStr + $(this).parent().parent().children("td").eq("2").text() + "@";//TempleatName
        selectStr = selectStr + $(this).parent().parent().children("td").eq("3").text() + "@";//CreatedTime
        selectStr = selectStr + $(this).parent().parent().children("td").eq("4").text() + "@";//UseUnit
        selectStr = selectStr + $(this).parent().parent().children("td").eq("5").text() + ";";//Author
    });

    return selectStr;
}
//关闭页面
function ClosePermissionPage() {
    $("#tablePermissionInfo").find("tbody").html("");
    $("#ulLeftRootFolder").html("");
    $("#ulRightUserPermission").html("");
    $("#txtSearchName").val("");
    layer.closeAll();
}

//添加按钮
function addPermissionSelector() {
    var $selectOrgLi = $("#ulLeftRootFolder li.hover");
    if ($selectOrgLi.length > 0) {
        var orgName = $selectOrgLi.children("a").text();
        var orgAttrName = $selectOrgLi.children("a").attr("name");
        var isGroup = orgAttrName.split("#")[1];
        //judege whethe exist the current group
        var existNum = $("#ulRightUserPermission p[name='" + orgAttrName + "']").length;
        if (parseInt(existNum) > 0) {
            //Exist
            var index = $.layer({
                area: ['auto', 'auto'],
                dialog: {
                    msg: '您已添加过该项',
                    type: 8

                },
                btns: 1,
                btn: ['确定'],
                closeBtn: false,
                yes: function () {
                    layer.close(index);
                }
            });

            //layer.alert("您已添加过该项。", 8);
        } else {
            var newLi = "";
            //Add new row
            if (isGroup == "p") {
                newLi = "<li><p onclick=\"rightPClick(this); \" class=\"userChoiceName userPeople\" name=\"" + orgAttrName + "\""
                        + "title=\"" + orgName + "\"><span class=\"userNameOverflow\">" + orgName + "</span>"
                + "<span class=\"textOverflow\">...</span></p>"
                + "<input class=\"userChoiceIpt Manage\" type=\"checkbox\" />"
                + "<label class=\"userChoiceTxt\">管理</label>"
                + "<input class=\"userChoiceIpt Read userChoiceIptLayout\" type=\"checkbox\" />"
                + "<label class=\"userChoiceTxt\">查看</label></li>";
            }
            else {
                newLi = "<li><p onclick=\"rightPClick(this); \" class=\"userChoiceName userGroup\" name=\"" + orgAttrName + "\""
                        + "title=\"" + orgName + "\"><span class=\"userNameOverflow\">" + orgName + "</span>"
                        + "<span class=\"textOverflow\">...</span></p>"
                               + "<input class=\"userChoiceIpt Manage\" type=\"checkbox\" />"
                               + "<label class=\"userChoiceTxt\">管理</label>"
                               + "<input class=\"userChoiceIpt Read userChoiceIptLayout\" type=\"checkbox\" />"
                               + "<label class=\"userChoiceTxt\">查看</label></li>";
            }

            $("#ulRightUserPermission").append(newLi);
            BindAddULLI();
        }

    } else {
        //layer.alert("请先选中您需要添加的项。", 8);
        var index = $.layer({
            area: ['auto', 'auto'],
            dialog: {
                msg: '请先选中您需要添加的项',
                type: 8

            },
            btns: 1,
            btn: ['确定'],
            closeBtn: false,
            yes: function () {
                layer.close(index);
            }
        });
    }
}

//全部添加按钮
function addAllPermissionSelector() {
    var existOrgName = "";
    $("#ulLeftRootFolder").children("li").each(function () {
        var orgName = $(this).children("a").text();
        var orgAttrName = $(this).children("a").attr("name");

        var existNum = $("#ulRightUserPermission p[name='" + orgAttrName + "']").length;
        if (parseInt(existNum) > 0) {
            existOrgName = existOrgName + orgName + ",";
        }
    });
    if ($.trim(existOrgName).length > 0) {
        existOrgName = existOrgName.substr(0, existOrgName.length - 1);//remove last ","
    }
    if ($.trim(existOrgName) == "") {
        //No exist, Add All
        var allLis = "";
        $("#ulLeftRootFolder").children("li").each(function () {
            var orgName = $(this).children("a").text();
            var orgAttrName = $(this).children("a").attr("name");
            //orgAttrName = "o@" + orgAttrName;//whereFrom+itemType+itemguid
            //Add new Li
            var newLi = "<li><p onclick=\"rightPClick(this); \" class=\"userChoiceName userGroup\" name=\"" + orgAttrName + "\">" + orgName + "</p>"
                                + "<input class=\"userChoiceIpt Manage\" type=\"checkbox\" />"
                                + "<label class=\"userChoiceTxt\">管理</label>"
                                + "<input class=\"userChoiceIpt Read userChoiceIptLayout\" type=\"checkbox\" />"
                                + "<label class=\"userChoiceTxt\">查看</label></li>";


            allLis = allLis + newLi;
        });
        if ($.trim(allLis) != "") {
            $("#ulRightUserPermission").append(allLis);
            BindAddULLI();
        }
    } else {
        var msg = "<a style='color:red'>" + existOrgName + "</a>已添加，请删除过后再添加!";
        //layer.alert(msg, 8);
        var index = $.layer({
            area: ['auto', 'auto'],
            dialog: {
                msg: msg,
                type: 8

            },
            btns: 1,
            btn: ['确定'],
            closeBtn: false,
            yes: function () {
                layer.close(index);
            }
        });
    }
}
//全部删除按钮
function delAllPermissionSelector() {
    //var index = layer.confirm('您确认要删除所有项吗?', function (index) {
    //    $("#ulRightUserPermission li").remove();
    //    layer.close(index);
    //});
    var index = $.layer({
        area: ['auto', 'auto'],
        dialog: {
            msg: '您确认要删除所有项吗?',
            type: 4,
            btns: 2
        },
        btn: ['确定', '取消'],
        closeBtn: false,
        yes: function () {
            layer.close(index);
            $("#ulRightUserPermission li").remove();
        }
    });
}
//删除按钮
function delPermissionSelector() {
    var $selectTr = $("#ulRightUserPermission li.hover");
    if ($selectTr.length > 0) {
        var index = $.layer({
            area: ['auto', 'auto'],
            dialog: {
                msg: '您确认要删除选择的项吗?',
                type: 4,
                btns: 2
            },
            btns: 2,
            btn: ['确定', '取消'],
            closeBtn: false,
            yes: function () {
                layer.close(index);
                $selectTr.remove();

            }
        });

    } else {
        //layer.alert("请先选中您需要删除的项。", 8);
        var index = $.layer({
            area: ['auto', 'auto'],
            dialog: {
                msg: '请先选中您需要删除的项',
                type: 8

            },
            btns: 1,
            btn: ['确定'],
            closeBtn: false,
            yes: function () {
                layer.close(index);
            }
        });
    }
}

function rightPClick(obj) {
    $("#ulRightUserPermission li").removeClass("userChoiceListCheckOn");
    $(obj).parent().addClass("userChoiceListCheckOn");
}

//Bind the click event of add orginazation tr
function BindAddULLI() {
    $("#ulRightUserPermission li").each(function () {
        $(this).off();
        $(this).click(function () {
            $("#ulRightUserPermission li").removeClass("hover");
            $(this).addClass("hover");
        });

    });
}

//绑定全选
function BindPermissionSelectAllEvent() {

    $("#cboPermissionCheckAll").click(function () {
        if ($(this)[0].checked) {
            $("#tablePermissionInfo tr:gt(0) input[type='checkbox']").each(function () {
                this.checked = true;
            });
        } else {
            $("#tablePermissionInfo tr:gt(0) input[type='checkbox']").each(function () {
                this.checked = false;
            });
        }
    });


}

//Bind checkbox click event of each row             
function BindPermissionEachCboEvent() {

    $("#tablePermissionInfo tr:gt(0) input[type='checkbox']").each(function () {
        $(this).click(function () {
            var flag = false;//Flag tag to judge whether all selected
            if ($(this)[0].checked) {
                //if selected
                $("#tablePermissionInfo tr:gt(0) input[type='checkbox']").each(function () {
                    if (this.checked) {
                        flag = true;
                    } else {
                        flag = false;
                        return false;//break each
                    }
                });
                if (flag) {
                    $("#cboPermissionCheckAll")[0].checked = true;
                } else {
                    $("#cboPermissionCheckAll")[0].checked = false;
                }
            } else {
                //if unselected
                if ($("#cboPermissionCheckAll")[0].checked) {
                    $("#cboPermissionCheckAll")[0].checked = false;
                }
            }
        });
    });


}

/*-------------授权页面-----------------*/

/*-------------查询人员权限页面-----------------*/
//显示页面
function ViewPersonRights() {
    ClearPeoplePicker();
    $("#hidSearchUserLoginName").val("");

    $.layer({
        type: 1, //0-4的选择,
        title: false,
        border: [0],
        closeBtn: [0, false],
        shadeClose: false,
        area: ['580px', '375px'],
        offset: ['', ''],
        shade: [0.2, '#000'],
        zIndex: 1,
        page: {
            dom: '#divPersonalPages'
        }
    });

    //$(".adminIptPeople").find("a[title='浏览']").css("display", "none");
}

//查询该人员有权限访问的页面
function SerachPersonalRightsPages() {
    var peoplePicker = $(".peoplePicker");
    if (peoplePicker.find(".ms-entity-resolved").length == 0) {
        //var index = layer.alert("请输入您要查询的角色名称", 9, function () {
        //    layer.close(index);
        //});
        var index = $.layer({
            area: ['auto', 'auto'],
            dialog: {
                msg: '请输入您要查询的角色名称',
                type: 8

            },
            btns: 1,
            btn: ['确定'],
            closeBtn: false,
            yes: function () {
                layer.close(index);
            }
        });

        //var unresolvedSpan = peoplePicker.find(".ms-entity-unresolved");
        //if (unresolvedSpan.length > 0) {
        //    unresolvedSpan.addClass("ms-unresolvedSpan");
        //}
        return;
    }
    var name = peoplePicker.find(".ms-entity-resolved")[0].id;

    name = name.replace("\\", "@").replace("span", "");
    $("#hidSearchUserLoginName").val(name);

    var siteUrl = _spPageContextInfo.siteAbsoluteUrl;
    var serviceUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/SerachPersonRightsPages";

    var loadIndex = layer.load(serachLoad);

    $.ajax({
        url: serviceUrl,
        data: '{"siteUrl":"' + siteUrl + '","name":"' + name + '" }',
        type: "post",
        contentType: "application/json",
        dataType: "json",
        success:
        function (data) {
            var result = data.SerachPersonRightsPagesResult;
            layer.close(loadIndex);

            $("#tbPersonRight").find("tbody").html("");
            $("#tbPersonRight").next("div.noData").remove();

            if (result != "") {

                if (result.split("@")[0] == "success") {
                    var personalPages = $.parseJSON(result.split("@")[1]);

                    var newTableBody = "";

                    if (personalPages.length > 0) {
                        for (var index = 0; index < personalPages.length ; index++) {
                            var chkManageChecked = personalPages[index].PagePermission.indexOf(pageManage) > -1 ? true : false;
                            var chkReadChecked = personalPages[index].PagePermission.indexOf(pageRead) > -1 ? true : false;

                            var newTableTR = "<tr><td><a name=\"" + personalPages[index].WebUrl + ";" + personalPages[index].FileGuid
                                + "\" href=\"" + personalPages[index].Href + " \"></a>" + personalPages[index].PageName
                                + "</td><td><div style=\"display:none\" class=\"divPersonalRights\"><input type=\"checkbox\" class=\"cboManage\""
                                + " id=\"cboManage\"  value=\"" + pageManage + "\"";
                            if (chkManageChecked) {
                                newTableTR = newTableTR + " checked=\"" + chkManageChecked + "\"";
                            }
                            newTableTR = newTableTR + ">" + pageManage + "</input><input type=\"checkbox\" class=\"cboRead\" id=\"cboRead\""
                               + "  value=\"" + pageRead + "\"";
                            if (chkReadChecked) {
                                newTableTR = newTableTR + " checked=\"" + chkReadChecked + "\"";
                            }
                            newTableTR = newTableTR + ">" + pageRead + "</input></div><label class=\"lblPersonalPermission\">"
                                + personalPages[index].PagePermission + "</label></td><td><div id=\"divEditRight\">"
                                + "<a class=\"operateBtn edit\" title=\"编辑\" onclick=\"EditPersonalPageRight(this,true);\" href=\"javascript:;\"></a>"
                                + "<a class=\"operateBtn remove\" title=\"删除\" onclick=\"DeletePageRight(this);\" href=\"javascript:;\"></a></div>"
                                + "<div id=\"divSaveRight\" style=\"display:none\">"
                                + "<a class=\"operateBtn save\" title=\"保存\" onclick=\"SavePersonalPageRight(this);\" href=\"javascript:;\"></a>"
                                + "<a class=\"operateBtn cancel\" title=\"取消\" onclick=\"CancelPersonalPageRight(this);\" href=\"javascript:;\"></a></div>"
                                + "</td></tr>";

                            newTableBody = newTableBody + newTableTR;
                        }

                        var personHtml = $("#tbPersonRight").html();

                        $("#tbPersonRight").html(personHtml + newTableBody);
                    }
                    else {
                        ShowNoDataDiv();
                    }

                    BindCheckRightEvent();
                }
                else {
                    //layer.alert(serachFail, 9, function () {
                    //    layer.closeAll();
                    //});
                    var index = $.layer({
                        area: ['auto', 'auto'],
                        dialog: {
                            msg: serachFail,
                            type: 8

                        },
                        btns: 1,
                        btn: ['确定'],
                        closeBtn: false,
                        yes: function () {
                            ShowNoDataDiv();
                            layer.closeAll();
                        }
                    });
                }

            }
        },
        error:
            function (err) {
                //layer.alert(serachFail, 9, function () {
                //    layer.closeAll();
                //});
                var index = $.layer({
                    area: ['auto', 'auto'],
                    dialog: {
                        msg: serachFail,
                        type: 8

                    },
                    btns: 1,
                    btn: ['确定'],
                    closeBtn: false,
                    yes: function () {
                        ShowNoDataDiv();
                        //layer.closeAll();
                    }
                });
            }
    });

}

function ShowNoDataDiv() {
    var noDataDiv = "<div class=\"noData\">未找到该角色拥有的页面<div>";
    $("#tbPersonRight").parent().append(noDataDiv);
}

//删除对应页面的权限
function DeletePageRight(obj) {
    var confirmIndex = $.layer({
        area: ['auto', 'auto'],
        dialog: {
            msg: '您确认要删除该笔权限？',
            type: 4,
            btns: 2
        },
        btns: 2,
        btn: ['确定', '取消'],
        closeBtn: false,
        yes: function () {
            var infos = $(obj).parent().parent().parent().find("a").attr("name");
            var webUrl = infos.split(";")[0];
            var fileGuid = infos.split(";")[1];
            var userLoginName = $("#hidSearchUserLoginName").val();

            var siteUrl = _spPageContextInfo.siteAbsoluteUrl;
            var serviceUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/DeletePagePersonalRight";

            var loadIndex = layer.load(deleteLoad);

            $.ajax({
                url: serviceUrl,
                data: '{"siteUrl":"' + siteUrl + '","webUrl":"' + webUrl + '","fileGuid":"' + fileGuid + '","userLoginName":"' + userLoginName + '"  }',
                type: "post",
                contentType: "application/json",
                dataType: "json",
                success:
                function (data) {
                    var result = data.DeletePagePersonalRightResult;
                    if (result == "success") {
                        layer.close(loadIndex);
                        $(obj).parent().parent().parent().remove();

                    }
                    else {
                        var index = $.layer({
                            area: ['auto', 'auto'],
                            dialog: {
                                msg: deleteFail,
                                type: 8

                            },
                            btns: 1,
                            btn: ['确定'],
                            closeBtn: false,
                            yes: function () {
                                layer.close(index);
                            }
                        });
                    }

                },
                error:
                function (err) {
                    var index = $.layer({
                        area: ['auto', 'auto'],
                        dialog: {
                            msg: deleteFail,
                            type: 8

                        },
                        btns: 1,
                        btn: ['确定'],
                        closeBtn: false,
                        yes: function () {
                            layer.close(index);
                        }
                    });
                }
            });
        }
    });
}

//保存对应页面的权限
function SavePersonalPageRight(obj) {
    var infos = $(obj).parent().parent().parent().find("a").attr("name");
    var webUrl = infos.split(";")[0];
    var fileGuid = infos.split(";")[1];
    var userLoginName = $("#hidSearchUserLoginName").val();
    //var userRight = $(obj).parent().parent().parent().find(".cboManage")[0].checked ? pageManage : $(obj).parent().parent().parent().find(".cboRead")[0].checked ? pageRead : "";
    var userManageRight = $(obj).parent().parent().parent().find(".cboManage")[0].checked ? pageManage : "";
    var userReadRight = $(obj).parent().parent().parent().find(".cboRead")[0].checked ? pageRead : ""
    var userRight = userManageRight + "," + userReadRight;


    var loadIndex = layer.load(saveLoad);

    var siteUrl = _spPageContextInfo.siteAbsoluteUrl;
    var serviceUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/SavePagePersonalRight";

    $.ajax({
        url: serviceUrl,
        data: '{"siteUrl":"' + siteUrl + '","webUrl":"' + webUrl + '","fileGuid":"' + fileGuid + '","userLoginName":"' + userLoginName + '","userRight":"' + userRight + '"  }',
        type: "post",
        contentType: "application/json",
        dataType: "json",
        success:
        function (data) {
            var result = data.SavePagePersonalRightResult;
            if (result == "success") {
                var index = $.layer({
                    area: ['auto', 'auto'],
                    dialog: {
                        msg: saveSuccess,
                        type: 9

                    },
                    btns: 1,
                    btn: ['确定'],
                    closeBtn: false,
                    yes: function () {
                        $("#tbPersonRight").find("tbody").html("");
                        SerachPersonalRightsPages();
                        layer.close(index);
                    }
                });

            }
            else {
                var index = $.layer({
                    area: ['auto', 'auto'],
                    dialog: {
                        msg: saveFail,
                        type: 8

                    },
                    btns: 1,
                    btn: ['确定'],
                    closeBtn: false,
                    yes: function () {
                        $("#tbPersonRight").find("tbody").html("");
                        SerachPersonalRightsPages();
                        layer.close(index);
                    }
                });
            }

        },
        error:
            function (err) {
                var errorIndex = $.layer({
                    area: ['auto', 'auto'],
                    dialog: {
                        msg: saveSuccess,
                        type: 9

                    },
                    btns: 1,
                    btn: ['确定'],
                    closeBtn: false,
                    yes: function () {
                        layer.close(errorIndex);
                    }
                });

            }
    });
}


//编辑当前页面该人员的权限
function EditPersonalPageRight(obj, isEdit) {
    //编辑
    if (isEdit) {
        $(obj).parent().parent().parent().find(".lblPersonalPermission").css("display", "none");
        $(obj).parent().parent().parent().find(".divPersonalRights").css("display", "block");
        $(obj).parent().parent().find("#divEditRight").css("display", "none");
        $(obj).parent().parent().find("#divSaveRight").css("display", "block");
    }
    else {
        //保存
        $(obj).parent().parent().parent().find(".lblPersonalPermission").css("display", "block");
        $(obj).parent().parent().parent().find(".divPersonalRights").css("display", "none");
        $(obj).parent().parent().find("#divEditRight").css("display", "block");
        $(obj).parent().parent().find("#divSaveRight").css("display", "none");
    }

}

function CancelPersonalPageRight(obj) {
    EditPersonalPageRight(obj, false);
}

//清空人员选择按钮
function ClearPeoplePicker() {
    var peoplePicker = $(".peoplePicker");
    peoplePicker.find('.ms-inputuserfield').html("");
    peoplePicker.find("textarea:first").val("");
}

//关闭当前页面
function ClosePersonalPage() {
    ClearPeoplePicker();
    $("#hidSearchUserLoginName").val("");

    $("#tbPersonRight").find("tbody").html("");
    $(".ms-error").html("");
    $(".noData").css("display", "none")
    layer.closeAll();
}

/*-------------查询人员权限页面-----------------*/

/*-------------查看权限页面-----------------*/
function ViewRights(obj) {
    var objA = $(obj).parent().parent().find("a");
    if (objA != null) {
        var name = objA.attr("name");
        var info = name.split(";");
        var pageName = info[0];
        var url = info[1];
        fileGuid = info[2];
        webUrl = info[3];

        viewRightPageGuid = fileGuid;
        viewRightPageName = pageName;
        viewRightWebUrl = webUrl;

        GetPageRight(pageName, fileGuid, webUrl);
    }

}

//获取该页面的权限信息
function GetPageRight(pageName, fileGuid, webUrl) {
    var siteUrl = _spPageContextInfo.siteAbsoluteUrl;
    var serviceUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/GetPageRight";

    $.ajax({
        url: serviceUrl,
        data: '{"siteUrl":"' + siteUrl + '","webUrl":"' + webUrl + '","fileGuid":"' + fileGuid + '" }',
        type: "post",
        contentType: "application/json",
        dataType: "json",
        success:
        function (data) {
            var result = data.GetPageRightResult;
            if (result != "") {
                var resultInfo = result.split("@");

                if (resultInfo[0] == "success") {
                    var pageInfo = resultInfo[1];
                    var objInfo = $.parseJSON(pageInfo);
                    var newDiv = "<script type=\"text/javascript\">$(document).ready(function () { BindCheckRightEvent();});</script>" +
                        "<div class=\"administrationAuthority\" ><div class=\"adminBox\"><div class=\"adminBoxTitle\">"
                        + "<p class=\"adminTitle\">页面名称：" + pageName + "</p></div>";


                    var newTable = "<div id=\"divContent\" class=\"adminTableBox\">" +
                            "<table id=\"tbPageRightRead\" class=\"tableFour\"><thead><tr><th>角色名称</th><th>角色类别</th>" +
                            "<th>权限级别</th></tr></thead>";
                    var editNewTable = "<div id=\"divEditContent\" class=\"adminTableBox\" style=\"display:none\">" +
                    "<table id=\"tbPageRightEdit\" class=\"tableFour\"><thead><tr><th>角色名称</th><th>角色类别</th>" +
                    "<th colspan:2>权限级别</th><th>操作</th></tr></thead>";

                    $.each(objInfo, function (index, value) {
                        //只读
                        var userName = index.split("#")[0];
                        var userLoginName = index.split("#")[1];
                        var isGroup = index.split("#")[2];
                        var type = isGroup == "g" ? group : person;
                        var tableTD = "<tr><td name=\"" + userLoginName + "#" + isGroup + "\">" + userName +
                            "</td><td>" + type + "</td><td>" + value + "</td></tr>";
                        newTable = newTable + tableTD;

                        //编辑
                        var editTableTD = "<tr><td class=\"tdEditName\" name=\"" + userLoginName + "#" +
                            isGroup + "\">" + userName + "</td><td>" + type + "</td>" +
                            "<td><input type=\"checkbox\" class=\"cboManage\" id=\"cboManage\" value=\"" + pageManage + "\"";
                        if (value.indexOf(pageManage) > -1) {
                            editTableTD = editTableTD + "checked=\"true\"";
                        }
                        editTableTD = editTableTD + ">" + pageManage + "<input type=\"checkbox\" class=\"cboRead\" id=\"cboRead\" value=\"" + pageRead + "\"";
                        if (value.indexOf(pageRead) > -1) {
                            editTableTD = editTableTD + "checked=\"true\"";
                        }
                        editTableTD = editTableTD + ">" + pageRead + "</td><td><a class=\"operateBtn remove\" title=\"删除\"  onclick=\"DeletePersonRight(this);\" href=\"javascript:;\"></a></td></tr>";

                        editNewTable = editNewTable + editTableTD;

                    });


                    newTable = newTable + "</table></div>";
                    editNewTable = editNewTable + "</table></div>";

                    var divButtons = "<div id=\"divEditButtons\" class=\"operateBtnBox operateBtnLayout\">"
                    + "<a class=\"guideBtn nextLink\" href=\"javascript:;\" onclick=\"editPageRight(true);\">编辑</a>"
                    + "<a class=\"guideBtn prevLink\" href=\"javascript:;\" onclick=\"CloseViewRightsPage();\">关闭</a>"
                    + "</div><div id=\"divSaveButtons\" class=\"operateBtnBox operateBtnLayout\" style=\"display:none;\">"
                    + "<a class=\"guideBtn finishLink\" href=\"javascript:;\" onclick=\"UpdatePageRights();\">保存</a>"
                    + "<a class=\"guideBtn prevLink\" href=\"javascript:;\" onclick=\"editPageRight();\">取消</a>";
                    +"<a class=\"guideBtn prevLink\" href=\"javascript:;\" onclick=\"CloseViewRightsPage();\">关闭</a></div>";

                    newDiv = newDiv + newTable + editNewTable + divButtons + "<input type=\"hidden\" id=\"hidDeleteRightsUser\" /></div>";

                }

                layer.closeAll();

                $.layer({
                    type: 1, //0-4的选择,
                    title: false,
                    border: [0],
                    closeBtn: [0, false],
                    shadeClose: false,
                    area: ['580px', '375px'],
                    offset: ['', ''],
                    shade: [0.2, '#000'],
                    page: {
                        html: newDiv
                    }
                });

            }
        },
        error:
            function (err) {
                //layer.alert(checkInFail);
            }
    });

}

//删除该笔权限信息
function DeletePersonRight(obj) {
    var index = $.layer({
        area: ['auto', 'auto'],
        dialog: {
            msg: '您确认要删除该笔权限?',
            type: 4,
            btns: 2
        },
        btns: 1,
        btn: ['确定', '取消'],
        closeBtn: false,
        yes: function () {
            var userLoginName = $(obj).parent().parent().find(".tdEditName").attr("name");

            var siteUrl = _spPageContextInfo.siteAbsoluteUrl;
            var serviceUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/DeletePagePersonalRight";

            var loadIndex = layer.load(deleteLoad);

            $.ajax({
                url: serviceUrl,
                data: '{"siteUrl":"' + siteUrl + '","webUrl":"' + webUrl + '","fileGuid":"' + fileGuid + '","userLoginName":"' + userLoginName + '"  }',
                type: "post",
                contentType: "application/json",
                dataType: "json",
                success:
                function (data) {
                    var result = data.DeletePagePersonalRightResult;
                    if (result == "success") {
                        layer.close(loadIndex);
                        $(obj).parent().parent().remove();
                        $("#tbPageRightRead").find("tbody tr").each(function () {
                            var tdRead = $(this).find("td[name='" + userLoginName + "']");
                            if (tdRead != null) {

                                tdRead.parent().remove();
                            }
                        });
                    }
                    else {
                        var index = $.layer({
                            area: ['auto', 'auto'],
                            dialog: {
                                msg: deleteFail,
                                type: 8

                            },
                            btns: 1,
                            btn: ['确定'],
                            closeBtn: false,
                            yes: function () {
                                layer.close(index);
                            }
                        });
                    }

                },
                error:
                function (err) {
                    var index = $.layer({
                        area: ['auto', 'auto'],
                        dialog: {
                            msg: deleteFail,
                            type: 8

                        },
                        btns: 1,
                        btn: ['确定'],
                        closeBtn: false,
                        yes: function () {
                            layer.close(index);
                        }
                    });
                }
            });

            $(obj).parent("td").parent("tr").css("display", "none");
            layer.close(index);
        }
    });
}

//保存页面权限信息
function UpdatePageRights() {
    var pageRights = GetAllInfo();

    var siteUrl = _spPageContextInfo.siteAbsoluteUrl;
    var serviceUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/UpdatePagePersonRight";

    var deleteUserInfo = $("#hidDeleteRightsUser").val();

    layer.load(saveLoad);

    $.ajax({
        url: serviceUrl,
        data: '{"siteUrl":"' + siteUrl + '","webUrl":"' + webUrl + '","fileGuid":"' + fileGuid + '","pageRights":"' + pageRights + '" ,"deleteUserInfo":"' + deleteUserInfo + '" }',
        type: "post",
        contentType: "application/json",
        dataType: "json",
        success:
        function (data) {
            var result = data.UpdatePagePersonRightResult;
            $("#hidDeleteRightsUser").val("");
            if (result == "success") {
                //var index = layer.alert(saveSuccess, 9, function () {
                //    layer.close(index);
                //    GetPageRight(viewRightPageName, viewRightPageGuid, viewRightWebUrl);
                //});
                var index = $.layer({
                    area: ['auto', 'auto'],
                    dialog: {
                        msg: saveSuccess,
                        type: 9

                    },
                    btns: 1,
                    btn: ['确定'],
                    closeBtn: false,
                    yes: function () {
                        layer.close(index);
                        GetPageRight(viewRightPageName, viewRightPageGuid, viewRightWebUrl);
                    }
                });
            }
            else {
                //var index = layer.alert(saveFail, 9, function () {
                //    layer.close(index);
                //    GetPageRight(viewRightPageName, viewRightPageGuid, viewRightWebUrl);
                //});
                var index = $.layer({
                    area: ['auto', 'auto'],
                    dialog: {
                        msg: saveFail,
                        type: 8

                    },
                    btns: 1,
                    btn: ['确定'],
                    closeBtn: false,
                    yes: function () {
                        layer.close(index);
                        GetPageRight(viewRightPageName, viewRightPageGuid, viewRightWebUrl);
                    }
                });
            }

        },
        error:
        function (err) {
            $("#hidDeleteRightsUser").val("");
            //var index = layer.alert(saveFail, 9, function () {
            //    layer.close(index);
            //});
            var index = $.layer({
                area: ['auto', 'auto'],
                dialog: {
                    msg: saveFail,
                    type: 8

                },
                btns: 1,
                btn: ['确定'],
                closeBtn: false,
                yes: function () {
                    layer.close(index);
                }
            });
        }
    });

}

//编辑
function editPageRight(isEdit) {
    if (isEdit) {
        $("#divEditContent").css("display", "block");
        $("#divContent").css("display", "none");
        $("#divSaveButtons").css("display", "block");
        $("#divEditButtons").css("display", "none");
    }
    else {
        //将隐藏的行显示
        var editTable = $("#divEditContent").find("table");
        editTable.find("tr").each(function () {
            $(this).css("display", "table-row");
        });

        $("#hidDeleteRightsUser").val("");

        $("#divEditContent").css("display", "none");
        $("#divContent").css("display", "block");

        $("#divSaveButtons").css("display", "none");
        $("#divEditButtons").css("display", "block");
    }
}

function CloseViewRightsPage() {
    editPageRight();
    layer.closeAll();
}

//获取该页面的权限信息
function GetAllInfo() {
    var pageInfo = "";

    $("#tbPageRightEdit tbody tr").each(function () {
        var userName = $(this).find(".tdEditName").html();
        var userLoginName = $(this).find(".tdEditName").attr("name");
        var userManageRight = $(this).find(".cboManage")[0].checked ? pageManage : "";
        var userReadRight = $(this).find(".cboRead")[0].checked ? pageRead : ""
        var userRight = userManageRight + "," + userReadRight;

        //var userRight = $(this).find(".cboManage")[0].checked ? pageManage : $(this).find(".cboRead")[0].checked ? pageRead : "";
        pageInfo = pageInfo + ";" + userLoginName + "#" + userRight;
    });

    return pageInfo.substr(1);
}

//绑定页面权限单选
function BindCheckRightEvent() {

    //$(".cboManage").each(function () {
    //    $(this).click(function () {
    //        if ($(this)[0].checked) {
    //            $(this).next()[0].checked = false;
    //        }
    //    });
    //});
    //$(".cboRead").each(function () {
    //    $(this).click(function () {
    //        if ($(this)[0].checked) {
    //            $(this).prev()[0].checked = false;
    //        }
    //    });
    //});
}
/*-------------查看权限页面-----------------*/

/*-------------授权主页面-----------------*/
//签入页面
function CheckIn(obj, fileGuid, webUrl) {
    var siteUrl = _spPageContextInfo.siteAbsoluteUrl;
    var serviceUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/CheckInFile";

    var loadIndex = layer.load("签入中，请稍后...");

    $.ajax({
        url: serviceUrl,
        data: '{"siteUrl":"' + siteUrl + '","webUrl":"' + webUrl + '","fileGuid":"' + fileGuid + '" }',
        type: "post",
        contentType: "application/json",
        dataType: "json",
        success:
        function (data) {
            var result = data.CheckInFileResult;
            layer.close(loadIndex);
            if (result == "success") {
                //将页面显示改为已签入
                $(obj).parent().prev().html(isCheckIn);
                $(obj).css("display", "none");
            }
        },
        error:
        function (err) {
            //layer.alert(checkInFail);
            var index = $.layer({
                area: ['auto', 'auto'],
                dialog: {
                    msg: checkInFail,
                    type: 8

                },
                btns: 1,
                btn: ['确定'],
                closeBtn: false,
                yes: function () {
                    layer.close(index);
                }
            });
        }
    });
}

//绑定全选
function BindSelectAllEvent() {

    $("#cboCheckAll").click(function () {
        if ($(this)[0].checked) {
            $("#tableContent tr:gt(0) input[type='checkbox']").each(function () {
                this.checked = true;
            });
        } else {
            $("#tableContent tr:gt(0) input[type='checkbox']").each(function () {
                this.checked = false;
            });
        }
    });


}

//Bind checkbox click event of each row             
function BindEachCboEvent() {

    $("#tableContent tr:gt(0) input[type='checkbox']").each(function () {
        $(this).click(function () {
            var flag = false;//Flag tag to judge whether all selected
            if ($(this)[0].checked) {
                //if selected
                $("#tableContent tr:gt(0) input[type='checkbox']").each(function () {
                    if (this.checked) {
                        flag = true;
                    } else {
                        flag = false;
                        return false;//break each
                    }
                });
                if (flag) {
                    $("#cboCheckAll")[0].checked = true;
                } else {
                    $("#cboCheckAll")[0].checked = false;
                }
            } else {
                //if unselected
                if ($("#cboCheckAll")[0].checked) {
                    $("#cboCheckAll")[0].checked = false;
                }
            }
        });
    });


}

function changesizeright(pagesize) {
    $(".hidpagesize").val(pagesize);
    $(".searchRight").click();
    //setvalueselsearch(pagesize);
}

function setvalueselsearch(pagesize) {
    if ($(".pageBox").length > 0) {
        //$("#<%=pagerdiv.ClientID%>").find("select").val(obj);
        $(".pageBox").find(".dk_option_current").attr("class", "");
        $(".pageBox").find("a").each(function () {
            if ($(this).attr("class") != "dk_toggle dk_label") {
                if ($(this).attr("data-dk-dropdown-value") != undefined && $(this).attr("data-dk-dropdown-value") != null && $(this).attr("data-dk-dropdown-value") != "") {
                    if ($.trim($(this).attr("data-dk-dropdown-value")) == pagesize) {
                        $(this).parent().attr("class", "dk_option_current");
                        $(this).parent().parent().parent().prev().text($(this).text());
                    }
                }

            }
        });
    }
}
/*-------------授权主页面-----------------*/


