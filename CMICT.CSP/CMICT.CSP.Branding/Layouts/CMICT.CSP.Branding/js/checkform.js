﻿//$Id: checkForm.js,$
//tony 2004-3-30
//新增了对form的查询条件进行自动拼装的功能getQueryString();
/**
 * 检查送出的form的每个表单元素是否符合填写要求
 * @fm 需要检查的form元素。
 * @return 如果form的所有表单元素都符合要求，将返回true，
 * 否则将会报告不符合的原因，同时返回false。
 * 示例：
 * <form onsubmit="return checkForm(this)">
 *  	<input type=submit>
 *  	<input name=email title="请填写邮件地址~email!">
 * </form>
 * 说明：
 * 在form表单元素的title属性中指明此表单元素期望的格式。
 * 此格式说明如下
 *          请填写邮件地址~email!
 * 以最后一个"~"为界，前面的是提示信息，后面是格式信息。
 * 格式信息请遵守以下规则：
 * [number[f]:][type][!]
 * 说明：
 * number[f]:	一个数值后面跟一个":",表示此域的文本长度不可以超过指定的数值，如果在数值后面有个f表示固定长度必须为多少位
 * type可以是如下表达式
 *	 email	邮件地址
 *	 int		整数
 *	 float	浮点数
 *	 date		日期
 *	 time		时间
 *	 hasChinese     含有中文
 *	 allChinese	全部是中文
 *	 noChinese	没有中文
 *   decimal    最多4位小数的金额
 *   No0        不为0的int类型
 *	 /.../[gi]	自定义正则式
 *   A-Z_0-9    A-Z 0-9的限定
 * !表示此处文本不可以为空。
 * 对于<input type=radio > 格式串为
 * "请选择一个选项~!"表示此radio组必须选择一个选项
 * "请选择一个选项~"表示此radio组的选项可以不选。
 * 对于<input type=checkbox >或者<select multiple></select>格式串的意义为
 * 说明信息~min:0max:3
 * 对于<select ></select>非multiple类型
 * "请选择一个选项~!"表示此select不可以选择第一个选项
 * "请选择一个选项~"表示此select可以选择第一个选项
 */

//定义错误接收的函数
var Msg;
//定义验证状态
var MsgState = true;



function ReturnErrors() {

    return Msg;
}
function errorMsg(name, title) {
    if (document.getElementById('div' + name) != "undefined") {
        document.getElementById('div' + name).innerHTML = "<img width='16'  id='img" + name + "' src='/_layouts/15/CMICT.CSP.Branding/images/alert.png'>";
        document.getElementById('div' + name).className = "";
        document.getElementById('img' + name).alt = title;
    }

}

function deoxidizevalidate(obj) {
    if (obj.id != '' && document.getElementById('div' + obj.id) != null && document.getElementById('div' + obj.id) != "undefined" && document.getElementById('div' + obj.id).innerHTML != '') {
        document.getElementById('div' + obj.id).innerHTML = '';
        document.getElementById('div' + obj.id).style.width = 0;
        document.getElementById('div' + obj.id).style.height = 0;
    }
    else {
    }
}


function killErrors() {
    return true;
}

