<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommunicationConfig.ascx.cs" Inherits="CMICT.CSP.Web.TemplateConfig.CommunicationConfig.CommunicationConfig" %>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="progressBox">
            <ul class="progressStepBox">
                <li>
                    <div class="stepBox stepPrev">
                        <p>1.基本信息配置</p>
                        <span></span>
                    </div>
                </li>
                <li>
                    <div class="stepBox stepPrev">
                        <p>2.配置数据源</p>
                        <span></span>
                    </div>
                </li>
                <li>
                    <div class="stepBox stepPrev">
                        <p>3.配置展示方式</p>
                        <span></span>
                    </div>
                </li>
                <li>
                    <div class="stepBox stepPrev">
                        <p>4.配置筛选条件</p>
                        <span></span>
                    </div>
                </li>
                <li>
                    <div class="stepBox stepcurrent">
                        <p>5.配置业务信息查询通信</p>
                    </div>
                </li>
            </ul>
        </div>

        <div class="stepOperateBox">
            <p class="stepTitle columnshow" id="columntitle">通信关联配置</p>
            <table class="tableOne tableLayout columnshow" id="columncontent">
                <asp:Repeater ID="CommunicationList" runat="server">
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th width="47">序号</th>
                                <th width="300">业务名称</th>
                                <th>源模板</th>
                                <th>目标模板</th>
                                <th width="150">操作</th>
                            </tr>
                        </thead>
                        <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%#Container.ItemIndex+1 %></td>
                            <td>
                                <asp:Literal ID="lblName" runat="server" Text='<%#Eval("Name") %>'></asp:Literal></td>
                            <td>
                                <%#Eval("SourceName") %></td>
                            <td>
                                <%#Eval("TargetName") %></td>
                            <td>
                                <a id="lbtnEdit" onclick="editCommunication('<%#Eval("CommunicationID") %>')" href="javascript:void(0);" class="operateBtn edit"></a>
                                <a id="lbtnDelete" onclick="deleteCommunication('<%#Eval("CommunicationID") %>')" href="javascript:void(0);" class="operateBtn remove"></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                </tbody>
            </table>
            <div>
                <button id="btnAdd" onclick="return addCommunicationConfig();" class="addBtn fLeft"></button>
            </div>
            <div class="operateBtnBox operateBtnLayout">
                <asp:LinkButton ID="LinkButton1" runat="server" CssClass="guideBtn prevLink" OnClick="lbtnPrev_Click">上一步</asp:LinkButton>
                <asp:LinkButton ID="lbtnPreView" runat="server" CssClass="guideBtn nextLink" OnClick="lbtnPreView_Click">预览</asp:LinkButton>
                <asp:LinkButton ID="lbtnSave" runat="server" CssClass="guideBtn nextLink" OnClientClick="FinishTemplate();return false;">完成</asp:LinkButton>
            </div>
        </div>
        <%--弹窗--%>
        <div class="shade"></div>
        <!-- 选择列弹窗 Start -->
        <div class="toolTipBox communicationdetaildiv" style="width: 540px;height:580px;">
            <div class="toolTipContent" style="height:540px;overflow-y:auto;">
                <div class="operateBtnBox toolOperateLayout" style="text-align:right;padding-top:0px;padding-bottom:20px">
                    <a class="guideBtn nextLink" href="javascript:void(0)" onclick="saveDetail();">确定</a>
                    <a class="guideBtn prevLink" href="javascript:void(0)" onclick="closedetailwin();">取消</a>
                </div>
                <div class="cutOffRule clearBoth"></div>
                <div class="toolTipArea" style="padding-top:20px">
                    <table class="tableTwo" style="width: 100%">
                        <tr>
                            <td>
                                <label class="stepIptTxt">业务名称：</label></td>
                            <td>
                                <input class="stepIpt w210 txtName" type="text" id="txtName" runat="server" title="业务名称~50:!" onchange="deoxidizevalidate(this)" />
                                <span class="mustIcon">*</span>
                            </td>
                            <td>
                                <div id="div<% =txtName.ClientID %>"></div>
                            </td>
                        </tr>
                        <tr id="trbig" runat="server">
            <td>
                <label class="stepIptTxt">报表大类：</label></td>
            <td>
                <asp:DropDownList ID="ddlCATEGORY" runat="server" CssClass="setDropIpt default w210 ddlCATEGORY" onchange="changecatebig();" AutoPostBack="true" OnSelectedIndexChanged="ddlCATEGORY_SelectedIndexChanged">
                </asp:DropDownList><span class="mustIcon">*</span>
                <asp:TextBox ID="txtCATEGORY" runat="server" CssClass="txtDBName txtCATEGORY" title="报表大类~50:!" onchange="deoxidizevalidate(this)" Style="display: none;"></asp:TextBox>
            </td>
            <td>
                <div id="div<% =txtCATEGORY.ClientID %>"></div>
            </td>
        </tr>
        <tr id="trsmall" runat="server">
            <td>
                <label class="stepIptTxt">报表细类：</label></td>
            <td>
                <asp:DropDownList ID="ddlsmallcategory" runat="server" CssClass="setDropIpt default w210 ddlsmallcategory" onchange="changesmallcategory();" AutoPostBack="true" OnSelectedIndexChanged="ddlsmallcategory_SelectedIndexChanged">
                    
                </asp:DropDownList>
                <span class="mustIcon">*</span>
                <asp:TextBox ID="txtsmallcategory" runat="server" CssClass="txtDBName txtsmallcategory" title="报表细类~50:!" onchange="deoxidizevalidate(this)" Style="display: none;"></asp:TextBox>
            </td>
            <td>
                <div id="div<% =txtsmallcategory.ClientID %>"></div>
            </td>
        </tr>
                        <tr>
                            <td>
                                <label class="stepIptTxt">源模板：</label></td>
                            <td>
                                <asp:DropDownList ID="ddlSourceTemplate" runat="server" CssClass="setDropIpt default w210 ddlSourceTemplate" onchange="changeSourceTemplate(this);" AutoPostBack="true" OnSelectedIndexChanged="ddlSourceTemplate_SelectedIndexChanged">
                                </asp:DropDownList>
                                <span class="mustIcon">*</span>
                                <asp:TextBox ID="txtSourceTemplate" runat="server" CssClass="txtSourceTemplate" title="源模板~50:!" onchange="deoxidizevalidate(this)" Style="display: none;"></asp:TextBox>
                            </td>
                            <td>
                                <div id="div<% =txtSourceTemplate.ClientID %>"></div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">列映射配置<span style="color: red; padding-left: 50px">最多选择5个映射关系</span>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSelected" runat="server" CssClass="txtSelected" Style="display: none;"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                <table class="tableOne tableLayout columnshow" id="detailcontent">
                                    <asp:Repeater ID="CommunicationDetailList" runat="server" OnItemDataBound="CommunicationDetailList_ItemDataBound">
                                        <HeaderTemplate>
                                            <thead>
                                                <tr>
                                                    <th style="width: 10%">序号</th>
                                                    <th style="width: 30%">源列名</th>
                                                    <th style="width: 30%">源列类型</th>
                                                    <th style="width: 30%">目标列</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td><%#Container.ItemIndex+1 %></td>
                                                <td><%#Eval("ColumnName") %></td>
                                                <td>
                                                    <asp:Literal ID="lblSourceDataType" runat="server" Text=""></asp:Literal>
                                                    <div style="display: none">
                                                        <asp:Literal ID="lblColumnDataType" runat="server" Text='<%#Eval("ColumnDataType") %>'></asp:Literal>
                                                    </div>
                                                </td>
                                                <td>
                                                    <div style="display: none">
                                                        <asp:Literal ID="lblTargetColumnName" runat="server" Text='<%#Eval("TargetColumnName") %>'></asp:Literal>
                                                    </div>
                                                    <asp:DropDownList ID="ddlTargetColumn" runat="server" CssClass="setDropIpt default w210 ddlTargetColumn">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="cutOffRule clearBoth"></div>
                <div class="operateBtnBox toolOperateLayout" style="text-align:right;">
                    <a class="guideBtn nextLink" href="javascript:void(0)" onclick="saveDetail();">确定</a>
                    <a class="guideBtn prevLink" href="javascript:void(0)" onclick="closedetailwin();">取消</a>
                </div>

            </div>
        </div>
        <!-- 选择列弹窗 End -->
        <input type="hidden" id="hidSaveModel" class="hidSaveModel" runat="server" />
        <input type="hidden" id="hidCommunicationDetails" class="hidCommunicationDetails" runat="server" />
        <input type="hidden" id="hidTemplateID" class="hidTemplateID" runat="server" />
        <input type="hidden" id="hidsourceid" class="hidsourceid" runat="server" />
        <input type="hidden" id="hidCommunicationID" class="hidCommunicationID" runat="server" /><%--保存关联ID用于编辑与删除--%>
        <input type="hidden" id="hidtipcontent" class="hidtipcontent" runat="server" />
        <input type="hidden" id="hidtitle" runat="server" class="hidtitle" />
        <input type="hidden" id="hidisedit" runat="server" class="hidisedit" />
        <div style="display: none;">
            <asp:Button ID="btnDelete" runat="server" OnClick="btnDelete_Click" />
            <asp:Button ID="btnEdit" runat="server" OnClick="btnEdit_Click" />
            <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" />
            <asp:Button ID="btnAddNew" runat="server" OnClick="btnAddNew_Click" />
            <asp:Button ID="btnFinish" runat="server" OnClick="btnFinish_Click" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    function changecatebig() {
        $(".txtCATEGORY").val($(".ddlCATEGORY option:selected").val());
        $(".txtCATEGORY").change();
    }

    function changesmallcategory() {
        $(".txtsmallcategory").val($(".ddlsmallcategory option:selected").val());
        $(".txtsmallcategory").change();
    }
    $(function ($) {
        addtiptodbname();
        $(".hidtitle").val($(document).attr("title"));
    });
    //给数据源下拉加title
    function addtiptodbname() {
        var sourcenametip = $(".hidtipcontent").val();
        if ($.trim(sourcenametip) != "") {
            var stiplist = sourcenametip.split('!');
            $("div[id*='ddlSourceTemplate']").find("a").each(function () {
                if ($(this).attr("data-dk-dropdown-value") != undefined && $(this).attr("data-dk-dropdown-value") != null && $(this).attr("data-dk-dropdown-value") != "") {
                    var atipname = $(this).attr("data-dk-dropdown-value");
                    for (var i = 0; i < stiplist.length; i++) {
                        if (stiplist[i].indexOf(atipname) != -1) {
                            $(this).attr("title", stiplist[i].split(',')[1]);
                            break;
                        }
                    }
                }
            });
        }
        $("div[id*='ddlCATEGORY']").find("a").each(function () {
            var atipname = $(this).html();
            $(this).attr("title", atipname).addClass("costDes");
        });
        $("div[id*='ddlsmallcategory']").find("a").each(function () {
            var atipname = $(this).html();
            $(this).attr("title", atipname).addClass("costDes");
        });
    }
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_beginRequest(function () {

    });
    prm.add_endRequest(function (sender, args) {
        args.set_errorHandled(true);
        try {
            if ($('.default').length > 0) {
                $('.default').dropkick();
                addtiptodbname();
            }
            $(document).attr("title", $(".hidtitle").val());
        } catch (err) {

        }
    });
    var yes = '确定';
    var no = '取消';
    function FinishTemplate() {
        var msgs = "您确定完成吗？";
        var tipc = $(".hidisedit").val();
        if (tipc == "edit") {
            msgs = "修改模板配置会影响业务逻辑的查询，您确定要完成吗？";
        }
        $.layer({
            shade: [0.5, '#000', true],
            area: ['auto', 'auto'],
            dialog: {
                msg: msgs,
                btns: 2,
                type: 4,
                btn: [yes, no],
                yes: function (index) {
                    $("#<%=btnFinish.ClientID%>").click();
                    layer.close(index);
                },
                no: function (index) {
                    layer.close(index);
                }
            }
        });
    }

    function addCommunicationConfig() {
        $(".hidSaveModel").val("add");
        $("#<%=btnAddNew.ClientID%>").click();
        return false;
    }

    function changeSourceTemplate(ctrl) {
        $(".txtSourceTemplate").val($(ctrl).val());
    }

    function editCommunication(communicationId) {
        $(".hidCommunicationID").val(communicationId);
        $(".hidSaveModel").val("edit");
        $("#<%=btnEdit.ClientID%>").click();
    }

    function showDetail() {
        $(".shade").show();
        $(".communicationdetaildiv").show();
    }

    function closedetailwin() {
        $(".shade").hide();
        $(".communicationdetaildiv").hide();
    }

    function deleteCommunication(communicationId) {
        $(".hidCommunicationID").val(communicationId);
        $.layer({
            shade: [0.5, '#000', true],
            area: ['auto', 'auto'],
            dialog: {
                msg: '您确定删除吗？',
                btns: 2,
                type: 4,
                btn: [yes, no],
                yes: function (index) {
                    $("#<%=btnDelete.ClientID%>").click();
                    layer.close(index);
                },
                no: function (index) {
                    layer.close(index);
                }
            }
        });
    }

    function showMessage(msg,atype) {
        layer.alert(msg, atype);
    }
    //将目标列下拉框选择为空
    function resettargetcolumn(obj) {
        $(obj).parent().find(".dk_option_current").attr("class", "");
        $(obj).parent().find("a").each(function () {
            if ($(this).attr("data-dk-dropdown-value") != undefined && $(this).attr("data-dk-dropdown-value") != null) {
                if ($.trim($(this).attr("data-dk-dropdown-value")) == "") {
                    $(this).parent().attr("class", "dk_option_current");
                    $(this).parent().parent().parent().prev().text($(this).text());
                }
            }
        });
    }
    function matchColumnDataType(ctrltargetColumn, sourceDataType) {
        var isNotMatch = false;
        var targetColumnType = $.trim($(ctrltargetColumn).val().split('-')[1]);
        if (targetColumnType) {
            if (targetColumnType != sourceDataType) {
                isNotMatch = true;
                $.layer({
                    shade: [0.5, '#000', true],
                    area: ['auto', 'auto'],
                    dialog: {
                        msg: "选择的目标列类型与源列类型不匹配，请重新选择！",
                        btns: 1,
                        type: 4,
                        btn: ['确定'],
                        yes: function (index) {
                            layer.close(index);
                            resettargetcolumn(ctrltargetColumn);
                            //$(ctrltargetColumn)[0].selectedIndex = 0;
                        }
                    },
                    close: function (index) {
                        layer.close(index);
                        resettargetcolumn(ctrltargetColumn);
                        //$(ctrltargetColumn)[0].selectedIndex = 0;
                    }
                });
            }
        }

        if (!isNotMatch) {
            var selectCount = 0;
            $(".ddlTargetColumn").each(function () {
                if ($(this).val()) {
                    selectCount++;
                }
            });
            if (selectCount == 0) {
                $(".txtSelected").val("");
            }
            else {
                $(".txtSelected").val(selectCount);
            }
            //if (selectCount >= 5) {
            //    $(".ddlTargetColumn").each(function () {
            //        var sValue = $(this).val();
            //        if (sValue == "") {
            //            $(this).attr("disabled", "disabled");
            //        }
            //    });
            //}
            //else {
            //    $(".ddlTargetColumn").each(function () {
            //        var sValue = $(this).val();
            //        if (sValue == "") {
            //            $(this).removeAttr("disabled");
            //        }
            //    });
            //}
        }
    }

    function saveDetail() {
        if ($(".txtSelected").val() == "") {
            layer.alert("请至少选择一项目标列！");
        }
        else {
            var tcount = 0;
            $(".ddlTargetColumn").each(function () {
                if ($(this).val()) {
                    tcount++;
                }
            });
            if (tcount > 5) {
                layer.alert("映射关系不能多于5个！");
            }else{
            var b_return = checkForm(document.forms.item(0, null));
            var savemodel = $(".hidSaveModel").val();
            var msg = savemodel == "add" ? "您确定要新增吗？" : "您确定要修改吗？";
            if (b_return) {
                $.layer({
                    shade: [0.5, '#000', true],
                    area: ['auto', 'auto'],
                    dialog: {
                        msg: msg,
                        btns: 2,
                        type: 4,
                        btn: [yes, no],
                        yes: function (index) {
                            var details = "";

                            $("div[id*='ddlTargetColumn']").each(function () {
                                if ($(this).find(".dk_option_current").length > 0) {
                                    var sv = $.trim($(this).find(".dk_option_current").children('a').eq(0).attr("data-dk-dropdown-value"));
                                    if (sv != "") {
                                        var targetColumn = sv;
                                        if (targetColumn.indexOf('-') >= 0) {
                                            targetColumn = $.trim(targetColumn.split('-')[0]);
                                        }

                                        if (targetColumn) {
                                            var sourceColumn = $(this).parent().parent().children().get(1).innerText;
                                            if (details) {
                                                details += "|" + sourceColumn + ";" + targetColumn;
                                            }
                                            else {
                                                details = sourceColumn + ";" + targetColumn;
                                            }
                                        }
                                    }
                                }

                            });
                            $(".hidCommunicationDetails").val(details); 
                            $("#<%=btnSave.ClientID%>").click();
                            layer.close(index);
                        }

                    },
                    close: function (index) {
                        layer.close(index);
                    }
                });
            }
        }
    }
    }
    //预览
    function GotoPreView() {
        var temid = $.trim($(".hidTemplateID").val());
        if (temid != "") {
            //window.open("PreView.aspx?templateID=" + temid);
            var w = window.open();
            //setTimeout(function () {
                w.location = "PreView.aspx?templateID=" + temid;
            //}, 300);

            return false;
            //ExecuteOrDelayUntilScriptLoaded(function () {
            //    var options = SP.UI.$create_DialogOptions();
            //    options.title = "预览";
            //    options.width = 1000;
            //    options.height = 700;
            //    options.url = "PreView.aspx?templateID="+temid;
            //    //options.dialogReturnValueCallback = Function.createDelegate(null, UpdateRightContent);
            //    SP.UI.ModalDialog.showModalDialog(options);
            //}, 'sp.js');
        } else {
            layer.alert('未获取到模板信息,无法预览！', 8);
        }
        return false;
    }
</script>
