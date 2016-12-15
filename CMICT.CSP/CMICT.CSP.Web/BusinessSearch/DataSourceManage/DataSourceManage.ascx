<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DataSourceManage.ascx.cs" Inherits="CMICT.CSP.Web.BusinessSearch.DataSourceManage.DataSourceManage" %>
<%@ Register Assembly="AspNetPager, Version=7.4.5.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<style type="text/css">
    .widthMaxSource {
    width: 100% !important;
    min-width: 130px;
    height: 30px;
    line-height: 28px;
}
    .costDesSource {
        width: 100% !important;
        display: inline-block;
        overflow: hidden;
        white-space: nowrap;
        text-overflow: ellipsis;
    }
</style>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:Button ID="lbtndel" runat="server" Text="删除用" OnClick="lbtndel_Click" Style="display: none;" CssClass="lbtndel" />
        <input type="hidden" id="hidpagesize" runat="server" value="20" class="hidpagesize" />
        <input type="hidden" id="hidsourceid" runat="server" value="" class="hidsourceid" />
        <input type="hidden" id="hidisdefault" runat="server" class="hidisdefault" />
        <input type="hidden" id="hidtitle" runat="server" class="hidtitle" />
        <div class="filterBox">
            <div class="filterIptBox">
                <table class="filterTable">
                    <tr>
                        <td class="filterTxtWidth">
                            <label class="filterIptTxt fLeft">数据源名称：</label></td>
                        <td>
                            <asp:TextBox ID="txtSourceName" runat="server" CssClass="stepIpt widthMaxSource fLeft"></asp:TextBox></td>
                        <td class="filterTxtWidth">
                            <label class="filterIptTxt fLeft">报表大类：</label></td>
                        <td>
                            <asp:DropDownList ID="ddlCATEGORY" runat="server" CssClass="default widthMaxSource ddlCATEGORY" AutoPostBack="true" OnSelectedIndexChanged="ddlCATEGORY_SelectedIndexChanged">
                            </asp:DropDownList></td>
                        <td class="filterTxtWidth">
                            <label class="filterIptTxt fLeft">报表细类：</label></td>
                        <td>
                            <asp:DropDownList ID="ddlsmallcategory" runat="server" CssClass="default widthMaxSource ddlsmallcategory">
                            </asp:DropDownList></td>
                        <td class="filterTxtWidth">
                            <label class="filterIptTxt fLeft">数据获取类型：</label></td>
                        <td>
                            <asp:DropDownList ID="ddlDataType" runat="server" CssClass="default widthMaxSource">
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <td>
                            <label class="filterIptTxt fLeft">状态：</label></td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="default widthMaxSource">
                            </asp:DropDownList></td>
                        <td class="filterTxtWidth130">
                            <label class="filterIptTxt fLeft">数据库服务器地址：</label></td>
                        <td>
                            <asp:TextBox ID="txtSourceIP" runat="server" CssClass="stepIpt widthMaxSource fLeft"></asp:TextBox></td>
                        <td>
                            <label class="filterIptTxt fLeft">数据库名称：</label></td>
                        <td>
                            <asp:TextBox ID="txtDBName" runat="server" CssClass="stepIpt widthMaxSource fLeft"></asp:TextBox></td>
                        <td>
                            <label class="filterIptTxt fLeft">内部名称：</label></td>
                        <td>
                            <asp:TextBox ID="txtObjectName" runat="server" CssClass="stepIpt widthMaxSource fLeft"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td colspan="3">
                            <div class="operationBox">
                                <ul class="operationBtnBox">
                                    <li>
                                        <a class="addFiltrateBtnAstyle fLeft" href="<%=CMICT.CSP.BLL.Components.BaseComponent.GetReportTypeUrl() %>" target="_blank">新建报表类别</a>
