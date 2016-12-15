<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BusinessSearch.ascx.cs" Inherits="CMICT.CSP.Web.BusinessSearch.BusinessSearch.BusinessSearch" %>
<%@ Register Assembly="AspNetPager, Version=7.4.5.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<style type="text/css">

    .ms-TPBorder {
        width: 100% !important;
    }
     .widthMaxSource {
    width: 100% !important;
    min-width: 130px;
    height: 30px;
    line-height: 28px;
}
     .costDesBusiness {
    width: 100% !important;
    display: inline-block;
    overflow: hidden;
    white-space: nowrap;
    text-overflow: ellipsis;
}
</style>

<link rel="stylesheet" type="text/css" href="/_layouts/15/CMICT.CSP.Branding/css/jquery.multiselect.css" />
<link rel="stylesheet" type="text/css" href="/_layouts/15/CMICT.CSP.Branding/css/style.css" />
<link rel="stylesheet" type="text/css" href="/_layouts/15/CMICT.CSP.Branding/css/prettify.css" />
<link rel="stylesheet" type="text/css" href="/_layouts/15/CMICT.CSP.Branding/css/jquery-ui.css" /> 

<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/js/jquery.ui.core.js"></script>
<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/js/jquery.ui.widget.js"></script>
<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/js/prettify.js"></script>
<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/js/jquery.multiselect.js"></script>
<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/laydate/laydate.js"></script>
<a id="<%=this.ID %>_top"></a>
<div class="cutOffRule clearBoth"></div> 
<div class="filterBox marginTop10">
    <div class="filterIptBox" id="ulquerylist" runat="server">
    </div>
</div>
<div class="<%=this.ID %>_content"> 
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="btnExport" />
    </Triggers>
    <ContentTemplate>
        <asp:Button ID="btnSearch" runat="server" OnClientClick="layer.load('查询中，请稍候...',9);" OnClick="btnSearch_Click" Text="查询用" Style="display: none;" />
         <asp:Button ID="btntrsearch" runat="server" OnClick="btntrsearch_Click" Text="通信用" Style="display: none;" />
        <input type="hidden" runat="server" id="hdIsSender" class="hdIsSender" />
         <asp:Button ID="btnColumnSort" runat="server" OnClick="btnColumnSort_Click" Text="列名排序用" Style="display: none;" />
        <asp:Button ID="btnExport" runat="server" OnClick="btnExport_Click" Text="导出用" OnClientClick="layer.load('正在导出，请稍候...',9);" Style="display: none;" />
        <input type="hidden" id="hidqueryinfo" runat="server" />
        
        <input type="hidden" id="hidMergeColumn" runat="server" />
        <input type="hidden" id="hidDisplayName" runat="server" />
        <input type="hidden" id="hidColumnName" runat="server" />
        <input type="hidden" id="hidtrcontent" runat="server" />
        <input type="hidden" id="hidsortcolname" runat="server" />
        <input type="hidden" id="hidsortdisname" runat="server" />
        <input type="hidden" id="hidparainfo" runat="server" />
        <input type="hidden" id="hidsorttext" runat="server" />
        <input type="hidden" id="hiddisplaytype" runat="server" />
        <input type="hidden" id="hidtemplatename" runat="server" />
        <input type="hidden" id="hidclicktrid" runat="server" />
        <input type="hidden" id="hidispager" runat="server" />
        <input type="hidden" id="hidtrswhere" runat="server" />
        <input type="hidden" id="hidpagewebid" runat="server" />
         <input type="hidden" id="hidtitle" runat="server" />
        <input type="hidden" id="hidsearchbtnid" runat="server" />
        <input type="hidden" id="hidisautosearch" runat="server" />
        <input type="hidden" id="hidautosearchvalue" runat="server" />
        <input type="hidden" id="hiduserscript" runat="server" />
        <input type="hidden" id="hidnorepeaterclick" runat="server" />
        <input type="hidden" id="hidhavechild" runat="server" />
        <input type="hidden" id="hidisproc" runat="server" />
        <input type="hidden" id="hidfastdate" runat="server" />
        <input type="hidden" id="hidsortnames" runat="server" />
        <div class="cutOffRule clearBoth"></div> 
        <div class="resultBox">
            <table class="tableOne tableOneLayout tableOnenormal" id="<%=this.ID %>_normaltab">
                <thead>

                    </thead>
                <tbody>
                    <asp:Repeater ID="rpttablelist" runat="server">
                        <ItemTemplate>
                            <tr onclick="<%=this.ID %>gettrvaluetocommit(this,'<%=this.ID %>_tr_<%#Container.ItemIndex+1 %>')" id="<%=this.ID %>_tr_<%#Container.ItemIndex+1 %>">
                                <%#Eval("Columntr") %>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <asp:Label ID="lblnodata" runat="server" Text="暂无数据！" Visible="false" style="font-size: 12px;"></asp:Label>
        </div>
        <div class="pageBox" id="pagerdiv" runat="server" visible="false"> <input type="hidden" id="hidwebpartid" runat="server" /><input type="hidden" id="hidpagesize" runat="server"/>
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
                CustomInfoHTML="共<font color='#ff0000'>%RecordCount%</font>条  每页显示<select id='pcount' class='default pcount' onchange='changesizesearch(this.options[this.options.selectedIndex].value)'><option value='5'>5</option><option value='10'>10</option><option value='15'>15</option><option value='20' selected>20</option><option value='30'>30</option><option value='40'>40</option><option value='50'>50</option></select>"
                OnPageChanged="AspNetPager1_PageChanged">
            </webdiyer:AspNetPager>
        </div>

    </ContentTemplate>
