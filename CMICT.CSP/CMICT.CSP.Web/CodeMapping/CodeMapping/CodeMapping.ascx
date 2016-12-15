<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages" Assembly="Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CodeMapping.ascx.cs" Inherits="CMICT.CSP.Web.CodeMapping.CodeMapping.CodeMapping" %>

<%@ Register Assembly="AspNetPager, Version=7.4.5.0, Culture=neutral, PublicKeyToken=fb0a0fe055d40fd4" Namespace="Wuqi.Webdiyer" TagPrefix="webdiyer" %>

<script type="text/javascript" src="/_layouts/15/CMICT.CSP.Branding/js/CodeMapping.js"></script>

<asp:UpdatePanel ID="updatePanel" runat="server">
    <ContentTemplate>
         <asp:Button runat="server" CssClass="interfaceupload" OnClientClick="layer.load('正在导入，请稍候...');" ID="btninterfload" OnClick="btninterfload_Click" Style="display: none;" />
        <div class="filterBox">
            <div class="filterIptBox">
                <ul class="filterList">
                    <li>
                        <label class="filterTxt fLeft">目标公司名称：</label>
                        <input id="txtTargetCompanyName" class="stepIpt w165 fLeft" type="text" runat="server" />
                    </li>
                    <li>
                        <label class="filterTxt fLeft">语义内容：</label>
                        <input id="txtSemanticContent" class="stepIpt w165 fLeft" type="text" runat="server" />
                    </li>
                    <li>
                        <div class="fRight">
                            <label class="filterTxt">业务代码：</label>
                            <input id="txtBusinessCode" class="stepIpt w165" type="text" runat="server" />
                        </div>
                    </li>
                    <li>
                        <label class="filterTxt fLeft">目标客户代码：</label>
                        <input id="txtCustomerCode" class="stepIpt w165 fLeft" type="text" runat="server" />
                    </li>
                    <li>
                        <label class="filterTxt fLeft">我方代码：</label>
                        <input id="txtCMICTCode" class="stepIpt w165 fLeft" type="text" runat="server" />
                    </li>
                    <li>
                        <div class="operationBox">
                            <ul class="operationBtnBox">
                                <li>
                                    <%--<a class="operationBtn query" href="javascript:;" onclick="SearchCodeMap();">查询</a>--%>
                                    <asp:Button class="operationBtn query searchCode btnhsearch" OnClientClick="layer.load('查询中，请稍后...');" runat="server" ID="btnSearch" OnClick="btnSearch_Click" Text="查询"></asp:Button></li>
                                <li><a class="operationBtn query fLeft" onclick="OpenInterfaceImport()" href="javascript:void(0)">接口导入</a></li>
                            </ul>
                        </div>
                    </li>
                </ul>
            </div>
        </div>

        <div class="resultBox">
            <table id="tbCodeMapping" class="tableFour tableOneLayoutNew">
                <asp:Repeater ID="CodeMapList" runat="server">
                    <HeaderTemplate>
                        <thead>
                            <tr>
                                <th>
                                    <input id="cboCodeCheckAll" type="checkbox" /></th>
                                <th>目标公司名称</th>
                                <th>语义内容</th>
                                <th>业务代码</th>
                                <th>目标客户代码</th>
                                <th>目标客户代<br />
                                    码中文描述</th>
                                <th>目标客户代<br />
                                    码英文描述</th>
                                <th>我方代码</th>
                                <th>我方代码中文描述</th>
                                <th>我方代码英文描述</th>
                                <th>生效时间</th>
                                <th>失效时间</th>
                                <th class="lastMenu">操作</th>
                            </tr>
                        </thead>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tbody>
                            <tr>
                                <td>
                                    <input type="checkbox" /></td>
                                <td title="<%# Eval("CustomerName")%>"><%# Eval("CustomerName").ToString().Length>6?Eval("CustomerName").ToString().Substring(0,6)+"..":Eval("CustomerName") %></td>
                                <td title="<%# Eval("SemanticDesc")%>"><%# Eval("SemanticDesc").ToString().Length>6?Eval("SemanticDesc").ToString().Substring(0,6)+"..":Eval("SemanticDesc") %></td>
                                <td title="<%# Eval("BusinessCode")%>"><%# Eval("BusinessCode").ToString().Length>6?Eval("BusinessCode").ToString().Substring(0,6)+"..":Eval("BusinessCode") %></td>
                                <td title="<%# Eval("TargetCode")%>"><%# Eval("TargetCode").ToString().Length>6?Eval("TargetCode").ToString().Substring(0,6)+"..":Eval("TargetCode") %></td>
                                <td title="<%# Eval("TargetCodeCnDesc")%>"><%# Eval("TargetCodeCnDesc").ToString().Length>6?Eval("TargetCodeCnDesc").ToString().Substring(0,6)+"..":Eval("TargetCodeCnDesc") %></td>
                                <td title="<%# Eval("TargetCodeEnDesc")%>"><%# Eval("TargetCodeEnDesc").ToString().Length>6?Eval("TargetCodeEnDesc").ToString().Substring(0,6)+"..":Eval("TargetCodeEnDesc") %></td>
                                <td title="<%# Eval("CMICTCode")%>"><%# Eval("CMICTCode").ToString().Length>6?Eval("CMICTCode").ToString().Substring(0,6)+"..":Eval("CMICTCode") %></td>
                                <td title="<%# Eval("CMICTCodeCnDesc")%>"><%# Eval("CMICTCodeCnDesc").ToString().Length>10?Eval("CMICTCodeCnDesc").ToString().Substring(0,10)+"..":Eval("CMICTCodeCnDesc") %></td>
                                <td title="<%# Eval("CMICTCodeEnDesc")%>"><%# Eval("CMICTCodeEnDesc").ToString().Length>10?Eval("CMICTCodeEnDesc").ToString().Substring(0,10)+"..":Eval("CMICTCodeEnDesc") %></td>
                                <td title="<%# Eval("StartDate")%>"><%# Eval("StartDate") %></td>
                                <td title="<%# Eval("ExpireDate")%>"><%# Eval("ExpireDate") %></td>
                                <td id="tdGuid" style="display: none"><%#Eval("MappingID") %></td>
                                <td id="tdCustomerID" style="display: none"><%#Eval("CustomerID") %></td>
                                <td id="tdCustomerDesc" style="display: none"><%#Eval("CustomerDesc") %></td>
                                <td id="tdBusinessCodeDesc" style="display: none"><%#Eval("BusinessCodeDesc") %></td>
                                <td id="tdBusinessTranslation" style="display: none"><%#Eval("BusinessTranslation") %></td>
                                <td id="tdAuthor" style="display: none"><%#Eval("Author") %></td>
                                <td id="tdCreated" style="display: none"><%#Eval("Created") %></td>
                                <td>
                                    <a class="operateBtn view" href="javascript:;" title="查看" onclick="ViewCodeMap(this);"></a>
                                    <a class="operateBtn edit" href="javascript:;" title="编辑" onclick="EditCodeMap(this);"></a>
                                    <a class="operateBtn remove" href="javascript:;" title="删除" onclick="DeleteCodeMap(this);"></a>
                                </td>
                            </tr>
                        </tbody>
                    </ItemTemplate>
                </asp:Repeater>
            </table>
            <div class="pageBox">
                <webdiyer:AspNetPager ID="AspNetPager1" runat="server" PageSize="20"
                    HorizontalAlign="right" Width="100%" NumericButtonCount="5" ShowMoreButtons="true"
                    CssClass="pageList" AlwaysShow="true"
                     FirstPageText="首页" LastPageText="尾页" NextPageText="下一页"
                    PrevPageText="上一页" SubmitButtonText="Go" SubmitButtonClass="gostyle"
                    CustomInfoStyle="font-size:14px;text-align:right;padding-top: 2px;"
                    InputBoxStyle="width:25px; border:1px solid #999999; text-align:center; "
                    TextBeforeInputBox="转到第" TextAfterInputBox="页 " PageIndexBoxType="TextBox"
                    ShowPageIndexBox="Always" TextAfterPageIndexBox="页"
                    TextBeforePageIndexBox="转到" Font-Size="14px"
                    ShowCustomInfoSection="Right" CustomInfoSectionWidth="15%"
                    PagingButtonSpacing="3px"
                    CustomInfoHTML="共<font color='#ff0000'>%RecordCount%</font>条  每页显示<select id='pcount' class='default pcount' onchange='changesizecodemap(this.options[this.options.selectedIndex].value)'><option value='5'>5</option><option value='10'>10</option><option value='15'>15</option><option value='20' selected>20</option><option value='30'>30</option><option value='40'>40</option><option value='50'>50</option></select></div>"
                    OnPageChanged="AspNetPager1_PageChanged">
                </webdiyer:AspNetPager>
            </div>
        </div>

        <input type="hidden" id="hidpagesize" runat="server" value="20" class="hidpagesize" />
    </ContentTemplate>
