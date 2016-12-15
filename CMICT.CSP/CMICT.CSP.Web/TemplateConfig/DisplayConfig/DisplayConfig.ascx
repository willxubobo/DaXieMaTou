<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DisplayConfig.ascx.cs" Inherits="CMICT.CSP.Web.TemplateConfig.DisplayConfig.DisplayConfig" %>

<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/laydate/laydate.js"></script>
<%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <Triggers>
        <asp:PostBackTrigger ControlID="lbtnSave" />
        <asp:PostBackTrigger ControlID="lbtnPrev" />
    </Triggers>
    <ContentTemplate>--%>
        <div class="progressBox">
            <ul class="progressStepBox">
                <li>
                    <div class="stepBox stepPrev">
                        <p>1.基本信息配置</p>
                        <span></span>
                    </div>
                </li>
                <li>
                    <div class="stepBox stepPrev">
                        <p>2.配置数据源</p>
                        <span></span>
                    </div>
                </li>
                <li>
                    <div class="stepBox stepcurrent">
                        <p>3.配置展示方式</p>
                        <span></span>
                    </div>
                </li>
                <li>
                    <div class="stepBox">
                        <p>4.配置筛选条件</p>
                        <span></span>
                    </div>
                </li>
                <li>
                    <div class="stepBox">
                        <p>5.配置业务信息查询通信</p>
                    </div>
                </li>
            </ul>
        </div>