function checkForm(fm) {
    MsgState = true;
    Msg = new Array();
    //window.onerror = killErrors; 
    for (var i = 0; i < fm.length; i++) {

        var title = fm[i].title;
        if (title == "" || title == undefined || title == null) continue;//忽略未定义title的元素
        var p = title.lastIndexOf("~");
        if (p < 0) continue;//忽略title中未定义检查格式的元素
        var info = title.substring(0, p);
        var format = title.substring(p + 1, title.length);
        var format1 = title.substring(p + 1, title.length);
        var format2 = title.substring(p + 1, title.length);
        var name = fm[i].id;

        if (name == "") continue;//忽略没有名字的元素


        var value = trim(fm[i].value);
        //fm[i].value=value;//自动除去送出项的两端的空格

        if (fm[i].type == "radio") {
            if (checkRadio(fm, fm[i])) {
                continue;
            }
            else {
                MsgState = false;
                //return false;
            }
        }
        if (fm[i].type == "checkbox") {
            if (checkCheckbox(fm, fm[i])) {
                continue;
            }
            else {
                MsgState = false;
                //return false;
            }
        }
        if (fm[i].type == "select-one") {
            if (checkSelectOne(fm[i])) {
                continue;
            }
            else {
                MsgState = false;
                //					return false;
            }
        }
        if (fm[i].type == "select-multiple") {
            if (checkSelectMultiple(fm[i])) {
                continue;
            }
            else {
                MsgState = false;
                //					return false;
            }
        }
        var notNull = false;

        if (format.charAt(format.length - 1) == "!") {
            notNull = true;
            format = format.substring(0, format.length - 1);
        }
        if (notNull) {
            if (value == "") {
                //alert(info+"\n"+name+"的内容不可以为空。");
                //					alert(info+"\n其内容不可以为空。");
                //					try {
                //						fm[i].focus();  
                //					}
                //					catch (e){
                //					}
                //					finally {
                //						
                //						}
                //					
                //					return false;
                ms = new Array();
                ms.push(name);  //设置控件ID
                ms.push(info + "\n其内容不可以为空。"); //错误内容
                Msg.push(ms);   //将验证内容加入到对象列表中
                MsgState = false;
                continue;
            }
        }
        else {
            //内容可以为空时，
            if (value == "")
                continue;
        }

        ///判断是否为int类型
        if (format2.charAt(format2.length - 1) == "!") {
            format2 = format2.substring(0, format2.length - 1);
        }
        var colonP2 = format2.indexOf(":");
        format2 = format2.substring(colonP2 + 1, format2.length);
        //alert (format2);
        var colonP = format.indexOf(":");
        if (format2 != "int") {
            //内容的长度判断
            //var colonP=format.indexOf(":");
            if (colonP > 0) {
                if (format.charAt(colonP - 1) == 'f') {
                    var lengthLimit = format.substring(0, colonP - 1);
                    if (!isNaN(lengthLimit)) {
                        var strLength = countCharacters(value, lengthLimit);//字符个数，包括中文
                        if (strLength != lengthLimit) {
                            ms = new Array();
                            ms.push(name);  //设置控件ID
                            //ms.push( info+"\n其长度("+strLength+")超过限制"+lengthLimit); //错误内容
                            ms.push(info + "\n其长度超过限制" + lengthLimit); //错误内容
                            Msg.push(ms);   //将验证内容加入到对象列表中
                            MsgState = false;
                        }
                    }
                }
                else {
                    var lengthLimit = format.substring(0, colonP);
                    if (!isNaN(lengthLimit)) {
                        var strLength = countCharacters(value, lengthLimit);//字符个数，包括中文
                        if (strLength > lengthLimit) {
                            ms = new Array();
                            ms.push(name);  //设置控件ID
                            //ms.push( info+"\n其长度("+strLength+")超过限制"+lengthLimit); //错误内容
                            ms.push(info + "\n其长度超过限制" + lengthLimit); //错误内容
                            Msg.push(ms);   //将验证内容加入到对象列表中
                            MsgState = false;
                        }
                    }
                }
            }

        }
        format = format.substring(colonP + 1, format.length);

        //alert (format);
        if (format == "email") {
            //电子邮件格式
            var found = value.match(/\w+@.+\..+/);
            if (found == null) {

                ms = new Array();
                ms.push(name);  //设置控件ID
                ms.push(info + "\n其格式不正确:\n\"" + value + "\"不是一个Email地址"); //错误内容
                Msg.push(ms);   //将验证内容加入到对象列表中
                MsgState = false;

                //return false;

            }
        } else if (format == "int") {
            //整数
            var intVal = parseInt(value);
            if (isNaN(intVal) || intVal != value) {

                ms = new Array();
                ms.push(name);  //设置控件ID
                ms.push(info + "\n其格式不正确:\n" + value + "不是一个整数。"); //错误内容
                Msg.push(ms);   //将验证内容加入到对象列表中
                MsgState = false;
            }
            //内容的值的大小判断
            var colonP = format1.indexOf(":");
            if (colonP > 0) {

                var lengthLimit = format1.substring(0, colonP);
                if (!isNaN(lengthLimit)) {//alert(value);
                    if (parseInt(value) > parseInt(lengthLimit)) {

                        ms = new Array();
                        ms.push(name);  //设置控件ID
                        ms.push(info + "\n其值(" + value + ")超过限制" + lengthLimit); //错误内容
                        Msg.push(ms);   //将验证内容加入到对象列表中
                        MsgState = false;
                    }
                }
            }

        } else if (format == "float") {
            //浮点数
            var floatVal = parseFloat(value);
            if (isNaN(floatVal) || floatVal != value) {
                //alert(info+"\n"+name+"的格式不正确:\n"+value+"不是一个浮点数。");
                //					alert(info+"\n其格式不正确:\n"+value+"不是一个浮点数。");
                //					fm[i].focus();
                //					return false;
                ms = new Array();
                ms.push(name);  //设置控件ID
                ms.push(info + "\n其格式不正确。\n"); //错误内容
                Msg.push(ms);   //将验证内容加入到对象列表中
                MsgState = false;
            }
        } else if (format == "decimal") {
            //金额格式
            var found = value.match("^[+|-]{0,1}[0-9]+(.[0-9]{1,2})?$");
            if (found == null) {
                ms = new Array();
                ms.push(name);  //设置控件ID
                ms.push(info + "\n不是一个正确的金额格式:\n\"" + value + "\"应为最多2位小数的数值,\n取值范围应在:0.01 -  9999999999.99之间"); //错误内容
                Msg.push(ms);   //将验证内容加入到对象列表中
                MsgState = false;
            } else {
                var floatVal = parseFloat(value);
                if (isNaN(floatVal) || floatVal != value) {
                    var intVal = parseInt(value);
                    if (isNaN(intVal) || intVal != value) {
                        ms = new Array();
                        ms.push(name);  //设置控件ID
                        ms.push(info + "\n不是一个正确的金额格式:\n\"" + value + "\"应为最多2位小数的数值,\n取值范围应在:0.01 -  9999999999.99之间"); //错误内容
                        Msg.push(ms);   //将验证内容加入到对象列表中
                        MsgState = false;
                    }
                }
            }
        } else if (format == "decimalNo0") {
            //金额格式
            var found = value.match("^[+|-]{0,1}[0-9]+(.[0-9]{1,2})?$");
            if (found == null) {
                ms = new Array();
                ms.push(name);  //设置控件ID
                ms.push(info + "\n不是一个正确的金额格式:\n\"" + value + "\"应为最多2位小数的数值,\n取值范围应在:0.01 -  9999999999.99之间"); //错误内容
                Msg.push(ms);   //将验证内容加入到对象列表中
                MsgState = false;
            } else {
                var floatVal = parseFloat(value);
                if (isNaN(floatVal) || floatVal != value) {
                    var intVal = parseInt(value);
                    if (isNaN(intVal) || intVal != value) {
                        ms = new Array();
                        ms.push(name);  //设置控件ID
                        ms.push(info + "\n不是一个正确的金额格式:\n\"" + value + "\"应为最多2位小数的数值,\n取值范围应在:0.01 -  9999999999.99之间"); //错误内容
                        Msg.push(ms);   //将验证内容加入到对象列表中
                        MsgState = false;
                    }
                }
            }

            if (value <= 0) {
                ms = new Array();
                ms.push(name);  //设置控件ID
                ms.push(info + "：\n" + "必须大于零！"); //错误内容
                Msg.push(ms);   //将验证内容加入到对象列表中
                MsgState = false;
            }
        } else if (format == "date") {
            //日期
            var found = value.match(/(\d{1,5})-(\d{1,2})-(\d{1,2})/);
            var found2 = value.match(/(\d{1,5})\/(\d{1,2})\/(\d{1,2})/);
            if (found == null || found[0] != value || found[2] > 12 || found[3] > 31) {
                if (found2 == null || found2[0] != value || found2[2] > 12 || found2[3] > 31) {

                    ms = new Array();
                    ms.push(name);  //设置控件ID
                    ms.push(info + "\n其格式不正确:\n\"" + value + "\"不是一个日期\n提示：[2000-01-01]"); //错误内容
                    Msg.push(ms);   //将验证内容加入到对象列表中
                    MsgState = false;
                }
            }
            var year = trim0(found[1]);
            var month = trim0(found[2]) - 1;
            var date = trim0(found[3]);
            var d = new Date(year, month, date);
            if (d.getFullYear() != year || d.getMonth() != month || d.getDate() != date) {
                //alert(info+"\n"+name+"的内容不正确:\n\""+value+"\"不是一个正确的日期\n提示：[2000-01-01]");
                //					alert(info+"\n其内容不正确:\n\""+value+"\"不是一个正确的日期\n提示：[2000-01-01]");
                //					fm[i].focus();
                //					return false;
                ms = new Array();
                ms.push(name);  //设置控件ID
                ms.push(info + "\n其内容不正确:\n\"" + value + "\"不是一个正确的日期\n提示：[2000-01-01]"); //错误内容
                Msg.push(ms);   //将验证内容加入到对象列表中
                MsgState = false;
            }
        } else if (format == "datetime") {
            //alert(isDateStr(value));
            if (!isDateStr(value)) {

                ms = new Array();
                ms.push(name);  //设置控件ID
                ms.push(info + "\n其内容不正确:\n\"" + value + "\"不是一个正确的日期时间\n提示：[2000-01-01 05:38:00]"); //错误内容
                Msg.push(ms);   //将验证内容加入到对象列表中
                MsgState = false;
            }
        } else if (format == "time") {
            //时间
            var found = value.match(/(\d{2}):(\d{2}):(\d{2})/);
            if (found == null || found[0] != value || found[1] > 23 || found[2] > 59 || found[3] > 59) {

                ms = new Array();
                ms.push(name);  //设置控件ID
                ms.push(info + "\n其格式不正确:\n\"" + value + "\"不是一个时间\n提示：[05:38:00]"); //错误内容
                Msg.push(ms);   //将验证内容加入到对象列表中
                MsgState = false;
            }
        } else if (format == "hasChinese") {
            var _hasChinese = false;
            for (var j = 0; j < value.length; j++) {
                if (value.charCodeAt(j) > 255) {
                    _hasChinese = true;
                    break;
                }
            }
            if (!_hasChinese) {

                ms = new Array();
                ms.push(name);  //设置控件ID
                ms.push(info + "\n其内容需要中文:\n\"" + value + "\"不含有任何中文字符"); //错误内容
                Msg.push(ms);   //将验证内容加入到对象列表中
                MsgState = false;
            }
        } else if (format == "allChinese") {
            for (var j = 0; j < value.length; j++) {
                if (value.charCodeAt(j) <= 255) {

                    ms = new Array();
                    ms.push(name);  //设置控件ID
                    ms.push(info + "\n其内容要求全中文:\n\"" + value + "\"含有非中文字符"); //错误内容
                    Msg.push(ms);   //将验证内容加入到对象列表中
                    MsgState = false;
                }
            }
        } else if (format == "noChinese") {
            for (var j = 0; j < value.length; j++) {
                if (value.charCodeAt(j) > 255) {

                    ms = new Array();
                    ms.push(name);  //设置控件ID
                    ms.push(info + "\n其内容要求非中文:\n\"" + value + "\"含有中文字符"); //错误内容
                    Msg.push(ms);   //将验证内容加入到对象列表中
                    MsgState = false;
                    break;
                }
            }
        } else if (format == "No0") {

            var intVal = parseInt(value);
            if (isNaN(intVal) || intVal != value) {
                //不为整数
                ms = new Array();
                ms.push(name);  //设置控件ID
                ms.push(info + "\n其格式不正确:\n" + value + "不是一个整数。"); //错误内容
                Msg.push(ms);   //将验证内容加入到对象列表中
                MsgState = false;
            } else {
                if (value == 0) {
                    ms = new Array();
                    ms.push(name);  //设置控件ID
                    ms.push(info + "：\n" + info + "的记录数为0，请增加记录"); //错误内容
                    Msg.push(ms);   //将验证内容加入到对象列表中
                    MsgState = false;
                } else {
                    // var found=value.match("^[1-999999999]$");
                    // if(found==null)
                    if (value > 9999999999) {
                        ms = new Array();
                        ms.push(name);  //设置控件ID
                        ms.push(info + "\n其内容不正确:\n" + info + ":" + value + "应为1-999999999区间的值"); //错误内容
                        Msg.push(ms);   //将验证内容加入到对象列表中
                        MsgState = false;
                    }
                }
            }

        } else if (format != "") {
            //自定义
            try {
                var found = value.match(format);//eval(format))
                if (found == null || found[0] != value) {

                    ms = new Array();
                    ms.push(name);  //设置控件ID
                    //ms.push(info+"\n其格式不不符合要求:\""+value+"\"\n提示：["+format+"]"); //错误内容
                    ms.push(info + "\n其格式不符合要求:\"" + value + "\"\n提示：[只能输入逗号,句号,中文,大小写字母和数字]"); //错误内容 add by louis at 2015.9.25
                    Msg.push(ms);   //将验证内容加入到对象列表中
                    MsgState = false;
                }
            }
            catch (e) {

                ms = new Array();
                ms.push(name);  //设置控件ID
                ms.push(info + ":\n不合法的正则式\"" + format + "\""); //错误内容
                Msg.push(ms);   //将验证内容加入到对象列表中
                MsgState = false;
            }
        }
    }

    /////////////////////////////////

    //add will.xu检查名称唯一性验证
    if ($(".hidNameCheck").length > 0) {
        if ($.trim($(".hidNameCheck").val()) != "") {
            MsgState = false;
        }
    }
    if (MsgState == false) {
        var cfurl = '/_layouts/15/CMICT.CSP.Web/CheckFormMsg.html';
        $.layer({ type: 2, title: "错误提示", maxmin: false, border: [0], shadeClose: false, shade: [0.6, "#000"], area: ["310px", "300px"], offset: ["160px", ""], iframe: { src: cfurl, scrolling: "yes" } });
        //showPopWin('CheckFormMsg.htm', 400, 220,null);
        return false;
    } else {
        return true;
    }



    //return true;
}
function checkRadio(fm, opt) {
    var title = opt.title;
    if (title == "") return true;//忽略未定义title的元素
    var p = title.lastIndexOf("~");
    if (p < 0) return true;//忽略title中未定义检查格式的元素
    var info = title.substring(0, p);
    var format = title.substring(p + 1, title.length);
    var name = opt.name;
    if (name == "") return true;//忽略没有名字的元素
    if (format == "!") {
        //必须选择一个选项
        if (typeof (fm.all[name].length) == "undefined") {
            //同名radio只有一个
            if (opt.checked) {
                return true;
            }
            else {
                //alert(info+"\n必须选择"+name);
                alert(info + "\n必须选择");
                opt.focus();
                return false;
            }
        }
        else {
            //是一个radio组
            var radios = fm[name];
            for (var j = 0; j < radios.length; j++) {
                if (radios[j].checked == true) return true;
            }
            //alert(info+"\n必须选择"+name+"的一个选项");
            alert(info + "\n必须选择的一个选项");
            opt.focus();
            return false;
        }
    }
    else {
        //可以一个选项也不选
        return true;
    }
}
function checkCheckbox(fm, opt) {
    var title = opt.title;
    if (title == "") return true;//忽略未定义title的元素
    var p = title.lastIndexOf("~");
    if (p < 0) return true;//忽略title中未定义检查格式的元素
    var info = title.substring(0, p);
    var format = title.substring(p + 1, title.length);
    var name = opt.name;
    if (name == "") return true;//忽略没有名字的元素

    var min = format.match(/min:(\d+)\w*/);
    var max = format.match(/\w*max:(\d+)/);

    if (typeof (fm.all[name].length) == "undefined") {
        //只有一个同名checkbox
        if (min != null) {
            if (min[1] == 1 && !opt.checked) {
                //alert(info+"\n必须选上"+name+"选项");
                alert(info + "\n必须选上其中选项");
                opt.focus();
                return false;
            }
        }
    }
    else {
        //一个checkbox组
        var checkboxes = fm.all[name];
        var check_count = 0;
        for (var j = 0; j < checkboxes.length; j++) {
            if (checkboxes[j].checked) check_count++;
        }
        if (min != null) {
            if (min[1] > check_count) {
                //alert(info+"\n"+name+"至少需要选择"+min[1]+"个选项");
                alert(info + "\n至少需要选择" + min[1] + "个选项");
                opt.focus();
                return false;
            }
        }
        if (max != null) {
            if (max[1] < check_count) {
                //alert(info+"\n"+name+"至多可以选择"+max[1]+"个选项");
                alert(info + "\n至多可以选择" + max[1] + "个选项");
                opt.focus();
                return false;
            }
        }
    }
    return true;
}
function checkSelectOne(sel) {
    var title = sel.title;
    if (title == "") return true;//忽略未定义title的元素
    var p = title.lastIndexOf("~");
    if (p < 0) return true;//忽略title中未定义检查格式的元素
    var info = title.substring(0, p);
    var format = title.substring(p + 1, title.length);
    var name = sel.name;
    if (name == "") return true;//忽略没有名字的元素

    if (format == "!" && sel.selectedIndex == 0) {
        //alert(info+"\n"+name+"不可以选择第一个个选项");
        alert(info + "\n不可以选择第一个个选项");
        sel.focus();
        return false;
    }
    return true;
}
function checkSelectMultiple(sel) {
    var title = sel.title;
    if (title == "") return true;//忽略未定义title的元素
    var p = title.lastIndexOf("~");
    if (p < 0) return true;//忽略title中未定义检查格式的元素
    var info = title.substring(0, p);
    var format = title.substring(p + 1, title.length);
    var name = sel.name;
    if (name == "") return true;//忽略没有名字的元素

    var min = format.match(/min:(\d+)\w*/);
    var max = format.match(/\w*max:(\d+)/);

    var select_count = 0;
    for (var j = 0; j < sel.length; j++) {
        if (sel[j].selected) select_count++;
    }
    if (min != null) {
        if (min[1] > select_count) {
            //alert(info+"\n"+name+"至少需要选择"+min[1]+"个选项");
            alert(info + "\n至少需要选择" + min[1] + "个选项");
            sel.focus();
            return false;
        }
    }
    if (max != null) {
        if (max[1] < select_count) {
            //alert(info+"\n"+name+"至多可以选择"+max[1]+"个选项");
            alert(info + "\n至多可以选择" + max[1] + "个选项");
            sel.focus();
            return false;
        }
    }
    return true;
}
/**
 * 除去字符串变量s两端的空格。
 */
