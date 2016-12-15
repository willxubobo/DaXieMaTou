<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InspectionBooking.ascx.cs" Inherits="CMICT.CSP.Web.BusinessOnLine.InspectionBooking.InspectionBooking" %>
<script>
    function ensureBooking() {
        $("#bookingDiv").show();
        $(".shade").show();
    }
    function closeBooking() {
        $("#bookingDiv").hide();
        $(".shade").hide();
    }

    function ClearInfo()
    {
        $(".txtlinkman").val("");
        $(".txtphone").val("");
        return true;
    }


    function bookingClick() {
        if ($(".txtlinkman").val() == "" || $(".txtphone").val() == "")
        {
            layer.alert('请填写联系人员和联系电话！');
            return false;
        }
        reg = /^(\d{3,4}-)?[1-9]\d{6,7}(-(\d{3,}))?$/;
        reg2 = /^1\d{10}$/;
        var tel = $(".txtphone").val();
        if (!reg.test(tel) && !reg2.test(tel))
        {
            layer.alert('请填写正确的联系电话！');
            return false;
        }
        return true;
    }

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_beginRequest(function () {

    });
    prm.add_endRequest(function (sender, args) {
        args.set_errorHandled(true);
        try {
            $(".toUpper").blur(function () {
                $(this).val($(this).val().toUpperCase());
            });
        } catch (err) {

        }
    });

</script>

<asp:UpdatePanel runat="server" ID="UpdatePanel1">
    <ContentTemplate>
<div class="filterBox">
    <div class="filterIptBox">
        <table class="filterTable">
            <tr>
                                    <td class="filterTxtWidth">
                                        <label class="filterIptTxt fLeft">客户账户：</label>
                                    </td>
                                    <td>
                                        <input class="stepIpt widthMax fLeft grayBg" type="text" id="userName" runat="server" disabled="disabled" style="float:left"/>
                                    </td>
                                    <td class="filterTxtWidth">
                                        <label class="filterIptTxt fLeft">预约单位：</label>
                                    </td>
                                    <td colspan="3">
                                        <input class="stepIpt widthMax fLeft grayBg" type="text" id="txtcompany" runat="server" disabled="disabled"/>
                                    </td>
                                    <td class="filterTxtWidth">
                                        <label class="filterIptTxt fLeft">联系人：</label>
                                    </td>
                                    <td>
                                        <input class="stepIpt widthMax fLeft grayBg" type="text" id="linkMan" runat="server" disabled="disabled"/>
                                    </td>
                                                                      
                                </tr>
                                <tr>
                                    <td>
                                        <label class="filterIptTxt fLeft">联系电话：</label>
                                    </td>
                                    <td>
                                        <input class="stepIpt widthMax fLeft grayBg" type="text" id="phone" runat="server" disabled="disabled"/>
                                    </td> 
                                    <td>
                                        <label class="filterIptTxt fLeft">可预约日期：</label>
                                    </td>
                                    <td width="146">
                                        <input class="stepIpt width60 fLeft grayBg" type="text" id="inspectionTime" runat="server" disabled="disabled"/>
                    <input class="stepIpt width38 fRight grayBg" type="text" id="inspectionDay" runat="server" disabled="disabled"/>
                                    </td>
                                    <td width="115">
                                        <label class="filterIptTxt fLeft">预约开始时间：</label>
                                    </td>
                                    <td>
                                        <input class="stepIpt widthMax fLeft grayBg" type="text" id="beginTime" runat="server"  disabled="disabled"/>
                                    </td>
                                    <td>
                                        <label class="filterIptTxt fLeft">预约结束时间：</label>
                                    </td>
                                    <td>
                                        <input class="stepIpt widthMax fLeft grayBg" type="text" id="endTime" runat="server"  disabled="disabled"/>
                                    </td>                                       
                                </tr>
                                <tr class="tableHr">
                                    <td>
                                        <label class="filterIptTxt fLeft">箱号：</label>
                                    </td>
                                    <td>
                                        <input class="stepIpt widthMax fLeft toUpper" type="text" runat="server" id="boxNo" maxlength="20"  />
                                    </td>                                     
                                    <td>
                                        <label class="filterIptTxt fLeft">提单号：</label>
                                    </td>
                                    <td>
                                        <input class="stepIpt widthMax fLeft toUpper" type="text" runat="server" id="BLNo"  style="float:left" maxlength="50"/>
                                    </td>
                                    <td colspan="2">
                                        <div class="noteTips fLeft" style="display:none;">
                                            <input class="middle" type="checkbox" id="isReserved" runat="server"  />
                                            <label class="filterTxt middle">是否已预约</label>
                                            </div>
                                    </td>
                                    <td colspan="2">
                                        <div class="operationBox">
                                            <ul class="operationBtnBox">                       
                                                <li><asp:Button CssClass="operationBtn query" ID="btn_Search" runat="server" Text="查询" OnClick="btn_Search_Click" OnClientClick="layer.load('查询中，请稍候...');" /></li>
                                                <li><asp:Button CssClass="operationBtn export" ID="btn_Booking" runat="server" Text="预约" OnClick="btn_PreBooking_Click" OnClientClick="ClearInfo()" Visible="false" /></li>
                                            </ul>
                                        </div>
                                    </td>
                                </tr>
                                <tr style="display:none;">
                                    <td colspan="8">
                                        <div class="noteTips fLeft">
                                            <span class="onlineNote noteLayout">只能按提单号预约，一次性预约该提单号下所有海关和联合查验箱</span><br/>
                                            <span class="onlineNote noteLayout">海关有时会对箱子的查验指令进行更新，可能会导致预约查验日期失效，请17:00以后再复查以确认预约是否成功！我司正在协商解决海关查验指令重复收发导致预约查验日期失效的问题</span>
                                        </div>
                                    </td>
                                    
                                </tr>
        </table>
    </div>