<div id="putongdiv" class="hide layer_pageContent" style="display: none;"><img src="/_layouts/15/CMICT.CSP.Branding/images/putong.png"></div>
<div id="tianzidiv" class="hide layer_pageContent" style="display: none;"><img src="/_layouts/15/CMICT.CSP.Branding/images/tianzi.png"></div>
<div id="liehuizongdiv" class="hide layer_pageContent" style="display: none;"><img src="/_layouts/15/CMICT.CSP.Branding/images/liehuizong.png"></div>
<div id="hanghuizongdiv" class="hide layer_pageContent" style="display: none;"><img src="/_layouts/15/CMICT.CSP.Branding/images/hanghuizong.png"></div>
        <div class="stepOperateBox">
            <table class="tableTwo">
                <tr>
                    <td>
                        <label class="stepIptTxt">展现方式：</label></td>
                    <td>
                        <asp:DropDownList ID="ddlDisplayType" runat="server" CssClass="default w210 ddlDisplayType" onchange="changetype();">
                        </asp:DropDownList><span class="mustIcon">*</span>
                        <asp:TextBox ID="txtDisplayType" runat="server" CssClass="txtDisplayType" title="展现方式~50:!" onchange="deoxidizevalidate(this)" Style="display: none;"></asp:TextBox>
                    </td>
                    <td>
                        <div id="div<% =txtDisplayType.ClientID %>"></div>
                    </td>
                    <td><div class="putongliebiao tableview" style="display:none;cursor:pointer;" onclick="putong()"></div><div class="tianzixing tableview" style="display:none;cursor:pointer;" onclick="tianzi()"></div><div class="hanghuizong tableview" style="display:none;cursor:pointer;" onclick="hanghuizong()"></div><div class="liehuizong tableview" style="display:none;cursor:pointer;" onclick="liehuizong()"></div></td>
                </tr>
                <tr class="GRID" style="display: none;">
                    <td>
                        <label class="stepIptTxt">每行字段数：</label></td>
                    <td>
                        <asp:TextBox ID="txtColumnSize" runat="server" CssClass="txtColumnSize" title="每行字段数~50:" onchange="deoxidizevalidate(this)" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
                        <span class="mustIcon">*</span>
                    </td>
                    <td>
                        <div id="div<% =txtColumnSize.ClientID %>"></div>
                    </td>
                    <td></td>
                </tr>
            </table>

            <p class="stepTitle columnshow" id="columntitle" style="display: none;">列展现配置</p>&nbsp;&nbsp;<span class="onlineNote noteLayout columnshow" style="display:none;">若未填写显示名，将以列名展现</span>
            <table class="tableOne tableLayout columnshow" id="columncontent" style="display: none;">
                <asp:Repeater ID="ColumnList" runat="server" OnItemDataBound="ColumnList_ItemDataBound">
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th width="47">序号</th>
                                <th width="189">列名</th>
                                <th><input type="checkbox" id="chkall" onclick="selectall(this)" />&nbsp;是否显示<span class="redColor">(*)</span></th>
                                <th>显示名</th>
                                <th class="mergen" style="display: none;">合并表头名</th>
                                <th class="lastMenu">顺序<span class="redColor">(*)</span></th>
                            </tr>
                        </thead>
                        <tbody>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td><%#Container.ItemIndex+1 %></td>
                            <td>
                                <asp:Literal ID="lblName" runat="server" Text='<%#Eval("name") %>'></asp:Literal></td>
                            <td>
                                <asp:CheckBox ID="chkIsShow" runat="server"/><asp:TextBox ID="txtDataType" runat="server" style="display:none;" Text='<%#Eval("data_type") %>'></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtShowName" runat="server" CssClass="setIpt"></asp:TextBox></td>
                            <td class="mergen" style="display: none;">
                                <asp:TextBox ID="txtMergeName" runat="server" CssClass="setIpt"></asp:TextBox></td>
                            <td>
                                <asp:TextBox ID="txtSort" runat="server" CssClass="setIpt iptSeq" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox></td>
                        </tr>
                    </ItemTemplate>
                </asp:Repeater>
                </tbody>
            </table>

            <p class="stepTitle COLUMNH" style="display: none;">计算列配置&nbsp;&nbsp;<span class="onlineNote noteLayout COLUMNH">只能对数字型字段进行“计算列配置”​设置，如显示无配置内容请检查数据源</span></p>
            <table class="tableOne tableLayout" style="display: none;" id="columnhdemo">

                <tr id="columntrdemo">
                    <td>0</td>
                    <td>
                        <input class="setIpt" type="text" id="txtcal" runat="server" ondblclick="showcaldiv(this)" readonly="true" /></td>
                    <td>
                        <input class="setIpt" type="text" id="txtjshowname" runat="server" /></td>
                    <td>
                        <input class="setIpt" type="text" id="txtmergen" runat="server" /></td>
                    <td>
                        <asp:TextBox ID="txtjsort" runat="server" CssClass="setIpt" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox></td>
                    <td>
                        <asp:DropDownList ID="ddlDecimal" runat="server" CssClass="setDropIpt default w112">
                        </asp:DropDownList></td>
                    <td><a class="operateBtn remove" href="javascript:void(0)" title="删除" onclick="RegisterButtonConfirm('您确定要删除吗？','cal',this)"></a></td>
                </tr>

            </table>
            <table class="tableOne tableLayout COLUMNH" style="display: none;" id="columnhj">
                <thead>
                    <tr>
                        <th width="47">序号</th>
                        <th width="293">计算公式<span class="redColor">(*)</span></th>
                        <th>显示名<span class="redColor">(*)</span></th>
                        <th>合并表头名</th>
                        <th>排序</th>
                        <th>保留小数</th>
                        <th class="lastMenu">操作</th>
                    </tr>
                </thead>
                <tbody>
                    </tbody>
                    <tfoot>
                    <tr>
                        <td>
                            <button class="addListBtn" type="button" onclick="addcaltr();return false;"></button>
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                        </tfoot>
                
            </table>

            <p class="stepTitle ROWH" style="display: none;">汇总行配置&nbsp;&nbsp;<span class="onlineNote noteLayout ROWH">只能对数字型字段进行“汇总行配置”​设置，如显示无配置内容请检查数据源；如果未选择汇总列，则将被显示为总计</span></p>
            <table class="tableOne tableLayout" style="display: none;" id="groupbycaltabdemo">
                <tr id="groupbycalclone">
                    <td>0</td>
                    <td>
                        <asp:DropDownList ID="ddlgroupcalcol" runat="server" CssClass="setDropIpt default w210">
                        </asp:DropDownList></td>
                    <td>
                        <asp:DropDownList ID="ddlcomputertype" runat="server" CssClass="setDropIpt default w112">
                        </asp:DropDownList></td>
                    <td>
                        <asp:DropDownList ID="ddlComDecimal" runat="server" CssClass="setDropIpt default w112">
                        </asp:DropDownList></td>
                    <td><a class="operateBtn remove" href="javascript:void(0)" title="删除" onclick="RegisterButtonConfirm('您确定要删除吗？','computertype',this)"></a></td>

                </tr>
            </table>
            <table class="tableOne tableLayout" style="display: none;" id="rowhdemo">

                <tr id="rowtrdemo">
                    <td>0</td>
                    <td>
                        <input class="setIpt" type="text" id="txtgroupcol" runat="server" ondblclick="showgroupcoldiv(this)" readonly="true" /></td>
                    <td>
                        <input class="setIpt" type="text" id="txtgroupcal" runat="server" ondblclick="showgroupcaldiv(this)" readonly="true" /></td>
                    <td>
                        <input class="setRadioIpt" type="radio" name="1" value="MIDDLE" />
                        <label class="setRadioTxt">表中</label>
                        <input class="setRadioIpt" type="radio" name="1" value="LAST" />
                        <label class="setRadioTxt">表尾</label></td>
                    <td><a class="operateBtn remove" href="javascript:void(0)" title="删除" onclick="RegisterButtonConfirm('您确定要删除吗？','groupcal',this)"></a></td>
                </tr>

            </table>
            <table class="tableOne tableLayout ROWH" style="display: none;" id="groupbytab">
                <thead>
                    <tr>
                        <th width="47">序号</th>
                        <th width="293">汇总列名称</th>
                        <th>计算列配置<span class="redColor">(*)</span></th>
                        <th>显示位置<span class="redColor">(*)</span></th>
                        <th class="lastMenu">操作</th>
                    </tr>
                </thead>
                <tbody>
                    </tbody>
                    <tfoot>
                    <tr>
                        <td>
                            <button class="addListBtn" type="button" onclick="addgroupcaltr();return false;"></button>
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                </tfoot>
            </table>

            <p class="stepTitle sortc" style="display: none;">结果集排序</p>
            <table class="tableOne tableLayout" style="display: none;" id="sorttabdemo">
                <tr id="sortclone">
                    <td>
                        <asp:DropDownList ID="ddlColumnName" runat="server" CssClass="setDropIpt default w210">
                        </asp:DropDownList></td>
                    <td>
                        <asp:DropDownList ID="ddlSortType" runat="server" CssClass="setDropIpt default w112">
                            <asp:ListItem Text="请选择" Value=""></asp:ListItem>
                            <asp:ListItem Text="升序" Value="asc"></asp:ListItem>
                            <asp:ListItem Text="降序" Value="desc"></asp:ListItem>
                        </asp:DropDownList></td>
                    <td>
                        <asp:TextBox ID="txtSort" runat="server" CssClass="setIpt" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')"></asp:TextBox>
                    </td>
                    <td><a class="operateBtn remove" href="javascript:void(0)" title="删除" onclick="RegisterButtonConfirm('您确定要删除吗？','sort',this)"></a></td>

                </tr>
            </table>
            <table class="tableOne tableLayout sortc" style="display: none;" id="sorttab">
                <thead>
                    <tr>
                        <th>列名称<span class="redColor">(*)</span></th>
                        <th>排序方式<span class="redColor">(*)</span></th>
                        <th>优先级<span class="redColor">(*)</span></th>
                        <th class="lastMenu">操作</th>
                    </tr>
                </thead>
                <tbody>
                    </tbody>
                <tfoot>
                    <tr>
                        <td>
                            <a class="addFiltrateBtnAstyle" href="javascript:void(0)" onclick="addsorttr()">添加</a>
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                </tfoot>
            </table>

            <div class="cutOffRule"></div>
            <div class="operateBtnBox operateBtnLayout">
                <asp:LinkButton ID="lbtnPrev" runat="server" CssClass="guideBtn prevLink" OnClick="lbtnPrev_Click">上一步</asp:LinkButton>
                <asp:LinkButton ID="lbtnSave" runat="server" CssClass="guideBtn nextLink" OnClick="lbtnSave_Click" OnClientClick="return executesub();">下一步</asp:LinkButton>
            </div>
        </div>
        <%--弹窗--%>
        <div class="shade"></div>
        <!-- 选择公式弹窗 Start -->
        <div class="toolTipBox selcaldiv">
            <div class="toolTipContent">
                <div class="toolTipArea">
                    <div class="choiceArea">
                        <p class="toolTipTitle">列</p>
                        <ul class="choiceAreaList" id="selectcollist">
                            <asp:Repeater ID="rptcallist" runat="server">
                                <ItemTemplate>
                                    <li ondblclick="setcnametocal('<%#Eval("name") %>')">
                                        <p><%#Eval("name") %></p>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>
                        </ul>
                    </div>
                    <div class="centerArea"></div>
                    <div class="resultsArea">
                        <p class="toolTipTitle">公式</p>
                        <div class="resultAreaList">
                            <asp:TextBox TextMode="MultiLine" ID="txtCalInfo" runat="server" Width="210px" Height="238px" CssClass="txtCalInfo" Style="border: none;"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <div class="operateBtnBox toolOperateLayout">
                    <a class="guideBtn nextLink" href="javascript:void(0)" onclick="getcaltext()">确定</a>
                    <a class="guideBtn prevLink" href="javascript:void(0)" onclick="closecalwin()">取消</a>
                </div>

            </div>
        </div>
        <!-- 选择公式弹窗 End -->
        <!-- 选择列弹窗 Start -->
        <div class="toolTipBox selrowgroupby">
            <div class="toolTipContent">
                <div class="toolTipArea">
                    <div class="choiceArea">
                        <p class="toolTipTitle">列</p>
                        <ul class="choiceAreaList" id="selgroupcollist">
                           <%-- <asp:Repeater ID="rptgroupcol" runat="server">
                                <ItemTemplate>
                                    <li ondblclick="setgroupcnametocal('<%#Eval("name") %>')">
                                        <p><%#Eval("name") %></p>
                                    </li>
                                </ItemTemplate>
                            </asp:Repeater>--%>
                        </ul>
                    </div>
                    <div class="centerArea"></div>
                    <div class="resultsArea">
                        <p class="toolTipTitle">选择列</p>
                        <div class="resultAreaList groupresult">
                        </div>
                    </div>
                </div>

                <div class="operateBtnBox toolOperateLayout">
                    <a class="guideBtn nextLink" href="javascript:void(0)" onclick="getselectgroupcol()">确定</a>
                    <a class="guideBtn prevLink" href="javascript:void(0)" onclick="closegroupcalwin();">取消</a>
                </div>

            </div>
        </div>
        <!-- 选择列弹窗 End -->
        <!-- 选择group计算列弹窗 Start -->
        <div class="toolTipBox selrowgroupcbycal" style="width: 800px;left:40%;height:500px;">
            <div class="toolTipContent" style="height:460px;overflow-y:auto;">
                <div>
                    <table class="tableOne tableLayout" id="computertypetab">
                        <thead>
                            <tr>
                                <th>序号</th>
                                <th>计算列名称(只显示数值型字段)<span class="redColor">(*)</span></th>
                                <th>计算方式<span class="redColor">(*)</span></th>
                                <th>保留小数</th>
                                <th class="lastMenu">操作</th>
                            </tr>
                        </thead>
                        <tbody></tbody>
                            <tfoot>
                            <tr>
                                <td>
                                    <button class="addListBtn" type="button" onclick="addcomputertypetr();return false;"></button>
                                </td>
                                <td></td>
                                <td></td>
                                <td></td>
                                <td></td>
                            </tr>
                                </tfoot>
                        
                    </table>
                </div>

                <div class="operateBtnBox toolOperateLayout">
                    <a class="guideBtn nextLink" href="javascript:void(0)" onclick="getselectgroupcalcol();">确定</a>
                    <a class="guideBtn prevLink" href="javascript:void(0)" onclick="closecomcalwin();">取消</a>
                </div>

            </div>
        </div>
        <!-- 选择列弹窗 End -->
        <input type="hidden" id="hidopertype" class="hidopertype" runat="server" />
        <input type="hidden" id="hidTemplateID" class="hidTemplateID" runat="server" />
        <input type="hidden" id="hidsourceid" runat="server" class="hidsourceid" /><%--测试用--%>
        <input type="hidden" id="hidSortNum" class="hidSortNum" runat="server" value="0" />
        <input type="hidden" id="hidComputerNum" class="hidComputerNum" runat="server" value="0" />
        <input type="hidden" id="hidcalnum" class="hidcalnum" runat="server" value="0" />
        <input type="hidden" id="hidgroupnum" class="hidgroupnum" runat="server" value="0" />
        <input type="hidden" id="hidSortContent" class="hidSortContent" runat="server" value="" /><%--保存排序信息--%>
        <input type="hidden" id="hidSortContentEdit" class="hidSortContentEdit" runat="server" value="" /><%--保存排序信息--%>
        <input type="hidden" id="hidColumnShowName" class="hidColumnShowName" runat="server" value="" /><%--保存列配置中显示名信息--%>
        <input type="hidden" id="hidCalShowName" class="hidCalShowName" runat="server" value="" /><%--保存计算列中显示名信息--%>
        <input type="hidden" id="hidColumnContent" class="hidColumnContent" runat="server" value="" /><%--保存列配置信息--%>
        <input type="hidden" id="hidcolumnmergename" class="hidcolumnmergename" runat="server" value="" /><%--保存列配置中合并表头名信息--%>
        <input type="hidden" id="hidcalmergename" class="hidcalmergename" runat="server" value="" /><%--保存计算列中合并表头名信息--%>
        <input type="hidden" id="hidCalContent" class="hidCalContent" runat="server" value="" /><%--保存计算列信息--%>
        <input type="hidden" id="hidCalContentEdit" class="hidCalContentEdit" runat="server" value="" /><%--保存计算列信息--%>
        <input type="hidden" id="hidcaltextid" class="hidcaltextid" runat="server" /><%--保存计算列的编号用于返回数据用--%>
        <input type="hidden" id="hidgroupcolid" class="hidgroupcolid" runat="server" /><%--保存汇总行文本编号用于返回数据用--%>
        <input type="hidden" id="hidgroupcalcolid" class="hidgroupcalcolid" runat="server" /><%--保存汇总行中计算列文本编号用于返回数据用--%>
        <input type="hidden" id="hidgroupbycontent" class="hidgroupbycontent" runat="server" value="" /><%--保存行汇总信息--%>
        <input type="hidden" id="hidgroupbyeditcontent" class="hidgroupbyeditcontent" runat="server" value="" /><%--保存排序信息edit--%>
        <input type="hidden" id="hidselectcname" class="hidselectcname" runat="server" />
