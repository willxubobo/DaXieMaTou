<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplateManage.ascx.cs" Inherits="CMICT.CSP.Web.BusinessSearch.TemplateManage.TemplateManage" %>
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
        <input type="hidden" id="hidpagesize" runat="server" value="20" class="hidpagesize" />
        <asp:Button ID="lbtndel" OnClientClick="layer.load('正在删除，请稍候...');" runat="server" Text="删除用" OnClick="lbtndel_Click" Style="display: none;" CssClass="lbtndel" />
        <asp:Button ID="btncopy" OnClientClick="layer.load('正在复制，请稍候...');" runat="server" Text="复制用" OnClick="btncopy_Click" Style="display: none;" CssClass="btncopy" />
        <asp:Button ID="btnenable" OnClientClick="layer.load('正在禁用，请稍候...');" runat="server" Text="禁用用" OnClick="btnenable_Click" Style="display: none;" CssClass="btnenable" />
        <asp:Button ID="btnuse" OnClientClick="layer.load('正在启用，请稍候...');" runat="server" Text="启用" OnClick="btnuse_Click" Style="display: none;" CssClass="btnuse" />
        <input type="hidden" id="hidtemplateid" runat="server" value="" class="hidtemplateid" />
        <input type="hidden" id="hidCopyName" runat="server" value="" class="hidCopyName" />
        <input type="hidden" id="hidsourceid" runat="server" class="hidsourceid" />
        <input type="hidden" id="hidisdefault" runat="server" class="hidisdefault" />
        <input type="hidden" id="hidtitle" runat="server" class="hidtitle" />
        <div class="filterBox">
            <div class="filterIptBox"> 
                <table class="filterTable">
                    <tr>
                        <td class="filterTxtWidth">
                            <label class="filterIptTxt fLeft">模板名称：</label></td>
                        <td>
                            <asp:TextBox ID="txtTemplateName" runat="server" CssClass="stepIpt widthMax fLeft"></asp:TextBox></td>
                        <td class="filterTxtWidth">
                            <label class="filterIptTxt fLeft">报表大类：</label></td>
                        <td>
                            <asp:DropDownList ID="ddlCATEGORY" runat="server" CssClass="default widthMax" AutoPostBack="true" OnSelectedIndexChanged="ddlCATEGORY_SelectedIndexChanged">
                            </asp:DropDownList></td>
                        <td class="filterTxtWidth">
                            <label class="filterIptTxt fLeft">报表细类：</label></td>
                        <td>
                            <asp:DropDownList ID="ddlsmallcategory" runat="server" CssClass="default widthMax">
                            </asp:DropDownList></td>
                        <td class="filterTxtWidth">
                            <label class="filterIptTxt fLeft">创建者：</label></td>
                        <td>
                            <asp:TextBox ID="txtAuthor" runat="server" CssClass="stepIpt widthMax fLeft"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <label class="filterIptTxt fLeft">状态：</label></td>
                        <td>
                            <asp:DropDownList ID="ddlStatus" runat="server" CssClass="default widthMax ddlStatus">
                            </asp:DropDownList></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td colspan="3">
                            <div class="operationBox">
                                <ul class="operationBtnBox">
                                    <li><a class="operationBtn query fLeft" href="TemplateInfoConfig.aspx" target="_blank">添加</a></li>
                                       <li> <asp:Button ID="btnSearch" runat="server" OnClientClick="layer.load('查询中，请稍候...');" Text="查询" OnClick="btnSearch_Click" CssClass="operationBtn query btnhsearch fLeft" /></li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                </table>
                <ul class="filterList" style="display: none;">
                    <li>
                        <label class="filterTxt">创建时间段：</label>
                        <asp:TextBox ID="txtStartDate" runat="server" CssClass="filterIpt dateIpt" onfocus="WdatePicker()"></asp:TextBox>
                        <label>~</label>
                        <asp:TextBox ID="txtEndDate" runat="server" CssClass="filterIpt dateIpt" onfocus="WdatePicker()"></asp:TextBox>
                    </li>
                </ul>
            </div>
        </div>

        <div class="cutOffRule clearBoth"></div>

        <div class="resultBox">
            <table class="tableOne tableOneLayout">
                <asp:Repeater ID="TemplateList" runat="server" OnItemCommand="TemplateList_ItemCommand" OnItemDataBound="TemplateList_ItemDataBound">
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th style="width: 35px;">序号</th>
                                <th style="width: 110px;">
                                    <asp:LinkButton ID="TemplateName" runat="server" Text="模板名称" CommandName="TemplateName" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);" OnClientClick="layer.load('正在排序，请稍候...');"></asp:LinkButton></th>
                                <th style="width: 120px;">
                                    <asp:LinkButton ID="TemplateDesc" runat="server" Text="模板描述" CommandName="TemplateDesc" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);" OnClientClick="layer.load('正在排序，请稍候...');"></asp:LinkButton></th>
                                <th style="width: 110px;">
                                    <asp:LinkButton ID="BigCategory" runat="server" Text="报表大类" CommandName="BigCategory" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);" OnClientClick="layer.load('正在排序，请稍候...');"></asp:LinkButton></th>
                                <th style="width: 110px;">
                                    <asp:LinkButton ID="SmallCategory" runat="server" Text="报表细类" CommandName="SmallCategory" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);" OnClientClick="layer.load('正在排序，请稍候...');"></asp:LinkButton></th>
                                <th style="width: 50px;">报表使用单位</th>
                                <th width="90">
                                    <asp:LinkButton ID="Created" runat="server" Text="创建时间" CommandName="Created" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);" OnClientClick="layer.load('正在排序，请稍候...');"></asp:LinkButton></th>
                                <th style="width: 100px;">
                                    <asp:LinkButton ID="Author" runat="server" Text="创建者" CommandName="Author" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);" OnClientClick="layer.load('正在排序，请稍候...');"></asp:LinkButton></th>
                                <th style="width: 90px;">功能页面</th>
                                <th style="width: 35px;">
                                    <asp:LinkButton ID="TemplateStatus" runat="server" Text="状态" CommandName="TemplateStatus" Style="font-size: 12px; font-family: 'Microsoft YaHei', 微软雅黑, Arial, Helvetica, sans-serif; color: rgb(68, 68, 68);" OnClientClick="layer.load('正在排序，请稍候...');"></asp:LinkButton></th>
                                <th class="lastMenu" width="174">操作</th>
                            </tr>
                        </thead>
                        <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td width="35"><%#Eval("bs_row_num") %></td>
                            <td style="word-wrap: break-word; word-break: break-all; width: 110px;"><%#Eval("TemplateName") %></td>
                            <td style="word-wrap: break-word; word-break: break-all; width: 120px;"><%#Eval("TemplateDesc") %></td>
                            <td style="word-wrap: break-word; word-break: break-all; width: 110px;"><%#Eval("bigcate") %></td>
                            <td style="word-wrap: break-word; word-break: break-all; width: 110px;"><%#Eval("smallcate") %></td>
                            <td style="word-wrap: break-word; word-break: break-all; width: 50px;"><%#Eval("Unit") %></td>
                            <td style="word-wrap: break-word; word-break: break-all; width: 90px;"><%#Eval("Created") %></td>
                            <td style="word-wrap: break-word; word-break: break-all; width: 100px;"><%#CMICT.CSP.BLL.Components.BaseComponent.GetDisplayName(Eval("Author").ToString().Split(';')[0]) %></td>
                            <td style="word-wrap: break-word; word-break: break-all; width: 90px;"><%#Eval("pagelist") %></td>
                            <td width="35"><%#Eval("statusname") %></td>
                            <td width="174">
                                <a class="operateBtn copy" href="javascript:void(0);" title="复制" onclick="executecopy('<%#Eval("TemplateID") %>','<%#Eval("TemplateName") %>');" style='<%# (Convert.ToString(Eval("TemplateStatus"))=="DRAFT"||Convert.ToString(Eval("TemplateStatus"))=="DISABLE")? "display:none": ""%>'></a>
                                <a class="operateBtn edit" href="TemplateInfoConfig.aspx?templateID=<%#Eval("TemplateID") %>&sourceID=<%#Eval("SourceID") %>&type=edit" title="编辑" target="_blank"></a>
                                <a class="operateBtn add" href="javascript:void(0);" title="禁用" onclick="executeenable('<%#Eval("TemplateID") %>');" style='<%# (Convert.ToString(Eval("TemplateStatus"))=="DRAFT"||Convert.ToString(Eval("TemplateStatus"))=="DISABLE")? "display:none": ""%>'></a>
                                <a class="operateBtn start" href="javascript:void(0);" title="启用" onclick="executeuse('<%#Eval("TemplateID") %>');" style='<%# (Convert.ToString(Eval("TemplateStatus"))=="DISABLE")? "": "display:none"%>'></a>
                                <a class="operateBtn remove" href="javascript:void(0);" title="删除" onclick="executedel('<%#Eval("TemplateID") %>','<%#Eval("SourceID") %>');" style='<%# (Convert.ToString(Eval("TemplateStatus"))=="ENABLE")? "display:none": ""%>'></a>
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
            layer.closeLoad();
            $(document).attr("title", $(".hidtitle").val());
            addtiptodropdown();
        } catch (err) {

        }
    });
    //禁用
    function executeenable(sid) {
        $(".hidtemplateid").val(sid);
        var msg = "您确定要禁用吗？";
        RegisterButtonConfirm(msg, 4);
        return false;
    }
    //启用
    function executeuse(sid) {
        $(".hidtemplateid").val(sid);
        var msg = "您确定要启用吗？";
        RegisterButtonConfirm(msg, 8);
        return false;
    }
    //复制
    function executecopy(sid, sname) {
        $(".hidtemplateid").val(sid);
        var d = new Date();
        var copyname = d.getFullYear().toString() + (d.getMonth() + 1).toString() + d.getDate().toString() + d.getHours().toString() + d.getMinutes().toString() + d.getSeconds().toString() + d.getMilliseconds().toString();
        var newname = sname + copyname;
        $(".hidCopyName").val(newname);
        var msg = "模板将被复制，会生成一个新的模板，配置信息与原模板一致，模板名称为" + newname + ",您确定要复制吗？";
        RegisterButtonConfirm(msg, 2);
        return false;
    }
    function executedel(sid, souid) {
        $(".hidtemplateid").val(sid);
        $(".hidsourceid").val(souid);
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
                    if (type == 2) {//复制
                        $(".btncopy").click();
                    }
                    if (type == 4) {//禁用
                        $(".btnenable").click();
                    }
                    if (type == 8) {//启用
                        $(".btnuse").click();
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
