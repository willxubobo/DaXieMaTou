<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConnectionConfig.ascx.cs" Inherits="CMICT.CSP.Web.TemplateConfig.ConnectionConfig.ConnectionConfig" %>
<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/js/jquery.autocomplete.js"></script>
<link rel="stylesheet" type="text/css" href="/_layouts/15/CMICT.CSP.Branding/css/jquery.autocomplete.css" />
<div class="stepOperateBox">
    <table class="tableTwo">
        <tr>
            <td style="width: 110px;">
                <label class="stepIptTxt">数据库服务器地址：</label></td>
            <td>
                <input class="stepIpt w210" type="text" id="txtSourceIP" runat="server" title="数据库服务器地址~50:!" onchange="deoxidizevalidate(this)" />
                <span class="mustIcon">*</span>
            </td>
            <td>
                <div id="div<% =txtSourceIP.ClientID %>"></div>
            </td>
            <td></td>
        </tr>

        <tr>
            <td>
                <label class="stepIptTxt">用户名：</label></td>
            <td>
                <input class="stepIpt w210" type="text" id="txtUserName" runat="server" title="用户名~50:!" onchange="deoxidizevalidate(this)" />
                <span class="mustIcon">*</span></td>
            <td>
                <div id="div<% =txtUserName.ClientID %>"></div>
            </td>
            <td></td>
        </tr>
        <tr>
            <td>
                <label class="stepIptTxt">密码：</label></td>
            <td>
                <asp:TextBox ID="txtPwd" runat="server" TextMode="Password" CssClass="stepIpt w210" onchange="deoxidizevalidate(this)" title="密码~50:!"></asp:TextBox>
                <span class="mustIcon">*</span>

            </td>
            <td>
                <div id="div<% =txtPwd.ClientID %>"></div>
            </td>
            <td>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <asp:LinkButton ID="lbtnConnect" runat="server" CssClass="guideBtn nextLink" OnClick="lbtnConnect_Click" OnClientClick="return executesub('conn');">连接</asp:LinkButton><span style="color: red;">(测试连接耗时较长，请等待！)</span>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table class="tableTwo">
                <tr class="sucshow" style="display: none;">
                    <td style="width: 110px;">
                        <label class="stepIptTxt">数据源名称：</label></td>
                    <td>
                        <input class="stepIpt w210 txtSourceName" type="text" id="txtSourceName" runat="server" title="数据源名称~50:" onchange="deoxidizevalidate(this)" />
                        <span class="mustIcon">*</span>
                    </td>
                    <td>
                        <div id="div<% =txtSourceName.ClientID %>"></div>
                    </td>
                    <td></td>
                </tr>

                <tr class="sucshow" style="display: none;">
                    <td>
                        <label class="stepIptTxt">数据库名称：</label></td>
                    <td>
                        <asp:DropDownList ID="ddlDBName" AutoPostBack="true" OnSelectedIndexChanged="ddlDBName_SelectedIndexChanged" runat="server" CssClass="default w210 ddlDBName" onchange="changedbname();">
                        </asp:DropDownList><span class="mustIcon">*</span>
                        <asp:TextBox ID="txtDBName" runat="server" CssClass="txtDBName" title="数据库名称~500:" onchange="deoxidizevalidate(this)" Style="display: none;"></asp:TextBox>
                    </td>
                    <td>
                        <div id="div<% =txtDBName.ClientID %>"></div>
                    </td>
                    <td></td>
                </tr>
                <tr class="sucshow" style="display: none;">
                    <td>
                        <label class="stepIptTxt">类型：</label></td>
                    <td>
                        <asp:DropDownList ID="ddlObjectType" runat="server" CssClass="default w210 ddlObjectType" onchange="changeobjecttype();" AutoPostBack="True" OnSelectedIndexChanged="ddlObjectType_SelectedIndexChanged">
                        </asp:DropDownList><span class="mustIcon">*</span>
                        <asp:TextBox ID="txtObjectType" CssClass="txtObjectType" runat="server" title="类型~50:" onchange="deoxidizevalidate(this)" Style="display: none;"></asp:TextBox>
                    </td>
                    <td>
                        <div id="div<% =txtObjectType.ClientID %>"></div>
                    </td>
                    <td></td>
                </tr>
                <tr class="sucshow" style="display: none;">
                    <td>
                        <label class="stepIptTxt">内部名称：</label></td>
                    <td>
                        <asp:DropDownList ID="ddlObjectName" runat="server" CssClass="default w210 ddlObjectName" onchange="changeobjectname();">
                        </asp:DropDownList><span class="mustIcon">*</span>
                        <asp:TextBox ID="txtObjectName" CssClass="txtObjectName" runat="server" title="内部名称~50:" onchange="deoxidizevalidate(this)" Style="display: none;"></asp:TextBox>
                    </td>
                    <td>
                        <div id="div<% =txtObjectName.ClientID %>"></div>
                    </td>
                    <td></td>
                </tr>
                <tr class="sucshow" style="display: none;">
                    <td>
                        <label class="stepIptTxt">报表大类：</label></td>
                    <td>
                        <asp:DropDownList ID="ddlCATEGORY" runat="server" CssClass="default w210 ddlCATEGORY" onchange="changecatebig();" AutoPostBack="true" OnSelectedIndexChanged="ddlCATEGORY_SelectedIndexChanged">
                        </asp:DropDownList><span class="mustIcon">*</span>
                        <asp:TextBox ID="txtCATEGORY" runat="server" CssClass="txtCATEGORY" title="报表大类~50:" onchange="deoxidizevalidate(this)" Style="display: none;"></asp:TextBox>
                    </td>
                    <td>
                        <div id="div<% =txtCATEGORY.ClientID %>"></div>
                    </td>
                    <td></td>
                </tr>
                <tr class="sucshow" style="display: none;">
                    <td>
                        <label class="stepIptTxt">报表细类：</label></td>
                    <td>
                        <asp:DropDownList ID="ddlsmallcategory" runat="server" CssClass="default w210 ddlsmallcategory" onchange="changesmallcategory();">
                        </asp:DropDownList>
                        <span class="mustIcon">*</span>
                        <asp:TextBox ID="txtsmallcategory" runat="server" CssClass="txtsmallcategory" title="报表细类~50:" onchange="deoxidizevalidate(this)" Style="display: none;"></asp:TextBox>
                    </td>
                    <td>
                        <div id="div<% =txtsmallcategory.ClientID %>"></div>
                    </td>
                    <td></td>
                </tr>
                 <tr class="sucshow" style="display: none;">
            <td>
                <label class="stepIptTxt">数据源描述：</label></td>
            <td colspan="3">
                <textarea class="stepTextarea" id="txtSourceDesc" runat="server"></textarea>
            </td>
        </tr>
            </table>

            <div class="cutOffRule"></div>
            <div class="operateBtnBox operateBtnLayout">
                <asp:LinkButton ID="lbtnTestCon" OnClick="lbtnTestCon_Click" runat="server" CssClass="addFiltrateBtnAstyle inlinestyle" OnClientClick="return testconsub();">测试数据源</asp:LinkButton>
                <asp:LinkButton ID="lbtnSave" runat="server" CssClass="operationBtn query" OnClientClick="return executesub('');return false;">保存</asp:LinkButton>
                <a class="operationBtn query" href="DataSourceManage.aspx" target="_parent">取消</a>
                <asp:Button ID="Button1" runat="server" Text="Button" Style="display: none;" CssClass="btnsubmit" OnClick="lbtnSave_Click" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<input type="hidden" id="hidid" runat="server" class="hidid" />
