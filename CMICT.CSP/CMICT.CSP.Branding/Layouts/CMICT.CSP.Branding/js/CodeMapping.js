var siteUrl = _spPageContextInfo.siteAbsoluteUrl;
var saveFail = "保存失败！";
var saveSuccess = "保存成功！";
var deleteFail = "删除失败";
var deleteSuccess = "删除成功";
var saveLoad = "保存中,请稍后...";
var deleteLoad = "删除中,请稍后...";
var isUpdate = false;

$(document).ready(function () {
    BindSelectAllEvent();
    BindEachCboEvent();
    $(".upload").css("display", "none");
});

function ClosePage() {
    $("#divCodeMapping").find("input").each(function () {
        $(this).removeAttr("disabled");
    });

    $(".mappingTable").find("div").each(function () {
        $(this).html("");
    });
    layer.closeAll();
}

//批量导入
function OpenBatchImport() {
    $.layer({
        type: 1, //0-4的选择,
        title: false,
        border: [0],
        closeBtn: [0, false],
        shadeClose: false,
        area: ['510px', '150px'],
        offset: ['', ''],
        shade: [0.2, '#000'],
        zIndex: 1,
        page: {
            dom: '#divImport'
        }
    });
}

function CloseImportPage() {
    $("#txtFileName").val("");
    $("#fileUpload").select();
    layer.closeAll();
}

function ImportExcel() {
    var filePath = $(".fileName").val();

    if (filePath == "") {
        var index = $.layer({
            area: ['auto', 'auto'],
            dialog: {
                msg: '请选择文件后再导入。',
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
    else {
        var fileTypeIndex = filePath.lastIndexOf('.');
        var fileType = filePath.substring(fileTypeIndex + 1);

        if (fileType != "xls" && fileType != "xlsx") {
            var index = $.layer({
                area: ['auto', 'auto'],
                dialog: {
                    msg: '请选择Excel文件后再导入。',
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

        filePath = encodeURI(filePath);
    }
    $("#hidCurrentUser").val(currentUserLoginName + ";" + currentUserName);

    layer.load("导入中,请稍后...");

    $(".upload").click();
}

function UploadSuccess() {
    var index = $.layer({
        area: ['auto', 'auto'],
        dialog: {
            msg: '文件导入成功。',
            type: 9
        },
        btns: 1,
        btn: ['确定'],
        closeBtn: false,
        yes: function () {
            parentSearch();
            layer.close(index);
        }
    });
}

function UploadFailed(msg) {
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
            parentSearch();
            layer.close(index);
        }
    });
}

function ShowFile(control) {
    $("#hidFileName").val("");
    var file = control.value;
    var fileName = getFileName(file);
    $("#txtFileName").val(fileName);
    $("#hidFilePath").val(file);
}

function getFileName(o) {
    var pos = o.lastIndexOf('\\');
    return o.substring(pos + 1);
}

function parentSearch() {
    var btnSearch = $(".searchCode");
    if (btnSearch != null) {
        btnSearch.click();
    }
    BindSelectAllEvent();
    BindEachCboEvent();
}

//查询代码映射信息
function SearchCodeMap() {
    layer.load("查询中，请稍后...");

    $(".searchCode").click();
}

//删除当前代码映射
function DeleteCodeMap(obj) {

    var serviceUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/DeleteCodeMapping";

    var selIndex = $.layer({
        area: ['auto', 'auto'],
        dialog: {
            msg: '您确认要删除此代码映射关系?',
            type: 4,
            btns: 2,
        },
        btns: 2,
        btn: ['确定', '取消'],
        closeBtn: false,
        yes: function () {
            var guid = $(obj).parent().parent("tr").find("#tdGuid").html();

            if (guid == null || guid == "") {
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
                        return;
                    }
                });
            }
            var loadIndex = layer.load(deleteLoad);

            $.ajax({
                url: serviceUrl,
                data: '{"dataGuid":"' + guid + '"}',
                type: "post",
                contentType: "application/json",
                dataType: "json",
                success:
                function (data) {
                    var result = data.DeleteCodeMappingResult;
                    if (result == "success") {
                        layer.close(loadIndex);
                        parentSearch();
                        
                        //$(obj).parent().parent().remove();
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
                                BindSelectAllEvent();
                                BindEachCboEvent();
                                layer.close(index);
                            }
                        });
                    }

                    ;

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
                            BindSelectAllEvent();
                            BindEachCboEvent();
                            layer.close(index);
                        }
                    });

                    
                }
            });
        }
    });
}

