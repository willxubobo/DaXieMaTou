$(document).ready(function () {

    $(".toUpper").blur(function () {
        $(this).val($(this).val().toUpperCase());
    });

    SP.SOD.executeFunc('sp.js', 'SP.ClientContext', getCurrentUserName);

    $(".tableOne").each(function () {
        $(this).find("tbody tr:odd").children("td").css("background-color", "#f7f7f7");
    });
   
    // 子菜单下拉列表hover效果
    $(document).on("mouseover mouseout",".topNav>li",function(event){
    if(event.type == "mouseover"){
        $(this).addClass("topLinkCur");
        if ($(this).children("a").hasClass("ClickLink")) {
            $(this).find("ul").show();
        };
        //add the width of li to ul
        var liWidth = $(this).width();
        $(this).children('ul').eq(0).css("width",liWidth);
     }else if(event.type == "mouseout"){
        $(this).removeClass("topLinkCur");
        $(this).find("ul").hide();
     }
    }); 

    $(document).on("click",".mainLink",function(){
        $(".mainLink").removeClass("active");
        $(this).addClass("active");
    });

    $(document).on("mouseover mouseout",".mainNav li",function(event){
    if(event.type == "mouseover"){
        $(this).siblings().children(".mainLink").removeClass("hoverOn");
        $(this).children(".mainLink").addClass("hoverOn");
        $(this).siblings("li").find("ul").hide();
        $(this).children(".mainLink").next("ul").show();
     }else if(event.type == "mouseout"){
       $(this).children(".mainLink").removeClass("hoverOn");
        $(this).children(".mainLink").next("ul").hide(); 
     }
    });



    $(".subLink").click(function (e) {
        $(".subLink").removeClass("subLinkOn");
        $(this).addClass("subLinkOn");
        $(this).parents(".subNav").hide();
        var ev = e || window.event;
        if (ev.stopPropagation) {
            ev.stopPropagation();
        }
        else if (window.event) {
            window.event.cancelBubble = true;//兼容IE
        }
    });
    $(".subLink").hover(
        function () {
            $(".subLink").removeClass("subLinkOn");
            $(this).addClass("subLinkOn");
        },
        function () {
            $(this).removeClass("subLinkOn");
        }
    );

    $(".choiceAreaList li:even").css("background-color", "#f7f7f7");

    // $(".contentBox .odd").hover(
    //     function () {
    //         $(".contentBox .odd").removeClass("oddHoveron");
    //         $(this).addClass("oddHoveron");
    //     },
    //     function () {
    //         $(this).removeClass("oddHoveron");
    //     }
    // );
    // $(".contentBox .even").hover(
    //     function () {
    //         $(".contentBox .even").removeClass("evenHoveron");
    //         $(this).addClass("evenHoveron");
    //     },
    //     function () {
    //         $(this).removeClass("evenHoveron");
    //     }
    // );

    //2015-10-13  Frank 添加用户登录后自动去除"id=s4-workspace"标签上的width和height样式
    $("#s4-workspace").css("width", "auto !important");
    $("#s4-workspace").css("height", "auto !important");

// 2015-09-21  --Yorick  部门收缩菜单
//$(document).on("click",".impowerUserList a",function(){  
//    // $(".impowerUserList a").removeClass("clickOn");
//    // $(this).addClass("clickOn"); 
//    $(this).toggleClass("clickBgOpen"); 
//    $(".impowerUserList ul:last a").addClass("bgNone");
//    $(this).next("ul").toggle();
//});


// 2015-09-29   --Yorick  账户登陆下拉菜单
//$(".ClickLink").click(function (e) {

//    $(this).parent("li").siblings("li").find("ul").hide();
//    $(this).next("ul").toggle();
//    $(this).children(".loginLink").toggleClass("loginLinkOn");
//    var ev = e || window.event;
//    if (ev.stopPropagation) {
//        ev.stopPropagation();
//    }
//    else if (window.event) {
//        window.event.cancelBubble = true;//兼容IE
//    }
//});
//document.onclick = function () {
//    $(".loginOperation").hide();
//}
    // 2015-10-10 --Yorick  表格hover或者click高亮
$(document).on("click", ".tableOne tbody td", function () {
    $(this).parent("tr").siblings().children("td").removeClass("tableOneTrClickBg");
    $(this).addClass("tableOneTrClickBg");
    $(this).siblings("td").addClass("tableOneTrClickBg");
});
$(document).on("mouseover mouseout", ".tableOne tbody td", function (event) {

    if (event.type == "mouseover") {
        $(this).addClass("tableOneTrHoverBg");
        $(this).siblings("td").addClass("tableOneTrHoverBg");
    } else if (event.type == "mouseout") {
        $(this).removeClass("tableOneTrHoverBg");
        $(this).siblings("td").removeClass("tableOneTrHoverBg");
    }
});
    // 2015-10-10 --Yorick  表格hover或者click高亮
$(document).on("click", ".tableInner td", function () {
    $(this).parent("tr").siblings().children("td").removeClass("tableOneTrClickBg");
    $(this).addClass("tableOneTrClickBg");
    $(this).siblings("td").addClass("tableOneTrClickBg");
});
$(document).on("mouseover mouseout", ".tableInner td", function (event) {

    if (event.type == "mouseover") {
        $(this).addClass("tableOneTrHoverBg");
        $(this).siblings("td").addClass("tableOneTrHoverBg");
    } else if (event.type == "mouseout") {
        $(this).removeClass("tableOneTrHoverBg");
        $(this).siblings("td").removeClass("tableOneTrHoverBg");
    }
});
});
$(window).load(function() {
    //index banner img 居中
    var contentWidth = $(".contentBg").width();
    var contentimgWidth = $(".contentBg img").width();
    var contentleftWidth = -(contentimgWidth-contentWidth)/2;
    $(".contentBg").each(function(){
        $(".contentBg img").css("left",contentleftWidth);
    });

    //index header img 居中
    var headertWidth = $(".headerBg").width();
    var headerimgWidth = $(".headerBg img").width();
    var headerleftWidth = -(headerimgWidth-headertWidth)/2;
    $(".headerBg").each(function(){
        $(".headerBg img").css("left",headerleftWidth);
    });
    $(window).resize(function(){               //窗口变化banner img居中  footer贴底边显示
        //index banner img 居中
        var contentWidth = $(".contentBg").width();
        var contentimgWidth = $(".contentBg img").width();
        var contentleftWidth = -(contentimgWidth-contentWidth)/2;
        $(".contentBg").each(function(){
            $(".contentBg img").css("left",contentleftWidth);
        });

        //index header img 居中
        var headertWidth = $(".headerBg").width();
        var headerimgWidth = $(".headerBg img").width();
        var headerleftWidth = -(headerimgWidth-headertWidth)/2;
        $(".headerBg").each(function(){
            $(".headerBg img").css("left",headerleftWidth);
        });

        //页面小于屏幕高度footer贴底边显示
        var pageHeight = $(window).height();
        var htmlHeight = $("html").height();
        if (htmlHeight<pageHeight) {
            $(".footer").addClass("bottom");
        }else{
            $(".footer").removeClass("bottom");
        };

    });

    //页面小于屏幕高度footer贴底边显示
    var pageHeight = $(window).height();
    var htmlHeight = $("html").height();
    if (htmlHeight<pageHeight) {
        $(".footer").addClass("bottom");
    }else{
        $(".footer").removeClass("bottom");
    };

    // 2015-09-15 --Yorick    
    //$(".tableInner").each(function () {
    //    $(this).find("tr:odd").children("td").css("background-color", "#f7f7f7");
    //    $(this).find("tr:even").children("td").css("background-color", "#ffffff");
    //});
});

