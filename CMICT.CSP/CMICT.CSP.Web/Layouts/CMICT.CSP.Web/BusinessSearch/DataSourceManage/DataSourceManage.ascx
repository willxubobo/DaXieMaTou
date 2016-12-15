<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataSourceManage.ascx.cs" Inherits="CMICT.CSP.Web.BusinessSearch.DataSourceManage.DataSourceManage" %>
<%@ Register Assembly="AspNetPager, Version=7.4.5.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>



    <div class="filterBox">
        <div class="filterIptBox">
            <ul class="filterList">
                <li>
                    <label class="filterTxt">数据源名称：</label>
                    <asp:TextBox ID="txtSourceName" runat="server" CssClass="filterIpt common"></asp:TextBox>
                </li>
                <li>
                    <label class="filterTxt">数据库服务器地址：</label>
                    <asp:TextBox ID="txtSourceIP" runat="server" CssClass="filterIpt common"></asp:TextBox>
                </li>
                <li>
                    <label class="filterTxt dropTxt">数据库名称：</label>
                    <asp:TextBox ID="txtDBName" runat="server" CssClass="filterIpt common"></asp:TextBox>
                </li>
                <li>
                    <label class="filterTxt dropTxt">数据获取类型：</label>
                    <asp:DropDownList ID="ddlDataType" runat="server">
                        <asp:ListItem Text="数据表" Value=""></asp:ListItem>
                        <asp:ListItem Text="视图" Value=""></asp:ListItem>
                        <asp:ListItem Text="存储过程" Value=""></asp:ListItem>
                    </asp:DropDownList>
                </li>
                <li>
                    <label class="filterTxt">内部名称：</label>
                    <asp:TextBox ID="txtObjectName" runat="server" CssClass="filterIpt common"></asp:TextBox>
                </li>
                <li>
                    <label class="filterTxt">状态：</label>
                    <asp:TextBox ID="TextBox2" runat="server" CssClass="filterIpt common"></asp:TextBox>
                </li>
                <li class="listRight">
                    <div class="operationBox">
                        <ul class="operationBtnBox">
                            <li>
                                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" CssClass="operationBtn query" />
                                <asp:Button ID="btnAdd" runat="server" Text="添加" CssClass="operationBtn query" />
                                </li>
                        </ul>
                    </div>
                </li>
            </ul>
        </div>
    </div>

    <div class="cutOffRule"></div>

    <div class="resultBox">
        <table class="tableOne tableOneLayout">
            <asp:Repeater ID="DataSourceList" runat="server">
                <HeaderTemplate>
                    <thead>
                        <tr>
                            <th>数据源名称</th>
                            <th>数据库服务器地址</th>
                            <th>数据库名称</th>
                            <th>数据获取类型</th>
                            <th>内部名称</th>
                            <th>中文名称</th>
                            <th>状态</th>
                            <th class="lastMenu" width="155">操作</th>
                        </tr>
                    </thead>
                    <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%#Eval("SourceName") %></td>
                        <td><%#Eval("SourceIP") %></td>
                        <td><%#Eval("DBName") %></td>
                        <td><%#Eval("ObjectType") %></td>
                        <td><%#Eval("ObjectName") %></td>
                        <td><%#Eval("ObjectName") %></td>
                        <td><%#Eval("SourceStatus") %></td>
                        <td>
                            <a class="operateBtn edit" href="TemplateInfoConfig.aspx?templateID=<%#Eval("TemplateID") %>" title="编辑"></a>
                            <a class="operateBtn remove" href="#" title="删除"></a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            </tbody>                       
        </table>
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
            CustomInfoHTML="共<font color='#ff0000'>%RecordCount%</font>条  每页显示<select id='pcount' class='pcount' onchange='changesize(this.options[this.options.selectedIndex].value)'><option value='20'>20</option><option value='40'>40</option><option value='60'>60</option></select></div>"
            OnPageChanged="AspNetPager1_PageChanged">
        </webdiyer:AspNetPager>
    </div>
<input type="hidden" id="hidpagesize" runat="server" value="20" class="hidpagesize" />