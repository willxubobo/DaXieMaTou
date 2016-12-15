<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MainNavigation.ascx.cs" Inherits="CMICT.CSP.Branding.WebParts.MainNavigation.MainNavigation" %>
<script src="/_layouts/15/CMICT.CSP.Branding/js/jquery.SuperSlide.2.1.1.js"></script> 
    <script type="text/javascript">
        $(document).ready(function () {
            jQuery(".txtScroll-left").slide({ titCell: ".hd ul", mainCell: ".bd ul", autoPage: true, effect: "left", autoPlay: false, scroll: 1, vis: 1, trigger: "click" });
        })
    </script>
<div class="mainPageBox">
    <div class="main visible relative">
        <div class="erweima"></div>
        <div class="logoTxt"></div>
        <ul class="contentBox">

            <asp:Literal ID="Literal1" runat="server"></asp:Literal>

        </ul>
    </div>
</div>
