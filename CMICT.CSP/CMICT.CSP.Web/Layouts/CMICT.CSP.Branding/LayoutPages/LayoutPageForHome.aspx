<%@ Page language="C#"   Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,Microsoft.SharePoint.Publishing,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" %>
<%@ Register Tagprefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<asp:Content ContentPlaceholderID="PlaceHolderPageTitle" runat="server">
	<SharePointWebControls:FieldValue id="PageTitle" FieldName="Title" runat="server"/>
</asp:Content>
<asp:Content ContentPlaceholderID="PlaceHolderMain" runat="server">
    <div class="contentBg">
                <img src="/_layouts/15/CMICT.CSP.Branding/images/warp-bg.png" alt=""/>
            </div>
            <div class="main visible relative">
                <div class="erweima"></div>
                <div class="logoTxt"></div>
                <ul class="contentBox">
                    <li class="odd">
                        <p class="contentTitle">船舶信息查询</p>
                        <a class="contentLink" href="#">进箱计划查询 ></a>
                        <a class="contentLink" href="#">船期计划查询 ></a>
                        <a class="contentLink" href="#">单证中心截单公告 ></a>
                        <a class="contentLink" href="#">现有航线船期表 ></a>
                        <a class="contentLink" href="#">船舶作业计划查询 ></a>
                        <a class="contentLink" href="#">海关放行报文查询 ></a>
                    </li>
                    <li class="even">
                        <p class="contentTitle">箱货信息查询</p>
                        <a class="contentLink" href="#">单箱信息查询 ></a>
                        <a class="contentLink" href="#">出口箱单证放行查询 ></a>
                        <a class="contentLink" href="#">进口箱单证放行查询 ></a>
                        <a class="contentLink" href="#">查验箱到位查询 ></a>
                    </li>
                    <li class="odd">
                        <p class="contentTitle">其他信息查询</p>
                        <a class="contentLink" href="#">在场集卡查询 ></a>
                        <a class="contentLink" href="#">在场箱堆位置查询 ></a>
                    </li>
                    <li class="even">
                        <p class="contentTitle">网上业务办理</p>
                    </li>
                </ul>
            </div>
</asp:Content>
