<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConnectionConfig.ascx.cs" Inherits="CMICT.CSP.Web.TemplateConfig.ConnectionConfig.ConnectionConfig" %>

<div class="stepOperateBox">
    <table class="tableTwo">
        <tr>
            <td>
                <label class="stepIptTxt">数据库服务器地址：</label></td>
            <td>
                <input class="stepIpt w210" type="text" id="txtSourceIP" runat="server" />
                <span class="mustIcon">*</span>
            </td>
        </tr>

        <tr>
            <td>
                <label class="stepIptTxt">用户名：</label></td>
            <td>
                <input class="stepIpt w210" type="text" id="txtUserName" runat="server" />
                <span class="mustIcon">*</span></td>
        </tr>
        <tr>
            <td>
                <label class="stepIptTxt">密码：</label></td>
            <td>
                <input class="stepIpt w210" type="text" id="txtPwd" runat="server" />
                <span class="mustIcon">*</span>
                <asp:LinkButton ID="lbtnConnect" runat="server" CssClass="guideBtn nextLink" OnClick="lbtnConnect_Click">连接</asp:LinkButton>
            </td>
        </tr>

        <tr class="sucshow" style="display: none;">
            <td>
                <label class="stepIptTxt">数据源名称：</label></td>
            <td>
                <input class="stepIpt w210" type="text" id="Text1" runat="server" />
                <span class="mustIcon">*</span>
            </td>
        </tr>

        <tr class="sucshow" style="display: none;">
            <td>
                <label class="stepIptTxt">数据库名称：</label></td>
            <td>
                <asp:DropDownList ID="ddlDBName" runat="server" CssClass="default">
                </asp:DropDownList></td>
        </tr>
    </table>

    <div class="cutOffRule"></div>
    <div class="operateBtnBox operateBtnLayout">
        <asp:LinkButton ID="lbtnSave" runat="server" CssClass="guideBtn nextLink" OnClick="lbtnSave_Click">保存</asp:LinkButton>
    </div>
</div>