<input type="hidden" id="hiddbtype" class="hiddbtype" runat="server" />
   <%-- </ContentTemplate>
</asp:UpdatePanel>--%>
<script type="text/javascript">

    $(function ($) {
        InitSeq();

    });

    function InitSeq()
    {
        if ($('#columncontent tbody tr').find("input[type='checkbox'][checked]").length == 0)
        {
            var i = 1;
            $('#columncontent tbody tr').each(function () {
                $(this).find('.iptSeq').val(i + "0");
                i++;
            })
        }
    }
    //点击计算列名加载到公式文本框
    function setcnametocal(obj) {
        var calcontent = $(".txtCalInfo").val();
        $(".txtCalInfo").val(calcontent + obj);
    }

    //返回公式信息
    function getcaltext() {
        var calcolumns = "";
        //验证公式的有效性
        $("#selectcollist li").each(function () {//取出所有计算列
            if ($(this).find("p").length > 0) {
                var cn = $.trim($(this).find("p").text());
                if (cn != "") {
                    calcolumns += cn + "|";
                }
            }
        });
        if (calcolumns == "") {
            layer.alert("没有数字类型的列信息可供计算！");
        } else {
            var calcontent = $.trim($(".txtCalInfo").val());
            if (calcontent == "") {
                layer.alert("计算公式不能为空！");
            } else {
                var nonullcont = calcontent.replace(/ /g, '');//首先去除公式中所有空字符
                var collist = calcolumns.split('|');
                var rresult = nonullcont;//保存替换后的值
                for (var i = 0; i < collist.length; i++) {//替换掉所有字段为空
                    if ($.trim(collist[i]) != "") {
                        re = new RegExp("\\[" + collist[i] + "\\]", "g");
                        rresult = rresult.replace(re, "");
                        re = new RegExp(collist[i], "g");
                        rresult = rresult.replace(re, "");
                    }
                }
                //替换掉所有运算符
                rresult = rresult.replace(/-/g, '');
                re = new RegExp('\\+', 'g');
                rresult = rresult.replace(re, "");
                re = new RegExp("\\*", "g");
                rresult = rresult.replace(re, "");
                re = new RegExp("\\/", "g");
                rresult = rresult.replace(re, "");
                re = new RegExp("\\(", "g");
                rresult = rresult.replace(re, "");
                re = new RegExp("\\)", "g");
                rresult = rresult.replace(re, "");
                if ($.trim(rresult) != "") {
                    layer.alert("计算公式不正确，请检查公式有效性！");
                } else {
                    //第二步检查运算符
                    rresult = nonullcont;//保存替换后的值
                    re = new RegExp("\\(", "g");
                    rresult = rresult.replace(re, "");
                    re = new RegExp("\\)", "g");
                    rresult = rresult.replace(re, "");
                    for (var i = 0; i < collist.length; i++) {//替换掉所有字段为a
                        if ($.trim(collist[i]) != "") {
                            re = new RegExp("\\[" + collist[i] + "\\]", "g");
                            rresult = rresult.replace(re, "a");
                            re = new RegExp(collist[i], "g");
                            rresult = rresult.replace(re, "a");
                        }
                    }
                    if ($.trim(rresult) == "") {
                        layer.alert("计算公式不正确，请检查公式有效性！");
                    } else {
                        var calist = rresult.split('a');
                        if (calist.length <= 2 || calist[0] != "" || calist[calist.length - 1] != "") {//运算符出现在第一个与最后一个报错
                            layer.alert("计算公式不正确，请检查公式有效性！");
                        } else {
                            var issuc = true;
                            for (var m = 1; m < (calist.length - 1) ; m++) {
                                if ($.trim(calist[m]) == "" || $.trim(calist[m]).length > 1) {
                                    issuc = false;
                                    break;
                                }
                            }
                            if (!issuc) {
                                layer.alert("计算公式不正确，请检查公式有效性！");
                            } else {
                                //判断括号正反数量是否相等
                                var znum = nonullcont.split('(').length - 1;
                                var fnum = nonullcont.split(')').length - 1;
                                if (znum != fnum) {
                                    layer.alert("计算公式不正确，请检查公式有效性！");
                                } else {
                                    if (nonullcont.indexOf(")(") != -1) {
                                        layer.alert("计算公式不正确，请检查公式有效性！");
                                    } else {
                                        rresult = nonullcont;//保存替换后的值
                                        for (var i = 0; i < collist.length; i++) {//替换掉所有字段为a
                                            if ($.trim(collist[i]) != "") {
                                                re = new RegExp("\\[" + collist[i] + "\\]", "g");
                                                rresult = rresult.replace(re, "a");
                                                re = new RegExp(collist[i], "g");
                                                rresult = rresult.replace(re, "a");
                                            }
                                        }
                                        var zanum = rresult.split('(a').length - 1;
                                        var fanum = rresult.split('a)').length - 1;
                                        if (zanum != fanum) {
                                            layer.alert("计算公式不正确，请检查公式有效性！");
                                        } else {
                                            $("#" + $(".hidcaltextid").val()).val(calcontent);
                                            closecalwin();
                                            $(".txtCalInfo").val("");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    //弹出计算列配置公式窗口
    function showcaldiv(obj) {
        $(".shade").show();
        $(".selcaldiv").show();
        $(".hidcaltextid").val($(obj).attr("id"));
        var sname = $(obj).val();
        $(".txtCalInfo").val(sname);
    }
    //弹出汇总行窗口
    function showgroupcoldiv(obj) {
        $(obj).blur();
        getgroupbycolnames();//得到汇总列
        $(".shade").show();
        $(".selrowgroupby").show();
        $(".hidgroupcolid").val($(obj).attr("id"));
        initgroupcolnames($(obj).attr("id"));
    }
    //行汇总弹出汇总列根据已选 的值初始化
    function initgroupcolnames(objname) {
        var pcont = $.trim($("#" + objname).val());
        $(".groupresult").find("p").each(function () {
            $(this).remove();
        });
        if (pcont != "") {
            var pclist = pcont.split(',');
            for (var i = 0; i < pclist.length; i++) {
                var addchtml = "<p class='resultList'>" + pclist[i] + "<a class='resultListclose' href='javascript:void(0);' onclick='$(this).parent().remove();'></a></p>";
                var yy = $(".groupresult").html();
                $(".groupresult").html(yy + addchtml);
            }
        } 
    }
    //弹出汇总行中计算列配置窗口
    function showgroupcaldiv(obj) {
        $(obj).blur();
        getgroupbycalcol();
        $(".shade").show();
        $(".selrowgroupcbycal").show();
        initgroupcomlist($(obj).attr("id"));
        $(".hidgroupcalcolid").val($(obj).attr("id"));
    }

    //行汇总弹出计算列配置窗口时根据计算列中内容初始化弹出列表
    function initgroupcomlist(objname) {
        var pcont = $.trim($("#" + objname).val());
        $(".hidComputerNum").val("0");
        var sorttrid = $(".hidComputerNum").val();
        sorttrid = parseInt(sorttrid);
        $("#computertypetab tbody tr").each(function () {
            if ($(this).children('td').eq(0).find("button").length <= 0) {
                $(this).remove();
            }
            if($(this).attr("id")!=undefined&&$(this).attr("id")!=null&&$(this).attr("id")!=""){
                $(this).remove();
}
        });
        if (pcont != "") {
            bindgroupcomlist(pcont);
            bindgroupcomlistsecone(pcont);
        }        
    }
    //修改时绑定汇总行计算列信息
    function bindgroupcomlist(objcontent) {
        var sortcontent = objcontent;
                var slist = sortcontent.split(';');
                var sorttrid = $(".hidComputerNum").val();
                sorttrid = parseInt(sorttrid);
                for (var i = 0; i < slist.length; i++) {
                    if ($.trim(slist[i]) != "") {
                        sorttrid++;
                        var sinfolist = slist[i].split(',')[0].split('(');
                        var comtype = sinfolist[1].substr(0, sinfolist[1].length - 1);
                        $("#computertypetab tbody").append($("#groupbycalclone").clone(true).attr("id", "computertypeclone_" + sorttrid));
                        $(".hidComputerNum").val(sorttrid);
                        $("#computertypeclone_" + sorttrid + " td").each(function () {//循环克隆的新行里面的td 
                            //修改相关属性 
                            $(this).find("select[name*='ddlgroupcalcol']").attr("id", "ddlgroupcalcol" + sorttrid).attr("name", "ddlgroupcalcol" + sorttrid);
                            $(this).find("div[id*='ddlgroupcalcol']").attr("id", "dk_container_ddlgroupcalcol" + sorttrid);
                            //修改相关属性 
                            $(this).find("select[name*='ddlComDecimal']").attr("id", "ddlComDecimal" + sorttrid).attr("name", "ddlComDecimal" + sorttrid);
                            $(this).find("div[id*='ddlComDecimal']").attr("id", "dk_container_ddlComDecimal" + sorttrid);
                            //修改排序名称
                            $(this).find("select[name*='ddlcomputertype']").attr("id", "ddlcomputertype" + sorttrid).attr("name", "ddlcomputertype" + sorttrid);
                            $(this).find("div[id*='ddlcomputertype']").attr("id", "dk_container_ddlcomputertype" + sorttrid);
                            if ($(this).index() == "0") {
                                $(this).text(sorttrid);
                            }
                        });
                    }
                }
    }
    //上面方法的第二步
    function bindgroupcomlistsecone(objcontent) {
        var sortcontent = objcontent;
        var slist = sortcontent.split(';');
        for (var i = 0; i < slist.length; i++) {
            if ($.trim(slist[i]) != "") {
                var gnlist = slist[i].split(',');
                var sinfolist = gnlist[0].split('(');
                var comtype = sinfolist[1].substr(0, sinfolist[1].length - 1);
                $("#computertypeclone_" + (i+1) + " td").each(function () {//循环克隆的新行里面的td 
                    $(this).find(".dk_option_current").attr("class", "");
                });
                //首先更改模板，再复制
                //$("#groupbycalclone td").each(function () {//循环克隆的新行里面的td 
                //    $(this).find(".dk_option_current").attr("class", "");
                //});
                var decimalc = gnlist[1];
                if (decimalc == "-1") {
                    decimalc = "自动";
                }
                $("#computertypeclone_" + (i + 1) + "").find("a").each(function () {//循环克隆的新行里面的td 
                    if ($(this).attr("class") != "dk_toggle dk_label") {
                        if ($(this).attr("data-dk-dropdown-value") != undefined && $(this).attr("data-dk-dropdown-value") != null && $(this).attr("data-dk-dropdown-value") != "") {
                            if ($.trim($(this).attr("data-dk-dropdown-value")) == sinfolist[0]) {
                                $(this).parent().attr("class", "dk_option_current");
                                $(this).parent().parent().parent().prev().text($(this).text());
                            }
                        }
                        if ($.trim($(this).text()) == comtype) {
                            $(this).parent().attr("class", "dk_option_current");
                            $(this).parent().parent().parent().prev().text(comtype);
                        }
                        if ($.trim($(this).text()) == decimalc) {
                            $(this).parent().attr("class", "dk_option_current");
                            $(this).parent().parent().parent().prev().text(decimalc);
                        }
                    }
                });
            }
        }
    }
    //汇总行中双击列名加载到右侧
    function setgroupcnametocal(obj) {
        //判断有无重复
        var rresult = "";
        $(".groupresult").find("p").each(function () {
            rresult += $.trim($(this).text()) + ";";
        });
        if (rresult.indexOf(obj) == -1) {
            var addchtml = "<p class='resultList'>" + obj + "<a class='resultListclose' href='javascript:void(0);' onclick='$(this).parent().remove();'></a></p>";
            var yy = $(".groupresult").html();
            $(".groupresult").html(yy + addchtml);
        } else {
            layer.alert("已选择过此汇总列！");
        }
    }
    //得到选择的汇总列
    function getselectgroupcol() {
        var rresult = "";
        $(".groupresult").find("p").each(function () {
            rresult += $.trim($(this).text()) + ",";
        });
        $("#" + $(".hidgroupcolid").val()).val(rresult.substr(0,rresult.length-1));
        closegroupcalwin();
    }
    //隐藏添加汇总列窗口
    function closegroupcalwin() {
        $(".shade").hide();
        $(".selrowgroupby").hide();
    }
    //隐藏添加公式窗口
    function closecalwin() {
        $(".shade").hide();
        $(".selcaldiv").hide();
    }
    //得到添加的计算列配置信息group by
    function getselectgroupcalcol() {
        var pid = $(".hidgroupcalcolid").val();
        if (GetComTypeTable('add', pid)) {
            closecomcalwin();
        }
    }
    //隐藏添加计算列窗口
    function closecomcalwin() {
        $(".shade").hide();
        $(".selrowgroupcbycal").hide();
    }

    //汇总行添加行
    function addgroupcaltr() {
        var rr = true;
        var caltrid = $(".hidgroupnum").val();
        caltrid = parseInt(caltrid);
        if (caltrid > 0) {
            rr = GetGroupCalTable('add');//判断已添加的行是否已填写完整，未填写完整不能继续添加
        }
        if (rr) {
            caltrid++;
            $("#groupbytab tbody").append($("#rowtrdemo").clone(true).attr("id", "groupcaltrid_" + caltrid));
            $(".hidgroupnum").val(caltrid);
            $("#groupcaltrid_" + caltrid + " td").each(function () {//循环克隆的新行里面的td 
                //修改相关属性 
                $(this).find("input[id*='txtgroupcol']").attr("id", "txtgroupcol" + caltrid).attr("name", "txtgroupcol" + caltrid).val("");
                $(this).find("input[id*='txtgroupcal']").attr("id", "txtgroupcal" + caltrid).attr("name", "txtgroupcal" + caltrid).val("");
                $(this).find(".setRadioIpt").attr("name", "rad" + caltrid);
                $(this).find(".setRadioIpt").prop("checked", false);

                if ($(this).index() == "0") {
                    $(this).text(caltrid);
                }
            });
        }
    }
    //遍历汇总表取值
    function GetGroupCalTable(type) {
        var istrue = false;
        var result = "";
        var sorttrid = $(".hidgroupnum").val();
        sorttrid = parseInt(sorttrid);
        if (sorttrid == 0) {
            istrue = true;//汇总行可以为空
        } else {
            //遍历tr取值
            $("#groupbytab tbody tr").each(function () {
                if ($(this).children('td').eq(1).find("input").length > 0) {
                    var cname = $.trim($(this).children('td').eq(1).find("input").val());//汇总列名
                    var sorttype = $.trim($(this).children('td').eq(2).find("input").val());//计算列配置
                    var sortnum = $.trim($(this).children('td').eq(3).find("input[type='radio']:checked").val());
                    if (sorttype == "" || sortnum == "") {
                        if (type == "add") {
                            layer.alert("行汇总配置信息不完整，标*号的为必填项！");
                        } else {
                            layer.alert("行汇总配置信息不完整，标*号的为必填项！");
                        }
                        result = "";
                        istrue = false;
                        return false;
                    } else {
                        if (cname == "" && sortnum!="LAST") {//汇总列为空，只能选表尾
                            layer.alert("未配置汇总列名称，显示位置只能选择表尾！");
                            result = "";
                            istrue = false;
                            return false;
                        }
                        var tempsort = cname + "|" + sorttype + "|" + sortnum + "$";
                        if (result.indexOf(tempsort) == -1) {
                            result += tempsort;
                            istrue = true;
                        } else {
                            layer.alert("行汇总配置中列名：" + cname + " 不能重复配置！");
                            result = "";
                            istrue = false;
                            return false;
                        }

                    }
                }

            });
        }
        //if ($.trim(result) != "") {
        if (type != "add" && istrue) {//新增行验证时不保存
            $(".hidgroupbycontent").val(result);//获取排序条件内容
        }
        //}

        return istrue;
    }

    //计算列添加行
    function addcaltr() {
        var rr = true;
        var caltrid = $(".hidcalnum").val();
        caltrid = parseInt(caltrid);
        if (caltrid > 0) {
            rr = GetCalTable('add');//判断已添加的行是否已填写完整，未填写完整不能继续添加
        }
        if (rr) {
            caltrid++;
            $("#columnhj tbody").append($("#columntrdemo").clone(true).attr("id", "caltrid_" + caltrid));
            $(".hidcalnum").val(caltrid);
            $("#caltrid_" + caltrid + " td").each(function () {//循环克隆的新行里面的td 
                //修改相关属性 
                $(this).find("select[name*='ddlDecimal']").attr("id", "ddlDecimal" + caltrid).attr("name", "ddlDecimal" + caltrid);
                $(this).find("div[id*='ddlDecimal']").attr("id", "dk_container_ddlDecimal" + caltrid);
                $(this).find(".dk_label").text("自动");//列名
                $(this).find(".dk_option_current").attr("class", "");
                //修改相关属性 
                $(this).find("input[id*='txtcal']").attr("id", "txtcal" + caltrid).attr("name", "txtcal" + caltrid).val("");
                $(this).find("input[id*='txtjshowname']").val("");
                $(this).find("input[id*='txtmergen']").val("");
                $(this).find("input[id*='txtjsort']").val("");
                if ($(this).index() == "0") {
                    $(this).text(caltrid);
                }
            });
        }
    }

    //遍历计算列表取值
    function GetCalTable(type) {
        var calmergenames = "";
        var istrue = false;
        var result = "";
        var caltrid = $(".hidcalnum").val();
        caltrid = parseInt(caltrid);
        if (caltrid == 0) {
            istrue = true;//排序可以为空
        } else {
            //遍历tr取值
            $("#columnhj tbody tr").each(function () {
                if ($(this).children('td').eq(1).find("input").length > 0) {
                    var cname = $.trim($(this).children('td').eq(1).find("input").val());//公式名
                    var showname = $.trim($(this).children('td').eq(2).find("input").val());//显示名
                    var mergename = $.trim($(this).children('td').eq(3).find("input").val());//合并表头名
                    var sortnum = $.trim($(this).children('td').eq(4).find("input").val());
                    var decimalcount = $.trim($(this).children('td').eq(5).find(".dk_label").text());//列名
                    if (decimalcount == "自动") {
                        decimalcount = -1;
                    }
                    if (cname == "" || showname == "") {
                        if (type == "add") {
                            layer.alert("计算列配置信息不完整！除了合并表头名与排序，其它都是必填项！");
                        } else {
                            layer.alert("计算列配置信息不完整！除了合并表头名与排序，其它都是必填项！");
                        }
                        result = "";
                        istrue = false;
                        return false;
                    } else {
                        var columnshownams = $(".hidColumnShowName").val();//与列配置中的显示名做比较，不能重复
                        if (columnshownams.indexOf("|" + showname + "|") == -1) {
                            if (mergename != "") {//与列配置中的合并表头名作比较，不能重复
                                calmergenames += mergename + "|";
                                var columnmergenames = $(".hidcolumnmergename").val();
                                if (columnmergenames.indexOf("|" + mergename + "|") != -1) {
                                    layer.alert("计算列配置中合并表头名：" + mergename + " 已被列配置中使用，不能重复配置！");
                                    result = "";
                                    istrue = false;
                                    return false;
                                }
                            }
                            var rresult = "[" + cname + "]";
                            rresult = rresult.replace(/-/g, ']-[');
                            re = new RegExp('\\+', 'g');
                            rresult = rresult.replace(re, "]+[");
                            re = new RegExp("\\*", "g");
                            rresult = rresult.replace(re, "]*[");
                            re = new RegExp("\\/", "g");
                            rresult = rresult.replace(re, "]/[");
                            re = new RegExp("\\[\\(", "g");
                            rresult = rresult.replace(re, "([");
                            re = new RegExp("\\)\\]", "g");
                            rresult = rresult.replace(re, "])");
                            cname = rresult;
                            var tempsort = cname + "|" + showname + "|" + mergename + "|" + sortnum + "|"+decimalcount+";"
                            if (result.indexOf("|" + showname + "|") == -1) {
                                result += tempsort;
                                istrue = true;
                            } else {
                                layer.alert("计算列配置中显示名：" + showname + " 不能重复配置！");
                                result = "";
                                istrue = false;
                                return false;
                            }
                        } else {
                            layer.alert("计算列配置中显示名：" + showname + " 已被列配置中使用，不能重复配置！");
                            result = "";
                            istrue = false;
                            return false;
                        }
                    }
                }

            });
        }
        if (istrue && type != "add") {
            if (calmergenames != "") {
                var mlist = calmergenames.split('|');
                var muimname = 0;
                var repeatername = "";
                for (var i = 0; i < mlist.length; i++) {//判断合并表头名是否>=2
                    var mm = 0;
                    if ($.trim(mlist[i]) != "") {
                        for (var j = 0; j < mlist.length; j++) {
                            if ($.trim(mlist[i]) == $.trim(mlist[j])) {
                                mm++;
                            }
                        }
                        if (mm == 1) {//只有一个合并表头名报错
                            muimname = 1;
                            repeatername = mlist[i];
                            break;
                        }
                    }
                }
                if (muimname == 1) {
                    istrue = false;
                    result = "";
                    layer.alert("计算列配置中合并表头名：" + repeatername + " 不能少于两列！");
                }
            }
        }
        //if ($.trim(result) != "") {
        if (type != "add" && istrue) {//新增行验证时不保存
            $(".hidCalContent").val(result);//获取计算列内容
        }
        //}

        return istrue;
    }

    //计算列配置添加行
    function addcomputertypetr() {
        var rr = true;
        var sorttrid = $(".hidComputerNum").val();
        sorttrid = parseInt(sorttrid);
        if (sorttrid > 0) {
            rr = GetComTypeTable('add', '');//判断已添加的行是否已填写完整，未填写完整不能继续添加
        }
        if (rr) {
            sorttrid++;
            $("#computertypetab tbody").append($("#groupbycalclone").clone(true).attr("id", "computertypeclone_" + sorttrid));
            $(".hidComputerNum").val(sorttrid);
            $("#computertypeclone_" + sorttrid + " td").each(function () {//循环克隆的新行里面的td 
                $(this).find("select[name*='ddlComDecimal']").attr("id", "ddlComDecimal" + sorttrid).attr("name", "ddlComDecimal" + sorttrid);
                $(this).find("div[id*='ddlComDecimal']").attr("id", "dk_container_ddlComDecimal" + sorttrid);
                $(this).find("div[id*='ddlComDecimal']").find(".dk_label").text("自动");//列名
                //修改相关属性 
                $(this).find("select[name*='ddlgroupcalcol']").attr("id", "ddlgroupcalcol" + sorttrid).attr("name", "ddlgroupcalcol" + sorttrid);
                $(this).find("div[id*='ddlgroupcalcol']").find(".dk_label").text("请选择");//列名
                $(this).find(".dk_option_current").attr("class", "");
                //修改排序名称
                $(this).find("select[name*='ddlcomputertype']").attr("id", "ddlcomputertype" + sorttrid).attr("name", "ddlcomputertype" + sorttrid);
                $(this).find("div[id*='ddlcomputertype']").find(".dk_label").text("请选择");//列名
                if ($(this).index() == "0") {
                    $(this).text(sorttrid);
                }
            });
            inittableonetrstyle();
        }
    }
    //排序添加行
    function addsorttr() {
        var rr = true;
        var sorttrid = $(".hidSortNum").val();
        sorttrid = parseInt(sorttrid);
        if (sorttrid > 0) {
            rr = GetSortTable('add');//判断已添加的行是否已填写完整，未填写完整不能继续添加
        }
        if (rr) {
            sorttrid++;
            $("#sorttab tbody").append($("#sortclone").clone(true).attr("id", "sortclone_" + sorttrid));
            $(".hidSortNum").val(sorttrid);
            $("#sortclone_" + sorttrid + " td").each(function () {//循环克隆的新行里面的td 
                //修改相关属性 
                $(this).find("select[name*='ddlColumnName']").attr("id", "ddlColumnName" + sorttrid).attr("name", "ddlColumnName" + sorttrid);
                $(this).find("div[id*='ddlColumnName']").attr("id", "dk_container_ddlColumnName" + sorttrid);
                $(this).find(".dk_label").text("请选择");//列名
                $(this).find(".dk_option_current").attr("class", "");
                //修改排序名称
                $(this).find("select[name*='ddlSortType']").attr("id", "ddlSortType" + sorttrid).attr("name", "ddlSortType" + sorttrid);
                $(this).find("div[id*='ddlSortType']").attr("id", "dk_container_ddlSortType" + sorttrid);
                $(this).find("input[name*='txtSort']").val("");
            });
            inittableonetrstyle();
        }
    }
    //修改时绑定排序行信息
    function bindsortlist() {
        var otype = $(".hidopertype").val();
        if (otype == "edit") {
            var sortcontent = $.trim($(".hidSortContentEdit").val());
            if (sortcontent != "") {
                $(".hidSortNum").val("0");
                var slist = sortcontent.split(';');
                var sorttrid = $(".hidSortNum").val();
                sorttrid = parseInt(sorttrid);
                for (var i = 0; i < slist.length; i++) {
                    sorttrid++;
                    var sinfolist = slist[i].split('|');
                    $("#sorttab tbody").append($("#sortclone").clone(true).attr("id", "sortclone_" + sorttrid));
                    $(".hidSortNum").val(sorttrid);
                    $("#sortclone_" + sorttrid + " td").each(function () {//循环克隆的新行里面的td 
                        //修改相关属性 
                        $(this).find("select[name*='ddlColumnName']").attr("id", "ddlColumnName" + sorttrid).attr("name", "ddlColumnName" + sorttrid);
                        $(this).find("div[id*='ddlColumnName']").attr("id", "dk_container_ddlColumnName" + sorttrid);
                        $(this).find(".dk_label").text(sinfolist[0]);//列名
                        $(this).find(".dk_option_current").attr("class", "");
                        //修改排序名称
                        $(this).find("select[name*='ddlSortType']").attr("id", "ddlSortType" + sorttrid).attr("name", "ddlSortType" + sorttrid);
                        $(this).find("div[id*='ddlSortType']").attr("id", "dk_container_ddlSortType" + sorttrid);
                    });
                }
                Initsortifedit();
            }
        }
    }
    //修改时给排序下拉赋值
    function Initsortifedit() {
        var otype = $(".hidopertype").val();
        if (otype == "edit") {
            var sortcontent = $.trim($(".hidSortContentEdit").val());
            if (sortcontent != "") {
                var slist = sortcontent.split(';');
                for (var i = 0; i < slist.length; i++) {
                    var sinfolist = slist[i].split('|');
                    var sn = "";
                    if (sinfolist[1] == "asc") {
                        sn = "升序";
                    }
                    if (sinfolist[1] == "desc") {
                        sn = "降序";
                    }
                    if ($("#sortclone_" + (i + 1) + "").length > 0) {
                        $("#sortclone_" + (i + 1) + " td").each(function () {//循环克隆的新行里面的td 
                            $(this).find(".dk_option_current").attr("class", "");
                            if ($(this).find("input").length > 0) {
                                $(this).find("input").val(sinfolist[2]);
                            }
                            $(this).find("select[name*='ddlColumnName']").val(sinfolist[0]);
                            $(this).find("select[name*='ddlSortType']").val(sinfolist[1]);
                        });
                    }
                }
            }
        }
    }
    //修改时绑定计算列信息
    function bindcallist() {
        var otype = $(".hidopertype").val();
        if (otype == "edit") {

            var calcontent = $.trim($(".hidCalContentEdit").val());
            if (calcontent != "") {
                $(".hidcalnum").val("0");
                var slist = calcontent.split(';');
                var sorttrid = $(".hidcalnum").val();
                sorttrid = parseInt(sorttrid);
                for (var i = 0; i < slist.length; i++) {
                    sorttrid++;
                    var sinfolist = slist[i].split('|');
                    $("#columnhj tbody").append($("#columntrdemo").clone(true).attr("id", "caltrid_" + sorttrid));

                    $(".hidcalnum").val(sorttrid);
                    $("#caltrid_" + sorttrid + " td").each(function () {//循环克隆的新行里面的td 
                        $(this).find("input[id*='txtcal']").attr("id", "txtcal" + sorttrid).attr("name", "txtcal" + sorttrid).val(sinfolist[0]);
                        if ($(this).index() == "0") {
                            $(this).text(sorttrid);
                        }
                        $(this).find("input[id*='txtjshowname']").val(sinfolist[1]);
                        $(this).find("input[id*='txtmergen']").val(sinfolist[2]);
                        $(this).find("input[id*='txtjsort']").val(sinfolist[3]);
                        //修改相关属性 
                        $(this).find("select[name*='ddlDecimal']").attr("id", "ddlDecimal" + sorttrid).attr("name", "ddlDecimal" + sorttrid).val(sinfolist[4]);
                        $(this).find("div[id*='ddlDecimal']").attr("id", "dk_container_ddlDecimal" + sorttrid);
                        var stext = sinfolist[4];
                        if (stext == "-1") {
                            stext = "自动";
                        }
                        $(this).find(".dk_label").text(stext);//列名
                        $(this).find(".dk_option_current").attr("class", "");
                    });
                }
            }
        }
    }
    //修改时绑定行汇总信息
    //修改时绑定计算列信息
    function bindgroupbylist() {
        var otype = $(".hidopertype").val();
        if (otype == "edit") {
            var calcontent = $.trim($(".hidgroupbyeditcontent").val());
            if (calcontent != "") {
                $(".hidgroupnum").val("0");
                var slist = calcontent.split('$');
                var sorttrid = $(".hidgroupnum").val();
                sorttrid = parseInt(sorttrid);
                for (var i = 0; i < slist.length; i++) {
                    sorttrid++;
                    var sinfolist = slist[i].split('|');
                    $("#groupbytab tbody").append($("#rowtrdemo").clone(true).attr("id", "groupcaltrid_" + sorttrid));
                    $(".hidgroupnum").val(sorttrid);
                    $("#groupcaltrid_" + sorttrid + " td").each(function () {//循环克隆的新行里面的td 
                        $(this).find("input[id*='txtgroupcol']").attr("id", "txtgroupcol" + sorttrid).attr("name", "txtgroupcol" + sorttrid).val(sinfolist[0]);
                        $(this).find("input[id*='txtgroupcal']").attr("id", "txtgroupcal" + sorttrid).attr("name", "txtgroupcal" + sorttrid).val(sinfolist[1]);
                        $(this).find(".setRadioIpt").attr("name", "radedit" + sorttrid);
                        $(this).find(".setRadioIpt").each(function () {
                            if ($(this).val() == sinfolist[2]) {
                                $(this).prop("checked", true);
                            }
                        });

                        if ($(this).index() == "0") {
                            $(this).text(sorttrid);
                        }
                    });
                }
            }
        }
    }

    //$(function ($) {
    //    Initsortifedit();
    //});
    //删除计算列行
    function removecaltr(trname) {
        var sorttrid = $(".hidcalnum").val();
        sorttrid = parseInt(sorttrid);
        if (sorttrid > 0) {
            sorttrid--;
        }
        $(".hidcalnum").val(sorttrid);
        $("#" + trname).remove();
        var m = 1;
        $("#columnhj tbody tr").each(function () {
            if ($(this).children('td').eq(1).find("input").length > 0) {
                $(this).children('td').eq(0).html(m);
                $(this).attr("id", "caltrid_" + m);
                m++;
            }
        });
    }
    //删除汇总行
    function removegroupcaltr(trname) {
        var sorttrid = $(".hidgroupnum").val();
        sorttrid = parseInt(sorttrid);
        if (sorttrid > 0) {
            sorttrid--;
        }
        $(".hidgroupnum").val(sorttrid);
        $("#" + trname).remove();
        var m = 1;
        $("#groupbytab tbody tr").each(function () {
            if ($(this).children('td').eq(1).find("input").length > 0) {
                $(this).children('td').eq(0).html(m);
                $(this).attr("id", "groupcaltrid_" + m);
                m++;
            }
        });
    }
    //绑定前先初始化排序信息，修改时用
    function initsorttabempty() {
        $("#sorttab tbody").empty();
        //var str = '<tr><td><button class="addListBtn" type="button" onclick="addsorttr();return false;"></button></td><td></td><td></td><td></td></tr>';
        //$("#sorttab tbody").append(str);
    }
    //删除排序行
    function removesorttr(trname) {
        var sorttrid = $(".hidSortNum").val();
        sorttrid = parseInt(sorttrid);
        if (sorttrid > 0) {
            sorttrid--;
        }
        $(".hidSortNum").val(sorttrid);
        $("#" + trname).remove();
        var zid = 1;
        //遍历tr取值
        $("#sorttab tbody tr").each(function () {
            if ($(this).children('td').eq(0).find("select").length > 0) {
                $(this).attr("id", "sortclone_" + zid);
                zid++;
            }
        });
    }
    //删除计算配置列行
    function removegroupcomcaltr(trname) {
        var sorttrid = $(".hidComputerNum").val();
        sorttrid = parseInt(sorttrid);
        if (sorttrid > 0) {
            sorttrid--;
        }
        $(".hidComputerNum").val(sorttrid);
        $("#" + trname).remove();
        var m = 1;
        $("#computertypetab tbody tr").each(function () {
            if ($(this).children('td').eq(1).find("select").length > 0) {
                $(this).children('td').eq(0).html(m);
                $(this).attr("id", "computertypeclone_" + m);
                m++;
            }
        });
    }
    //注册确认方法groupcal
    function RegisterButtonConfirm(msgs, type, obj) {
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
                    if (type == "sort") {
                        var pid = $(obj).parent().parent().attr('id');
                        removesorttr(pid);
                        inittableonetrstyle();
                    }
                    if (type == "cal") {
                        var pid = $(obj).parent().parent().attr('id');
                        removecaltr(pid);
                        inittableonetrstyle();
                    }
                    if (type == "groupcal") {
                        var pid = $(obj).parent().parent().attr('id');
                        removegroupcaltr(pid);
                        inittableonetrstyle();
                    }
                    if (type == "computertype") {
                        var pid = $(obj).parent().parent().attr('id');
                        removegroupcomcaltr(pid);
                        inittableonetrstyle();
                    }
                },
                no: function () {
                }
            }
        });

    }
    //遍历排序表取值
    function GetSortTable(type) {
        var istrue = false;
        var result = "";
        var sorttrid = $(".hidSortNum").val();
        sorttrid = parseInt(sorttrid);
        if (sorttrid == 0) {
            istrue = true;//排序可以为空
        } else {
            //遍历tr取值
            $("#sorttab tbody tr").each(function () {
                if ($(this).children('td').eq(2).find("input").length > 0) {
                    var cname = $.trim($(this).children('td').eq(0).find(".dk_label").text());//列名
                    var sorttype = $.trim($(this).children('td').eq(1).find(".dk_label").text());//排序类型
                    var sortnum = $.trim($(this).children('td').eq(2).find("input").val());
                    if (cname == "" || cname == "请选择" || sorttype == "" || sorttype == "请选择" || sortnum == "") {
                        if (type == "add") {
                            layer.alert("结果集排序配置中每列都是必填项，请填写完整！");
                        } else {
                            layer.alert("结果集排序配置中每列都是必填项，请填写完整！");
                        }
                        result = "";
                        istrue = false;
                        return false;
                    } else {
                        var tempsort = cname + "|" + sorttype + "|" + sortnum + ";";
                        if (result.indexOf(tempsort) == -1) {
                            result += tempsort;
                            istrue = true;
                        } else {
                            layer.alert("结果集排序配置中列名：" + cname + " 不能重复配置！");
                            result = "";
                            istrue = false;
                            return false;
                        }

                    }
                }

            });
        }
        //if ($.trim(result) != "") {
        if (type != "add" && istrue) {//新增行验证时不保存
            $(".hidSortContent").val(result);//获取排序条件内容
        }
        //}

        return istrue;
    }
    //遍历计算表取值
    function GetComTypeTable(type, obj) {
        var istrue = false;
        var result = "";
        var sorttrid = $(".hidComputerNum").val();
        sorttrid = parseInt(sorttrid);
        if (sorttrid == 0) {
            istrue = true;//排序可以为空
        } else {
            //遍历tr取值
            $("#computertypetab tbody tr").each(function () {
                if ($(this).children('td').eq(1).find("select").length > 0) {
                    var cname = $.trim($(this).children('td').eq(1).find(".dk_label").text());//列名
                    var sorttype = $.trim($(this).children('td').eq(2).find(".dk_option_current").children('a').eq(0).attr("data-dk-dropdown-value"));//排序类型
                    var decimalcount = $.trim($(this).children('td').eq(3).find(".dk_label").text());//列名
                    if (decimalcount == "自动") {
                        decimalcount = -1;
                    }
                    if (cname == "" || cname == "请选择" || sorttype == "" || sorttype=="请选择") {
                        if (type == "add") {
                            layer.alert("计算列配置信息不完整,标*号的为必填项！");
                        } else {
                            layer.alert("计算列配置信息不完整,标*号的为必填项！");
                        }
                        result = "";
                        istrue = false;
                        return false;
                    } else {
                        var tempsort = sorttype + "(" + cname + ")" + "," + decimalcount+";";
                        if (result.indexOf("(" + cname + ")") == -1) {
                            result += tempsort;
                            istrue = true;
                        } else {
                            layer.alert("计算列名称：" + cname + " 不能重复配置！");
                            result = "";
                            istrue = false;
                            return false;
                        }

                    }
                }

            });
        }
        //if ($.trim(result) != "") {
        if (istrue) {//新增行验证时不保存
            if ($.trim(obj) != "") {
                if ($("#" + obj).length > 0) {
                    $("#" + obj).val(result.substr(0,result.length-1));//获取排序条件内容
                }
            }
        }
        //}

        return istrue;
    }
    //选择显示列给分组列赋值
    function getgroupbycolnames() {
        $("#selgroupcollist").empty();
        var result = "";
        //遍历tr取值
        $("#columncontent tbody tr").each(function () {
            //var defcolname = $.trim($(this).children('td').eq(1).text());
            var isshow = $(this).children('td').eq(2).find("input[name*='chkIsShow']").prop("checked");
            var showname = $.trim($(this).children('td').eq(3).find("input[name*='txtShowName']").val());//显示名
            if (isshow) {//选中的列名，显示名必填
                var clname = $.trim($(this).children('td').eq(1).text());//列名
                result += "<li ondblclick=\"setgroupcnametocal('"+clname+"')\">";
                result += "<p>" + clname + "</p></li>";
            }
        });
        $("#selgroupcollist").append(result);
    }
    //选择显示列给分组列计算列赋值
    function getgroupbycalcol() {
        $("select[id*='ddlgroupcalcol']").empty();
        $("div[id*='ddlgroupcalcol']").find("ul").empty();
        var selectresult = "<option value>请选择</option>";
        var ulresult = "<li class=\"dk_option_current\"><a data-dk-dropdown-value>请选择</a></li>";
        //遍历tr取值
        $("#columncontent tbody tr").each(function () {
            var isshow = $(this).children('td').eq(2).find("input[name*='chkIsShow']").prop("checked");
            var datatype = $.trim($(this).children('td').eq(2).find("input[id*='txtDataType']").val());
            var showname = $.trim($(this).children('td').eq(3).find("input[name*='txtShowName']").val());//显示名
            if (isshow) {//选中的列名，显示名必填
                if (datatype == "int" || datatype == "decimal" || datatype == "smallint" || datatype == "bigint" || datatype == "float" || datatype == "numeric") {
                    var clname = $.trim($(this).children('td').eq(1).text());//列名
                    ulresult += "<li class><a data-dk-dropdown-value=\"" + clname + "\">" + clname + "</a></li>";
                    selectresult += "<option value=\"" + clname + "\">" + clname + "</option>";
                }
            }
        });
        $("select[id*='ddlgroupcalcol']").append(selectresult);
        $("div[id*='ddlgroupcalcol']").find("ul").append(ulresult);
    }
    //列配置验证
    function checkcolumn() {
        var cmergename = "";//合并表头名
        var cshowname = "";//保存显示名
        var selectcname = "";//保存选中的列名
        var content = "";
        var result = false;
        var num = 0;
        if ($("#columncontent tbody").children("tr").length == 0) {//无数据时不通过验证
            return false;
        }
        //遍历tr取值
        $("#columncontent tbody tr").each(function () {
            var isshow = $(this).children('td').eq(2).find("input[name*='chkIsShow']").prop("checked");
            if (isshow) {//选中的列名，显示名与排序必填
                num++;
                var showname = $.trim($(this).children('td').eq(3).find("input[name*='txtShowName']").val());//显示名
                var clname = $.trim($(this).children('td').eq(1).text());//列名
                var sortnum = $.trim($(this).children('td').eq(5).find("input[name*='txtSort']").val());//排序
                var mergename = $.trim($(this).children('td').eq(4).find("input[name*='txtMergeName']").val());//合并表头名
                var datatype = $.trim($(this).children('td').eq(2).find("input[name*='txtDataType']").val());//字段类型
                if (showname == "") {
                    $(this).children('td').eq(3).find("input[name*='txtShowName']").val(clname);
                    showname = clname;
                }
                if (showname == "") {
                    content = ""; cshowname = ""; cmergename = ""; selectcname = "";
                    layer.alert("列配置中列名：" + clname + " 的显示名不能为空！");
                    result = false;
                    return false;
                } else {
                    if (sortnum == "") {
                        content = ""; cshowname = ""; cmergename = ""; selectcname = "";
                        layer.alert("列配置中列名：" + clname + " 的顺序不能为空！");
                        result = false;
                        return false;
                    }
                    if (mergename != "") {
                        cmergename += mergename + "|";
                    }
                    //if (cshowname.indexOf("|"+showname+"|") == -1) {
                        content += clname + "|" + isshow + "|" + showname + "|" + sortnum + "|" + mergename + "|"+datatype+";";
                        cshowname += "|"+showname + "|";
                        selectcname += clname + "|";
                        result = true;
                   // }
                    //else {
                    //    content = ""; cshowname = ""; cmergename = ""; selectcname = "";
                    //    layer.alert("列配置中显示名：" + showname + " 不能重复！");
                    //    result = false;
                    //    return false;
                    //}
                }
            }
        });
        if (num == 0) {
            layer.alert("列配置中列名至少显示一项！");
        }

        $(".hidColumnContent").val(content);
        $(".hidColumnShowName").val(cshowname);
        $(".hidselectcname").val(selectcname);
        if (!result) {
            $(".hidColumnContent").val("");//未通过验证清空保存值
            $(".hidColumnShowName").val("");
            $(".hidcolumnmergename").val("");
            $(".hidselectcname").val("");
        } else {
            if (cmergename != "") {
                $(".hidcolumnmergename").val(cmergename);
                var mlist = cmergename.split('|');
                var muimname = 0;
                var repeatername = "";
                for (var i = 0; i < mlist.length; i++) {//判断合并表头名是否>=2
                    var mm = 0;
                    if ($.trim(mlist[i]) != "") {
                        for (var j = 0; j < mlist.length; j++) {
                            if ($.trim(mlist[i]) == $.trim(mlist[j])) {
                                mm++;
                            }
                        }
                        if (mm == 1) {//只有一个合并表头名报错
                            muimname = 1;
                            repeatername = mlist[i];
                            break;
                        }
                    }
                }
                if (muimname == 1) {
                    result = false;
                    layer.alert("列配置中合并表头名：" + repeatername + " 不能少于两列！");
                }
            }
        }
        return result;
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
            //bindsortlist();
            //Initsortifedit();//edit sort
            //bindcallist();
        } catch (err) {

        }
    });
    //提交验证
    function executesub() {
        var b_return = checkForm(document.forms.item(0, null));
        var type = $(".ddlDisplayType option:selected").val();
        if (b_return) {//检查每行字段数是否大于0
            if (type == "GRID") {
                var colsize = $.trim($(".txtColumnSize").val());
                if (colsize == "0") {
                    layer.alert("每行字段数不能小于1个！");
                    b_return = false;
                }
            }
        }
        if (b_return) {
            //check列配置
            var checkc = checkcolumn();
            if (checkc) {//首先检查列配置
                if (type == "COLUMN" || type == "ROW") {//列汇总
                    if (!GetCalTable('submit')) {
                        b_return = false;
                    }

                }
                if (type == "ROW") {//列汇总
                    if (!GetGroupCalTable('submit')) {
                        b_return = false;
                    }

                }
                var rr = GetSortTable('submit');
                if (rr) {//check排序

                } else {
                    //layer.alert("排序配置未填写完整！");
                    b_return = false;
                }
            } else {
                b_return = false;
            }
        }
        return b_return;
    }
    function changetype() {
        //$(".hidSortNum").val("0");
        //$(".hidComputerNum").val("0");
        //$(".hidcalnum").val("0");
        var type = $(".ddlDisplayType option:selected").val();
        $(".txtDisplayType").val(type);
        $(".txtDisplayType").change();
        //绑定前先删除之前数据
        //initsorttabempty();

        controlshow(type);
        //bindsortlist();
        //bindcallist();
        //bindgroupbylist();
        //controlshow(type);
    }

    function controlshow(type) {
        $(".tableview").hide();
        $(".columnshow").hide();
        $(".ROWH").hide();
        $(".COLUMNH").hide();
        $(".mergen").hide();
        $(".GRID").hide();
        if ($.trim(type) != "") {
            $(".sortc").show();
        }
        $(".sortc").show();
        $(".txtColumnSize").attr("title", "每行字段数~50:");
        if (type == "NORMAL") {//普通列表
            $(".columnshow").show();
            $(".putongliebiao").show();
        }
        if (type == "GRID") {//田字列表
            $(".txtColumnSize").attr("title", "每行字段数~50:!");
            $(".GRID").show();
            $(".columnshow").show();
            $(".tianzixing").show();
        }
        if (type == "COLUMN") {//列汇总
            $(".COLUMNH").show();
            $(".columnshow").show();
            $(".mergen").show();
            $(".liehuizong").show();
        }
        if (type == "ROW") {//行汇总
            $(".columnshow").show();
            $(".ROWH").show();
            $(".COLUMNH").show();
            $(".mergen").show();
            $(".hanghuizong").show();
        }
    }

    //全选
    function selectall(obj) {
        var arrChk = $("input[name*='chkIsShow']");
        if ($(obj).is(':checked')) {
            $(arrChk).each(function () {
                this.checked = true;
            });
        } else {
            $(arrChk).each(function () {
                this.checked = false;
            });
        }
    }

    function putong() {
        var i = $.layer({
            type: 1,
            title: false,
            fix: false,
            offset: ['80px', ''],
            area: ['auto', 'auto'],
            page: { dom: '#putongdiv' }
        });
    }
    function tianzi() {
        var it = $.layer({
            type: 1,
            title: false,
            fix: false,
            offset: ['80px', ''],
            area: ['auto', 'auto'],
            page: { dom: '#tianzidiv' }
        });
        
    }
    function liehuizong() {
        var itl = $.layer({
            type: 1,
            title: false,
            fix: false,
            offset: ['80px', ''],
            area: ['auto', 'auto'],
            page: { dom: '#liehuizongdiv' }
        });
        
    }
    function hanghuizong() {
        var itlh = $.layer({
            type: 1,
            title: false,
            fix: false,
            offset: ['80px', ''],
            area: ['auto', 'auto'],
            page: { dom: '#hanghuizongdiv' }
        });
        
    }
</script>
