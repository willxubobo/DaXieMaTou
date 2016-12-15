<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplateManage.ascx.cs" Inherits="CMICT.CSP.Web.BusinessSearch.TemplateManage.TemplateManage" %>
<%@ Register Assembly="AspNetPager, Version=7.4.5.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

    <div class="filterBox">
        <div class="filterIptBox">
            <ul class="filterList">
                <li>
                    <label class="filterTxt">模板名称：</label>
                    <asp:TextBox ID="txtTemplateName" runat="server" CssClass="filterIpt common"></asp:TextBox>
                </li>
                <li>
                    <label class="filterTxt">创建时间段：</label>
                    <asp:TextBox ID="txtStartDate" runat="server" CssClass="filterIpt dateIpt"></asp:TextBox>
                    <label>~</label>
                    <asp:TextBox ID="txtEndDate" runat="server" CssClass="filterIpt dateIpt"></asp:TextBox>
                </li>
                <li>
                    <label class="filterTxt dropTxt">创建者：</label>
                    <asp:TextBox ID="txtAuthor" runat="server" CssClass="filterIpt common"></asp:TextBox>
                </li>
                <li>
                    <label class="filterTxt dropTxt">状态：</label>
                    <input class="filterIpt common" type="text" />
                </li>
                <li class="listRight">
                    <div class="operationBox">
                        <ul class="operationBtnBox">
                            <li>
                                <asp:Button ID="btnSearch" runat="server" Text="查询" OnClick="btnSearch_Click" CssClass="operationBtn query" />
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
            <asp:Repeater ID="TemplateList" runat="server">
                <HeaderTemplate>
                    <thead>
                        <tr>
                            <th>序号</th>
                            <th>模板名称</th>
                            <th>模板描述</th>
                            <th width="120">创建时间</th>
                            <th>创建者</th>
                            <th>功能页面</th>
                            <th>状态</th>
                            <th class="lastMenu" width="155">操作</th>
                        </tr>
                    </thead>
                    <tbody>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%#Container.ItemIndex+1 %></td>
                        <td><%#Eval("TemplateName") %></td>
                        <td><%#Eval("TemplateDesc") %></td>
                        <td><%#Eval("Created") %></td>
                        <td><%#Eval("Author") %></td>
                        <td><%#GetPageInfoByTemplateID(Convert.ToString(Eval("TemplateID"))) %></td>
                        <td><%#Eval("TemplateStatus") %></td>
                        <td>
                            <a class="operateBtn copy" href="#" title="复制" style='<%# (Convert.ToString(Eval("TemplateStatus"))=="草稿"||Convert.ToString(Eval("TemplateStatus"))=="禁用")? "display:none":""%>'></a>
                            <a class="operateBtn edit" href="TemplateInfoConfig.aspx?templateID=<%#Eval("TemplateID") %>" title="编辑"></a>
                            <a class="operateBtn add" href="#" title="授权" style='<%# (Convert.ToString(Eval("TemplateStatus"))=="草稿"||Convert.ToString(Eval("TemplateStatus"))=="禁用")? "display:none":""%>'></a>
                            <a class="operateBtn remove" href="#" title="删除"></a>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater>
            </tbody>                       
        </table>
    </div>

    <div class="pageBox">
        <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="2"
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
