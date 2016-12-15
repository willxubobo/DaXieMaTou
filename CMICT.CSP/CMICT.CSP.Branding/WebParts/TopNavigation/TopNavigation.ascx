<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopNavigation.ascx.cs" Inherits="CMICT.CSP.Branding.WebParts.TopNavigation.TopNavigation" %>
        
<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/js/ChangePwd.js"></script>

<div class="header s4-notdlg">
            <div class="main">
                <div class="logo"></div>
                <ul class="topNav">
                    <li><a class="topLink" runat="server" id="index" href="">无忧首页</a></li>
                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                    <li>
                        <a  id="linkUser" runat="server" style="cursor:pointer" href="javascript:;">用户登陆</a>
                        <ul class="loginOperation" style="display:none">
                            <li>
                                <a class="userOperateLink" href="javascript:;" onclick="ChangePwd();">重置密码</a>
                            </li>
                            <%--<li>
                                <a class="userOperateLink otherUserLogin" href="javascript:;" onclick="loginwithanotheruser();">以其他身份登录</a>
                            </li>--%>
                            <li>
                                <a class="userOperateLink" href="javascript:;" onclick="signout();">退出</a>
                            </li>
                        </ul>
                    </li>
               </ul>
            </div>
        </div>