function trim(s) {
    s = s.replace(/^ */, "");
    s = s.replace(/ *$/, "");
    return s;
}
/**
 * 除去字符串表示的数值变量开头的所有的"0"。
 * 比如
 * 	trim0("01")将返回"1"
 * 	trim0("1")将返回"1"
 * 	trim0("10")将返回"10"
 * 	trim0("000")将返回"0"
 */
function trim0(s) {
    if (s.length == 0) return s;
    s = s.replace(/^0*/, "");
    if (s.length == 0) s = "0";
    return s;
}
/**
 * 取得一个form对像所送出时内部送出参数的QueryString
 * 形如:
 * ?accountName=&userName=&email=&area=0&credit_low=&credit_high=&age_low=&age_high=&userLevel=0
 */
function getQueryString(fm) {
    var qStr = "";
    for (var i = 0; i < fm.length; i++) {
        if (!fm[i].disabled) {
            var n = fm[i].name;
            if (n == null) continue;
            if (n.length == 0) continue;
            if (fm[i].type == "select-multiple") {
                var _vs = fm[i].options;
                for (var _j = 0; _j < _vs.length; _j++) {
                    var _opt = _vs(_j);
                    if (_opt.selected) {
                        var v = _opt.value;
                        qStr = qStr + "&" + n + "=" + ec(v);
                    }
                }
            }
            else {
                var v = fm[i].value;
                if (fm[i].type == "radio" || fm[i].type == "checkbox") {
                    if (!fm[i].checked) continue;
                }
                qStr = qStr + "&" + n + "=" + ec(v);
            }
        }
    }
    if (qStr.length > 0) qStr = "?" + qStr.substr(1);
    return qStr;
}
function ec(va) {
    return va.replace(/\n/g, "%0D%0A");
}