</asp:UpdatePanel>
<div class="operateBtnBox csOperateStyle">
    <a class="largeButton" href="javascript:;" onclick="AddCodeMap();">添加</a>
    <a class="largeButton" href="javascript:;" onclick="BatchDeleteCodeMap();">批量删除</a>
    <a class="largeButton" href="javascript:;" onclick="OpenBatchImport();">批量导入</a>
</div>


<!-- 弹窗 Start -->
<div id="divCodeMapping" style="display: none" class="mappingBox addRelationship">
    <!-- 添加、编辑映射关系 -->
    <div class="adminBox">
        <p class="mappingTitle">添加/编辑 映射关系</p>
        <table class="mappingTable">
            <tr>
                <td width="165">
                    <span class="mappingTxt">客户公司代码</span>
                    <span class="mustIcon">*</span>
                </td>
                <td>
                    <input id="txtChildCustomerID" class="stepIpt widthMax" type="text" maxlength="50" title="客户公司代码~50:^[A-Za-z0-9,.\u4e00-\u9fa5]+$!" onchange="deoxidizevalidate(this)"></td>
                <td>
                    <div id="divtxtChildCustomerID"></div>
                </td>
                <td width="156">
                    <span class="mappingTxt">客户公司名称</span>
                    <span class="mustIcon">*</span>
                </td>
                <td>
                    <input id="txtChildCustomerName" class="stepIpt widthMax" type="text" maxlength="50" title="客户公司名称~50:^[A-Za-z0-9,.\u4e00-\u9fa5]+$!" onchange="deoxidizevalidate(this)"></td>
                <td>
                    <div id="divtxtChildCustomerName"></div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="mappingTxt">客户公司说明</span>
                    <span class="mustIcon">*</span>
                </td>
                <td colspan="4">
                    <input id="txtChildCustomerDesc" class="stepIpt widthMax" type="text" maxlength="200" title="客户公司说明~200:^[A-Za-z0-9,.\u4e00-\u9fa5]+$!" onchange="deoxidizevalidate(this)"></td>
                <td>
                    <div id="divtxtChildCustomerDesc"></div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="mappingTxt">业务代码</span>
                    <span class="mustIcon">*</span>
                </td>
                <td>
                    <input id="txtChildBusinessCode" class="stepIpt widthMax" type="text" maxlength="50" title="业务代码~50:^[A-Za-z0-9,.\u4e00-\u9fa5]+$!" onchange="deoxidizevalidate(this)"></td>
                <td>
                    <div id="divtxtChildBusinessCode"></div>
                </td>
                <td>
                    <span class="mappingTxt">语义内容</span>
                    <span class="mustIcon">*</span>
                </td>
                <td>
                    <input id="txtChildSemnaticDesc" class="stepIpt widthMax" type="text" maxlength="200" title="语义内容~200:^[A-Za-z0-9,.\u4e00-\u9fa5]+$!" onchange="deoxidizevalidate(this)"></td>
                <td>
                    <div id="divtxtChildSemnaticDesc"></div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="mappingTxt">业务代码说明</span>
                    <span class="mustIcon">*</span>
                </td>
                <td>
                    <input id="txtChildBusinessCodeDesc" class="stepIpt widthMax" type="text" maxlength="500" title="业务代码说明~500:^[A-Za-z0-9,.\u4e00-\u9fa5]+$!" onchange="deoxidizevalidate(this)"></td>
                <td>
                    <div id="divtxtChildBusinessCodeDesc"></div>
                </td>
                <td>
                    <span class="mappingTxt">业务翻译说明</span>
                    <span class="mustIcon">*</span>
                </td>
                <td>
                    <input id="txtChildBusinessTranslation" class="stepIpt widthMax" type="text" maxlength="500" title="业务翻译说明~500:^[A-Za-z0-9,.\u4e00-\u9fa5]+$!" onchange="deoxidizevalidate(this)"></td>
                <td>
                    <div id="divtxtChildBusinessTranslation"></div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="mappingTxt">客户代码</span>
                    <span class="mustIcon">*</span>
                </td>
                <td colspan="4">
                    <input id="txtChildTargetCode" class="stepIpt widthMax" type="text" maxlength="50" title="客户代码~50:^[A-Za-z0-9,.\u4e00-\u9fa5]+$!" onchange="deoxidizevalidate(this)"></td>
                <td>
                    <div id="divtxtChildTargetCode"></div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="mappingTxt">客户代码英文描述</span>
                    <span class="mustIcon">*</span>
                </td>
                <td colspan="4">
                    <input id="txtChilTargetCodeEnDesc" class="stepIpt widthMax" type="text" maxlength="500" title="客户代码英文描述~500:^[A-Za-z0-9,.\u4e00-\u9fa5]+$!" onchange="deoxidizevalidate(this)"></td>
                <td>
                    <div id="divtxtChilTargetCodeEnDesc"></div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="mappingTxt">客户代码中文描述</span>
                    <span class="mustIcon">*</span>
                </td>
                <td colspan="4">
                    <input id="txtChildTargetCodeCnDesc" class="stepIpt widthMax" type="text" maxlength="500" title="客户代码中文描述~500:^[A-Za-z0-9,.\u4e00-\u9fa5]+$!" onchange="deoxidizevalidate(this)"></td>
                <td>
                    <div id="divtxtChildTargetCodeCnDesc"></div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="mappingTxt">我方代码</span>
                    <span class="mustIcon">*</span>
                </td>
                <td colspan="4">
                    <input id="txtChildCMCITCode" class="stepIpt widthMax" type="text" maxlength="50" title="我方代码~50:^[A-Za-z0-9,.\u4e00-\u9fa5]+$!" onchange="deoxidizevalidate(this)"></td>
                <td>
                    <div id="divtxtChildCMCITCode"></div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="mappingTxt">我方代码英文描述</span>
                    <span class="mustIcon">*</span>
                </td>
                <td colspan="4">
                    <input id="txtChildCMCITCodeEnDesc" class="stepIpt widthMax" type="text" maxlength="500" title="我方代码英文描述~500:^[A-Za-z0-9,.\u4e00-\u9fa5]+$!" onchange="deoxidizevalidate(this)"></td>
                <td>
                    <div id="divtxtChildCMCITCodeEnDesc"></div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="mappingTxt">我方代码中文描述</span>
                    <span class="mustIcon">*</span>
                </td>
                <td colspan="4">
                    <input id="txtChildCMCITCodeCnDesc" class="stepIpt widthMax" type="text" maxlength="500" title="我方代码中文描述~500:^[A-Za-z0-9,.\u4e00-\u9fa5]+$!" onchange="deoxidizevalidate(this)"></td>
                <td>
                    <div id="divtxtChildCMCITCodeCnDesc"></div>
                </td>
            </tr>
            <tr>
                <td>
                    <span class="mappingTxt">生效时间</span>
                    <span class="mustIcon">*</span>
                </td>
                <td>
                    <input id="txtChildStartDate" class="stepIpt widthMax" type="text">
                    <input class="filterIpt dateBg" title="生效时间~:!" style="display: none" type="text" id="picChildStartDate" onclick="WdatePicker({ lang: 'zh-cn', dateFmt: 'yyyy-MM-dd', readOnly: true })" onchange="deoxidizevalidate(this)" />
                </td>
                <td>
                    <div id="divpicChildStartDate"></div>
                </td>
                <td>
                    <span class="mappingTxt">失效时间</span>
                </td>
                <td>
                    <input id="txtChildExpireDate" class="stepIpt widthMax" type="text">
                    <input class="filterIpt dateBg" style="display: none" type="text" id="picChildExpireDate" onclick="WdatePicker({ lang: 'zh-cn', dateFmt: 'yyyy-MM-dd', minDate: '#F{$dp.$D(\'picChildStartDate\')}', readOnly: true })" />
                </td>
                <td>
                    <div id="divpicChildExpireDate"></div>
                </td>
            </tr>
        </table>
        <div class="operateBtnBox operateBtnBoxStyle">
            <a class="guideBtn finishLink saveCode" href="javascript:;" onclick="SaveCodeMapping();">保存</a>
            <a class="guideBtn prevLink" href="javascript:;" onclick="ClosePage();">关闭</a>
        </div>
        <input type="hidden" id="hidMappingID">
        <input type="hidden" id="hidAuthor">
        <input type="hidden" id="hidCreated">
    </div>