//批量删除选中的代码映射数据
function BatchDeleteCodeMap() {
    var serviceUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/BatchDeleteCodeMapping";

    var guids = GetAllSelectInfo();

    if (guids == "") {
        var selIndex = $.layer({
            area: ['auto', 'auto'],
            dialog: {
                msg: '请至少选择一条要删除的代码映射关系。',
                type: 8
            },
            btns: 1,
            btn: ['确定'],
            closeBtn: false,
            yes: function () {
                layer.close(selIndex);
            }
        });
    }
    else {
        $.layer({
            area: ['auto', 'auto'],
            dialog: {
                msg: '您确认要删除选择的代码映射关系？',
                type: 4,
                btns: 2
            },
            btns: 2,
            btn: ['确定', '取消'],
            closeBtn: false,
            yes: function () {
                var loadIndex = layer.load(deleteLoad);

                $.ajax({
                    url: serviceUrl,
                    data: '{"dataGuids":"' + guids + '"}',
                    type: "post",
                    contentType: "application/json",
                    dataType: "json",
                    success:
                    function (data) {
                        layer.close(loadIndex);
                        $("#cboCodeCheckAll")[0].checked = false;
                        var result = data.BatchDeleteCodeMappingResult;
                        if (result.indexOf("@") > -1) {
                            if (result.split("@")[0] == "success") {
                                var errData = result.split("@")[1];

                                if (errData != "") {
                                    var errMsg = "<span class=\"mustIcon\">" + errData + "</span>";
                                    var index = $.layer({
                                        area: ['auto', 'auto'],
                                        dialog: {
                                            msg: errMsg,
                                            type: 8
                                        },
                                        btns: 1,
                                        btn: ['确定'],
                                        closeBtn: false,
                                        yes: function () {
                                            parentSearch();
                                            //RemoveAllSelectInfo(errData);
                                            layer.close(index);
                                        }
                                    });
                                }
                                else {
                                    var index = $.layer({
                                        area: ['auto', 'auto'],
                                        dialog: {
                                            msg: deleteSuccess,
                                            type: 9
                                        },
                                        btns: 1,
                                        btn: ['确定'],
                                        closeBtn: false,
                                        yes: function () {
                                            parentSearch();

                                            //layer.close(index);
                                        }
                                    });

                                    //RemoveAllSelectInfo("");
                                }
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
                                        BindSelectAllEvent();
                                        BindEachCboEvent();
                                        layer.close(index);
                                    }
                                });
                            }
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
                                BindSelectAllEvent();
                                BindEachCboEvent();
                                layer.close(index);
                            }
                        });

                        
                    }
                });
            }
        });
    }
}