function select_img(html_code) {
    var file_name = window.showModalDialog('img_select_index.asp?O_type=Select_Img', '', 'dialogWidth=640px;dialogHeight=480px;help=no;status=no;');
    if (file_name != undefined) {
        File_Name_Array = file_name.split("|");
        try {
            document.all[html_code].value = File_Name_Array[1];
            document.all[html_code + '_Preview'].innerHTML = '<img src=' + File_Name_Array[0] + '/Min_' + File_Name_Array[1] + ' border=0><a href=javascript:open_win("' + File_Name_Array[0] + File_Name_Array[1] + '");>放大图片</a><br>';
        }
        catch (e) {
            document.all[html_code].value = File_Name_Array[0] + File_Name_Array[1];
        }

    }
}


function select_img_File(html_code) {
    var file_name = window.showModalDialog('img_select_index.asp?O_type=Select_Img', '', 'dialogWidth=640px;dialogHeight=480px;help=no;status=no;');
    if (file_name != undefined) {
        File_Name_Array = file_name.split("|");
        Div_Name = "Div_"
        for (i = 0; i < 5; i++) {
            intTemp = Math.random() * 26;
            Div_Name = Div_Name + String.fromCharCode(65 + intTemp);
        }
        ba = ''
        document.all[html_code].insertAdjacentHTML('beforeBegin', '<table id="' + Div_Name + '" cellpadding="0" cellspacing="0" border="0"><tr><td><input type="hidden" value="' + File_Name_Array[2] + '" name="Product_Img">' + File_Name_Array[1] + '&nbsp;</td><td ><a href="javascript:;" onclick=javascript:document.all("' + Div_Name + '").outerHTML=ba>移除</a>&nbsp;<a href=javascript:open_win("' + File_Name_Array[0] + File_Name_Array[1] + '");>查看图片</a></td></tr></table>');
    }
}

