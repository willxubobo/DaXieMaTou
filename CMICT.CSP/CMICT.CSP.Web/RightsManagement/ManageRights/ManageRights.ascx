<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageRights.ascx.cs" Inherits="CMICT.CSP.Web.RightsManagement.ManageRights.ManageRights" %>

<%@ Register Assembly="AspNetPager, Version=7.4.5.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>

<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/js/ManageRights.js"></script>
<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/js/OrgnazationUser.js"></script>
<link rel="stylesheet" type="text/css" href="/_layouts/15/CMICT.CSP.Branding/css/permission.css" />

<asp:UpdatePanel ID="UpdatePanel2" runat="server">
    <ContentTemplate>
        <div class="filterBox">
            <div class="filterIptBox">
                <ul class="filterList">
                    <li>
                        <label class="filterTxt">功能页面：</label>
                        <asp:TextBox ID="txtPageName" name="页面名称" class="stepIpt w165 grayBg UsageFilter" type="text" runat="server" />
                    </li>
                    <li>
                        <label class="filterTxt">使用单位：</label>
                        <asp:TextBox ID="txtUseCompany" name="使用单位" class="stepIpt w165 grayBg UsageFilter" type="text" runat="server" />
                    </li>
                    <li>
                        <label class="filterTxt dropTxt">创建者：</label>
                        <asp:TextBox ID="txtCreater" name="创建者" class="stepIpt w165 grayBg UsageFilter" type="text" runat="server" />
                    </li>
                    <li class="listRight">
                        <div class="operationBox">
                            <ul class="operationBtnBox">
                                <li>
                                    <%--<a class="operationBtn query" href="javascript:;" onclick="SearchRight();">查询</a>--%>
                                    <asp:Button ID="btnSearch" OnClientClick="layer.load('查询中，请稍后...');" class="operationBtn query buttonFont searchRight" runat="server" Text="查询" OnClick="btnSearch_Click" />
                                </li>
                                <li>
                                    <asp:Button ID="btnSync" OnClientClick="layer.load('同步中，请稍后...');" class="operationBtn query buttonFont" runat="server" Text="AD同步" OnClick="btnSync_Click" />
                                </li>
                                <li>
                                    <input class="operationBtn query" onclick="ShowPermissionPage();" value="授权" type="button"></input></li>
                                <li>
                                    <a class="impowerBtn" onclick="ViewPersonRights();">查看人员权限</a></li>
                            </ul>
                        </div>
                    </li>
                </ul>
            </div>
        </div>

        <div class="resultBox">
            <table id="tableContent" class="tableOne onlineTable">
                <asp:Repeater ID="DataSourceList" runat="server" OnItemCommand="DataSourceList_ItemCommand" OnItemDataBound="DataSourceList_ItemDataBound">
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th>
                                    <input type="checkbox" id="cboCheckAll"></th>
                                <th>
                                    <asp:LinkButton ID="Title" OnClientClick="layer.load('排序中，请稍后...');" runat="server" Text="功能页面" CommandName="Title" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);"></asp:LinkButton></th>
                                <th>
                                    <asp:LinkButton ID="TemplateName" OnClientClick="layer.load('排序中，请稍后...');" runat="server" Text="模板名称" CommandName="TemplateName" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);"></asp:LinkButton></th>
                                <th>
                                    <asp:LinkButton ID="Created" OnClientClick="layer.load('排序中，请稍后...');" runat="server" Text="创建时间" CommandName="Created" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);"></asp:LinkButton></th>
                                <th>
                                    <asp:LinkButton ID="UseUnit" OnClientClick="layer.load('排序中，请稍后...');" runat="server" Text="使用单位" CommandName="UseUnit" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);"></asp:LinkButton></th>
                                <th>
                                    <asp:LinkButton ID="Author" OnClientClick="layer.load('排序中，请稍后...');" runat="server" Text="创建者" CommandName="Author" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);"></asp:LinkButton></th>
                                <th>
                                    <asp:LinkButton ID="PageStatus" OnClientClick="layer.load('排序中，请稍后...');" runat="server" Text="状态" CommandName="PageStatus" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);"></asp:LinkButton></th>
                                <th>操作</th>
                            </tr>
                        </thead>
                        <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <input type="checkbox"></td>
                            <td><a class="aPermission" name="<%#Eval("Title")+";"+ Eval("Href")+";"+ Eval("FileGuid")+";"+ Eval("WebUrl")%>" title="<%#Eval("SubTitle") %>" href="<%#Eval("Href") %>" target="_blank"><%#Eval("Title") %></a> </td>
                            <td><%#Eval("TemplateName") %></td>
                            <td><%#Eval("Created") %></td>
                            <td><%#Eval("UseUnit") %></td>
                            <td><%#Eval("Author") %></td>
                            <td><%#Eval("PageStatus") %></td>
                            <td>
                                <a class="operateBtn jurisdiction" title="查看权限" onclick="ViewRights(this)" href="javascript:;"></a>
                                <a class="operateBtn checkin" title="签入" style='display: <%#Eval("IsDisplay")%>' href="javascript:;" onclick='CheckIn(this,"<%#Eval("FileGuid") %>    ","<%#Eval("WebUrl") %>    ")'></a></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <asp:Label ID="lblnodata" runat="server" Text="暂无数据！" Visible="false" Style="font-size: 12px;"></asp:Label>
        </div>

        <div class="pageBox">
            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20"
                HorizontalAlign="right" Width="100%"
                CssClass="pageList"
                AlwaysShow="true" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页"
                PrevPageText="上一页" SubmitButtonText="Go" SubmitButtonClass="gostyle"
                CustomInfoStyle="font-size:14px;text-align:right;padding-top: 2px;"
                InputBoxStyle="width:25px; border:1px solid #999999; text-align:center; "
                TextBeforeInputBox="转到第" TextAfterInputBox="页 " PageIndexBoxType="TextBox"
                ShowPageIndexBox="Always" TextAfterPageIndexBox="页"
                TextBeforePageIndexBox="转到" Font-Size="14px"
                ShowCustomInfoSection="Right" CustomInfoSectionWidth="15%"
                PagingButtonSpacing="3px"
                CustomInfoHTML="共<font color='#ff0000'>%RecordCount%</font>条  每页显示<select id='pcount' class='default pcount' onchange='changesizeright(this.options[this.options.selectedIndex].value)'><option value='5'>5</option><option value='10'>10</option><option value='15'>15</option><option value='20' selected>20</option><option value='30'>30</option><option value='40'>40</option><option value='50'>50</option></select></div>"
                OnPageChanged="AspNetPager1_PageChanged">
            </webdiyer:AspNetPager>
        </div>
        <input type="hidden" id="hidpagesize" runat="server" value="20" class="hidpagesize" />
        <input type="hidden" id="hidSortColumnName" runat="server" class="hidSortColumnName" />
        <input type="hidden" id="hidSortType" runat="server" class="hidSortType" />
    </ContentTemplate>
