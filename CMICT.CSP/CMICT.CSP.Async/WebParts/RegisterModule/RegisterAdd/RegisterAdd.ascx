<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegisterAdd.ascx.cs" Inherits="CMICT.CSP.Async.WebParts.RegisterModule.RegisterAdd.RegisterAdd" %>

<script src="/_layouts/15/CMICT.CSP.Branding/jquery/jquery-1.9.1.min.js"></script>
<script src="/_layouts/15/CMICT.CSP.Branding/jquery/layer/layer.min.js"></script>
<link rel="stylesheet" type="text/css" href="/_layouts/15/CMICT.CSP.Branding/css/base.css" />

<script type="text/javascript">
    function CheckEntity() {
        var entityName = $("#<%=this.txtModuleName.ClientID%>").val();
        if ($.trim(entityName) == "") {
            alert("实体不能为空！");
            return false;
        } else return true;
    }
</script>

<div class="stepOperateBox">
    <table class="tableTwo">
        <tbody>
            <tr>
                <td>
                    <label class="stepIptTxt">注册新增</label></td>
                <td>
                </td>
                <td>
                   
                </td>
            </tr>


            <tr id="trpage">
                <td>
                    <label class="stepIptTxt">实体：</label></td>
                <td>
                    <asp:TextBox ID="txtModuleName" CssClass="stepIpt w210 fLeft" runat="server"></asp:TextBox>
                </td>
                <td></td>
            </tr>
        </tbody>
    </table>
    <div class="cutOffRule"></div>
    <div class="operateBtnBox operateBtnLayout">
        <asp:LinkButton ID="lbtnRegister" CssClass="guideBtn nextLink" OnClientClick="return CheckEntity();" runat="server" OnClick="lbtnRegister_Click">注册</asp:LinkButton>
    </div>
</div>