function select_img_down_file(html_code) {
    var file_name = window.showModalDialog('img_select_index.asp?O_type=Select_Img', '', 'dialogWidth=640px;dialogHeight=480px;help=no;status=no;');
    if (file_name != undefined) {
        File_Name_Array = file_name.split("|");
        Div_Name = "Div_"
        for (i = 0; i < 5; i++) {
            intTemp = Math.random() * 26;
            Div_Name = Div_Name + String.fromCharCode(65 + intTemp);
        }
        ba = ''
        document.all[html_code].insertAdjacentHTML('beforeBegin', '<table id="' + Div_Name + '" cellpadding="0" cellspacing="0" border="0"><tr><td><input type="hidden" value="' + File_Name_Array[2] + '" name="Product_down_file">' + File_Name_Array[1] + '&nbsp;</td><td ><a href="javascript:;" onclick=javascript:document.all("' + Div_Name + '").outerHTML=ba>移除</a>&nbsp;<a href=javascript:open_win("' + File_Name_Array[0] + File_Name_Array[1] + '");>查看档案</a></td></tr></table>');
    }
}

function open_win(http_url) {
    if (http_url != '') {
        window.showModalDialog(http_url, '', 'dialogWidth=665px;dialogHeight=480px;help=no;status=no;');
    }
    else {
        alert('请选择文件!');
    }
}

