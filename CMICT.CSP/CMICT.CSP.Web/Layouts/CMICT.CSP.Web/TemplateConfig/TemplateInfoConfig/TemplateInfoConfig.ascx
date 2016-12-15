<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TemplateInfoConfig.ascx.cs" Inherits="CMICT.CSP.Web.TemplateConfig.TemplateInfoConfig.TemplateInfoConfig" %>




    <div class="progressBox">
        <ul class="progressStepBox">
            <li>
                <div class="stepBox stepcurrent">
                    <p>1.基本信息配置</p>
                    <span></span>
                </div>
            </li>
            <li>
                <div class="stepBox">
                    <p>2.配置数据源</p>
                    <span></span>
                </div>
            </li>
            <li>
                <div class="stepBox">
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

    <div class="cutOffRule"></div>

    <div class="stepOperateBox">
        <table class="tableTwo">
            <tr>
                <td>
                    <label class="stepIptTxt">模板名称：</label></td>
                <td>
                    <input class="stepIpt w210" type="text" id="txtTemplateName" runat="server" />
                    <span class="mustIcon">*</span>
                </td>
            </tr>
            
            <tr>
                <td>
                    <label class="stepIptTxt">模板描述：</label></td>
                <td>
                    <textarea class="stepTextarea" id="txtTemplateDesc" runat="server"></textarea></td>
            </tr>
            <tr>
                <td>
                    <label class="stepIptTxt">每页条数：</label></td>
                <td>
                    <input class="stepIpt w210" type="text" id="txtPageSize" runat="server" onkeyup="this.value=this.value.replace(/\D/g,'')" onafterpaste="this.value=this.value.replace(/\D/g,'')" />
                    <span class="mustIcon">*</span>
                </td>
            </tr>
           
            <tr id="trpage" runat="server" visible="false">
                <td>
                    <label class="stepIptTxt">关联页面：</label></td>
                <td>
                    <input class="stepIpt w210 fLeft" type="text" id="txtPageInfo" runat="server" />
                    <button class="addBtn fLeft"></button>
                </td>
            </tr>
            <tr>
                <td>
                    <label class="stepIptTxt">是否禁用：</label></td>
                <td>
                    <input class="stepCheckBox" type="checkbox" id="chkDisabled" runat="server" />
                    <label class="checkTxt">禁用</label>
                    <input type="hidden" id="hidDisabled" runat="server" class="hidDisabled" />
                </td>
            </tr>
        </table>
        <div class="cutOffRule"></div>
        <div class="operateBtnBox operateBtnLayout">
            <asp:LinkButton ID="lbtnNext" runat="server" CssClass="guideBtn nextLink" OnClick="lbtnNext_Click">下一步</asp:LinkButton>
        </div>
    </div>


<input type="hidden" id="hidOperType" runat="server" class="hidOperType" />
