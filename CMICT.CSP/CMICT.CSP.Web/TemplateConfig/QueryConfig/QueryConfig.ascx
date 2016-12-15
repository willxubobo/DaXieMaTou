<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QueryConfig.ascx.cs" Inherits="CMICT.CSP.Web.TemplateConfig.QueryConfig.QueryConfig" %>
<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/laydate/laydate.js"></script>
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
            <div class="stepBox stepPrev">
                <p>3.配置展示方式</p>
                <span></span>
            </div>
        </li>
        <li>
            <div class="stepBox stepcurrent">
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
<div class="cutOffRule"></div>

<div class="stepOperateBox">
    <p class="stepTitle">默认筛选条件</p>
    <div class="addFiltrate">
        <label class="filtrateTitle">默认筛选条件</label>
        <input class="filtrateIpt" type="radio" name="defaultmain" value="and" runat="server" id="radand" />
        <label class="filtrateTxt">and</label>
        <input class="filtrateIpt" type="radio" name="defaultmain" value="or" runat="server" id="rador" />
        <label class="filtrateTxt">or</label>
    </div>
    <div class="addFiltrateBtn">
        <a class="addFiltrateBtnAstyle" href="javascript:void(0)" onclick="adddefaultquery()">添加默认筛选条件</a>
    </div>
    <table class="tableOne tableLayout" id="defaultquerytabdemo" style="display: none;">
        <tr id="defaulttrdemo">
            <td>1</td>
            <td style="width: 500px;">
                <asp:DropDownList ID="ddlColumnName" runat="server" CssClass="setDropIpt default w210 middle" onchange='changeradiostatus(this.options[this.options.selectedIndex].text,this)'>
                </asp:DropDownList>
                <asp:DropDownList ID="ddlCompare" runat="server" CssClass="setDropIpt default w112 middle">
                </asp:DropDownList>
                <input class="setIpt setLayout middle" type="text" id="txtFilter" runat="server" />
            </td>
            <td>
                <input class="setIpt describeIpt" type="text" id="txtremark" runat="server" /></td>
            <td><a class="operateBtn remove" href="javascript:void(0)" title="删除" onclick="RegisterButtonConfirm('您确定要删除吗？','deltr',this)"></a></td>
        </tr>
    </table>
    <div style="display: none;" id="defaultdiv0">
        <div class="filtrateBox" id="defaultdivdemo">
            <div class="addFiltrate">
                <label class="filtrateTitle">默认筛选条件1</label>
                <input class="filtrateIpt" type="radio" name="raddemo" value="and" />
                <label class="filtrateTxt">and</label>
                <input class="filtrateIpt" type="radio" name="raddemo" value="or" />
                <label class="filtrateTxt">or</label>
                <a class="removeIptBtn" href="javascript:void(0)" onclick="RegisterButtonConfirm('您确定要删除吗？', 'deldefault', this); return false;"></a>
                <br /><span class="onlineNote noteLayout">筛选条件：以@符号开头的列名为数据源中存储过程的参数名，如配置一条，则此块筛选条件中只能配置存储过程参数！</span><br />
                <span class="onlineNote noteLayout">若要基于网站上的当前用户进行筛选，请键入[本人]作为筛选值！</span>
            </div>
            <table class="tableOne tableLayout" id="tabcontentdemo">
                <thead>
                    <tr>
                        <th width="47">序号</th>
                        <th width="293">筛选条件<span class="redColor">(*)</span></th>
                        <th width="504">描述</th>
                        <th class="lastMenu">操作</th>
                    </tr>
                </thead>
                <tbody></tbody>
                <tfoot>
                    <tr>
                        <td>
                            <button class="addListBtn" type="button" onclick="addtabcontentinfo(this);return false;"></button>
                            <input type="hidden" id="hidtabcontent0" class="hidtabcontent0" runat="server" value="0" />
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                </tfoot>
            </table>

        </div>
    </div>
    <asp:Repeater ID="rptdefaultmain" runat="server" OnItemDataBound="rptdefaultmain_ItemDataBound">
        <ItemTemplate>
            <div class="filtrateBox" id="defaultdiv<%#Container.ItemIndex+1 %>">
                <div class="addFiltrate">
                    <asp:Label ID="lblmoduleid" runat="server" Text='<%#Eval("ModuleID") %>' Visible="false"></asp:Label>
                    <label class="filtrateTitle">默认筛选条件<%#Container.ItemIndex+1 %></label>
                    <input class="filtrateIpt" type="radio" name="rad<%#Container.ItemIndex+1 %>" value="and" <%#Eval("sublogic").ToString()=="and"?"checked":"" %> />
                    <label class="filtrateTxt">and</label>
                    <input class="filtrateIpt" type="radio" name="rad<%#Container.ItemIndex+1 %>" value="or" <%#Eval("sublogic").ToString()=="or"?"checked":"" %> />
                    <label class="filtrateTxt">or</label>
                    <a class="removeIptBtn" href="javascript:void(0)" onclick="RegisterButtonConfirm('您确定要删除吗？', 'deldefault', this); return false;"></a>
                    <br /><span class="onlineNote noteLayout">筛选条件：以@符号开头的列名为数据源中存储过程的参数名，如配置一条，则此块筛选条件中只能配置存储过程参数！</span><br />
                <span class="onlineNote noteLayout">若要基于网站上的当前用户进行筛选，请键入[本人]作为筛选值！</span>
                </div>
                <table class="tableOne tableLayout" id="tabcontent<%#Container.ItemIndex+1 %>">
                    <thead>
                        <tr>
                            <th width="47">序号</th>
                            <th width="293">筛选条件</th>
                            <th width="504">描述</th>
                            <th class="lastMenu">操作</th>
                        </tr>
                    </thead>
                    <tbody>
                        <asp:Repeater ID="rptdefaultmaininfo" runat="server">
                            <ItemTemplate>
                                <tr id="defaulttr<%#Container.ItemIndex+1 %>">
                                    <td><%#Container.ItemIndex+1 %></td>
                                    <td style="width: 500px;">
                                        <asp:DropDownList ID="ddlColumnName" runat="server" CssClass="setDropIpt default w210 middle" onchange='changeradiostatus(this.options[this.options.selectedIndex].text,this)'>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblcolumnname" runat="server" Text='<%#Eval("ColumnName") %>' Visible="false"></asp:Label>
                                        <asp:DropDownList ID="ddlCompare" runat="server" CssClass="setDropIpt default w112 middle">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblCompare" runat="server" Text='<%#Eval("Compare") %>' Visible="false"></asp:Label>
                                        <input class="setIpt setLayout middle" type="text" id="txtFilter" runat="server" value='<%#Eval("CompareValue") %>' />
                                    </td>
                                    <td>
                                        <input class="setIpt describeIpt" type="text" id="txtremark" runat="server" value='<%#Eval("Desction") %>' /></td>
                                    <td><a class="operateBtn remove" href="javascript:void(0)" title="删除" onclick="RegisterButtonConfirm('您确定要删除吗？','deltr',this)"></a></td>
                                </tr>
                            </ItemTemplate>
                        </asp:Repeater></tbody>
                    <tfoot>
                        <tr>
                            <td>
                                <button class="addListBtn" type="button" onclick="addtabcontentinfo(this);return false;"></button>
                                <input type="hidden" id="hidtabcontent0" class='hidtabcontent<%#Container.ItemIndex+1 %>' runat="server" value='<%#Eval("subcount") %>' />
                            </td>
                        <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </tfoot>
                </table>

            </div>
        </ItemTemplate>
    </asp:Repeater>
    <input type="hidden" id="hidquerynum" class="hidquerynum" runat="server" value="0" />
    <p class="stepTitle stepTitleLayout">用户筛选条件</p>&nbsp;&nbsp;<span class="onlineNote noteLayout">类型如果选择枚举(多选)或日期(快捷)，多个默认值以分号隔开。日期(快捷)类型默认值只能为数字或上个月、2个月、3个月、半年、1年。</span>
    <table class="tableOne tableLayout" style="display: none;" id="userquerydemo">
        <tr id="uqtrclone">
            <td>1</td>
            <td>
                <asp:DropDownList ID="ddluqcol" runat="server" CssClass="setDropIpt default w210" onchange="initdisplaynamedf(obj,'3');">
                </asp:DropDownList>
            </td>
            <td>
                <input class="setIpt" type="text" id="txtshowname" runat="server" />
            </td>
            <td>
                <asp:DropDownList ID="ddlcontroltype" runat="server" CssClass="setDropIpt default w112" onchange="hidecomparebytype();">
                </asp:DropDownList>

            </td>
            <td>
                <input class="setIpt" type="text" id="txtsort" runat="server" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" /></td>
            <td>
                <asp:DropDownList ID="ddlcomparetype" runat="server" CssClass="setDropIpt default w112">
                </asp:DropDownList>

            </td>
            <td>
                <input class="setIpt" type="text" id="txtdefault" runat="server" /></td>
            <td>
                <input class="setIpt" type="text" id="txtdesc" runat="server" /></td>
            <td><a class="operateBtn remove" href="javascript:void(0)" title="删除" onclick="RegisterButtonConfirm('您确定要删除吗？','userquerydel',this)"></a></td>
        </tr>

    </table>
    <table class="tableOne tableLayout" id="userquerytab">
        <thead>
            <tr>
                <th width="47">序号</th>
                <th width="133">筛选列<span class="redColor">(*)</span></th>
                <th width="133">显示名<span class="redColor">(*)</span></th>
                <th width="133">类型<span class="redColor">(*)</span></th>
                <th width="133">排序</th>
                <th width="133">比较符<span class="redColor">(*)</span></th>
                 <th width="133">默认值</th>
                <th width="133">注释</th>
                <th class="lastMenu" style="width: 30px;">操作</th>
            </tr>
        </thead>
        <tbody>
            <asp:Repeater ID="UserQueryList" runat="server" OnItemDataBound="UserQueryList_ItemDataBound">
                <ItemTemplate>
                    <tr id="userqueryclone_<%#Container.ItemIndex+1 %>">
                        <td><%#Container.ItemIndex+1 %></td>
                        <td>
                            <asp:DropDownList ID="ddluqcol" runat="server" CssClass="setDropIpt default w210" onchange="initdisplayname(this);">
                            </asp:DropDownList>
                            <asp:Label ID="lblcolumnname" runat="server" Text='<%#Eval("ColumnName") %>' Visible="false"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtshowname" runat="server" CssClass="setIpt" Text='<%#Eval("DisplayName") %>'></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcontroltype" runat="server" CssClass="setDropIpt default w112" onchange="hidecomparebytyperp(this);">
                            </asp:DropDownList>
                            <asp:Label ID="lblctype" CssClass="lblctype" runat="server" Text='<%#Eval("ControlType") %>' style="display:none;"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtsort" runat="server" CssClass="setIpt" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" Text='<%#Eval("Sequence") %>'></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlcomparetype" runat="server" CssClass="setDropIpt default w112">
                            </asp:DropDownList>
                            <asp:Label ID="lblcompare" runat="server" Text='<%#Eval("Compare") %>' Visible="false"></asp:Label>
                        </td>
                         <td>
                            <asp:TextBox ID="txtdefault" runat="server" CssClass="setIpt" Text='<%#Eval("DefaultValue") %>'></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="txtdesc" runat="server" CssClass="setIpt" Text='<%#Eval("Reminder") %>'></asp:TextBox></td>
                        <td><a class="operateBtn remove" href="javascript:void(0)" title="删除" onclick="RegisterButtonConfirm('您确定要删除吗？','userquerydel',this)"></a></td>
                    </tr>
                </ItemTemplate>
            </asp:Repeater> </tbody>
        <tfoot>
            <tr>
                <td>
                    <button class="addListBtn" type="button" onclick="adduserquery();return false;"></button>
                </td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
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
<input type="hidden" id="hidopertype" class="hidopertype" runat="server" />
<input type="hidden" id="hidTemplateID" class="hidTemplateID" runat="server" />
<input type="hidden" id="hidsourceid" runat="server" class="hidsourceid" />
<input type="hidden" id="hidSortNum" class="hidSortNum" runat="server" value="0" />
<input type="hidden" id="hidUserQueryContent" class="hidUserQueryContent" runat="server" />
<input type="hidden" id="hidDefaultContent" class="hidDefaultContent" runat="server" />
<input type="hidden" id="hidprocpara" class="hidprocpara" runat="server" />
<input type="hidden" id="hidmodid" class="hidmodid" runat="server" />
<input type="hidden" id="hidprocparalist" class="hidprocparalist" runat="server" />
<input type="hidden" id="hidconfigcolinfo" class="hidconfigcolinfo" runat="server" />