</div>

<!-- 批量导入 -->
<div id="divImport" class="mappingBox removeBox" style="display: none">
    <div class="adminBox">
        <div class="viewBox">
            <input class="stepIpt w252 fLeft iptStyle fileName" id='txtFileName' type="text" />
            <a class="operationBtn query fLeft" href="javascript:;">浏览</a>
            <%--<input class="viewFileBtn" id="fileField" accept=".xls,.xlsx" type="file" name="file" value="浏览" onchange="ShowFile(this);" />--%>
            <input type="file" class="viewFileBtn" runat="server" id="fileField" accept=".xls,.xlsx" onchange="ShowFile(this);" />
            <p class="viewBoxTips">（请根据系统提供的模板上传，模板下载地址：<a href="/Documents/TemplateFiles/代码映射维护模板.xls">模板</a>）</p>
            <input type="hidden" id="hidCurrentUser" runat="server" />
            <input type="hidden" id="hidFileName" />
        </div>
        <div class="operateBtnBox operateBtnBoxStyle">
            <a class="guideBtn finishLink" name="import" href="javascript:;" onclick="ImportExcel();">导入</a>
            <asp:Button runat="server" CssClass="upload" ID="btnUpload" OnClick="btnUpload_Click" />
            <a class="guideBtn prevLink" href="javascript:;" onclick="CloseImportPage();">取消</a>
        </div>
    </div>
