<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.ascx.cs" Inherits="CMICT.CSP.Web.ChangeUserPwd.ChangePassword.ChangePassword" %>

<link rel="stylesheet" type="text/css" href="/_layouts/15/CMICT.CSP.Branding/css/base.css" />
<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/js/ChangePwd.js"></script>
<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/jquery/jquery-1.9.1.min.js"></script>
<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/jquery/layer/layer.min.js"></script>
<%--<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/js/jquery-ui/jquery-ui.min.js"></script>--%>

<div class="resetPasswordBox">
    <div class="adminBox">
        <div class="resetPasswordEnter">
            <table class="tablePassword">
                <tr>
                    <td>用户名：</td>
                    <td>
                        <label id="lblUserName" class="adminColor" runat="server">管理员</label></td>
                </tr>
                <tr>
                    <td>旧密码：</td>
                    <td>
                        <input id="txtOldPwd" class="stepIpt w210" type="password" runat="server" />
                        <span class="mustIcon">*</span>

                    </td>
                </tr>
                <tr>
                    <td>重置密码：</td>
                    <td>
                        <input id="txtNewPwd" class="stepIpt w210" type="password" runat="server" />
                        <span class="mustIcon">*</span>
                    </td>
                </tr>
                <tr>
                    <td>确认重置密码：</td>
                    <td>
                        <input id="txtConfirmNewPwd" class="stepIpt w210" type="password" runat="server" />
                        <span class="mustIcon">*</span>
                    </td>
                </tr>
            </table>
        </div>
        <div class="operateBtnBox operateBtnBoxStyle">
            <a class="guideBtn finishLink" href="javascript:;" onclick="Reset();">重置</a>
            <asp:Button CssClass="Reset" runat="server" ID="btnSave" OnClick="btnSave_Click" Text="重置"></asp:Button>
            <a class="guideBtn prevLink" href="javascript:;" onclick="closePage();">取消</a>
        </div>
    </div>
</div>


