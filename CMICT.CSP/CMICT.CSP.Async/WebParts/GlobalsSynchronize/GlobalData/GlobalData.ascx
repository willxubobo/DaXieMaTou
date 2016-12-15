<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GlobalData.ascx.cs" Inherits="CMICT.CSP.Async.WebParts.GlobalsSynchronize.GlobalData.GlobalData" %>
<%@ Register Assembly="AspNetPager, Version=7.4.5.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>
<script src="/_layouts/15/CMICT.CSP.Branding/jquery/jquery-1.9.1.min.js"></script>
<script src="/_layouts/15/CMICT.CSP.Branding/jquery/layer/layer.min.js"></script>
<script src="/_layouts/15/CMICT.CSP.Branding/jqcool.net-DropKick/jquery.dropkick.js"></script>
<link rel="stylesheet" type="text/css" href="/_layouts/15/CMICT.CSP.Branding/css/base.css" />
<style type="text/css">
    .auto-style1 {
        height: 20px;
    }

    .btn {
        display: inline-block;
        text-align: center;
        width: 60px;
        height: 29px;
        border-radius: 2px;
        -moz-border-radius: 2px;
        -webkit-border-radius: 2px;
        font-size: 12px !important;
        font-family: Microsoft YaHei,微软雅黑;
        color: #fff !important;
        line-height: 29px;
        padding: 0px !important;
        margin-left: 5px;
        margin-right: 5px;
        min-width: 0px !important;
        border: none !important;
        cursor: pointer;
    }

    .cutOffRule {
        width: 100%;
        height: 1px;
        border-top: 1px solid #d9d9d9;
    }
</style>