//查看当前代码映射
function ViewCodeMap(obj) {
    EditCodeMap(obj);

    //设置只读
    $("#divCodeMapping").find("input").each(function () {
        //$(this).attr("readonly", "readonly");
        $(this).attr("disabled", "true");
    });

    //隐藏保存按钮
    $(".saveCode").css("display", "none");

    //显示文本框
    $("#txtChildStartDate").css("display", "block");
    $("#txtChildExpireDate").css("display", "block");
    $("#picChildStartDate").css("display", "none");
    $("#picChildExpireDate").css("display", "none");

}
//编辑当前代码映射
function EditCodeMap(obj) {
    AddCodeMap();
    var CustomerName = $(obj).parent().parent().children("td").eq(1).attr("title"); //.html();
    var SemanticDesc = $(obj).parent().parent().children("td").eq(2).attr("title"); //.html();
    var BusinessCode = $(obj).parent().parent().children("td").eq(3).attr("title"); //.html();
    var TargetCode = $(obj).parent().parent().children("td").eq(4).attr("title"); //.html();
    var TargetCodeCnDesc = $(obj).parent().parent().children("td").eq(5).attr("title"); //.html();
    var TargetCodeEnDesc = $(obj).parent().parent().children("td").eq(6).attr("title"); //.html();
    var CMICTCode = $(obj).parent().parent().children("td").eq(7).attr("title"); //.html();
    var CMICTCodeCnDesc = $(obj).parent().parent().children("td").eq(8).attr("title"); //.html();
    var CMICTCodeEnDesc = $(obj).parent().parent().children("td").eq(9).attr("title"); //.html();
    var StartDate = $(obj).parent().parent().children("td").eq(10).attr("title"); //.html();
    var ExpireDate = $(obj).parent().parent().children("td").eq(11).attr("title"); //.html();
    if (ExpireDate == "-") {
        ExpireDate = "";
    }
    var MappingID = $(obj).parent().parent().children("td").eq(12).html();
    var CustomerID = $(obj).parent().parent().children("td").eq(13).html();
    var CustomerDesc = $(obj).parent().parent().children("td").eq(14).html();
    var BusinessCodeDesc = $(obj).parent().parent().children("td").eq(15).html();
    var BusinessTranslation = $(obj).parent().parent().children("td").eq(16).html();
    var Author = $(obj).parent().parent().children("td").eq(17).html();
    var Created = $(obj).parent().parent().children("td").eq(18).html();

    //给页面赋值
    $("#txtChildCustomerID").val(CustomerID);
    $("#txtChildCustomerName").val(CustomerName);
    $("#txtChildCustomerDesc").val(CustomerDesc);
    $("#txtChildBusinessCode").val(BusinessCode);
    $("#txtChildSemnaticDesc").val(SemanticDesc);
    $("#txtChildBusinessCodeDesc").val(BusinessCodeDesc);
    $("#txtChildBusinessTranslation").val(BusinessTranslation);
    $("#txtChildTargetCode").val(TargetCode);
    $("#txtChilTargetCodeEnDesc").val(TargetCodeEnDesc);
    $("#txtChildTargetCodeCnDesc").val(TargetCodeCnDesc);
    $("#txtChildCMCITCode").val(CMICTCode);
    $("#txtChildCMCITCodeEnDesc").val(CMICTCodeEnDesc);
    $("#txtChildCMCITCodeCnDesc").val(CMICTCodeCnDesc);
    $("#txtChildStartDate").val(StartDate);
    $("#txtChildExpireDate").val(ExpireDate);
    $("#hidMappingID").val(MappingID);
    $("#hidAuthor").val(Author);
    $("#hidCreated").val(Created);
    $("#picChildStartDate").val(StartDate);
    $("#picChildExpireDate").val(ExpireDate);

    $(".saveCode").css("display", "inline-block");

    isUpdate = true;
}
//添加映射关系
function AddCodeMap() {
    $.layer({
        type: 1, //0-4的选择,
        title: false,
        border: [0],
        closeBtn: [0, false],
        shadeClose: false,
        area: ['650px', '610px'],
        offset: ['', ''],
        shade: [0.2, '#000'],
        zIndex: 1,
        page: {
            dom: '#divCodeMapping'
        }
    });

    //清空页面的值
    $("#txtChildCustomerID").val("");
    $("#txtChildCustomerName").val("");
    $("#txtChildCustomerDesc").val("");
    $("#txtChildBusinessCode").val("");
    $("#txtChildSemnaticDesc").val("");
    $("#txtChildBusinessCodeDesc").val("");
    $("#txtChildBusinessTranslation").val("");
    $("#txtChildTargetCode").val("");
    $("#txtChilTargetCodeEnDesc").val("");
    $("#txtChildTargetCodeCnDesc").val("");
    $("#txtChildCMCITCode").val("");
    $("#txtChildCMCITCodeEnDesc").val("");
    $("#txtChildCMCITCodeCnDesc").val("");
    $("#txtChildStartDate").val("");
    $("#txtChildExpireDate").val("");
    $("#hidMappingID").val("");
    $("#hidAuthor").val("");
    $("#hidCreated").val("");
    $("#picChildStartDate").val("");
    $("#picChildExpireDate").val("");

    //显示日期控件
    $("#txtChildStartDate").css("display", "none");
    $("#txtChildExpireDate").css("display", "none");
    $("#picChildStartDate").css("display", "block");
    $("#picChildExpireDate").css("display", "block");

    $(".saveCode").css("display", "inline-block");

    isUpdate = false;
}