function open_win_pay(http_url) {
    if (http_url != '') {
        window.showModalDialog(http_url, '', 'dialogWidth=820px;dialogHeight=600px;help=no;status=no;');
    }
    else {
        alert('请选择文件!');
    }
}

function open_win_file(http_url) {
    if (http_url != '') {
        window.open(http_url, '', 'Width=665,Height=480,resizable=yes');
    }
    else {
        alert('请选择文件!');
    }
}

function funEditor(html_code) {
    window.open('/webedit/return.asp?return_name=' + html_code, 'Rate', 'width=590,height=380,left=100,top=50,scrollbars=1')
}


function runCode(obj) {
    var winname = window.open('', "_blank", '');
    winname.document.open('text/html', 'replace');
    winname.document.writeln(obj.value);
    winname.document.close();
}
function bbimg(o) {
    var zoom = parseInt(o.style.zoom, 10) || 100; zoom += event.wheelDelta / 12; if (zoom > 0) o.style.zoom = zoom + '%';
    return false;
}

function Format(num, dotLen) {
    //将num按小数位为dotLen来进行格式化  如无小数位参数则为2位小数
    var dot = 0
    var num1 = 0
    if (typeof dotLen == "undefined" || dotLen == null)
        dot = 2
    else
        dot = dotLen
    if (isNaN(parseFloat(num)))
        return 0
    else
        num1 = parseFloat(num)
    var n1 = Math.pow(10, dot)
    if (n1 == 0)
        var iValue = Math.round(num1)
    else
        var iValue = Math.round(num1 * n1) / n1
    var sValue = iValue.toString();
    if (sValue.indexOf(".") == -1) {
        sValue = sValue + ".00";
    }
    else {
        if (sValue.indexOf(".") == sValue.length - 1) {
            sValue = sValue + "00";
        }
        else if (sValue.indexOf(".") == sValue.length - 2) {
            sValue = sValue + "0";
        }
    }
    return sValue
}