</div>
<!-- 弹窗 End -->
<!-- 接口导入 -->

<div id="interfaceImport" class="mappingBox" style="display: none">
    <div class="adminBox">
        <div class="viewBox codemaploaddivwidth">
            <asp:UpdatePanel ID="interfaceupdatep" runat="server">
                <ContentTemplate>
                    <table class="filterTable">
                        <tr>
                            <td class="filterTxtWidth"><label class="filterIptTxt fLeft">公司名称：</label></td>
                            <td>
                                <asp:DropDownList ID="ddlcompanyname" runat="server" CssClass="ddlcompanyname default widthMax" AutoPostBack="true" OnSelectedIndexChanged="ddlcompanyname_SelectedIndexChanged">
                                </asp:DropDownList></td>
                            <td class="filterTxtWidth"><label class="filterIptTxt fLeft">业务类型：</label></td>
                            <td class="codemaploadtdwidth">
                                <asp:DropDownList ID="ddlbusinesstype" runat="server" CssClass="ddlbusinesstype default widthMax">
                                </asp:DropDownList></td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
        <div class="operateBtnBox operateBtnBoxStyle">
            <a class="guideBtn finishLink" name="import" href="javascript:;" onclick="ImportInterfaceData();">导入</a>
           
            <a class="guideBtn prevLink" href="javascript:;" onclick="CloseInterFaceImport();">取消</a>
        </div>
    </div>
