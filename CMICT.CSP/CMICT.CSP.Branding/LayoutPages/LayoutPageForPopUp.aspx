<%@ Page language="C#"   Inherits="Microsoft.SharePoint.Publishing.PublishingLayoutPage,Microsoft.SharePoint.Publishing,Version=15.0.0.0,Culture=neutral,PublicKeyToken=71e9bce111e9429c" meta:progid="SharePoint.WebPartPage.Document" %>
<%@ Register Tagprefix="SharePointWebControls" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="PublishingWebControls" Namespace="Microsoft.SharePoint.Publishing.WebControls" Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> <%@ Register Tagprefix="PublishingNavigation" Namespace="Microsoft.SharePoint.Publishing.Navigation" Assembly="Microsoft.SharePoint.Publishing, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register  TagPrefix="SPBranding" Namespace="CMICT.CSP.Branding.WebParts.ImageRender" Assembly="CMICT.CSP.Branding, Version=1.0.0.0, Culture=neutral, PublicKeyToken=fe66cb5506b011aa" %>
<%@ Register  TagPrefix="SPBranding" Namespace="CMICT.CSP.Branding.WebParts.MainNavigation" Assembly="CMICT.CSP.Branding, Version=1.0.0.0, Culture=neutral, PublicKeyToken=fe66cb5506b011aa" %>
<asp:Content ContentPlaceholderID="PlaceHolderPageTitle" runat="server">
	<SharePointWebControls:FieldValue id="PageTitle" FieldName="Title" runat="server"/>
</asp:Content>
<asp:Content ContentPlaceholderID="PlaceHolderMain" runat="server">
    <div>
        <WebPartPages:WebPartZone runat="server" AllowPersonalization="false" FrameType="None" Title="Content" ID="Content">
		<ZoneTemplate></ZoneTemplate>
		</WebPartPages:WebPartZone>
    </div>
</asp:Content>