//this function is used to compare two date,author:rautinee

function compareDate(DateOne, DateTwo) {
    var OneMonth = DateOne.substring(5, DateOne.lastIndexOf("-"));
    var OneDay = DateOne.substring(DateOne.length, DateOne.lastIndexOf("-") + 1);
    var OneYear = DateOne.substring(0, DateOne.indexOf("-"));

    var TwoMonth = DateTwo.substring(5, DateTwo.lastIndexOf("-"));
    var TwoDay = DateTwo.substring(DateTwo.length, DateTwo.lastIndexOf("-") + 1);
    var TwoYear = DateTwo.substring(0, DateTwo.indexOf("-"));

    if (Date.parse(OneMonth + "/" + OneDay + "/" + OneYear) > Date.parse(TwoMonth + "/" + TwoDay + "/" + TwoYear)) {
        return true;
    }
    else {
        return false;
    }
}
function MenuOnMouseOver(obj) {
    obj.className = 'menubar_button';
}

function MenuOnMouseOut(obj) {
    obj.className = 'menubar_button_on';
}

function DelData(DeleteUrl) {
    if (confirm("您确定要删除当前记录吗?\n\n注意:删除后不能恢复!")) {
        window.location.href = DeleteUrl;
    }
}

//移动select中选项
function moveUpDown(aim, obj) {//Obj是需用移动的对象;
    //document.my.up.disabled=false;
    //document.my.down.disabled=false; 
    //var Obj=document.my.a; 

    var Obj = document.getElementById(obj);
    if (aim == "up")//如果向上移动;
    {

        if (Obj.length - Obj.selectedIndex == Obj.length) {
            alert("已是最靠上的一项了，无法再向上移动！");
            return;
        }
        else if (Obj.selectedIndex != -1) {
            oldSelected = Obj.selectedIndex;
            oldText = Obj.options[Obj.selectedIndex].text;
            oldValue = Obj.options[Obj.selectedIndex].value;
            Obj.options[Obj.selectedIndex] = new Option(Obj.options[Obj.selectedIndex - 1].text, Obj.options[Obj.selectedIndex - 1].value)
            //当前选择的项值与文字等于该选择上一项的值与文字;
            Obj.options[oldSelected - 1] = new Option(oldText, oldValue);
            //原选择项的上一项的值与文字等于原选择的值与文字;
            Obj.options[oldSelected - 1].selected = true;
            //原选择项的上一项被选中状态;

        }
        else {
            alert("请选择您要移动的一项！"); return;

        }
    }
    else if (aim == "down")//向下移动;
    {
        if (Obj.selectedIndex == -1) {
            alert("请选择您要移动的一项！"); return;
        }
        else if (Obj.length - Obj.selectedIndex == 1) {
            alert("已是最靠下的一项了，无法再向下移动！");
            return;
        }
        else {
            current_ = Obj.selectedIndex;
            current_text = Obj.options[Obj.selectedIndex].text;
            current_value = Obj.options[Obj.selectedIndex].value;
            Obj.options[Obj.selectedIndex] = new Option(Obj.options[Obj.selectedIndex + 1].text, Obj.options[Obj.selectedIndex + 1].value);
            //新建一项，当前选择项值等于当前选择之下一项值;
            //Obj.options[Obj.selectedIndex].text=Obj.options[Obj.selectedIndex+1].text;
            //Obj.options[Obj.selectedIndex].value=Obj.options[Obj.selectedIndex+1].value;
            //得到当前项下一项的值与文字;
            //Obj.options[current_+1]=new Option(current_text,current_value)  
            Obj.options[current_ + 1].text = current_text;
            Obj.options[current_ + 1].value = current_value;
            //原选择中的项的下一项的文字与值分别等于原选择项的值与文字，以实现替换;        
            Obj.options[current_ + 1].selected = true;//选择原选择项的下一项;
        }

    }

}