</asp:UpdatePanel>
</div>
<script type="text/javascript">
    _spSuppressFormOnSubmitWrapper = true;

    function <%=this.ID %>checkenter(objid) {
        objid = objid.replace("searchb", "");
        $("input[id*='hidsearchbtnid']").each(function () {
            $(this).val(objid);
        });
    }
    
    document.onkeydown = function (e) {
        if (!e)
            e = window.event;
        if ((e.keyCode || e.which) == 13) {
            var sbtnid = "";
            $("input[id*='hidsearchbtnid']").each(function () {
                sbtnid=$(this).val();
            });
            if (sbtnid != "") {
                //layer.load('查询中，请稍候...');
                //$("#" + sbtnid).click();
                setTimeout(function () {
                    eval("" + sbtnid + "searchform()");
                }, 300);
                
            }
        }
    };
    //显示隐藏form
    function <%=this.ID%>visableform(obj) {
        var btext = $.trim($(obj).text());
        if (btext == "隐藏") {
            $(".<%=this.ID%>_content").hide();
            $(obj).text("显示");
        } else {
            $(".<%=this.ID%>_content").show();
            $(obj).text("隐藏");
        }
    }
    //分页用 －will.xu
    function <%=this.ID %>changesizesearch(obj) {
        $("#<%=hidpagesize.ClientID%>").val(obj);
        $("#<%=btnSearch.ClientID%>").click();
    }
    function <%=this.ID %>setvalueselsearch(obj) {
        if ($("#<%=pagerdiv.ClientID%>").length > 0) {
            //$("#<%=pagerdiv.ClientID%>").find("select").val(obj);
            $("#<%=pagerdiv.ClientID%>").find(".dk_option_current").attr("class", "");
            $("#<%=pagerdiv.ClientID%>").find("a").each(function () {
                if ($(this).attr("class") != "dk_toggle dk_label") {
                    if ($(this).attr("data-dk-dropdown-value") != undefined && $(this).attr("data-dk-dropdown-value") != null && $(this).attr("data-dk-dropdown-value") != "") {
                        if ($.trim($(this).attr("data-dk-dropdown-value")) == obj) {
                            $(this).parent().attr("class", "dk_option_current");
                            $(this).parent().parent().parent().prev().text($(this).text());
                        }
                    }
                    
                }
            });
        }
    }
    //通信
    function <%=this.ID %>gettrvaluetocommit(obj, trid) {
        var havechild = "";
        $("input[id*='hidhavechild']").each(function () {
            var hcid = $.trim($(this).val());
            if (hcid != "") {
                havechild="yes";
            }
        });
        if ($("#<%=hdIsSender.ClientID%>").val() == "Y") {
            layer.load('子报表查询中，请稍候...');
        }
        $("#<%=hidclicktrid.ClientID%>").val(trid);
        
$("input[id*='hidnorepeaterclick']").each(function () {
            $(this).val(trid);
        });
        var result = '';
        $(obj).find("td").each(function () {
            var cname = $(this).attr("cname");
            if (cname != undefined && cname != "" && cname != null) {
                cname = cname.replace(/!/g, '<tr>');
                cname = cname.replace(/;/g, '<td>');
                var ttext = $(this).text();
                ttext = ttext.replace(/!/g, '<tr>');
                ttext = ttext.replace(/;/g, '<td>');
                result += cname + "!" + ttext + ";";
            }
        }); 
        if (result != '' && $("#<%=hdIsSender.ClientID%>").val() == "Y") {
            $("#<%=hidtrcontent.ClientID%>").val(result);
            $("#<%=btntrsearch.ClientID%>").click();
        }
    }
    //默认执行点击第一行通信
    function <%=this.ID %>gettrvaluetocommitdefault(trid) {
        if (trid == "0") {
            $("#<%=hidtrcontent.ClientID%>").val("");
            if ($("#<%=hdIsSender.ClientID%>").val() == "Y") {
                $("#<%=btntrsearch.ClientID%>").click();
            }
            } else {
            $("#<%=hidclicktrid.ClientID%>").val(trid);
            var result = '';
            $("#" + trid).find("td").each(function () {
                $(this).attr("class", "tableOneTrClickBg");
                var cname = $(this).attr("cname");
                if (cname != undefined && cname != "" && cname != null) {
                    cname = cname.replace(/!/g, '<tr>');
                    cname = cname.replace(/;/g, '<td>');
                    var ttext = $(this).text();
                    ttext = ttext.replace(/!/g, '<tr>');
                    ttext = ttext.replace(/;/g, '<td>');
                    result += cname + "!" + ttext + ";";
                }
            });
            if (result != '') {
                $("#<%=hidtrcontent.ClientID%>").val(result);
                if ($("#<%=hdIsSender.ClientID%>").val() == "Y") {
                    $("#<%=btntrsearch.ClientID%>").click();
                }
            }
        }
    }
    //生成非田字型表头
    function <%=this.ID %>createtabthead(sortcolname, sorttext, type) {
        var mertr = '';
        var nortr = '<tr>';
        var mernames = $("#<%=hidMergeColumn.ClientID%>").val();
        var disnames = $("#<%=hidDisplayName.ClientID%>").val();
        var colnames = $("#<%=hidColumnName.ClientID%>").val();
        var sortnames = $.trim($("#<%=hidsortnames.ClientID%>").val()).split(',');
        var colnamelist = colnames.split(',');//列名集合
        var disnamelist = disnames.split(',');
        var havemer = $.trim(mernames.replace(/,/g, ''));
        if (havemer != "") {//有合并表头的情况
            nortr += "<th rowspan='2' style='width:50px !important;'>序号</th>";
            mertr = '<tr>';
            var mernamelist = mernames.split(',');
            for (var i = 0; i < mernamelist.length; i++) {
                if ($.trim(mernamelist[i]) != "") {//有合并表头
                    var csnum = <%=this.ID %>getmercolspan(mernamelist, i, mernamelist[i]);
                    nortr += "<th colspan='" + csnum + "'>" + mernamelist[i] + "</th>";
                    var csnumcount = parseInt(csnum);
                    if (csnumcount > 1) {
                        i = i + csnumcount-1;
                    }
                } else {
                    var dname = disnamelist[i];
                    if ($.trim(sortcolname) == "") {
                        for (var u = 0; u < sortnames.length; u++) {
                            if ($.trim(sortnames[u]) != "") {
                                var sclist=$.trim(sortnames[u]).split(' ');
                                var scname = sclist[0].replace("[", "").replace("]", "");
                                var scasc = sclist[1];
                                if ($.trim(scname) == colnamelist[i]) {
                                    if (scasc == "asc") {
                                        dname = disnamelist[i] + "▲";
                                    }
                                    if (scasc == "desc") {
                                        dname = disnamelist[i] + "▼";
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    if ($.trim(sortcolname) != "") {
                        if (colnamelist[i] == sortcolname && sorttext!="") {
                            dname = sorttext;
                        } 
                    }
                    var sortclick = "";
                    //if (type != "ROW") {
                        sortclick = " style='cursor: pointer;' onclick=\"<%=this.ID %>columnsort('" + colnamelist[i] + "','" + disnamelist[i] + "')\"";
                    //} else {
                    //    dname = disnamelist[i];
                    //}
                    nortr += "<th rowspan='2' "+sortclick+">" + dname + "</th>";
                }
            }
            for (var i = 0; i < mernamelist.length; i++) {//单独累加第二行
                if ($.trim(mernamelist[i]) != "") {//有合并表头
                    var dname = disnamelist[i];
                    if ($.trim(sortcolname) == "") {
                        for (var u = 0; u < sortnames.length; u++) {
                            if ($.trim(sortnames[u]) != "") {
                                var sclist = $.trim(sortnames[u]).split(' ');
                                var scname = sclist[0].replace("[", "").replace("]", "");
                                var scasc = sclist[1];
                                if ($.trim(scname) == colnamelist[i]) {
                                    if (scasc == "asc") {
                                        dname = disnamelist[i] + "▲";
                                    }
                                    if (scasc == "desc") {
                                        dname = disnamelist[i] + "▼";
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    if ($.trim(sortcolname) != "") {
                        if (colnamelist[i] == sortcolname && sorttext != "") {
                            dname = sorttext;
                        }
                    }
                    var sortclick = "";
                    //if (type != "ROW") {
                        sortclick = "style='cursor: pointer;' onclick=\"<%=this.ID %>columnsort('" + colnamelist[i] + "','" + disnamelist[i] + "')\"";
                    //} else {
                    //    dname = disnamelist[i];
                    //}
                    mertr += "<th " + sortclick + ">" + dname + '</th>';
                }
            }
            mertr += '</tr>';
        } else {//没有合并表头
            nortr += "<th style='width:50px !important;'>序号</th>";
            for (var i = 0; i < disnamelist.length; i++) {
                var dname = disnamelist[i];
                if ($.trim(sortcolname) == "") {
                    for (var u = 0; u < sortnames.length; u++) {
                        if ($.trim(sortnames[u]) != "") {
                            var sclist = $.trim(sortnames[u]).split(' ');
                            var scname = sclist[0].replace("[", "").replace("]", "");
                            var scasc = sclist[1];
                            if ($.trim(scname) == colnamelist[i]) {
                                if (scasc == "asc") {
                                    dname = disnamelist[i] + "▲";
                                }
                                if (scasc == "desc") {
                                    dname = disnamelist[i] + "▼";
                                }
                                break;
                            }
                        }
                    }
                }
                if ($.trim(sortcolname) != "") {
                    if (colnamelist[i] == sortcolname && sorttext != "") {
                        dname = sorttext;
                    }
                }
                var sortclick = "";
                //if (type != "ROW") {
                    sortclick = "style='cursor: pointer;' onclick=\"<%=this.ID %>columnsort('" + colnamelist[i] + "','" + disnamelist[i] + "')\"";
                //} else {
                //    dname = disnamelist[i];
                //}
                nortr += "<th " + sortclick + ">" + dname + "</th>";
            }
        }

        nortr += '</tr>';
        $("#<%=this.ID %>_normaltab thead").empty();
        $("#<%=this.ID %>_normaltab thead").append(nortr + mertr);
    }
    //生成田字型表头
    function <%=this.ID %>creategridthead(templatename) {
        var nortr = '<tr>';
        nortr += '<th width="50">序号</th>';
        nortr += "<th class=\"lastMenu\">"+templatename+"</th>";
        nortr += '</tr>';
        $("#<%=this.ID %>_normaltab thead").empty();
        $("#<%=this.ID %>_normaltab thead").append(nortr);
        $("#<%=this.ID %>_normaltab").attr("class", "tableThree tableOneLayout");
    }
    //执行排序方法
    function <%=this.ID %>columnsort(colname, disname) {
        if ($.trim(colname) != "") {
            $("#<%=hidsortcolname.ClientID%>").val(colname);
            $("#<%=hidsortdisname.ClientID%>").val(disname);
            $("#<%=btnColumnSort.ClientID%>").click();
        }
    }
    //得到colspan数据
    function <%=this.ID %>getmercolspan(mernamelist, sindex, comparev) {
        var csnum = 1;
        for (var i = (sindex+1) ; i < mernamelist.length; i++) {
            if (comparev == mernamelist[i]) {
                csnum++;
            } else {
                break;
            }
        }
        return csnum;
    }
    //导出
    function <%=this.ID%>exportform() {
        $("#<%=btnExport.ClientID %>").click();
    }
    //查询上个月
    function <%=this.ID%>lastmonthsearch(obj,colname, startd, endd) {
        $(document).find(".<%=this.ID%>fasta").each(function () {
            $(this).addClass("fastdateacolor").removeClass("fastdateacolorclick");
        });
        $(obj).removeClass("fastdateacolor").addClass("fastdateacolorclick");
        var fastwhere = colname + ">='" + startd + "' and " + colname + "<='" + endd + "'";
        $("#<%=hidfastdate.ClientID%>").val(fastwhere);
        eval("<%=this.ID%>searchform()");
    }
    function <%=this.ID%>searchform() {
        var queryinfo = '';
        var parainfo = '';//存储过程参数
        $("#<%=ulquerylist.ClientID%>").find("td").each(function () {
            if ($(this).find("select").length > 0) {
                var gtext = "";
                if ($(this).find(".dk_label").length > 0) {
                    gtext = $.trim($(this).find(".dk_label").text());//选择项
                } else {
                        if($(this).find("button").length>0){
                            gtext = $(this).find("button").attr("sv");
                        }else{
                                gtext = "全部";
                        }
                    //if ($(this).find("select").multiselect("getChecked").length > 0) {
                      //  gtext = $.trim($(this).find("select").multiselect("MyValues"));//选择项
                   // } else {
                      //  gtext = "全部";
                   // }
                }
                var comparev = $(this).find("select").attr("comparev");
                if (gtext != "全部"&&gtext!="") {
                    if (comparev.indexOf('@') == -1) {
                        if (gtext.indexOf(',') != -1) {
                            var gtlist = gtext.split(',');
                            queryinfo += "(";
                            for (var m = 0; m < gtlist.length; m++) {
                                var gvalue = comparev.replace("{0}", $.trim(gtlist[m]));
                                queryinfo += gvalue;
                                if ((m + 1) < gtlist.length) {
                                    queryinfo += " or "
                                }
                            }
                            queryinfo += ") and ";
                        } else {
                            var gvalue = comparev.replace("{0}", $.trim(gtext));
                            queryinfo += gvalue + "and ";
                        }
                    } else {
                        var paraname = comparev.split('=')[0];
                        parainfo += paraname + "!" + gtext + ";";
                    }
                } else {
                    if (comparev.indexOf('@') != -1) {
                        var paraname = comparev.split('=')[0];
                        parainfo += paraname + "!;";
                    }
                }
            } else {
                if ($(this).find("input").length > 0) {
                    var gtext = $.trim($(this).find("input").val());
                    var comparev = $(this).find("input").attr("comparev");
                    if (gtext != "") {
                        if (comparev.indexOf('@') == -1) {
                            var gvalue = comparev.replace("{0}", gtext);
                            queryinfo += gvalue + "and ";
                        } else {
                            var paraname = comparev.split('=')[0];
                            parainfo += paraname + "!" + gtext + ";";
                        }
                    } else {
                        if (comparev.indexOf('@') != -1) {
                            var paraname = comparev.split('=')[0];
                            parainfo += paraname + "!;";
                        }
                    }
                }
            }
        });
        var fastdate = $("#<%=hidfastdate.ClientID%>").val();
        if ($.trim(queryinfo) != "") {
            queryinfo = queryinfo.substr(0, queryinfo.length - 4);
            if ($.trim(fastdate) != "") {
                queryinfo += " and " + fastdate;
            }
        } else {
            if ($.trim(fastdate) != "") {
                queryinfo = fastdate;
            }
        }
        //if ($.trim(parainfo) != "") {
            $("#<%=hidparainfo.ClientID%>").val(parainfo);
        //}
        $("#<%=hidqueryinfo.ClientID%>").val(queryinfo);
        $("#<%=btnSearch.ClientID %>").click();
    }
    
    function <%=this.ID%>resetform() {
        $(document).find(".<%=this.ID%>fasta").each(function () {
            $(this).addClass("fastdateacolor").removeClass("fastdateacolorclick");
        });
        $("#<%=hidfastdate.ClientID%>").val("");
        $("#<%=ulquerylist.ClientID%>").find("td").each(function () {
            if ($(this).find("select").length > 0) {
                var multiple=$(this).find("select").attr("multiple");
                if (multiple != null && multiple != undefined && multiple != "") {
                    $(this).find("select").multiselect("uncheckAll");
                } else {
                    var stt = $(this).find("select").find("option:first").val();
                    $(this).find(".dk_label").text(stt).attr("title",stt);//选择项
                    $(this).find(".dk_options_inner").find("a").each(function () {
                        if ($.trim($(this).text()) == stt) {
                            $(this).parent().attr("class", "dk_option_current");
                        } else {
                            $(this).parent().attr("class", "");
                        }
                    });
                }
            } else {
                if ($(this).find("input").length > 0) {
                    $(this).find("input").val("");
                }
            }
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
            ChangeOnChange();
            createtheadonfresh();
            footerstyle();
            recovertrbg();
            layer.closeAll();
            $(document).attr("title", $("#<%=hidtitle.ClientID%>").val());
            inittableonetrstyle();
            var isautosearch = $("#<%=hidautosearchvalue.ClientID %>").val();
            //if(isautosearch=="Y"){
                var userscript = $.trim($("#<%=hiduserscript.ClientID %>").val());
                if(userscript!=""){
                     eval(userscript);
                   
                }
            <%=this.ID %>addtiptoddl();
            //}
        } catch (err) {

        }
    });
    //防止SQL注入
    function <%=this.ID %>AntiSqlValid(obj) {
        var objvalue = $(obj).val();
        re = /select|update|delete|exec|count|'|"|=|;|>|<|%/i;
        if (re.test(objvalue)) {
            layer.alert("请您不要在文本框中输入特殊字符和SQL关键字！"); //注意中文乱 码
            $(obj).val("");
            $(obj).focus();
            return false;
        }
    }
    //更改每页显示数量onchange
    function ChangeOnChange() {
        $(".pageBox").each(function () {
            var webid = $(this).find("input[id*='hidwebpartid']").val();
            var pagesize = $(this).find("input[id*='hidpagesize']").val();
            $(this).find("select").attr("onchange", webid + "changesizesearch(this.options[this.options.selectedIndex].value)");
        });
    }
    //页面刷新时生成表头
    function createtheadonfresh() {
        $("input[id*='hidwebpartid']").each(function () {
            var webid = $(this).val();
            var sortcolname = $("input[id*='" + webid + "_hidsortcolname']").val();
            var sorttext = $("input[id*='" + webid + "_hidsorttext']").val();
            var distype = $("input[id*='" + webid + "_hiddisplaytype']").val();
            var temname = $("input[id*='" + webid + "_hidtemplatename']").val();
            var pagesize = $("input[id*='" + webid + "_hidpagesize']").val();
            if (distype != "GRID") {
                eval(""+webid+"createtabthead('"+sortcolname+"', '"+sorttext+"', '"+distype+"')");
            } else {
                eval("" + webid + "creategridthead('" + temname + "')");
            }
            eval("" + webid + "setvalueselsearch('" + pagesize + "')");
        });
    }
    //页面刷新后还原点击的行背景色
    function recovertrbg() {
        $("input[id*='hidclicktrid']").each(function () {
            var trid = $(this).val();
            if ($.trim(trid) != "") {
                $("#" + trid + " td").each(function () {
                    $(this).addClass("tableOneTrClickBg");
                });
            }
        });
        
    }
    function <%=this.ID %>defaultclickonetr(trid){
        var trtid=trid+"_tr_1";
        if ($("#" + trtid).length > 0) {
                <%=this.ID %>gettrvaluetocommitdefault(trtid);
        } else {
            <%=this.ID %>gettrvaluetocommitdefault('0');
        }
        //window.location.href = '#<%=this.ID %>_top';
    }

    function <%=this.ID %>disvisbileexport(objid) {
        if ($("." + objid).length > 0) {
            $("." + objid).hide();
        }
    }
    function <%=this.ID %>visbileexport(objid) {
        if ($("." + objid).length > 0) {
            $("." + objid).show();
        }
    }
    
    $(function ($) {
        ChangeOnChange();
        createtheadonfresh();
        footerstyle();
        recovertrbg();
        inittableonetrstyle();
        $("#<%=hidtitle.ClientID%>").val($(document).attr("title"));
        $(".multisel").multiselect({
            noneSelectedText: "全部",
            checkAllText: "全选",
            uncheckAllText: '清空',
            selectedList: 4
        });
        var isautos = $("#<%=hidisautosearch.ClientID%>").val();
        if (isautos == "Y") {
            //var trid = "=this.ID%>_tr_1";
            //if ($("#" + trid).length > 0) {
                var mid = "<%=this.ID %>";
                <%=this.ID %>defaultclickonetr(mid);
            //}
        }
        <%=this.ID %>addtiptoddl();
    });
    //给数据源下拉加title
    function <%=this.ID %>addtiptoddl() {
        $("div[class*='_UsageFilter']").find("a").each(function () {
            var atipname = $(this).html();
            $(this).attr("title", atipname).addClass("costDesBusiness");
        });
    }
</script>