//保存代码映射关系
function SaveCodeMapping() {

    var loadIndex = layer.load(saveLoad);


    //检查页面内容
    var PageErr = checkForm(document.forms.item(0, null));

    if (!PageErr) {
        layer.close(loadIndex);
        return;
    }

    //获取页面控件的值
    var CustomerID = $("#txtChildCustomerID").val();
    var CustomerName = $("#txtChildCustomerName").val();
    var CustomerDesc = $("#txtChildCustomerDesc").val();
    var BusinessCode = $("#txtChildBusinessCode").val();
    var SemanticDesc = $("#txtChildSemnaticDesc").val();
    var BusinessCodeDesc = $("#txtChildBusinessCodeDesc").val();
    var BusinessTranslation = $("#txtChildBusinessTranslation").val();
    var TargetCode = $("#txtChildTargetCode").val();
    var TargetCodeEnDesc = $("#txtChilTargetCodeEnDesc").val();
    var TargetCodeCnDesc = $("#txtChildTargetCodeCnDesc").val();
    var CMICTCode = $("#txtChildCMCITCode").val();
    var CMICTCodeEnDesc = $("#txtChildCMCITCodeEnDesc").val();
    var CMICTCodeCnDesc = $("#txtChildCMCITCodeCnDesc").val();
    var MappingID = $("#hidMappingID").val();
    var StartDate = $("#picChildStartDate").val();
    var ExpireDate = $("#picChildExpireDate").val();
    var Created = $("#hidCreated").val();

    var currentUser = currentUserLoginName + ";" + currentUserName;
    currentUser = currentUser.replace("\\", "\\\\\\\\");
    var author = currentUser;

    if (isUpdate && $("#hidAuthor").val() != "") {
        author = $("#hidAuthor").val().replace("\\", "\\\\\\\\");
    }

    if (MappingID == "") {
        MappingID = "00000000-0000-0000-0000-000000000000";
    }

    if (ExpireDate == "") {
        ExpireDate = "12/31/9999";//设置最大时间
    }

    var CodeMapping = "{CustomerID:'" + CustomerID + "',CustomerName:'" + CustomerName + "',CustomerDesc:'" + CustomerDesc +
        "',BusinessCode:'" + BusinessCode + "',SemanticDesc:'" + SemanticDesc +
        "',BusinessCodeDesc:'" + BusinessCodeDesc + "',BusinessTranslation:'" + BusinessTranslation + "',TargetCode:'" + TargetCode
        + "',TargetCodeEnDesc:'" + TargetCodeEnDesc + "',TargetCodeCnDesc:'" + TargetCodeCnDesc + "',TargetCodeEnDesc:'"
        + TargetCodeEnDesc + "',CMICTCode:'" + CMICTCode + "',CMICTCodeEnDesc:'" + CMICTCodeEnDesc + "',CMICTCodeCnDesc:'"
        + CMICTCodeCnDesc + "',MappingID:'" + MappingID + "',StartDate:'" + StartDate + "',ExpireDate:'" + ExpireDate
        + "',Created:'" + Created + "',Modified:'',Author:'" + author + "',Editor:'" + currentUser + "'}";


    //检查代码映射关系的唯一性
    var checkServiceUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/CheckCodeMappingOnly";

    $.ajax({
        url: checkServiceUrl,
        data: '{"codeMapData":"' + CodeMapping + '","isUpdate":"' + isUpdate + '"}',
        type: "post",
        contentType: "application/json",
        dataType: "json",
        success:
        function (data) {
            var isOnly = data.CheckCodeMappingOnlyResult;
            if (isOnly == "yes") {
                var serviceUrl = siteUrl + "/_vti_bin/CMICTServices/CMICT.svc/SaveAndUpdateCodeMapping";

                $.ajax({
                    url: serviceUrl,
                    data: '{"codeMapData":"' + CodeMapping + '","isUpdate":"' + isUpdate + '"}',
                    type: "post",
                    contentType: "application/json",
                    dataType: "json",
                    success:
                    function (data) {
                        var result = data.SaveAndUpdateCodeMappingResult;
                        layer.close(loadIndex);

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
                                    parentSearch();
                                    //layer.closeAll();
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
                                    BindSelectAllEvent();
                                    BindEachCboEvent();
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
                                BindSelectAllEvent();
                                BindEachCboEvent();
                                layer.close(index);
                            }
                        });
                    }
                });
            }
            else {
                layer.close(loadIndex);
                var index = $.layer({
                    area: ['auto', 'auto'],
                    dialog: {
                        msg: '要保存的代码映射关系与已经存在的重复，请修改！',
                        type: 8
                    },
                    btns: 1,
                    btn: ['确定'],
                    closeBtn: false,
                    yes: function () {
                        layer.close(index);
                        return;
                    }
                });
            }

            
        },
        error:
        function (err) {
            var index = $.layer({
                area: ['auto', 'auto'],
                dialog: {
                    msg: '要保存的代码映射关系与已经存在的重复，请修改！',
                    type: 8
                },
                btns: 1,
                btn: ['确定'],
                closeBtn: false,
                yes: function () {
                    BindSelectAllEvent();
                    BindEachCboEvent();

                    layer.close(index);
                    return;
                }
            });
        }
    });




}

