<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BoxInfoTransfer.ascx.cs" Inherits="CMICT.CSP.Web.BusinessOnLine.BoxInfoTransfer.BoxInfoTransfer" %>
<script type="text/javascript">

    _spSuppressFormOnSubmitWrapper = true;

    function HideButton() {
        $(".operationBtn.export").hide();
        return true;
    }
</script>


<asp:FileUpload ID="FileUpload1" runat="server" />
<asp:Button ID="btn_Verify" runat="server" CssClass="operationBtn query" Text="上传" OnClick="btn_Verify_Click" />
<asp:Button ID="btn_Download" runat="server" CssClass="operationBtn export" Text="生成报文" OnClientClick="return HideButton()" OnClick="btn_Download_Click" Visible="false" />
<input id="filepath" runat="server" type="hidden" />
<br/>
<p class="onlineNote">(请根据系统提供的模板上传，下载地址：<a href="/Documents/TemplateFiles/修箱信息转换模板.xls">修箱信息转换模板.xls</a>)</p>
<br/>
<asp:Literal ID="lblError" runat="server"></asp:Literal>