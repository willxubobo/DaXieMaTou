using CMICT.CSP.BLL.Components;
using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI.WebControls.WebParts;

namespace CMICT.CSP.Web.BusinessSearch.TemplateManage
{
    [ToolboxItemAttribute(false)]
    public partial class TemplateManage : WebPart
    {
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public TemplateManage()
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
                AspNetPager1.CurrentPageIndex = 1;
                AspNetPager1.NumericButtonCount = int.MaxValue;
                BindTemplateList();
            }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            AspNetPager1.CurrentPageIndex = 1;
            AspNetPager1.PageSize = Convert.ToInt32(hidpagesize.Value);
            BindTemplateList();
            Page.RegisterStartupScript("setvc", "<script>setvaluesel('" + hidpagesize.Value + "');</script>");
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            BindTemplateList();
        }

        protected void BindTemplateList()
        {
            int i, j;
            string TemplateName = txtTemplateName.Text.Trim();
            string StartDate = txtStartDate.Text.Trim();
            string EndDate = txtEndDate.Text.Trim();
            string Author = txtAuthor.Text.Trim();
            string TemplateStatus = "";
            int startIndex = (AspNetPager1.CurrentPageIndex - 1) * AspNetPager1.PageSize + 1;
            int PageSize = AspNetPager1.PageSize;
            TemplateManageComponent tmbll = new TemplateManageComponent();
            DataTable dt = tmbll.GetTemplateList(TemplateName, StartDate, EndDate, Author, TemplateStatus, AspNetPager1.CurrentPageIndex, PageSize, out i, out j);
            AspNetPager1.RecordCount = i;
            TemplateList.DataSource = dt;
            TemplateList.DataBind();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="TemplateID"></param>
        /// <returns></returns>
        public string GetPageInfoByTemplateID(string TemplateID)
        {
            string pinfo = string.Empty;
            if (!string.IsNullOrEmpty(TemplateID))
            {
                TemplateManageComponent pbll = new TemplateManageComponent();
                DataTable dt = pbll.GetListByTemplateID(TemplateID);
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        pinfo += "<a href='" + dr["Url"].ToString() + "' target='_blank' class='operateLink'>" + dr["PageName"].ToString() + "</a>,";
                    }
                }
            }
            return pinfo.TrimEnd(',');
        }
    }
}