//批量删除后移除删除的数据
function RemoveAllSelectInfo(errGuids) {
    $("#tbCodeMapping tr:gt(0) input[type='checkbox']:checked").each(function () {
        if (errGuids != "") {
            var guid = $(this).parent().parent().find("#tdGuid").html();
            if (errGuids.indexOf(guid) > -1) {

            }
            else {
                $(this).parent().parent().remove();
            }
        }
        else {
            $(this).parent().parent().remove();
        }

    });


}

function GetAllSelectInfo() {
    var selectStr = "";

    $("#tbCodeMapping tr:gt(0) input[type='checkbox']:checked").each(function () {
        selectStr = selectStr + $(this).parent().parent().find("#tdGuid").html() + ";";//TempleatName
    });

    return selectStr;
}

//绑定全选
function BindSelectAllEvent() {

    $("#cboCodeCheckAll").click(function () {
        if ($(this)[0].checked) {
            $("#tbCodeMapping tr:gt(0) input[type='checkbox']").each(function () {
                this.checked = true;
            });
        } else {
            $("#tbCodeMapping tr:gt(0) input[type='checkbox']").each(function () {
                this.checked = false;
            });
        }
    });


}

//Bind checkbox click event of each row             
function BindEachCboEvent() {

    $("#tbCodeMapping tr:gt(0) input[type='checkbox']").each(function () {
        $(this).click(function () {
            var flag = false;//Flag tag to judge whether all selected
            if ($(this)[0].checked) {
                //if selected
                $("#tbCodeMapping tr:gt(0) input[type='checkbox']").each(function () {
                    if (this.checked) {
                        flag = true;
                    } else {
                        flag = false;
                        return false;//break each
                    }
                });
                if (flag) {
                    $("#cboCodeCheckAll")[0].checked = true;
                } else {
                    $("#cboCodeCheckAll")[0].checked = false;
                }
            } else {
                //if unselected
                if ($("#cboCodeCheckAll")[0].checked) {
                    $("#cboCodeCheckAll")[0].checked = false;
                }
            }
        });
    });


}

function changesizecodemap(pagesize) {
    $(".hidpagesize").val(pagesize);
    $(".searchCode").click();
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