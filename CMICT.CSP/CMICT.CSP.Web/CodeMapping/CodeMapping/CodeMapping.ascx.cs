using CMICT.CSP.BLL.Components;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace CMICT.CSP.Web.CodeMapping.CodeMapping
{
    [ToolboxItemAttribute(false)]
    public partial class CodeMapping : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        CodeMappingComponent codeMap = new CodeMappingComponent();

        public CodeMapping()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                //AspNetPager1.PageSize = Convert.ToInt32(hidpagesize.Value);
                btnUpload.Style.Add("display", "none");
                AspNetPager1.CurrentPageIndex = 1;
                AspNetPager1.PageSize = Convert.ToInt32(hidpagesize.Value);
               // AspNetPager1.NumericButtonCount = int.MaxValue;
                BindCodeMap();
                BindCompanyList();
            }
        }

        protected void BindCompanyList()
        {
            try
            {
                DataTable clist = codeMap.GetCompanies();
                ddlcompanyname.DataSource = clist;
                ddlcompanyname.DataTextField = "TEXT";
                ddlcompanyname.DataValueField = "VALUE";
                ddlcompanyname.DataBind();
                if (ddlcompanyname.SelectedValue != null)
                {
                    BindBusType(ddlcompanyname.SelectedValue.Split(';')[0]);
                }
            }
            catch (Exception ex)
            {
                BaseComponent.Error("codemapping-bindcompanylist:"+ex.Message);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "bindcomerror", "layer.alert('绑定公司名称出现错误！',8);", true);
            }
        }
        protected void BindBusType(string comname)
        {
            if (!string.IsNullOrEmpty(comname))
            {
                ddlbusinesstype.DataSource = codeMap.GetCodeTypes(comname);
                ddlbusinesstype.DataTextField = "DESCRIPTION";
                ddlbusinesstype.DataValueField = "CODE_TYPE";
                ddlbusinesstype.DataBind();
            }
        }
        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BindCodeMap();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setvsel", "setvaluesel('" + hidpagesize.Value + "');", true);
        }

        private void BindCodeMap()
        {
            try
            {
                
                int recordCount;
                string strTargetName = txtTargetCompanyName.Value.Trim();
                string strCustomerID = txtCustomerCode.Value.Trim();
                string strSemnatic = txtSemanticContent.Value.Trim();
                string strBusinessCode = txtBusinessCode.Value.Trim();
                string strCMICTCode = txtCMICTCode.Value.Trim();

                int startIndex = (AspNetPager1.CurrentPageIndex - 1) * AspNetPager1.PageSize + 1;
                int PageSize = AspNetPager1.PageSize;
                int RecordCount;
                int PageCount;
                DataTable dtCode = codeMap.GetCodeMappingData(strTargetName, strSemnatic, strCustomerID, strBusinessCode, strCMICTCode
                    , startIndex, PageSize, out RecordCount, out PageCount);

                AspNetPager1.RecordCount = RecordCount;
                CodeMapList.DataSource = dtCode;
                CodeMapList.DataBind();


            }
            catch (Exception ex)
            {
                BaseComponent.Error("代码映射数据绑定： " + ex.Message + "--------" + ex.StackTrace);
            }


        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            AspNetPager1.CurrentPageIndex = 1;
            AspNetPager1.PageSize = Convert.ToInt32(hidpagesize.Value);
            BindCodeMap();
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setvsel", "layer.closeAll();", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setvselsdes", "setvaluesel('" + hidpagesize.Value + "');", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setAllsel", "BindSelectAllEvent();", true);
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setseldeds", "BindEachCboEvent();", true);

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                UploadFile();
            });

        }

        private void UploadFile()
        {
            try
            {
                var files = Page.Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    if (files[i] == null || files[i].ContentLength == 0)
                        continue;
                    FileInfo fileInfo = new FileInfo(files[i].FileName);
                    string fileName = fileInfo.Name;

                    int index = fileName.LastIndexOf(".");
                    string fileExtension = fileName.Substring(index);

                    string newName = Guid.NewGuid().ToString();//生成新的文件名，保证唯一性

                    string path = SPUtility.GetCurrentGenericSetupPath(@"TEMPLATE\LAYOUTS\CMICT.CSP.Web\CodeMappingFile\");

                    string filepath = path + newName + fileExtension;

                    // 判定该路径是否存在
                    if (!Directory.Exists(path))
                        Directory.CreateDirectory(path);

                    files[i].SaveAs(filepath);//存储到服务器上

                    string currentUserName = hidCurrentUser.Value.Trim();

                    string result = codeMap.ImportCodeMapping(filepath, currentUserName);

                    if (File.Exists(filepath))
                    {
                        //如果存在则删除
                        File.Delete(filepath);
                    }

                    if (string.IsNullOrWhiteSpace(result))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "UploadSuccess();", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "UploadFailed('" + result + "');", true);
                    }
                }
            }
            catch (Exception ex)
            {
                BaseComponent.Error("代码映射批量导入： " + ex.Message + "--------" + ex.StackTrace);

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "UploadFailed('请按照系统提供的模板进行上传。');", true);
            }

        }

        protected void btninterfload_Click(object sender, EventArgs e)
        {
            try
            {
                System.ServiceModel.WSHttpBinding binding = new System.ServiceModel.WSHttpBinding(System.ServiceModel.SecurityMode.None);
                binding.MaxReceivedMessageSize = int.MaxValue;
                string comtype = ddlcompanyname.SelectedValue;
                string bustype = ddlbusinesstype.SelectedValue;
                if (!string.IsNullOrEmpty(comtype) && !string.IsNullOrEmpty(bustype)&&comtype.Contains(";"))
                {
                    string[] comlist = comtype.Split(';');
                    string currentUserName = BaseWebPart.GetCurrentUserLoginId();
                    string dtcolstr = "目标客户公司代码,目标客户公司名称,目标客户说明,语义内容,业务代码,业务代码说明,业务翻译说明,目标客户代码,目标客户代码英文描述,目标客户代码中文描述,我方代码,我方代码英文描述,我方代码中文描述,生效时间,失效时间";
                    CBOSServices.CodSwicthTableServiceClient client = new CBOSServices.CodSwicthTableServiceClient();
                    CBOSServices.CodSwicthTable[] result = client.getCodSwitchMessage2(comlist[1], bustype);
                    if (result != null && result.Length > 0)
                    {
                        codeMap.DelCodeMapByComIDAndBusID(comlist[1].Trim(), bustype.Trim());//插入前先删除
                        foreach (CBOSServices.CodSwicthTable re in result)
                        {
                            if (string.IsNullOrEmpty(re.CODE_CBOS)) { 
                                continue;
                            }
                            DataTable dt = new DataTable();
                            string[] dtitlelist = dtcolstr.Split(',');
                            foreach (string str in dtitlelist)
                            {
                                dt.Columns.Add(str);
                            }
                            DataRow dr = dt.NewRow();
                            dr[0] = comlist[1];
                            dr[1] = comlist[0];
                            dr[2] = "";
                            dr[3] = "";
                            dr[4] = re.SWITCH_TAB_NAME;
                            dr[5] = "";
                            dr[6] = "";
                            dr[7] = re.CODE_CBOS;
                            dr[8] = re.CODE_NAME_E_CBOS;
                            dr[9] = re.CODE_NAME_C_CBOS;
                            dr[10] = re.CODE_SWITCH;
                            dr[11] = re.CODE_NAME_E_SWITCH;
                            dr[12] = re.CODE_NAME_C_SWITCH;
                            dr[13] = "";
                            dr[14] = "";
                            dt.Rows.Add(dr);
                            codeMap.InsertTableFromDataTable(dt, currentUserName);
                        }
                        
                    }
                    BindCodeMap();
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "interfaceloadsuc", "InterfaceUploadSuccess();", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "interfaceloaderror", "layer.alert('公司名称与业务代码不能为空！',8);", true);
                }
            }
            catch (Exception ex)
            {
                BaseComponent.Error("代码映射接口导入： " + ex.Message + "--------" + ex.StackTrace);

                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "interfaceload", "layer.alert('接口导入出现异常，请联系管理员！',8);", true);
            }
            //finally
            //{
            //    //ddlcompanyname.SelectedValue = "all";
            //    //ddlbusinesstype.SelectedValue = "all";
            //}
        }

        protected void ddlcompanyname_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlcompanyname.SelectedValue != null)
            {
                BindBusType(ddlcompanyname.SelectedValue.Split(';')[0]);
            }
        }
    }
}
