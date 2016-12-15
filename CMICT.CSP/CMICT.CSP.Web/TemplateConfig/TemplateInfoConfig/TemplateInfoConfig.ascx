<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplateInfoConfig.ascx.cs" Inherits="CMICT.CSP.Web.TemplateConfig.TemplateInfoConfig.TemplateInfoConfig" %>



<div class="progressBox">
    <ul class="progressStepBox">
        <li>
            <div class="stepBox stepcurrent">
                <p>1.基本信息配置</p>
                <span></span>
            </div>
        </li>
        <li>
            <div class="stepBox">
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
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>    
<div class="stepOperateBox">
    <table class="tableTwo">
        <tr>
            <td>
                <label class="stepIptTxt">模板名称：</label></td>
            <td>
                <input class="stepIpt w210 txtTemplateName" type="text" id="txtTemplateName" runat="server" title="模板名称~50:!" onchange="deoxidizevalidate(this)" />
                <span class="mustIcon">*</span>
            </td>
            <td>
                <div id="div<% =txtTemplateName.ClientID %>"></div>
            </td>
        </tr>

        <tr>
            <td>
                <label class="stepIptTxt">模板描述：</label></td>
            <td>
                <textarea class="stepTextarea" id="txtTemplateDesc" runat="server" title="模板描述~200:!" onchange="deoxidizevalidate(this)"></textarea>
                 <span class="mustIcon">*</span>
            </td>
            <td>
                <div id="div<% =txtTemplateDesc.ClientID %>"></div>
            </td>
        </tr>
        <tr>
            <td>
                <label class="stepIptTxt">报表大类：</label></td>
            <td>
                <asp:DropDownList ID="ddlCATEGORY" runat="server" CssClass="default w210 ddlCATEGORY" onchange="changecatebig();" AutoPostBack="true" OnSelectedIndexChanged="ddlCATEGORY_SelectedIndexChanged">
                </asp:DropDownList><span class="mustIcon">*</span>
                <asp:TextBox ID="txtCATEGORY" runat="server" CssClass="txtDBName txtCATEGORY" title="报表大类~50:!" onchange="deoxidizevalidate(this)" Style="display: none;"></asp:TextBox>
            </td>
            <td>
                <div id="div<% =txtCATEGORY.ClientID %>"></div>
            </td>
        </tr>
        <tr>
            <td>
                <label class="stepIptTxt">报表细类：</label></td>
            <td>
                <asp:DropDownList ID="ddlsmallcategory" runat="server" CssClass="default w210 ddlsmallcategory" onchange="changesmallcategory();">
                    
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
                <label class="stepIptTxt">报表使用单位：</label></td>
            <td>
                <input class="stepIpt w210" type="text" id="txtUnit" runat="server" title="报表使用单位~50:" />
                
            </td>
            <td>
                <div id="div<% =txtUnit.ClientID %>"></div>
            </td>
        </tr>
        <tr>
            <td>
                <label class="stepIptTxt">每页条数：</label></td>
            <td>
                <asp:DropDownList ID="ddlPageSize" runat="server" CssClass="default w210 ddlPageSize" onchange="changepagesize();">
                </asp:DropDownList>
                <input class="stepIpt w210 txtPageSize" type="text" id="txtPageSize" runat="server" title="每页条数~50:!" style="display: none;" onchange="deoxidizevalidate(this)" />
                <span class="mustIcon">*</span>
            </td>
            <td>
                <div id="div<% =txtPageSize.ClientID %>"></div>
            </td>
        </tr>

        <tr id="trpage">
            <td>
                <label class="stepIptTxt">关联页面：</label></td>
            <td>
                <asp:Label ID="lblpageinfo" runat="server"></asp:Label>
                <input class="stepIpt w210 fLeft" type="text" id="txtPageInfo" runat="server" style="display: none;" />
            </td>
            <td></td>
        </tr>
        <tr style="display: none;">
            <td>
                <label class="stepIptTxt">是否禁用：</label></td>
            <td>
                <input class="stepCheckBox" type="checkbox" id="chkDisabled" runat="server" />
                <label class="checkTxt">禁用</label>
                <input type="hidden" id="hidDisabled" runat="server" class="hidDisabled" />
            </td>
            <td></td>
        </tr>
    </table>
    <div class="cutOffRule"></div>
    <div class="operateBtnBox operateBtnLayout">
        <asp:LinkButton ID="lbtnNext" runat="server" CssClass="guideBtn nextLink" OnClientClick="return executesub();">下一步</asp:LinkButton>
        <asp:Button ID="Button1" runat="server" Text="Button" style="display:none;" CssClass="btnsubmit" OnClick="lbtnNext_Click" />
    </div>
</div>
</ContentTemplate>
</asp:UpdatePanel>

<input type="hidden" id="hidid" runat="server" class="hidid" />
<input type="hidden" id="hidOperType" runat="server" class="hidOperType" />
<input type="hidden" id="hidNameCheck" runat="server" class="hidNameCheck" />
<input type="hidden" id="hidsourceid" runat="server" class="hidsourceid" />
<input type="hidden" id="hidtitle" runat="server" class="hidtitle" />
<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_beginRequest(function () {

    });
    prm.add_endRequest(function (sender, args) {
        args.set_errorHandled(true);
        try {
            if ($('.default').length > 0) {
                $('.default').dropkick();
            }
            $(document).attr("title", $(".hidtitle").val());
            addtiptodropdown();
        } catch (err) {

        }
    });
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
        checktemplatename();
        //var b_return = checkForm(document.forms.item(0, null));

        return false;
    }
    function changepagesize() {
        $(".txtPageSize").val($(".ddlPageSize option:selected").val());
        $(".txtPageSize").change();
    }

    function changecatebig() {
        $(".txtCATEGORY").val($(".ddlCATEGORY option:selected").val());
        $(".txtCATEGORY").change();
    }

    function changesmallcategory() {
        $(".txtsmallcategory").val($(".ddlsmallcategory option:selected").val());
        $(".txtsmallcategory").change();
    }
    //检查模板名称是否唯一
    function checktemplatename() {
        var b_return = false;
        $(".hidNameCheck").val("");
        var TemplateName = $.trim($(".txtTemplateName").val());
        var TemplateID = $.trim($(".hidid").val());
        if (TemplateName != "") {
            $.ajax({
                type: "post",
                url: "/_layouts/15/CMICT.CSP.Web/TemplateConfigHandler/TemplatePageCheck.ashx",
                data: { "OperType": "CheckTemplateName", "TemplateName": encodeURI(TemplateName), "TemplateID": TemplateID },
                dataType: 'text',
                success: function (e) {
                    if (e != null && e != undefined && e != "") {
                        if (e == "true") {
                            $(".hidNameCheck").val("<%=txtTemplateName.ClientID%>|已存在此模板名称！");
                            
                        } else {
                            $(".hidNameCheck").val("");
                            
                        }
                        b_return = checkForm(document.forms.item(0, null));
                        if (b_return) {
                            $(".btnsubmit").click();
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
    $(function ($) {
        $(".hidtitle").val($(document).attr("title"));
        addtiptodropdown();
    });
</script>
