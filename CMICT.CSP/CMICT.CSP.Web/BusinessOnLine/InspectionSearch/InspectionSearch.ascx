<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %> 
<%@ Register Tagprefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register Tagprefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %> 
<%@ Register Tagprefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InspectionSearch.ascx.cs" Inherits="CMICT.CSP.Web.BusinessOnLine.InspectionSearch.InspectionSearch" %>

<script>
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_beginRequest(function () {

    });
    prm.add_endRequest(function (sender, args) {
        args.set_errorHandled(true);
        try {;

            $(".toUpper").blur(function () {
                $(this).val($(this).val().toUpperCase());
            });
        } catch (err) {

        }
    });
</script>

<asp:UpdatePanel runat="server" ID="UpdatePanel1">
    <ContentTemplate>

  

<p class="onlineNote">海关有时会对箱子的查验指令进行更新，可能会导致预约查验日期失效，请17:00以后再复查以确认预约是否成功！我司正在协商解决海关查验指令重复收发导致预约查验日期失效的问题</p>

                    <div class="filterBox">
                        <div class="filterIptBox">
                            <ul class="filterList">
                                <li>
                                    <label class="filterTxt">箱号：</label>
                                    <input class="stepIpt w210 toUpper" type="text" runat="server" id="boxNo"  maxlength="20" />
                                </li>
                                <li>
                                    <label class="filterTxt">提单号：</label>
                                    <input class="stepIpt w210 toUpper" type="text" runat="server" id="BLNo"  maxlength="50" />
                                </li>
                                <li>
                                    <input class="middle" type="checkbox" id="isReserved" runat="server" checked="checked"/>
                                    <label class="filterTxt middle">是否已预约</label>
                                </li>
                                <li class="fLeft">
                                    <div class="operationBox">
                                        <ul class="operationBtnBox">                       
                                            <li><asp:Button CssClass="operationBtn query" ID="btn_Search" runat="server" Text="查询" OnClientClick="layer.load('查询中，请稍候...');" OnClick="btn_Search_Click"  /></li>
                                        </ul>
                                    </div>
                                </li>
                            </ul>
                        </div>                        
                    </div>

                    <div class="cutOffRule clearBoth"></div>
                    
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
                                    <th>废品</th>
                                    <th>冷藏箱</th>
                                    <th>预约查验日期</th>
                                    <th>移箱到位</th>
                                    <th>查验完成</th>
                                    <th class="lastMenu">落场位置</th>
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
                                </tr>
                                </ItemTemplate>
            </asp:Repeater>
                            </tbody>                       
                        </table>
                    </div>

          </ContentTemplate>
</asp:UpdatePanel>