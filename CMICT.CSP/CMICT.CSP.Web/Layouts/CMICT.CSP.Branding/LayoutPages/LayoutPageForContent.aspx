<%@ Page language="C#"   Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,Microsoft.SharePoint.Publishing,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" %>
<%@ Register Tagprefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceholderID="PlaceHolderPageTitle" runat="server">
	<SharePointWebControls:FieldValue id="PageTitle" FieldName="Title" runat="server"/>
</asp:Content>
<asp:Content ContentPlaceholderID="PlaceHolderMain" runat="server">
    <div class="headerBg">
                <img src="/_layouts/15/CMICT.CSP.Branding/images/header-bg.png" alt="">
            </div>
    <div class="mainNavBox">
                <div class="main">
                    <ul class="mainNav">
                        <li>
                            <a class="mainLink active" href="#"><span class="iconMainA"></span>船舶信息查询</a>
                            <ul class="subNav">
                                <li><a class="subLink" href="#">进箱计划查询</a></li>
                                <li><a class="subLink" href="#">船期计划查询</a></li>
                                <li><a class="subLink" href="#">船舶作业计划查询</a></li>
                            </ul>
                        </li>
                        <li>
                            <a class="mainLink" href="#"><span class="iconMainB"></span>箱货信息查询</a>
                        </li>
                        <li>
                            <a class="mainLink" href="#"><span class="iconMainC"></span>在线办单业务</a>
                        </li>
                        <li>
                            <a class="mainLink" href="#"><span class="iconMainD"></span>系统管理</a>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="mainPage">
                <div class="mainContent">
                    <div class="breadcrumbNavBox">
                        <ul class="breadcrumbNav">
                            <li><a class="breadLink" href="#">船舶信息查询</a></li>
                            <li><a class="breadLink last" href="#">船期计划查询</a></li>
                        </ul>
                    </div>

                   
	<WebPartPages:WebPartZone runat="server" AllowPersonalization="false" FrameType="None" Title="Content" ID="Content">
		<ZoneTemplate></ZoneTemplate>
		</WebPartPages:WebPartZone>
                </div>
            </div>
</asp:Content>