<input type="hidden" id="hidOperType" runat="server" class="hidOperType" />
<input type="hidden" id="hidcreated" runat="server" class="hidcreated" />
<input type="hidden" id="hidauthor" runat="server" class="hidauthor" />
<input type="hidden" id="hidNameCheck" runat="server" class="hidNameCheck" />
<input type="hidden" id="hidSourceStatus" runat="server" class="hidSourceStatus" />
<input type="hidden" id="hidtemnames" runat="server" class="hidtemnames" />
 <input type="hidden" id="hidtitle" runat="server" class="hidtitle" />
<script type="text/javascript">
    var strlist_ip = '';
    $(function ($) {
        addtiptodbname();
        $("#<%=txtSourceIP.ClientID%>").focus().autocomplete(strlist_ip);
        $(".hidtitle").val($(document).attr("title"));
    });
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_beginRequest(function () {

    });
    prm.add_endRequest(function (sender, args) {
        args.set_errorHandled(true);
        try {
            if ($('.default').length > 0) {
                $('.default').dropkick();
                addtiptodbname();
                layer.closeLoad();
            }
            $(document).attr("title", $(".hidtitle").val());
            var optype = $(".hidOperType").val();
            if (optype == "edit") {
                executeconnectsur();
            }
        } catch (err) {

        }
    });
    //检查模板名称是否唯一
    function checksourcename() {
        $(".hidNameCheck").val("");
        var SourceName = $.trim($(".txtSourceName").val());
        var SourceID = $.trim($(".hidid").val());
        if (SourceName != "") {
            $.ajax({
                type: "post",
                url: "/_layouts/15/CMICT.CSP.Web/TemplateConfigHandler/TemplatePageCheck.ashx",
                data: { "OperType": "CheckSourceName", "SourceName": encodeURI(SourceName), "SourceID": SourceID },
                dataType: 'text',
                success: function (e) {
                    if (e != null && e != undefined && e != "") {
                        if (e == "true") {
                            $(".hidNameCheck").val("<%=txtSourceName.ClientID%>|已存在此数据源名称！");
                        } else {
                            $(".hidNameCheck").val("");
                        }
                        var tems = $.trim($(".hidtemnames").val());
                        var b_return = checkForm(document.forms.item(0, null));
                        if (b_return) {
                            if (tems == "") {
                                layer.load('正在保存中');
                                $(".btnsubmit").click();
                            } else {
                                var msg = "修改数据源将对以下模板产生影响：" + tems + " 您确定要修改吗？";
                                RegisterButtonConfirm(msg, "edit");
                            }
                        }
                    }
                }
            });
        } else {

            b_return = checkForm(document.forms.item(0, null));
        }
    }
    function CheckNameOnly() {
        return $(".hidNameCheck").val();
    }
    //底部测试连接
    function testconsub() {
        var b_return = checkForm(document.forms.item(0, null));
        if (!b_return) {
            layer.closeLoad();
        }
        else {
            layer.load('连接测试中...');
        }
        return b_return;
    }
    function executesub(type) {
        if (type == "conn") {
            executeconnecterror();
            var b_return = checkForm(document.forms.item(0, null));
            if (!b_return) {
                layer.closeLoad();
            }
            else {
                layer.load('连接测试中...');
            }
            return b_return;
        }
        if ($(".sucshow").css("display") != "none") {
            checksourcename();
        } else {
            var optype = $(".hidOperType").val();
            var tems = $.trim($(".hidtemnames").val());
            var b_return = checkForm(document.forms.item(0, null));
            if (b_return) {
                if (tems == "") {
                    layer.load('正在保存中');
                    $(".btnsubmit").click();
                } else {
                    var msg = "修改数据源将对以下模板产生影响：" + tems + " 您确定要修改吗？";
                    RegisterButtonConfirm(msg, "edit");
                }
            }
        }
        return false;
    }
    //注册确认方法
    function RegisterButtonConfirm(msgs, type) {
        var iss = 0;
        var yes = '确定';
        var no = '取消';
        $.layer({
            shade: [0.5, '#000', true],
            area: ['auto', 'auto'],
            dialog: {
                msg: msgs,
                btns: 2,
                type: 4,
                btn: [yes, no],
                yes: function (index) {
                    layer.close(index);
                    if (type == "edit") {
                        layer.load('正在保存中');
                        $(".btnsubmit").click();
                    }
                },
                no: function () {
                }
            }
        });
    }
    function changecatebig() {
        $(".txtCATEGORY").val($(".ddlCATEGORY option:selected").val());
        $(".txtCATEGORY").change();
    }

    function changesmallcategory() {
        $(".txtsmallcategory").val($(".ddlsmallcategory option:selected").val());
        $(".txtsmallcategory").change();
    }
    //连接成功显示其余参数
    function executeconnectsur() {
        $(".sucshow").show();
        $(".txtSourceName").attr("title", "数据源名称~50:!");
        $(".txtDBName").attr("title", "数据库名称~500:!");
        $(".txtObjectType").attr("title", "类型~50:!");
        $(".txtObjectName").attr("title", "内部名称~50:!");
        $(".txtCATEGORY").attr("title", "报表大类~50:!");
        $(".txtsmallcategory").attr("title", "报表细类~50:!");
    }
    //连接失败
    function executeconnecterror() {
        $(".sucshow").hide();
        $(".txtSourceName").attr("title", "数据源名称~50:");
        $(".txtDBName").attr("title", "数据库名称~500:");
        $(".txtObjectType").attr("title", "类型~50:");
        $(".txtObjectName").attr("title", "内部名称~50:");
        $(".txtCATEGORY").attr("title", "报表大类~50:");
        $(".txtsmallcategory").attr("title", "报表细类~50:");
    }

    function changedbname() {
        $(".txtDBName").val($(".ddlDBName option:selected").val());
        $(".txtDBName").change();
    }

    function changeobjecttype() {
        $(".txtObjectType").val($(".ddlObjectType option:selected").val());
        $(".txtObjectType").change();
    }

    function changeobjectname() {
        $(".txtObjectName").val($(".ddlObjectName option:selected").val());
        $(".txtObjectName").change();
    }
    function closewin() {
        var index = parent.layer.getFrameIndex(window.name); //获取当前窗体索引
        window.opener.reload();
        parent.layer.close(index); //执行关闭

    }
    //给数据库下拉加title
    function addtiptodbname() {
        $("div[id*='ddlDBName']").find("a").each(function () {
            if ($(this).attr("data-dk-dropdown-value") != undefined && $(this).attr("data-dk-dropdown-value") != null && $(this).attr("data-dk-dropdown-value") != "") {
                $(this).attr("title", $(this).attr("data-dk-dropdown-value"));
            }
        });
        $("div[id*='ddlObjectName']").find("a").each(function () {
            if ($(this).attr("data-dk-dropdown-value") != undefined && $(this).attr("data-dk-dropdown-value") != null && $(this).attr("data-dk-dropdown-value") != "") {
                $(this).attr("title", $(this).attr("data-dk-dropdown-value"));
            }
        });
        $("div[id*='ddlCATEGORY']").find("a").each(function () {
            var atipname = $(this).html();
            $(this).attr("title", atipname).addClass("costDes");
        });
        $("div[id*='ddlsmallcategory']").find("a").each(function () {
            var atipname = $(this).html();
            $(this).attr("title", atipname).addClass("costDes");
        });
    }
</script>