</div>
<!-- 弹窗 End -->

<script type="text/javascript">
    function InterfaceUploadSuccess() {
        var index = $.layer({
            area: ['auto', 'auto'],
            dialog: {
                msg: '接口导入成功。',
                type: 9
            },
            btns: 1,
            btn: ['确定'],
            closeBtn: false,
            yes: function () {
                parentSearch();
                layer.close(index);
            }
        });
    }
    //取消接口导入
    function CloseInterFaceImport() {
        layer.closeAll();
    }
    //接口导入操作
    function ImportInterfaceData() {
        var comname = $(".ddlcompanyname option:selected").val();
        var bustype = $(".ddlbusinesstype option:selected").val();

        if (comname == "all" || bustype == "all") {
            var index = $.layer({
                area: ['auto', 'auto'],
                dialog: {
                    msg: '请选择公司名称与业务类型后再导入。',
                    type: 8
                },
                btns: 1,
                btn: ['确定'],
                closeBtn: false,
                yes: function () {
                    layer.close(index);
                }
            });
            return;
        } else {
            $(".interfaceupload").click();
        }
    }
    //接口导入展示
    function OpenInterfaceImport() {
        $.layer({
            type: 1, //0-4的选择,
            title: false,
            border: [0],
            closeBtn: [0, false],
            shadeClose: false,
            area: ['650px', '170px'],
            offset: ['', ''],
            shade: [0.2, '#000'],
            zIndex: 1,
            page: {
                dom: '#interfaceImport'
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
            layer.closeLoad();
        } catch (err) {

        }
    });
</script>