</li>
                                    <li>
                                    <a class="operationBtn query fLeft" href="ConnectionConfig.aspx" target="_parent">添加</a>
                                    </li>
                                    <li>
                                        <asp:Button ID="btnSearch" runat="server" Text="查询" OnClientClick="layer.load('查询中，请稍候...');" OnClick="btnSearch_Click" CssClass="operationBtn query btnhsearch fLeft" />
                                    </li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                </table>

            </div>
        </div>

        <div class="cutOffRule clearBoth"></div>

        <div class="resultBox">
            <table class="tableOne tableOneLayout">
                <asp:Repeater ID="DataSourceList" runat="server" OnItemCommand="TemplateList_ItemCommand" OnItemDataBound="TemplateList_ItemDataBound">
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th style="width: 142px;">
                                    <asp:LinkButton ID="SourceName" runat="server" Text="数据源名称" CommandName="SourceName" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);"></asp:LinkButton></th>
                                <th style="width: 102px;">数据库服务器地址</th>
                                <th style="width: 97px;">
                                    <asp:LinkButton ID="DBName" runat="server" Text="数据库名称" CommandName="DBName" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);"></asp:LinkButton></th>
                                <th style="width: 77px;">
                                    <asp:LinkButton ID="ObjectType" runat="server" Text="数据获取类型" CommandName="ObjectType" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);"></asp:LinkButton></th>
                                <th style="width: 127px;">
                                    <asp:LinkButton ID="ObjectName" runat="server" Text="内部名称" CommandName="ObjectName" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);"></asp:LinkButton></th>
                                <th style="width: 120px;">
                                    <asp:LinkButton ID="BigCategory" runat="server" Text="报表大类" CommandName="BigCategory" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);"></asp:LinkButton></th>
                                <th style="width: 120px;">
                                    <asp:LinkButton ID="SmallCategory" runat="server" Text="报表细类" CommandName="SmallCategory" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);"></asp:LinkButton></th>
                                <th style="width: 140px;">模板名称</th>
                                <th style="width: 30px;">
                                    <asp:LinkButton ID="SourceStatus" runat="server" Text="状态" CommandName="SourceStatus" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);"></asp:LinkButton></th>
                                <th class="lastMenu" style="width: 92px; word-wrap: break-word;">操作</th>
                            </tr>
                        </thead>
                        <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="word-wrap: break-word; word-break: break-all; width: 142px;"><%#Eval("SourceName") %></td>
                            <td style="word-wrap: break-word; word-break: break-all; width: 102px;"><%#Eval("SourceIP") %></td>
                            <td style="word-wrap: break-word; word-break: break-all; width: 97px;"><%#Eval("DBName") %></td>
                            <td style="word-wrap: break-word; word-break: break-all; width: 77px;"><%#CMICT.CSP.BLL.Components.BaseComponent.GetLookupNameBuValue("BS_OBJECT_TYPE",Convert.ToString(Eval("ObjectType"))) %></td>
                            <td style="word-wrap: break-word; word-break: break-all; width: 127px;"><%#Eval("ObjectName") %></td>
                            <td style="word-wrap: break-word; word-break: break-all; width: 120px;"><%#CMICT.CSP.BLL.Components.BaseComponent.GetUserTypeNameByCode(Convert.ToString(Eval("BigCategory"))) %></td>
                            <td style="word-wrap: break-word; word-break: break-all; width: 120px;"><%#CMICT.CSP.BLL.Components.BaseComponent.GetUserLookupNameBuValue(Convert.ToString(Eval("BigCategory")),Convert.ToString(Eval("SmallCategory"))) %></td>
                            <td style="word-wrap: break-word; word-break: break-all; width: 140px;"><%#GetTemplateInfoBySourceID(Convert.ToString(Eval("SourceID"))) %></td>
                            <td style="word-wrap: break-word; word-break: break-all; width: 30px;"><%#CMICT.CSP.BLL.Components.BaseComponent.GetLookupNameBuValue("BS_SOURCE_STATUS",Convert.ToString(Eval("SourceStatus"))) %></td>
                            <td style="word-wrap: break-word; word-break: break-all; width: 92px;">
                                <a class="operateBtn edit" href="ConnectionConfig.aspx?sourceID=<%#Eval("SourceID") %>" title="编辑" target="_parent"></a>
                                <a class="operateBtn remove" href="javascript:void(0);" title="删除" onclick="executedel('<%#Eval("SourceID") %>');" style='<%# (Convert.ToString(Eval("SourceStatus"))=="ENABLE")? "display:none": ""%>'></a>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                </tbody>                       
            </table>
            <asp:Label ID="lblnodata" runat="server" Text="暂无数据！" Visible="false" Style="font-size: 12px;"></asp:Label>
        </div>

        <div class="pageBox">
            <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20"
                HorizontalAlign="right" Width="100%"
                CssClass="pageList" NumericButtonCount="5" ShowMoreButtons="true"
                AlwaysShow="true" FirstPageText="首页" LastPageText="尾页" NextPageText="下一页"
                PrevPageText="上一页" SubmitButtonText="Go" SubmitButtonClass="gostyle"
                CustomInfoStyle="font-size:14px;text-align:right;padding-top: 2px;"
                InputBoxStyle="width:25px; border:1px solid #999999; text-align:center; "
                TextBeforeInputBox="转到第" TextAfterInputBox="页 " PageIndexBoxType="TextBox"
                ShowPageIndexBox="Always" TextAfterPageIndexBox="页"
                TextBeforePageIndexBox="转到" Font-Size="14px"
                ShowCustomInfoSection="Right" CustomInfoSectionWidth="15%"
                PagingButtonSpacing="3px"
                CustomInfoHTML="共<font color='#ff0000'>%RecordCount%</font>条  每页显示<select id='pcount' class='default pcount' onchange='changesize(this.options[this.options.selectedIndex].value)'><option value='5'>5</option><option value='10'>10</option><option value='15'>15</option><option value='20' selected>20</option><option value='30'>30</option><option value='40'>40</option><option value='50'>50</option></select></div>"
                OnPageChanged="AspNetPager1_PageChanged">
            </webdiyer:AspNetPager>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>