</div>
<div class="cutOffRule"></div>

<div class="resultBox">

    <table class="tableOne onlineTable">
        <asp:Repeater ID="DataSourceList" runat="server">
            <HeaderTemplate>
                <thead>
                    <tr>
                        <th>箱号</th>
                        <th>尺寸</th>
                        <th>箱型</th>
                        <th>提单号</th>
                        <th>进港类型</th>
                        <th>船名</th>
                        <th>航次</th>
                        <th>查验类型</th>
                        <th>是否废品</th>
                        <th>是否冷藏</th>
                        <th class="lastMenu">是否已预约</th>
                    </tr>
                </thead>
                <tbody>
            </HeaderTemplate>
            <ItemTemplate>
                <tr>
                    <td><%#Eval("CONTAINERNO") %></td>  
                    <td><%#Eval("CONTAINERSIZE") %></td>
                    <td><%#Eval("CONTAINERTYPE") %></td>
                    <td><%#Eval("BLNO") %></td>
                    <td><%#Eval("INAIM") %></td>
                    <td><%#Eval("ENVESSELNAME") %></td>
                    <td><%#Eval("VOYAGE") %></td>
                    <td><%#Eval("TYPE") %></td>
                    <td><%#Eval("ISWASTER") %></td>
                    <td><%#Eval("ISREEFER") %></td>
                    <td><%#Eval("RESERVED") %></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Repeater ID="DataSourceList2" runat="server">
                <HeaderTemplate>
                            <thead>
                                <tr>
                                    <th>箱号</th>
                                    <th>尺寸</th>
                                    <th>箱型</th>
                                    <th>提单号</th>
                                    <th>进港类型</th>
                                    <th>船名</th>
                                    <th>航次</th>
                                    <th>查验类型</th>
                                    <th>废品</th>
                                    <th>冷藏箱</th>
                                    <th>预约查验日期</th>
                                    <th>移箱到位</th>
                                    <th>查验完成</th>
                                    <th>落场位置</th>
                                    <th class="lastMenu">是否已预约</th>
                                </tr>
                            </thead>
                            <tbody>
                                </HeaderTemplate>
                <ItemTemplate>
                                <tr>
                                    <td><%#Eval("CONTAINERNO") %></td>
                    <td><%#Eval("CONTAINERSIZE") %></td>
                    <td><%#Eval("CONTAINERTYPE") %></td>
                    <td><%#Eval("BLNO") %></td>
                    <td><%#Eval("INAIM") %></td>
                    <td><%#Eval("ENVESSELNAME") %></td>
                    <td><%#Eval("VOYAGE") %></td>
                    <td><%#Eval("TYPE") %></td>
                    <td><%#Eval("ISWASTER") %></td>
                    <td><%#Eval("ISREEFER") %></td>
                                    <td><%#Eval("RESERVEDATE") %></td>
                                    <td><%#Eval("ISPREPARE") %></td>
                                    <td><%#Eval("ISCHECKED") %></td>
                                    <td><%#Eval("PLANYARDCELL") %></td>
                                    <td><%#Eval("RESERVED") %></td>
                                </tr>
                                </ItemTemplate>
            </asp:Repeater>
        </tbody>
    </table>
</div>


<!-- 弹窗 Start -->
<div class="shade"></div>

<div class="toolTipBox toolTipLayout" id="bookingDiv">
    <div class="onlineappointBox">
        <p class="onlineBoxTitle">海关查验预约清单</p>
        <table class="onlineOperate">
            <tr>
                <td width="50%">提单号：<asp:label ID="lblno" runat="server"/></td>
                <td width="50%">箱数量：<asp:label ID="lblCount" runat="server"/></td>
            </tr>
            <tr>
                <td colspan="2">箱号：<asp:label ID="lblContainer" runat="server"/></td>
            </tr>
            <tr>
                <td colspan="2">预约查验日期：<asp:label ID="lblDate" runat="server"/></td>
            </tr>
            <tr>
                <td colspan="2">预约查验单位：<asp:label ID="lblCompany" runat="server"/></td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="cutOffRule"></div>
                </td>
            </tr>
            <tr>
                <td>
                    <label class="stepIptTxt">联系人员：</label>
                    <input class="stepIpt w180 txtlinkman" type="text" id="txtlinkman" name="bookingMan" runat="server" />
                    <span class="mustIcon">*</span>
                </td>
                <td>
                    <label class="stepIptTxt">联系电话：</label>
                    <input class="stepIpt w180 txtphone" type="text" id="txtphone" name="bookingPhone" runat="server" />
                    <span class="mustIcon">*</span>
                </td>
            </tr>
        </table>

        <div class=""></div>
        <div class="operateBtnBox">
            <asp:LinkButton runat="server" ID="btnbooking" CssClass="guideBtn finishLink" Text="预约" OnClick="btn_Booking_Click" OnClientClick="return bookingClick()"></asp:LinkButton>
            
            <%--<a class="guideBtn finishLink" href="#" onclick="checkBooking()">预约</a>--%>
            <a class="guideBtn prevLink" href="#" onclick="closeBooking()">取消</a>
            
        </div>

    </div>
</div>

<!-- 弹窗 End -->
              </ContentTemplate>
</asp:UpdatePanel>