</asp:UpdatePanel>

<div id="divPersonalPages" class="administrationAuthority" style="display: none">
    <div class="adminBox">
        <div class="adminBoxTitle peopleTitleHieght">
            <p class="adminTitle fLeft">角色名称：</p>
            <SharePoint:PeopleEditor ID="pplRole" Width="400px" CssClass="adminIptPeople w252 fLeft UsageFilter peoplePicker"
                ValidatorEnable="true" SelectionSet="User,DL,SecGroup,SPGroup" MultiSelect="false"
                AllowTypeIn="true" IsValid="true" runat="server" />
            <span class="mustIconPeople">*</span>
            <a class="operationBtn query fRight" href="javascript:;" onclick="SerachPersonalRightsPages();">查询</a>
        </div>
        <div class="adminTableBox peopleMargin">
            <table id="tbPersonRight" class="tableFour">
                <thead>
                    <tr>
                        <th>页面名称</th>
                        <th>权限级别</th>
                        <th>操作</th>
                    </tr>
                </thead>
            </table>
        </div>
        <div class="operateBtnBox operateBtnLayout">
            <a class="guideBtn nextLink" href="javascript:;" onclick="ClosePersonalPage();">关闭</a>
        </div>
    </div>
</div>


<input type="hidden" id="hidSearchUserLoginName" />

<div id="divPermissionPage" style="display: none">
    <div class="impowerPopup">
        <div class="adminBox">
            <div class="impowerBox">
                <table id="tablePermissionInfo" class="tableFour">
                    <thead>
                        <tr>
                            <th>
                                <input type="checkbox" id="cboPermissionCheckAll"></th>
                            <th>功能页面</th>
                            <th>模板名称</th>
                            <th>创建时间</th>
                            <th>使用单位</th>
                            <th class="lastMenu">创建者</th>
                        </tr>
                    </thead>
                    <tbody>
                    </tbody>
                </table>
            </div>
            <div class="adminBoxTitle">
                <input id="txtSearchName" onkeypress="GetSerach();" class="stepIpt w252 fLeft iptStyle" type="text" />
                <a class="operationBtn query fLeft" href="javascript:;" onclick="SearchNameInPermissionPage();">查询</a>
                <a class="query fLeft marginLeftReset" href="javascript:;" onclick="GetTheAllOrganization();">重置</a>
            </div>
            <div class="impowerChoiceBox">
                <div class="choiceArea">
                    <div class="impowerUserBox">
                        <ul id="ulLeftRootFolder" class="impowerUserList">
                        </ul>
                    </div>
                </div>
                <div class="centerArea centerAreaStyle">
                    <a class="impowerBtn impowerBtnLayout impowerBtnStyle" href="javascript:;" onclick="addAllPermissionSelector();">全部添加</a>
                    <a class="impowerBtn impowerBtnLayout" href="javascript:;" onclick="addPermissionSelector();">添加</a>
                    <a class="impowerBtn impowerBtnLayout" href="javascript:;" onclick="delAllPermissionSelector();">全部删除</a>
                    <a class="impowerBtn impowerBtnLayout" href="javascript:;" onclick="delPermissionSelector();">删除</a>
                </div>
                <div class="resultsArea fLeft">
                    <div class="userChoice">
                        <ul id="ulRightUserPermission" class="userChoiceList">
                        </ul>
                    </div>
                </div>
            </div>
            <div class="operateBtnBox operateBtnLayout">
                <a class="guideBtn finishLink" href="javascript:;" onkeypress="ForbitEnter();" onclick="SetPermission();">保存</a>
                <a class="guideBtn prevLink" href="javascript:;" onkeypress="ForbitEnter();" onclick="ClosePermissionPage();">取消</a>
            </div>
        </div>
    </div>
    <input type="hidden" id="hidTopName" class="topName" runat="server" />
</div>



<script type="text/javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_beginRequest(function () {

    });
    prm.add_endRequest(function (sender, args) {
        args.set_errorHandled(true);
        try {
            if ($('.default').length > 0) {
                $('.default').dropkick();
            }
            var pagesize = $(".hidpagesize").val();
            setvalueselsearch(pagesize);
        } catch (err) {

        }
    });
</script>