<script type="text/javascript">
    //给数据源下拉加title  dk_toggle dk_label
    function addtiptodropdown() {
        $("div[id*='ddlCATEGORY']").find("a").each(function () {
            var atipname = $(this).html();
            $(this).attr("title", atipname).addClass("costDesSource");
        });
        $("div[id*='ddlsmallcategory']").find("a").each(function () {
            var atipname = $(this).html();
            $(this).attr("title", atipname).addClass("costDesSource");
        });
    }
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_beginRequest(function () {

    });
    prm.add_endRequest(function (sender, args) {
        args.set_errorHandled(true);
        try {
            if ($('.default').length > 0) {
                $('.default').dropkick();
            }
            layer.closeAll();
            $(document).attr("title", $(".hidtitle").val());
            addtiptodropdown();
        } catch (err) {

        }
    });
    function executedel(sid) {
        $(".hidsourceid").val(sid);
        var msg = "您确定要删除吗？";
        RegisterButtonConfirm(msg, 1);
        return false;
    }
    //注册确认方法
    function RegisterButtonConfirm(msgs, type) {
        var iss = 0;
        var yes = '确定';
        var no = '取消';
        $.layer({
            shade: [0.5, '#000', true],
            area: ['auto', 'auto'],
            dialog: {
                msg: msgs,
                btns: 2,
                type: 4,
                btn: [yes, no],
                yes: function (index) {
                    layer.close(index);
                    if (type == 1) {
                        $(".lbtndel").click();
                    }
                },
                no: function () {
                }
            }
        });
        if (iss == 1) {
            return true;
        } else {
            return false;
        }
    }
    function AddDataSource() {
        $.layer({
            type: 2,
            title: '添加数据源',
            maxmin: false,
            border: [0],
            shadeClose: false, //  
            area: ['610px', '530px'],
            offset: ['98px', ''],
            iframe: { src: 'ConnectionConfig.aspx?isContent=1', scrolling: 'no' }
        });
        return false;
    }
    $(function ($) {
        $(".hidtitle").val($(document).attr("title"));
        addtiptodropdown();
    });
    //禁止回车事件响应
    $(this).keydown(function (e) {
        var key = window.event ? e.keyCode : e.which;
        //alert(key.toString());
        if (key.toString() == "13") {
            return false;
        }
    });
</script>
