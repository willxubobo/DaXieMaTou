<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataSourceConfig.ascx.cs" Inherits="CMICT.CSP.Web.TemplateConfig.DataSourceConfig.DataSourceConfig" %>

<div class="progressBox">
    <ul class="progressStepBox">
        <li>
            <div class="stepBox stepPrev">
                <p>1.基本信息配置</p>
                <span></span>
            </div>
        </li>
        <li>
            <div class="stepBox stepcurrent">
                <p>2.配置数据源</p>
                <span></span>
            </div>
        </li>
        <li>
            <div class="stepBox">
                <p>3.配置展示方式</p>
                <span></span>
            </div>
        </li>
        <li>
            <div class="stepBox">
                <p>4.配置筛选条件</p>
                <span></span>
            </div>
        </li>
        <li>
            <div class="stepBox">
                <p>5.配置业务信息查询通信</p>
            </div>
        </li>
    </ul>
</div>
<div class="cutOffRule"></div>
<asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
        <div class="stepOperateBox">
            <table class="tableTwo">
                <tr>
                    <td>
                        <label class="stepIptTxt">报表大类：</label></td>
                    <td>
                        <asp:DropDownList ID="ddlCATEGORY" runat="server" CssClass="default w210 ddlCATEGORY" onchange="changecatebig();" AutoPostBack="true" OnSelectedIndexChanged="ddlCATEGORY_SelectedIndexChanged">
                        </asp:DropDownList><span class="mustIcon">*</span>
                        <asp:TextBox ID="txtCATEGORY" runat="server" CssClass="txtCATEGORY" title="报表大类~50:!" onchange="deoxidizevalidate(this)" Style="display: none;"></asp:TextBox>
                    </td>
                    <td>
                        <div id="div<% =txtCATEGORY.ClientID %>"></div>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <label class="stepIptTxt">报表细类：</label></td>
                    <td>
                        <asp:DropDownList ID="ddlsmallcategory" runat="server" CssClass="default w210 ddlsmallcategory" onchange="changesmallcategory();" AutoPostBack="true" OnSelectedIndexChanged="ddlsmallcategory_SelectedIndexChanged">
                        </asp:DropDownList>
                        <span class="mustIcon">*</span>
                        <asp:TextBox ID="txtsmallcategory" runat="server" CssClass="txtsmallcategory" title="报表细类~50:!" onchange="deoxidizevalidate(this)" Style="display: none;"></asp:TextBox>
                    </td>
                    <td>
                        <div id="div<% =txtsmallcategory.ClientID %>"></div>
                    </td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <label class="stepIptTxt">数据源名称：</label></td>
                    <td>
                        <asp:DropDownList ID="ddlSourceName" runat="server" CssClass="default w210 ddlSourceName" onchange="changesname();">
                        </asp:DropDownList>
                        <span class="mustIcon">*</span>
                        <asp:TextBox ID="txtSourceName" runat="server" CssClass="txtSourceName" title="数据源名称~50:!" onchange="deoxidizevalidate(this)" Style="display: none;"></asp:TextBox>
                    </td>
                    <td>
                        <div id="div<% =txtSourceName.ClientID %>"></div>
                    </td>
                    <td><a class="addFiltrateBtnAstyle" href="DataSourceManage.aspx" target="_blank">新建数据源</a></td>
                </tr>
            </table>
            <div class="cutOffRule"></div>
            <div class="operateBtnBox operateBtnLayout">
                <asp:LinkButton ID="lbtnPrev" runat="server" CssClass="guideBtn prevLink" OnClick="lbtnPrev_Click">上一步</asp:LinkButton>
                <asp:LinkButton ID="lbtnSave" runat="server" CssClass="guideBtn nextLink" OnClick="lbtnSave_Click" OnClientClick="return executesub();">下一步</asp:LinkButton>
                <asp:Button ID="Button1" runat="server" Text="修改用" CssClass="btnsave" Style="display: none;" OnClick="lbtnSave_Click" />
            </div>
        </div>
        <input type="hidden" id="hidOperType" runat="server" class="hidOperType" />
        <input type="hidden" id="hidsourceid" runat="server" class="hidsourceid" />
        <input type="hidden" id="hidtipcontent" runat="server" class="hidtipcontent" />
        <input type="hidden" id="hidtitle" runat="server" class="hidtitle" />
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    $(function ($) {
        addtiptodropdown();
        addtiptodbname();
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
                addtiptodropdown();
            }
            $(document).attr("title", $(".hidtitle").val());
        } catch (err) {

        }
    });
    //给数据源下拉加title
    function addtiptodbname() {
        var sourcenametip = $(".hidtipcontent").val();
        if ($.trim(sourcenametip) != "") {
            var stiplist = sourcenametip.split('!');
            $("div[id*='ddlSourceName']").find("a").each(function () {
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
    }
    //给数据源下拉加title  dk_toggle dk_label
    function addtiptodropdown() {
        $("div[id*='ddlCATEGORY']").find("a").each(function () {
                var atipname = $(this).html();
                $(this).attr("title", atipname).addClass("costDes");
        });
        $("div[id*='ddlsmallcategory']").find("a").each(function () {
            var atipname = $(this).html();
            $(this).attr("title", atipname).addClass("costDes");
        });
    }
    function executesub() {
        var b_return = checkForm(document.forms.item(0, null));
        var sid = $.trim($(".hidsourceid").val());
        var oldsid = $.trim($(".ddlSourceName option:selected").val());
        var optype = $.trim($(".hidOperType").val());
        if (optype != "edit") {//新增
            return b_return;
        } else {
            if (b_return) {
                if (sid != oldsid) {
                    var msg = "更改数据源将会影响业务逻辑的查询，您确定要更改吗？";
                    RegisterButtonConfirm(msg, 'edit');
                } else {
                    return true;
                }
            }
            return false;
        }
        return false;
    }
    //注册确认方法groupcal
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
                        $(".btnsave").click();
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
    function changesname() {
        $(".txtSourceName").val($(".ddlSourceName option:selected").val());
        $(".txtSourceName").change();
    }
</script>