//分页用 －will.xu
function changesize(obj) {
    $(".hidpagesize").val(obj);
    $(".btnhsearch").click();
}
function setvaluesel(obj) {
    $(".pcount").val(obj);
}

var currentUserLoginName;
var currentUserName;
var context;
var user;
var web;

function getCurrentUserName() {
    context = new SP.ClientContext.get_current();
    web = context.get_web();
    user = web.get_currentUser();
    context.load(user);
    context.executeQueryAsync(onGetSiteUrlSuccess, onGetSiteUrlFail);
}

function onGetSiteUrlSuccess() {
    currentUserLoginName = user.get_loginName();
    currentUserName = user.get_title();
}

function onGetSiteUrlFail(sender, args) {

}


/********************************/

//add will.xu 初始化表格交错行样式
function inittableonetrstyle() {
    $(".tableOne").each(function () {
        $(this).find("tbody tr:odd").children("td").css("background-color", "#eeeeee");
        $(this).find("tbody tr:even").children("td").css("background-color", "#ffffff");
    });
    //$(".tableInner").each(function () {
    //    $(this).find("tr:odd").children("td").css("background-color", "#f7f7f7");
    //    $(this).find("tr:even").children("td").css("background-color", "#ffffff");
    //});
}
//add will.xu 页面刷新执行底部
function footerstyle() {
    //页面小于屏幕高度footer贴底边显示
    var pageHeight = $(window).height();
    var htmlHeight = $("html").height();
    if (htmlHeight < pageHeight) {
        $(".footer").addClass("bottom");
    } else {
        $(".footer").removeClass("bottom");
    };
}