//选中给定的列表框中所有的项。如果obj为null，则列表框为自身。
function selectAll(obj) {
    if (obj == null) obj = element;
    if (obj.tagName != "SELECT") return;
    var length = obj.options.length;
    for (var i = 0; i < length; i++) {
        obj.options[i].selected = true;
    }
}

function doConfirm(frm) {
    if (confirm('确定要执行操作吗！')) {
        return (true);
    } else {
        return (false);
    }
}
function Check_CheckBox(e) {
    e = getEvent();
    var xObj = e.srcElement || e.target;

    //var xObj = window.event.srcElement;
    if (xObj.tagName == "INPUT") {
        if (xObj.type == "checkbox") {
            var xname = xObj.name;
            var xBoolean = xObj.checked;
            if (xBoolean == true)
                xBoolean = false;
            else
                xBoolean = true;
            for (var i = 0; i < document.getElementsByName(xname).length; i++) {
                document.getElementsByName(xname)[i].checked = xBoolean;

            }
        }
    }
}

function getEvent() {
    var i = 0;
    if (document.all) return window.event;
    func = getEvent.caller;
    while (func != null) {
        var arg0 = func.arguments[0];
        if (arg0) {
            if (arg0.constructor == MouseEvent) {
                return arg0;
            }
        }
        func = func.caller;
    }
    return null;
}
/***********************************************************************
 * 判断一个字符串是否为合法的日期格式：YYYY-MM-DD HH:MM:SS
 * 或 YYYY-MM-DD 或 HH:MM:SS
 */
function isDateStr(ds) {
    parts = ds.split(' ');
    //alert(parts.length);
    switch (parts.length) {
        case 2:
            if (isDatePart(parts[0]) == true && isTimePart(parts[1])) {
                return true;
            } else {
                return false;
            }
        case 1:
            return false;
        default:
            return false;
    }
}

/***********************************************************************
 * 判断一个字符串是否为合法的日期格式：YYYY-MM-DD
 */
function isDatePart(dateStr) {
    var parts;

    if (dateStr.indexOf("-") > -1) {
        parts = dateStr.split('-');
    } else if (dateStr.indexOf("/") > -1) {
        parts = dateStr.split('/');
    } else {
        return false;
    }

    if (parts.length < 3) {
        //日期部分不允许缺少年、月、日中的任何一项
        return false;
    }

    for (i = 0 ; i < 3; i++) {
        //如果构成日期的某个部分不是数字，则返回false
        if (isNaN(parts[i])) {
            return false;
        }
    }

    y = parts[0];//年
    m = parts[1];//月
    d = parts[2];//日

    if (y > 3000) {
        return false;
    }

    if (m < 1 || m > 12) {
        return false;
    }

    switch (d) {
        case 29:
            if (m == 2) {
                //如果是2月份
                if ((y / 100) * 100 == y && (y / 400) * 400 != y) {
                    //如果年份能被100整除但不能被400整除 (即闰年)
                } else {
                    return false;
                }
            }
            break;
        case 30:
            if (m == 2) {
                //2月没有30日
                return false;
            }
            break;
        case 31:
            if (m == 2 || m == 4 || m == 6 || m == 9 || m == 11) {
                //2、4、6、9、11月没有31日
                return false;
            }
            break;
        default:

    }

    return true;
}

/***********************************************************************
 * 判断一个字符串是否为合法的时间格式：HH:MM:SS
 */
function isTimePart(timeStr) {
    var parts;

    parts = timeStr.split(':');

    if (parts.length < 2) {
        //日期部分不允许缺少小时、分钟中的任何一项
        return false;
    }

    for (i = 0 ; i < parts.length; i++) {
        //如果构成时间的某个部分不是数字，则返回false
        if (isNaN(parts[i])) {
            return false;
        }
    }

    h = parts[0];//年
    m = parts[1];//月

    if (h < 0 || h > 23) {
        //限制小时的范围
        return false;
    }
    if (m < 0 || h > 59) {
        //限制分钟的范围
        return false;
    }

    if (parts.length > 2) {
        s = parts[2];//日

        if (s < 0 || s > 59) {
            //限制秒的范围
            return false;
        }
    }

    return true;
}

/***********************************************************************
 * 计算字符个数，包括中文
 */
function countCharacters(str, lengthLimit) {
    var totalCount = 0;
    for (var i = 0; i < str.length; i++) {
        var c = str.charCodeAt(i);
        if ((c >= 0x0001 && c <= 0x007e) || (0xff60 <= c && c <= 0xff9f)) {
            totalCount++;
        }
        else {
            totalCount += 2;
        }

        if (totalCount > lengthLimit) {
            break;
        }
    }
    //alert(totalCount);

    return totalCount;
}