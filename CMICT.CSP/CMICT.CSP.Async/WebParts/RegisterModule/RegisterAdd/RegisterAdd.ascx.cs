using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;
using CMICT.CSP.Async.Commons;
using CMICT.CSP.Async.Module;
using Newtonsoft.Json;


namespace CMICT.CSP.Async.WebParts.RegisterModule.RegisterAdd
{
    [ToolboxItemAttribute(false)]
    public partial class RegisterAdd : WebPart
    {
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public RegisterAdd()
        {
        }

        private string Type
        {
            get
            {
                if (Page.Request.QueryString["type"] != null)
                {
                    return Page.Request.QueryString["type"];
                }
                else
                {
                    return "C";
                }
            }
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
                if (Page.Request.QueryString["entity"] != null)
                {
                    this.txtModuleName.Text = Page.Request.QueryString["entity"];
                }
            }
        }

        protected void lbtnRegister_Click(object sender, EventArgs e)
        {
            string isEnableNet = ConfigurationManager.AppSettings["isEnableNet"];
            //java
            if (string.IsNullOrEmpty(isEnableNet))
            {
                RegisterServiceEntity rs = new RegisterServiceEntity();
                List<DataCollection> list = new List<DataCollection>();
                DataCollection dc = new DataCollection();

                dc.username =Common.DataUserName;
                dc.password = Common.DataPwd;

                dc.ENTITY_CODE = this.txtModuleName.Text;
                dc.MD_CODE = "MDM_NBG";
                dc.SERVICE_NAME = "mdm_subscribe";
                dc.SYS_CODE = "BLCTZS";
                dc.TO_NODE = "BLCTZS";
                list.Add(dc);

                rs.password = Common.ApplyDataUserPwd;
                rs.username = Common.ApplyDataUserName;
                rs.action = "SERVICE_REGISTER";
                rs.data = list;
                rs.type = Type;

                string applyData = "[" + JsonConvert.SerializeObject(rs) + "]";


                WebJavaReference.EDIESBService javaeEdiesbService = new WebJavaReference.EDIESBService();

                string returnJson = javaeEdiesbService.callEDIESBPub("BLCTZS", "MDM", "mdm_register", applyData, Common.EdiUserName, Common.EdiPwd);

                var model = JsonConvert.DeserializeObject<ResultEntity>(returnJson);

                if (model != null)
                {
                    ResultEntity.Result2 result2 = model.Result;
                    if (result2 != null)
                    {
                        ResultEntity.ResultInfo[] resultInfos = result2.ResultInfos;
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "sdel", "layer.alert('" + resultInfos[0].Msg + "！',9);", true);
                    }
                }

            }
        }
    }
}
