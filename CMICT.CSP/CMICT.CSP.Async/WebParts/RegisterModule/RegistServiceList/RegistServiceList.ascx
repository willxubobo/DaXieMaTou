<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegistServiceList.ascx.cs" Inherits="CMICT.CSP.Async.WebParts.RegisterModule.RegistServiceList.RegistServiceList" %>

<script src="/_layouts/15/CMICT.CSP.Branding/jquery/jquery-1.9.1.min.js"></script>
<script src="/_layouts/15/CMICT.CSP.Branding/jquery/layer/layer.min.js"></script>
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
</style>
<div class="filterBox">
    <div class="filterIptBox">
        <table class="filterTable">
            <tr>
                <td>
                    <div class="operationBox">
                        <ul class="operationBtnBox">
                            <li><a class="operationBtn query fLeft" href="RegisterAdd.aspx" target="_blank">新增</a></li>
                        </ul>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</div>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <div class="resultBox">
            <table class="tableOne tableOneLayout">
                <thead>
                    <tr class="bordered">
                        <th style="width: 60%;">实体名</th>
                        <th style="width: 40%;">操作</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="rptEntityList" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hidName" Value='<%#Eval("ENTITYCODE") %>' runat="server" />
                                    <asp:Label ID="lblEntityName" runat="server" Text='<%#Eval("ENTITYCODE") %>'></asp:Label>
                                </td>
                                <td>
                                    <%-- <a class=" query btn" href="RegisterAdd.aspx?type=U&entity=<%#Eval("ENTITYCODE") %>" target="_blank">修改</a>--%>
                                    
                                    <a class=" query btn" onclick="DeleteEntity('<%#Eval("ENTITYCODE") %>')"  >删除</a>

                                   
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <asp:Label ID="lblnodata" runat="server" Text="暂无数据！" Visible="false" Style="font-size: 12px;"></asp:Label>
        </div>
        <div style="display: none">
            <asp:Button ID="lbtndel" CssClass="lbtndel" OnClick="lbtndel_OnClick" runat="server"></asp:Button>
            <asp:HiddenField ID="hidEntityCode" runat="server" />
            <input id="txtEntityCode" runat="server" type="text" />
        </div>
    </ContentTemplate>
</asp:UpdatePanel>

<script type="text/javascript">
    
    function DeleteEntity(code) {
        $("#<%=this.txtEntityCode.ClientID%>").val(code);
        RegisterButtonConfirm("你确认要删除吗？", 1);
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
</script>
