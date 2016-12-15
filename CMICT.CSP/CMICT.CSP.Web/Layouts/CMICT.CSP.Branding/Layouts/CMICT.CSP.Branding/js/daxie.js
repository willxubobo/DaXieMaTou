$(document).ready(function(){

	$(".tableOne tbody tr:odd").children("td").css("background-color","#f7f7f7");

    $(".mainLink").click(function(){
    	$(this).parent("li").siblings("li").find("ul").hide();
    	$(this).next("ul").toggle();
    	$(".mainLink").removeClass("active");
    	$(this).addClass("active");
    });
    $(".mainLink").hover(
    	function(){
    		$(".mainLink").removeClass("hoverOn");
    	    $(this).addClass("hoverOn");
    	},
    	function(){
            $(".mainLink").removeClass("hoverOn");
    	}
    );

    $(".subLink").click(function(){
    	$(".subLink").removeClass("subLinkOn");
    	$(this).addClass("subLinkOn");
    	$(this).parents(".subNav").hide();
    });
    $(".subLink").hover(
    	function(){
    		$(".subLink").removeClass("subLinkOn");
    	    $(this).addClass("subLinkOn");
    	},
    	function(){
    		$(this).removeClass("subLinkOn");
    	}
    );

});

//分页用 －will.xu
function changesize(obj) {
    $(".hidpagesize").val(obj);
    $(".operationBtn").click();
}
function setvaluesel(obj) {
    $(".pcount").val(obj);
}
/********************************/