<script type="text/javascript">
    //添加默认筛选条件详细信息
    function addtabcontentinfo(obj) {
        var tabid = $(obj).parent().parent().parent().parent().attr("id");
        var rr = true;
        var sorttrid = $("#" + tabid).find("input[type='hidden']").val();
        sorttrid = parseInt(sorttrid);
        if (sorttrid > 0) {
            rr = GetTabContentTable(tabid, 'add');//判断已添加的行是否已填写完整，未填写完整不能继续添加
        }
        if (rr) {
            sorttrid++;
            $("#" + tabid + " tbody").append($("#defaulttrdemo").clone(true).attr("id", "defaulttr_" + sorttrid));
            $("#" + tabid).find("input[type='hidden']").val(sorttrid);
            $("#" + tabid + " #defaulttr_" + sorttrid + " td").each(function () {//循环克隆的新行里面的td 
                //修改相关属性 
                $(this).find("select[name*='ddlColumnName']").attr("id", "ddlColumnName" + sorttrid).attr("name", "ddlColumnName" + sorttrid);
                $(this).find("div[id*='ddlColumnName']").attr("id", "dk_container_ddlColumnName" + sorttrid);
                $(this).find(".dk_label").text("请选择");//列名
                $(this).find(".dk_option_current").attr("class", "");
                //修改排序名称
                $(this).find("select[name*='ddlCompare']").attr("id", "ddlCompare" + sorttrid).attr("name", "ddlCompare" + sorttrid);
                $(this).find("div[id*='ddlCompare']").attr("id", "dk_container_ddlCompare" + sorttrid);
                $(this).find("input[name*='txtFilter']").val("");
                if ($(this).index() == "0") {
                    $(this).text(sorttrid);
                }
            });
            inittableonetrstyle();
        }
    }
    //遍历表取值
    function GetTabContentTable(tabid, type) {
        var noparacname = 1;
        var paracname = 1;
        var istrue = false;
        var result = "";
        var sorttrid = $("#" + tabid).find("input[type='hidden']").val();
        sorttrid = parseInt(sorttrid);
        if (sorttrid == 0) {
            istrue = false;//不可以为空
        } else {
            var modname = $("#" + tabid).prev().find(".filtrateTitle").html();//得到模块名称
            //遍历tr取值
            $("#" + tabid + " tbody tr").each(function () {
                if ($(this).children('td').eq(1).find("input").length > 0) {
                    var cname = $.trim($(this).children('td').eq(1).find("div[id*='ddlColumnName']").find(".dk_label").text());//列名
                    var cdatatypev = $.trim($(this).children('td').eq(1).find("div[id*='ddlColumnName']").find(".dk_option_current").children('a').eq(0).attr("data-dk-dropdown-value"));//列类型
                    var cdatatype = cdatatypev.split(';')[0];
                    var compare = $.trim($(this).children('td').eq(1).find("div[id*='ddlCompare']").find(".dk_option_current").children('a').eq(0).attr("data-dk-dropdown-value"));//比较符
                    var cvalue = $.trim($(this).children('td').eq(1).find("input").val());
                    var remark = $.trim($(this).children('td').eq(2).find("input").val());
                    var canempty = true;//比较值是否可以为空，等于与不等于可以
                    if (compare == "EQUAL" || compare == "NOTEQUAL") {
                        if (cvalue == "") {
                            canempty = false;
                        }
                    }
                    if (cvalue != "") {
                        canempty = false;
                    }
                    if (cname == "" || cname == "请选择" || compare == "" || compare == "请选择" || canempty) {
                        if (type == "add") {
                            layer.alert(modname + " 筛选条件配置信息不完整！");
                        } else {

                            layer.alert(modname + " 筛选条件配置信息不完整！");
                        }
                        result = "";
                        istrue = false;
                        return false;
                    } else {
                        var checkv = true;
                        checkv = checkdatatypev(cdatatype, cvalue, cname);
                        if (checkv) {
                            if (cname.indexOf('@') != -1) {//判断参数名有没有重复配置
                                paracname = 2;
                                if (result.indexOf(cname + "|") != -1) {
                                    layer.alert(modname + " 筛选条件配置中列名：" + cname + " 为存储过程参数，只能配置一条！");
                                    result = "";
                                    istrue = false;
                                    return false;
                                }
                            }
                            else {
                                noparacname++;
                            }
                            if (paracname == 2 && noparacname > 1) {//如一个模块中配置了一个参数则整个模块只能填参数
                                layer.alert(modname + " 筛选条件配置中存储过程参数与列不能混合配置！");
                                result = "";
                                istrue = false;
                                return false;
                            }
                            var tempsort = cname + "|" + compare + "|" + cvalue;
                            if (result.indexOf(tempsort) == -1) {
                                result += tempsort + "|" + remark + ";";
                                istrue = true;
                            } else {
                                layer.alert(modname + " 筛选条件配置中列名：" + cname + " 不能重复配置！");
                                result = "";
                                istrue = false;
                                return false;
                            }
                        } else {
                            result = "";
                            istrue = false;
                            return false;
                        }
                    }
                }

            });

        }
        //if ($.trim(result) != "") {
        //if (type != "add" && istrue) {//新增行验证时不保存
        //    $(".hidSortContent").val(result);//获取排序条件内容
        //}
        //}

        return istrue;
    }
    function checkdatatypev(cdatatype, cvalue, cname) {
        var checkv = true;
        if (cdatatype == "int" || cdatatype == "bigint" || cdatatype == "smallint" || cdatatype == "int16" || cdatatype == "int32" || cdatatype == "int64") {
            if (isNaN(cvalue)) {
                layer.alert("字段名：" + cname + " 为整数类型，比较条件值只能输入整数！");
                checkv = false;
            }
        }
        if (cdatatype == "decimal" || cdatatype == "float" || cdatatype == "double") {
            var tempValue = cvalue;
            var decimalpoint = "";
            if (tempValue.indexOf(".") >= 0) {
                decimalpoint = tempValue.substring(tempValue.indexOf("."));
            }
            if (isNaN(tempValue) || decimalpoint.length > 3) {
                tempValue = tempValue.replace(/[^\d.]/g, ""); //清除"数字"和"."以外的字符
                tempValue = tempValue.replace(/^\./g, ""); //验证第一个字符是数字而不是
                tempValue = tempValue.replace(/\.{2,}/g, "."); //只保留第一个. 清除多余的
                tempValue = tempValue.replace(".", "$#$").replace(/\./g, "").replace("$#$", ".");
                tempValue = tempValue.replace(/^(\-)*(\d+)\.(\d\d).*$/, '$1$2.$3'); //只能输入两个小数
            }
            if (tempValue != cvalue) {
                layer.alert("字段名：" + cname + " 为数字类型，比较条件值只能输入数字！");
                checkv = false;
            }
        }
        if (cdatatype == "datetime") {
            if (!checkDate(cvalue)) {
                layer.alert("字段名：" + cname + " 为日期类型，比较条件值只能输入日期格式！");
                checkv = false;
            }
        }
        return checkv;
    }
    //检查是否为日期格式
    function checkDate(d) {
        var dlist = d.split('/');
        if (dlist.length == 2) {
            return false;
        }
        var dlistd = d.split('-');
        if (dlistd.length == 2) {
            return false;
        }
        //if(isNaN(Date.parse(d))){
        //    return false;
        //} else {
        //    return true;
        //}
        var ds = d.match(/\d+/g), ts = ['getFullYear', 'getMonth', 'getDate'];
        var d = new Date(d.replace(/-/g, '/')), i = 3;
        if (ds == null || ds == undefined || ds == "") {
            return false;
        }
        ds[1]--;
        while (i--) if (ds[i] * 1 != d[ts[i]]()) return false;
        return true;
    }
    //添加默认筛选条件
    function adddefaultquery() {
        var cr = GetDefaultTable("add");
        if (cr) {
            var qnum = $(".hidquerynum").val();
            qnum = parseInt(qnum);
            qnum++;
            $("#defaultdivdemo").clone(true).attr("id", "defaultdiv" + qnum).insertAfter("#defaultdiv" + (qnum - 1));
            $("#defaultdiv" + qnum).find(".filtrateIpt").attr("name", "rad" + qnum);
            $("#defaultdiv" + qnum).find(".tableOne").attr("id", "tabcontent" + qnum);
            $("#defaultdiv" + qnum).find(".filtrateTitle").html("默认筛选条件" + qnum);
            $("#defaultdiv" + qnum).find("input[type='hidden']").attr("id", "hidtabcontent" + qnum).attr("class", "hidtabcontent" + qnum);
            $(".hidquerynum").val(qnum);
            $("#defaultdiv" + qnum).find("input[type='radio'][value='and']").prop("checked", true);
        }
    }
    //获取默认筛选条件值并验证值是否填写正确
    function GetDefaultTable(type) {
        var modid = 1;
        var istrue = false;
        var result = "";
        var sorttrid = $(".hidquerynum").val();
        sorttrid = parseInt(sorttrid);
        if (sorttrid == 0) {
            istrue = true;//可以为空
        }
        else {
            var logicv = "";
            $(".filtrateBox").each(function () {
                var noparacname = 1;
                var paracname = 1;
                if ($(this).attr("id") != "defaultdivdemo") {
                    var tableid = $(this).find(".tableOne").attr("id");
                    var modname = $("#" + tableid).prev().find(".filtrateTitle").html();//得到模块名称
                    var tcount = $("#" + tableid + " tbody").children("tr").length;
                    if (tcount == 0) {
                        layer.alert(modname + " 筛选条件配置不能为空！");
                        result = "";
                        istrue = false;
                        return false;
                    }
                    logicv = $.trim($("#" + tableid).prev().find("input[type='radio']:checked").val());
                    if (false) {
                        //layer.alert("请选择" + modname + "的关联关系and或or!");
                        //istrue = false;
                        //result = "";
                        //return false;
                    } else {
                        var tabcontentm = "";
                        //遍历tr取值
                        $("#" + tableid + " tbody tr").each(function () {
                            if ($(this).children('td').eq(1).find("select").length > 0) {
                                var cname = $.trim($(this).children('td').eq(1).find("div[id*='ddlColumnName']").find(".dk_label").text());//列名
                                var cdatatypev = $.trim($(this).children('td').eq(1).find("div[id*='ddlColumnName']").find(".dk_option_current").children('a').eq(0).attr("data-dk-dropdown-value"));//列类型
                                var cdatatype = cdatatypev.split(';')[0];
                                var compare = $.trim($(this).children('td').eq(1).find("div[id*='ddlCompare']").find(".dk_option_current").children('a').eq(0).attr("data-dk-dropdown-value"));//比较符
                                var cvalue = $.trim($(this).children('td').eq(1).find("input").val());
                                var remark = $.trim($(this).children('td').eq(2).find("input").val());
                                var canempty = true;//比较值是否可以为空，等于与不等于可以
                                if (compare == "EQUAL" || compare == "NOTEQUAL") {
                                    if (cvalue == "") {
                                        canempty = false;
                                    }
                                }
                                if (cvalue != "") {
                                    canempty = false;
                                }
                                if (cname != "" && cname.indexOf('@') != -1) {
                                    if (compare != "EQUAL") {
                                        layer.alert(modname + "中存储过程的参数名：" + cname + " 筛选条件只能配置等于!");
                                        result = ""; tabcontentm = "";
                                        istrue = false;
                                        return false;
                                    }
                                }
                                if (cname == "" || cname == "请选择" || compare == "" || compare == "请选择" || canempty) {
                                    //if (type == "add") {
                                    //    layer.alert("请先完成已有条件的配置！");
                                    //} else {

                                    layer.alert(modname + " 筛选条件配置信息不完整！");
                                    //}
                                    result = ""; tabcontentm = "";
                                    istrue = false;
                                    return false;
                                } else {
                                    var checkv = true;
                                    checkv = checkdatatypev(cdatatype, cvalue, cname);
                                    if (checkv) {
                                        var ispara = true;
                                        //if (cdatatype == "datetime") {
                                        //    re = new RegExp("\\/", "g");
                                        //    cvalue = cvalue.replace(re, "-");
                                        //}
                                        var tempsort = cname + "|" + compare + "|" + cvalue + "|";
                                        if (cname.indexOf('@') != -1) {//判断参数名有没有重复配置
                                            logicv = "proc";
                                            paracname = 2;
                                            if (tabcontentm.indexOf(cname + "|") != -1) {
                                                layer.alert(modname + " 筛选条件配置中列名：" + cname + " 为存储过程参数，只能配置一条！");
                                                result = ""; tabcontentm = "";
                                                istrue = false;
                                                return false;
                                            }
                                            if (result.indexOf(cname + "|") != -1) {
                                                layer.alert(modname + " 筛选条件配置中列名：" + cname + " 为存储过程参数，已配置过默认筛选条件，不能重复配置！");
                                                result = ""; tabcontentm = "";
                                                istrue = false;
                                                return false;
                                            }
                                        }
                                        else {
                                            noparacname++;
                                        }
                                        if (paracname == 2 && noparacname > 1) {//如一个模块中配置了一个参数则整个模块只能填参数
                                            layer.alert(modname + " 筛选条件配置中存储过程参数与列不能混合配置！");
                                            result = ""; tabcontentm = "";
                                            istrue = false;
                                            return false;
                                        }
                                            if (tabcontentm.indexOf(tempsort) == -1) {
                                                tabcontentm += tempsort + remark + "|" + modid + "|" + logicv + ";";

                                                istrue = true;
                                            } else {
                                                layer.alert(modname + " 筛选条件配置中列名：" + cname + " 不能重复配置！");
                                                result = ""; tabcontentm = "";
                                                istrue = false;
                                                return false;
                                            }

                                    } else {
                                        result = ""; tabcontentm = "";
                                        istrue = false;
                                        return false;
                                    }
                                }
                            }

                        });
                        if (!istrue) {
                            result = ""; tabcontentm = "";
                            return false;//跳出外面遍历
                        }
                        if (paracname != 2 && logicv == "" && tcount > 2) {
                            layer.alert("请选择" + modname + "的关联关系and或or!");
                            result = ""; tabcontentm = "";
                            istrue = false;
                            return false;
                        }
                        if ($.trim(tabcontentm) != "") {
                            result += tabcontentm;
                        }
                        modid++;
                    }
                }
            });

        }
        if (type != "add" && istrue) {//新增行验证时不保存
            $(".hidDefaultContent").val(result);//获取排序条件内容
            $(".hidmodid").val(modid);
        }
        return istrue;
    }
    //删除一块默认筛选条件
    function removemodule(obj) {
        $(obj).parent().parent().remove();
        var qnum = $(".hidquerynum").val();
        qnum = parseInt(qnum);
        qnum--;
        $(".hidquerynum").val(qnum);
        var m = 1;
        $(".filtrateBox").each(function () {
            if ($(this).attr("id") != "defaultdivdemo") {
                $(this).attr("id", "defaultdiv" + m);
                $(this).find(".filtrateIpt").attr("name", "rad" + m);
                $(this).find(".tableOne").attr("id", "tabcontent" + m);
                $(this).find(".filtrateTitle").html("默认筛选条件" + m);
                $(this).find("input[type='hidden']").attr("id", "hidtabcontent" + qnum).attr("class", "hidtabcontent" + qnum);
                m++;
            }
        });
    }
    //提交验证
    function executesub() {
        var b_return = false;
        var cr = GetDefaultTable("submit");
        if (cr) {
            var modnum = $.trim($(".hidmodid").val());
            if (modnum != "") {
                if (parseInt(modnum) > 2) {//两个及以上多个模块才需check模块间逻辑
                    var dc = $.trim($(".hidDefaultContent").val());
                    if (dc != "") {
                        var mainlogic = $("input[name*='defaultmain']:checked").val();
                        if (mainlogic == "" || mainlogic == undefined || mainlogic == null) {
                            layer.alert("请选择默认筛选条件的逻辑关系and或or!");
                            return false;
                        }
                    }
                }
            }
            b_return = GetUserQueryTable('submit');
            if (b_return) {
                var procpara = $.trim($(".hidprocparalist").val());
                if (procpara != "") {
                    var comparecontentpara = $.trim($(".hidDefaultContent").val()) + "|" + $(".hidUserQueryContent").val();
                    var proclist = procpara.split(',');
                    for (var i = 0; i < proclist.length; i++) {
                        if ($.trim(proclist[i]) != "") {
                            if (comparecontentpara.indexOf(proclist[i] + "|") == -1) {
                                layer.alert("存储过程中参数名：" + proclist[i] + "未配置筛选条件!");
                                b_return = false;
                                break;
                            }
                        }
                    }
                }
            }
        }
        return b_return;
    }
    //用户筛选条件添加行
    function adduserquery() {
        var rr = true;
        var sorttrid = $(".hidSortNum").val();
        sorttrid = parseInt(sorttrid);
        if (sorttrid > 0) {
            rr = GetUserQueryTable('add');//判断已添加的行是否已填写完整，未填写完整不能继续添加
        }
        if (rr) {
            sorttrid++;
            $("#uqtrclone").find("select[id*='ddluqcol']").attr("onchange", "initdisplaynamedf(this,'txtshowname" + sorttrid + "')");
            $("#uqtrclone").find("select[id*='ddlcontroltype']").attr("onchange", "hidecomparebytype(this,'userqueryclone_" + sorttrid + "')");
            $("#userquerytab tbody").append($("#uqtrclone").clone(true).attr("id", "userqueryclone_" + sorttrid));
            $(".hidSortNum").val(sorttrid);
            $("#userqueryclone_" + sorttrid + " td").each(function () {//循环克隆的新行里面的td 
                //修改相关属性 
                $(this).find("select[name*='ddluqcol']").attr("id", "ddluqcol" + sorttrid).attr("name", "ddluqcol" + sorttrid);
                $(this).find("div[id*='ddluqcol']").attr("id", "dk_container_ddluqcol" + sorttrid);

                $(this).find(".dk_label").text("请选择");//列名
                $(this).find(".dk_option_current").attr("class", "");
                //修改排序名称
                $(this).find("select[name*='ddlcontroltype']").attr("id", "ddlcontroltype" + sorttrid).attr("name", "ddlcontroltype" + sorttrid).val("");
                $(this).find("div[id*='ddlcontroltype']").attr("id", "dk_container_ddlcontroltype" + sorttrid);
                $(this).find("input[name*='txtshowname']").attr("name", "txtshowname" + sorttrid).attr("id", "txtshowname" + sorttrid).val("");
                $(this).find("input[name*='txtsort']").val("");
                $(this).find("input[name*='txtdesc']").val("");
                $(this).find("input[name*='txtdefault']").val("");
                if ($(this).index() == "0") {
                    $(this).text(sorttrid);
                }
            });
        }
    }
    //遍历用户筛选表取值
    function GetUserQueryTable(type) {
        var defaultvalue = $(".hidDefaultContent").val();
        var istrue = false;
        var result = "";
        var sorttrid = $(".hidSortNum").val();
        sorttrid = parseInt(sorttrid);
        if (sorttrid == 0) {
            istrue = true;//排序可以为空
        } else {
            //遍历tr取值
            $("#userquerytab tbody tr").each(function () {
                if ($(this).children('td').eq(2).find("input").length > 0) {
                    var cname = $.trim($(this).children('td').eq(1).find(".dk_label").text());//列名
                    var showname = $.trim($(this).children('td').eq(2).find("input").val());
                    var controltype = $.trim($(this).children('td').eq(3).find(".dk_option_current").children('a').eq(0).attr("data-dk-dropdown-value"));//控件类型
                    var sortnum = $.trim($(this).children('td').eq(4).find("input").val());
                    var comparetype = $.trim($(this).children('td').eq(5).find(".dk_option_current").children('a').eq(0).attr("data-dk-dropdown-value"));//比较符
                    var defaultval = $.trim($(this).children('td').eq(6).find("input").val());
                    var desc = $.trim($(this).children('td').eq(7).find("input").val()); 
                    if (controltype == "DATEFAST") {
                        comparetype = "EQUAL";
                    }
                    if (cname == "" || cname == "请选择" || showname == "" || controltype == "" || controltype == "请选择" || comparetype == "" || comparetype == "请选择") {
                        if (type == "add") {
                            layer.alert("用户筛选条件配置信息不完整！");
                        } else {
                            layer.alert("用户筛选条件配置信息不完整！");
                        }
                        result = "";
                        istrue = false;
                        return false;
                    } else {
                        var ispara = true;
                        if (cname.indexOf('@') != -1) {//存储过程参数判断是否重复
                            if (comparetype != "EQUAL") {
                                layer.alert("用户筛选条件配置中列名：" + cname + " 为存储过程中参数，比较符只能配置等于!");
                                result = "";
                                istrue = false;
                                return false;
                            }
                            if (controltype == "ENUM" || controltype == "MATCH") {
                                layer.alert("用户筛选条件配置中列名：" + cname + " 为存储过程中参数，类型不能配置为枚举或智能匹配!");
                                result = "";
                                istrue = false;
                                return false;
                            }
                            if (defaultvalue.indexOf(cname + "|") != -1) {//判断在默认筛选条件中是否已配置
                                layer.alert("用户筛选条件配置中列名：" + cname + " 为存储过程中参数，只能配置一条，此参数已在默认筛选条件中配置过！");
                                result = "";
                                istrue = false;
                                return false;
                            }
                            if (result.indexOf(cname + "|") != -1) {//判断在用户筛选条件中是否已配置
                                layer.alert("用户筛选条件配置中列名：" + cname + " 为存储过程中参数，只能配置一条！");
                                result = "";
                                istrue = false;
                                return false;
                            }
                        }
                        if (controltype == "ENUM" && comparetype != "EQUAL") {//枚举只能配置等于
                            layer.alert("用户筛选条件配置中列名：" + cname + " 类型为枚举，只能配置为等于!");
                            result = "";
                            istrue = false;
                            return false;
                        }
                        if (controltype == "DATEFAST") {//日期快捷默认值只能为数字或上个月、２个月、３个月、半年、１年
                            if (defaultval == "") {
                                layer.alert("用户筛选条件配置中列名：" + cname + " 类型为日期(快捷)，默认值不能为空!");
                                result = "";
                                istrue = false;
                                return false;
                            }
                            else {
                                var dlist = defaultval.split(';');
                                var isint = 0;
                                for (var j = 0; j < dlist.length; j++) {
                                    if ($.trim(dlist[j]) != "" && $.trim(dlist[j]) != "上个月" && $.trim(dlist[j]) != "2个月" && $.trim(dlist[j]) != "3个月" && $.trim(dlist[j]) != "半年" && $.trim(dlist[j]) != "1年") {
                                        var re = /^[1-9]+[0-9]*]*$/;
                                        if (!re.test($.trim(dlist[j]))) {
                                            isint = 1;
                                            break;
                                        }
                                    }
                                }
                                if (isint == 1) {
                                    layer.alert("用户筛选条件配置中列名：" + cname + " 类型为日期(快捷)，默认值只能为数字或上个月、2个月、3个月、半年、1年，多个值请以分号分隔!");
                                    result = "";
                                    istrue = false;
                                    return false;
                                }
                            }
                        }
                        var tempsort = cname + "|" + showname + "|" + controltype + "|" + comparetype;
                        if (result.indexOf(tempsort) == -1) {
                            result += tempsort + "|" + sortnum + "|" + desc + "|" + defaultval + "$";
                            istrue = true;
                        } else {
                            layer.alert("用户筛选条件配置中列名：" + cname + " 不能重复配置！");
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
            $(".hidUserQueryContent").val(result);//获取排序条件内容
        }
        //}

        return istrue;
    }

    //删除用户筛选条件
    function removeuserquerytr(trname) {
        var sorttrid = $(".hidSortNum").val();
        sorttrid = parseInt(sorttrid);
        if (sorttrid > 0) {
            sorttrid--;
        }
        $(".hidSortNum").val(sorttrid);
        $("#" + trname).remove();
        var zid = 1;
        //遍历tr取值
        $("#userquerytab tbody tr").each(function () {
            if ($(this).children('td').eq(1).find("select").length > 0) {
                $(this).children('td').eq(0).html(zid);
                $(this).attr("id", "userqueryclone_" + zid);
                zid++;
            }
        });
    }
    //删除行
    function removedefaulttabtr(obj) {
        var tabid = $(obj).parent().parent().parent().parent().attr("id");
        var sorttrid = $("#" + tabid).find("input[type='hidden']").val();
        sorttrid = parseInt(sorttrid);
        if (sorttrid > 0) {
            sorttrid--;
        }
        $("#" + tabid).find("input[type='hidden']").val(sorttrid);
        $(obj).parent().parent().remove();
        var zid = 1;
        //遍历tr取值
        $("#" + tabid + " tbody tr").each(function () {
            if ($(this).children('td').eq(1).find("select").length > 0) {
                $(this).attr("id", "defaulttr_" + zid);
                $(this).children('td').eq(0).html(zid);
                zid++;
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
                    if (type == "userquerydel") {
                        var pid = $(obj).parent().parent().attr('id');
                        removeuserquerytr(pid);
                        inittableonetrstyle();
                    }
                    if (type == "deldefault") {//删除默认筛选条件模块
                        removemodule(obj);
                        inittableonetrstyle();
                    }
                    if (type == "deltr") {//删除默认筛选条件table中tr
                        removedefaulttabtr(obj);
                        inittableonetrstyle();
                    }
                },
                no: function () {
                }
            }
        });

    }
    //变更列名时检查是否是参数，是则and或or不可编辑
    function changeradiostatus(objtext, obj) {
        if ($.trim(objtext).indexOf('@') != -1) {
            $(obj).parent().parent().parent().parent().parent().prev().find("input[type='radio']").prop("checked", false).prop("disabled", true);
        } else {
            $(obj).parent().parent().parent().parent().parent().prev().find("input[type='radio']").prop("disabled", false);
        }
    }
    //获取默认筛选条件值并验证值是否填写正确
    function initandordisabled() {
        $(".filtrateBox").each(function () {
            if ($(this).attr("id") != "defaultdivdemo") {
                var tableid = $(this).find(".tableOne").attr("id");
                //遍历tr取值
                $("#" + tableid + " tbody tr").each(function () {
                    if ($(this).children('td').eq(1).find("select").length > 0) {
                        var cname = $.trim($(this).children('td').eq(1).find("div[id*='ddlColumnName']").find(".dk_label").text());//列名
                        if (cname.indexOf('@') != -1) {
                            $("#" + tableid).prev().find("input[type='radio']").prop("checked", false).prop("disabled", true);
                            return false;
                        }
                    }

                });

            }
        });

    }
    function initdisplayname(obj) {
        var selectv = $(obj).val();
        var ccollist = $(".hidconfigcolinfo").val().split(';');
        for (var i = 0; i < ccollist.length; i++) {
            if ($.trim(ccollist[i]) != "") {
                var sstr = ccollist[i].split('|');
                if ($.trim(sstr[0]) == $.trim(selectv)) {
                    if ($(obj).parent().parent().next().find("input[name*='txtshowname']").length > 0) {
                        $(obj).parent().parent().next().find("input[name*='txtshowname']").val(sstr[1]);
                    }
                    break;
                }
            }
        }
    }
    function initdisplaynamedf(obj,mmid) {
        var selectv = $(obj).val();
        var ccollist = $(".hidconfigcolinfo").val().split(';');
        for (var i = 0; i < ccollist.length; i++) {
            if ($.trim(ccollist[i]) != "") {
                var sstr = ccollist[i].split('|');
                if ($.trim(sstr[0]) == $.trim(selectv)) {
                    if ($("#" + mmid).length > 0) {
                        $("#" + mmid).val(sstr[1]);
                    }
                    break;
                }
            }
        }
    }
    //如果类型是日期快捷，则隐藏比较符
    function hidecomparebytype(obj,trid) {
        var selectv = $(obj).val();
        if (selectv == "DATEFAST") {
            $("#" + trid + " td:eq(5)").find("div:first").css("cssText", "display: none!important;");
        } else {
            $("#" + trid + " td:eq(5)").find("div:first").css("cssText", "display: block;");
        }
        if (selectv == "DATE") {
            $("#" + trid + " td:eq(6)").find("input").attr("class", "setIpt laydate-icon dtleft").attr("onclick", "laydate({ istime: true, format: 'YYYY-MM-DD' })");
        }
        else if (selectv == "YDATE") {
            $("#" + trid + " td:eq(6)").find("input").attr("class", "setIpt laydate-icon").attr("onclick", "laydate({ format: 'YYYY' })");
        }
        else if (selectv == "YMDATE") {
            $("#" + trid + " td:eq(6)").find("input").attr("class", "setIpt laydate-icon").attr("onclick", "laydate({format: 'YYYY-MM' })");
        }
        else {
            $("#" + trid + " td:eq(6)").find("input").removeClass("laydate-icon").removeClass("dtleft").removeAttr("onclick");
        }
    }
    //如果类型是日期快捷，则隐藏比较符
    function hidecomparebytyperp(obj) {
        var selectv = $(obj).val();
        if (selectv == "DATEFAST") {
            $(obj).parent().parent().next().next().find("div:first").css("cssText", "display: none!important;");
        } else {
            $(obj).parent().parent().next().next().find("div:first").css("cssText", "display: block;");
        }
        if (selectv == "DATE") {
            $(obj).parent().parent().next().next().next().find("input").attr("class", "setIpt laydate-icon dtleft").attr("onclick", "laydate({ istime: true, format: 'YYYY-MM-DD' })");
        }
        else if (selectv == "YDATE") {
            $(obj).parent().parent().next().next().next().find("input").attr("class", "setIpt laydate-icon").attr("onclick", "laydate({ format: 'YYYY' })");
        }
        else if (selectv == "YMDATE") {
            $(obj).parent().parent().next().next().next().find("input").attr("class", "setIpt laydate-icon").attr("onclick", "laydate({format: 'YYYY-MM' })");
        }
        else {
            $(obj).parent().parent().next().next().next().find("input").removeClass("laydate-icon").removeClass("dtleft").removeAttr("onclick");
        }
    }
    //遍历用户筛选条件表，如果是日期快递则隐藏比较符，绑定用
    function hidecompareonload() {
        $("#userquerytab tbody tr").each(function () {
            if ($(this).find(".lblctype").length > 0) {
                var datatype = $(this).find(".lblctype").text();
                if (datatype == "DATEFAST") {
                    $(this).find("td:eq(5)").find("div:first").css("cssText", "display: none!important;");
                }
            }
        });
    }
    $(function ($) {
        initandordisabled();
        hidecompareonload();
    });
</script>