<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="lbtnExport" />
    </Triggers>
    <ContentTemplate>
        <div style="display: none;">
            <input type="hidden" id="hidpagesize" runat="server" value="20" class="hidpagesize" />
            <asp:Button ID="Button1" CssClass="btnhsearch" OnClick="lbtnQuery_OnClick" runat="server" Text="Button" />
            <asp:HiddenField ID="hdEntityName" runat="server" />
        </div>
        <div class="filterBox">
            <div class="filterIptBox">
                <table class="filterTable">
                    <tr>
                        <td style="width: 40%;">
                            <div style="width: 200px;">
                                <asp:DropDownList ID="ddlEntityList" runat="server" CssClass="default widthMax ddlStatus">
                                </asp:DropDownList>
                            </div>

                        </td>
                        <td>
                            <div class="operationBox">

                                <ul class="operationBtnBox">
                                    <li>
                                        <asp:LinkButton ID="lbtnSync" OnClientClick=" return CheckSelectEntityNeww();" OnClick="lbtnSync_OnClick" CssClass="operationBtn query fLeft" runat="server">全局同步</asp:LinkButton></li>
                                    <li><a class="operationBtn query fLeft" onclick="CheckSelectEntity();">查询</a>
                                    </li>
                                    <li>
                                        <asp:LinkButton ID="lbtnExport" CssClass="operationBtn query fLeft" OnClientClick="return CheckSelectEntityNew();" OnClick="lbtnExport_OnClick" runat="server">导出</asp:LinkButton></li>
                                </ul>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="cutOffRule clearBoth"></div>
        <div class="resultBox">
            <asp:GridView ID="GvDataSource" EmptyDataText="无数据" CssClass="tableOne tableOneLayout" OnPreRender="GvDataSource_OnPreRender" Width="100%" runat="server">
            </asp:GridView>
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
    _spSuppressFormOnSubmitWrapper = true;
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_beginRequest(function () {

    });
    prm.add_endRequest(function (sender, args) {
        args.set_errorHandled(true);
        try {
            if ($('.default').length > 0) {
                $('.default').dropkick();
            }


            InitTableColumns();
            $(document).attr("title", $(".hidtitle").val());
            layer.closeAll();

        } catch (err) {

        }
    });

    function CheckSelectEntity() {
        var sVal = $("#<%=this.ddlEntityList.ClientID%>").val();
        if ($.trim(sVal) == "0") {
            layer.alert("请选择实体！");
            return false;
        } else {
            $(".btnhsearch").click();
            layer.load('正在查询，请稍候...');
            return true;
        }
    }
    function CheckSelectEntityNew() {
        var sVal = $("#<%=this.ddlEntityList.ClientID%>").val();
        if ($.trim(sVal) == "0") {
            layer.alert("请选择实体！");
            return false;
        } else {
            layer.load('正在导出，请稍候...');
            return true;
        }
    }
    function CheckSelectEntityNeww() {

        layer.load('正在同步，请稍候...');
        $.ajax(
      {
          type: "POST",
          url: "/_layouts/15/CMICT.CSP.Async/ApplicationPageHandler.aspx/SysncGlobalData",
          contentType: "application/json; charset=utf-8",
          dataType: "text",
          timeout: 10000000,
          data: '',
          success: function (results) {
              if (results.d != undefined) {
                  var company = results.d;
              }
              layer.closeAll();
          },
          complete: function (XMLHttpRequest, status) { //请求完成后最终执行参数
              if (status == 'timeout') {//超时,status还有success,error等值的情况
                  layer.alert("超时");
              }
          },
          error: function (err) {
              layer.alert(err.status + " - " + err.statusText);
              layer.closeAll();
          }
      });
        return false;

    }
    //分页用 －will.xu
    function changesize(obj) {
        $(".hidpagesize").val(obj);
        $(".btnhsearch").click();
    }

    function setvaluesel(obj) {
        $(".pcount").val(obj);
    }

    function InitTableColumns() {
        $(".ee").parent().hide();
        $(".cc").parent().hide();
        var length = $(".tableOne").find("th").length;

        var pageSize = $("#<%=this.hidpagesize.ClientID%>").val();
        var addCol = "<td scope=\"col\" class='thAdd' style='width:50px;background-color:null;' rowspan='" + (Number(pageSize) + 1) + "'><a class='aExpansion' style='text-decoration: blink; cursor: pointer;' title='展开' onclick='ExpansionAll(this)'> >>> </a><a class='aColspan' style='display:none;text-decoration: blink; cursor: pointer;' title='收缩' onclick='ColspanAll(this)'> <<< </a></td>";
        //alert(length);
        $('.tableOne tr:first th').attr("style", " height: 30px;vertical-align: middle;background-color: #dcdcdc;border-right: 1px solid #fff;border-bottom: 1px solid #fff;font-size: 12px;font-weight: bold;text-align: center;font-family: Microsoft YaHei,微软雅黑;word-break: break-all;");
        if (length > 8) {
            $(".ee").parent().show();
            $(".cc").parent().hide();
            for (var i = 8; i <= length; i++) {
                $('.tableOne tr').find("td:eq(" + i + ")").hide();
                $('.tableOne tr').find("th:eq(" + i + ")").hide();
            }

            $('.tableOne tr:first').append(addCol);
        } else {
            $(".thAdd").remove();
        }

    }

    function ExpansionAll(obj) {
        $('.tableOne tr').find("td").show();
        $('.tableOne tr').find("th").show();
        //$(".expansionAll").hide();
        $(obj).hide();
        $(".aColspan").show();
    }

    function ColspanAll(obj) {
        $('.tableOne tr').find("td").show();
        $('.tableOne tr').find("th").show();
        //$(".expansionAll").hide();
        $(obj).hide();
        $(".aExpansion").show();

        var length = $(".tableOne").find("th").length;
        if (length > 8) {
            for (var i = 8; i <= length; i++) {
                $('.tableOne tr').find("td:eq(" + i + ")").hide();
                $('.tableOne tr').find("th:eq(" + i + ")").hide();
            }
        }

    }
</script>
