<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestUseAgeWebPart.ascx.cs" Inherits="CMICT.CSP.Web.TestUseAge.TestUseAgeWebPart.TestUseAgeWebPart" %>


<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/jquery/jquery-1.9.1.min.js"></script>
<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/js/Usage.js"></script>
<SharePoint:ScriptLink Name="SP.js" runat="server" ID="SPScriptLink" LoadAfterUI="True" OnDemand="true" Localizable="false" />

<div class="filterBox">
    <div class="filterIptBox">
        <ul class="filterList">
            <li>
                <label class="filterTxt">靠泊开始时间：</label>
                <input name="靠泊开始时间" class="filterIpt dateIpt UsageFilter" type="text" />
            </li>
            <li>
                <label class="filterTxt">靠泊结束时间：</label>
                <input name="靠泊结束时间" class="filterIpt dateIpt UsageFilter" type="text" />
            </li>
            <li>
                <label class="filterTxt dropTxt">船名：</label>
                <span class="dk_wrap">
                    <select name="船名" class="default w162 UsageFilter">
                        <option value="1">田字形报表</option>
                        <option value="2">***报表</option>
                        <option value="3"></option>
                        <option value="4"></option>
                    </select>
                </span>
            </li>
            <li class="listRight">
                <div class="operationBox">
                    <ul class="operationBtnBox">
                        <li>
                            <asp:Button class="operationBtn reset" ID="btn" runat="server" Text="重置" OnClick="btnSearch_Click" />
                            <%--<button class="operationBtn reset" type="button" runat="server" onclick="GetUrl();">重置</button>--%>

                        </li>
                        <li>
                            <button class="operationBtn query" type="button" onclick="search(this,100,'','');">查询</button></li>
                        <li>
                            <button class="operationBtn export" type="button">导出</button></li>
                    </ul>
                </div>
            </li>
        </ul>
    </div>
</div>

