using CMICT.CSP.BLL.Components;
using CMICT.CSP.Model.BusinessSearchModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using Microsoft.SharePoint;
using System.IO;
using System.Web;
using Wuqi.Webdiyer;

namespace CMICT.CSP.Web.BusinessSearch.BusinessSearch
{
    [ToolboxItemAttribute(false)]
    public partial class BusinessSearch : WebPart, IWebPartParameters
    {
        [Personalizable((PersonalizationScope.Shared))] //标识属性为自定义属性
        [WebBrowsable(false)] //标识该属性是否显示在编辑界面中
        [WebDisplayName("模板")] //显示的友好名称
        public string TemplateID { get; set; }

        [Personalizable((PersonalizationScope.Shared))] //标识属性为自定义属性
        [WebBrowsable(false)] //标识该属性是否显示在编辑界面中
        [WebDisplayName("通信关联")] //显示的友好名称
        public string CommunicationID { get; set; }

        [Personalizable((PersonalizationScope.Shared))] //标识属性为自定义属性
        [WebBrowsable(false)] //标识该属性是否显示在编辑界面中
        [WebDisplayName("是否自动加载数据")] //显示的友好名称
        public string IsAutoSearch { get; set; }

        [Personalizable((PersonalizationScope.Shared))] //标识属性为自定义属性
        [WebBrowsable(false)] //标识该属性是否显示在编辑界面中
        [WebDisplayName("样式脚本")] //样式脚本
        public string JsScript { get; set; }

        [Personalizable((PersonalizationScope.Shared))] //标识属性为自定义属性
        [WebBrowsable(false)] //标识该属性是否显示在编辑界面中
        [WebDisplayName("标题")] //标题
        public string BusSearchTitle { get; set; }

        public bool IsReleased = true;

        // public List<string> SendWebPartID = new List<string>();


        /// <summary>
        /// 发送的数据 templateID
        /// </summary>
        public Dictionary<string, Dictionary<string, string>> CommunicationData
        {
            get
            {
                if (ViewState[this.ID + "CommunicationData"] == null)
                    return null;
                else
                    return (Dictionary<string, Dictionary<string, string>>)ViewState[this.ID + "CommunicationData"];
            }
            set
            {
                ViewState[this.ID + "CommunicationData"] = value;
            }
        }

        public TemplateModel TemplateModel
        {
            get
            {
                if (ViewState[this.ID + "templatemodel"] == null)
                    return null;
                else
                    return (TemplateModel)ViewState[this.ID + "templatemodel"];
            }
            set
            {
                ViewState[this.ID + "templatemodel"] = value;
            }
        }

        public override EditorPartCollection CreateEditorParts()
        {

            //EditorPartCollection baseParts = new EditorPartCollection(); //base.CreateEditorParts();

            List<EditorPart> editorParts = new List<EditorPart>(1);

            EditorPart part = new TemplateConfigEditorPart();

            part.ID = this.ID + "_TemplateConfigEditorPart";

            part.Title = "业务信息模板配置";

            editorParts.Add(part);

            return new EditorPartCollection(editorParts);
        }
        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public BusinessSearch()
        {
        }


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }
        BusinessSearchComponent com = new BusinessSearchComponent();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
            {
                if (hidisautosearch.Value == "Y")
                {
                    hidisautosearch.Value = "N";
                }
                return;
            }
            if (string.IsNullOrEmpty(TemplateID))
                return;

            //com.RefreshTemplateByGuid(TemplateID);
            hidwebpartid.Value = this.ID;
            hidpagewebid.Value = this.ID;
            TemplateModel model = com.GetTemplateByGuid(TemplateID, IsReleased);
            if (model != null)
            {
                if (model.SQLBuilder.IsProcudure)
                {
                    hidisproc.Value = "proc";
                }
                if (!string.IsNullOrEmpty(model.SQLBuilder.Orderby))
                {
                    hidsortnames.Value = model.SQLBuilder.Orderby.TrimEnd(',').Trim();//默认排序字段
                }
                ViewState[this.ID + "templatemodel"] = model;
                hidtemplatename.Value = model.TemplateName;
                hidpagesize.Value = model.SQLBuilder.PageSize;
                //hidpagesize.Value = "2";
                ViewState[this.ID + "DisplayType"] = model.GridBuilder.DisplayType;
                hidMergeColumn.Value = model.SQLBuilder.MergeColumnNames.Trim();
                hidDisplayName.Value = model.SQLBuilder.DisplayNames.Trim();
                hidColumnName.Value = model.SQLBuilder.ColumnNames.Trim();
                hiduserscript.Value = JsScript;
                if (BindSearchQuery(model))//绑定查询条件
                {
                    if (IsAutoSearch == "Y")
                    {
                        hidisautosearch.Value = "Y";
                        hidautosearchvalue.Value = "Y";
                        AspNetPager1.CurrentPageIndex = 1;
                        AspNetPager1.PageSize = Convert.ToInt32(model.SQLBuilder.PageSize);
                        BindTableList("");

                        //自动点击第一行
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "defaultconetr", this.ID + "defaultclickonetr('" + this.ID + "');", true);

                    }
                }
                //if (IsAutoSearch == "Y")
                //    btnSearch_Click(null, null);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertnomodel", "layer.alert('该业务信息查询页面正在维护，请稍候重试！',8);", true);
            }
        }
        //排序方法
        protected void btnColumnSort_Click(object sender, EventArgs e)
        {
            string sortcolname = hidsortcolname.Value.Trim();
            string sortdisname = hidsortdisname.Value.Trim();
            if (!string.IsNullOrEmpty(sortcolname))
            {
                if (ViewState[this.ID + sortcolname] == null)
                {
                    ViewState[this.ID + sortcolname] = "ASC";
                    sortdisname = sortdisname + "▲";
                }
                else
                {
                    if (ViewState[this.ID + sortcolname].ToString().Trim() == "ASC")
                    {
                        ViewState[this.ID + sortcolname] = "DESC";
                        if (sortdisname.IndexOf("▲") != -1)
                            sortdisname = sortdisname.Replace("▲", "▼");
                        else
                            sortdisname = sortdisname + "▼";
                    }
                    else
                    {
                        ViewState[this.ID + sortcolname] = "ASC";
                        if (sortdisname.IndexOf("▼") != -1)
                            sortdisname = sortdisname.Trim().Replace("▼", "▲");
                        else
                            sortdisname = sortdisname + "▲";
                    }
                }
                ViewState[this.ID + "text"] = sortdisname;
                ViewState[this.ID + "id"] = sortcolname;
                string scn = sortcolname;
                if (hidisproc.Value.Trim() != "proc")
                {
                    scn = "[" + sortcolname + "] ";
                }

                string orderby = scn + " " + ViewState[this.ID + sortcolname].ToString().Trim();
                ViewState[this.ID + "orderby"] = orderby;
                AspNetPager1.CurrentPageIndex = 1;
                AspNetPager1.PageSize = Convert.ToInt32(hidpagesize.Value);
                BindTableList(orderby);

                //自动点击第一行
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "defaultconetr", this.ID + "defaultclickonetr('" + this.ID + "');", true);
            }
        }
        //获取绑定列
        protected DataTable GetBindTable(DataTable dt, TemplateModel model, int startIndex, int PageSize)
        {
            string[] cnlist = model.SQLBuilder.ColumnNames.Split(',');
            string[] dislist = model.SQLBuilder.DisplayNames.Split(',');
            DataTable binddt = new DataTable(); ;
            binddt.Columns.Add("Columntr", Type.GetType("System.String"));
            int sindex = 1;
            if (startIndex > 1)
            {
                sindex = PageSize * (startIndex - 1) + 1;
            }
            if (model.GridBuilder.DisplayType.Trim().ToLower() != "grid")
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string trinfo = "<td style='width:50px !important;'>" + Convert.ToString(dr["bs_row_num"]) + "</td>";
                    DataRow drb = binddt.NewRow();
                    foreach (string str in cnlist)
                    {
                        string dvalue = dr[str].ToString();
                        if (dvalue.Contains("|"))//计算小数显示格式
                        {
                            string[] vlist = dvalue.Split('|');
                            double yz = -1;
                            double.TryParse(vlist[0], out yz);
                            int dcount = -1;
                            int.TryParse(vlist[1], out dcount);
                            if (yz != -1 && dcount != -1)
                            {
                                dvalue = Convert.ToDouble(vlist[0]).ToString("f"+dcount);
                            }
                            //else
                            //{
                            //    dvalue = "";
                            //}
                        }
                        trinfo += "<td cname='" + str + "'>" + dvalue + "</td>";
                    }
                    if (!string.IsNullOrEmpty(model.SQLBuilder.HiddenNames))
                    {
                        string[] hidclnames = model.SQLBuilder.HiddenNames.Split(',');
                        foreach (string hidstr in hidclnames)
                        {
                            trinfo += "<td cname='" + hidstr + "' style='display:none;'>" + dr[hidstr].ToString() + "</td>";
                        }
                    }
                    drb["Columntr"] = trinfo;
                    binddt.Rows.Add(drb);
                    sindex++;
                }
            }
            else
            {
                foreach (DataRow dr in dt.Rows)
                {
                    string trinfo = "<td style='width:50px !important;'>" + Convert.ToString(dr["bs_row_num"]) + "</td>";
                    int ColumnSize = Convert.ToInt32(model.GridBuilder.ColumnSize);
                    int dtcolumnsize = cnlist.Length;//数据源列数
                    DataRow drb = binddt.NewRow();
                    trinfo += "<td><table class=\"tableInner\">";
                    int trcount = dtcolumnsize / ColumnSize;
                    int tdlastcount = dtcolumnsize % ColumnSize;//最后一行要累加的td数
                    int addtrcount = 0;
                    if (trcount == 0)//只有一行
                    {
                        trinfo += "<tr>";
                        for (int j = 0; j < cnlist.Length; j++)
                        {
                            trinfo += "<td class=\"fontWeight\">" + dislist[j] + "</td><td cname='" + cnlist[j] + "'>" + (dr[cnlist[j]].ToString().Trim() == "" ? "&nbsp;" : dr[cnlist[j]].ToString()) + "</td>";
                        }
                        if (!string.IsNullOrEmpty(model.SQLBuilder.HiddenNames))
                        {
                            string[] hidclnames = model.SQLBuilder.HiddenNames.Split(',');
                            foreach (string hidstr in hidclnames)
                            {
                                trinfo += "<td cname='" + hidstr + "' style='display:none;'>" + dr[hidstr].ToString() + "</td>";
                            }
                        }
                        trinfo += "</tr>";
                    }
                    else
                    {

                        for (int j = 0; j < cnlist.Length; j++)
                        {
                            if (j % ColumnSize == 0)
                            {
                                trinfo += "<tr>";
                            }
                            trinfo += "<td class=\"fontWeight\">" + dislist[j] + "</td><td cname='" + cnlist[j] + "'>" + (dr[cnlist[j]].ToString().Trim() == "" ? "&nbsp;" : dr[cnlist[j]].ToString()) + "</td>";
                            addtrcount++;
                            if (addtrcount == ColumnSize)
                            {
                                if (!string.IsNullOrEmpty(model.SQLBuilder.HiddenNames))
                                {
                                    string[] hidclnames = model.SQLBuilder.HiddenNames.Split(',');
                                    foreach (string hidstr in hidclnames)
                                    {
                                        trinfo += "<td style='display:none;'>&nbsp;</td>";
                                    }
                                }
                                trinfo += "</tr>";
                                addtrcount = 0;
                            }
                        }
                        if (tdlastcount == 0)
                        {
                            if (!string.IsNullOrEmpty(model.SQLBuilder.HiddenNames))
                            {
                                string[] hidclnames = model.SQLBuilder.HiddenNames.Split(',');
                                foreach (string hidstr in hidclnames)
                                {
                                    trinfo += "<td style='display:none;'>&nbsp;</td>";
                                }
                            }
                            trinfo += "</tr>";
                        }
                        else
                        {
                            for (int i = 0; i < (ColumnSize - tdlastcount); i++)
                            {
                                trinfo += "<td>&nbsp;</td><td>&nbsp;</td>";
                            }
                            if (!string.IsNullOrEmpty(model.SQLBuilder.HiddenNames))
                            {
                                string[] hidclnames = model.SQLBuilder.HiddenNames.Split(',');
                                foreach (string hidstr in hidclnames)
                                {
                                    trinfo += "<td cname='" + hidstr + "' style='display:none;'>" + dr[hidstr].ToString() + "</td>";
                                }
                            }
                            trinfo += "</tr>";
                        }
                    }
                    trinfo += "</table></td>";
                    drb["Columntr"] = trinfo;
                    binddt.Rows.Add(drb);
                    sindex++;
                }
            }
            return binddt;
        }
        //绑定数据
        protected void BindTableList(string orderby, string fwhere = "", int istrstart = 0)
        {
            try
            {
                if (ViewState[this.ID + "templatemodel"] != null)
                {
                    TemplateModel model = ViewState[this.ID + "templatemodel"] as TemplateModel;
                    if (model != null)
                    {
                        if (ViewState[this.ID + "trswhere"] != null)
                        {
                            fwhere = ViewState[this.ID + "trswhere"].ToString();
                        }
                        string sWhere = (ViewState[this.ID + "hidqueryinfo"] == null ? hidqueryinfo.Value.Trim() : ViewState[this.ID + "hidqueryinfo"].ToString());
                        if (!string.IsNullOrEmpty(sWhere))
                        {
                            if (!string.IsNullOrEmpty(fwhere))
                            {
                                sWhere += " and " + fwhere;
                            }
                        }
                        else
                        {
                            sWhere = fwhere;
                        }
                        if (istrstart == 1)
                        {
                            hidhavechild.Value = "yes";
                            AspNetPager1.CurrentPageIndex = 1;
                        }
                        else
                        {
                            hidhavechild.Value = "";
                        }
                        int startIndex = AspNetPager1.CurrentPageIndex;
                        int PageSize = AspNetPager1.PageSize;
                        if (model.IsTruePaged)
                        {
                            DataTable dt = com.GetDataIsTruePaged(model, startIndex, PageSize, sWhere, orderby);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                DataTable binddt = GetBindTable(dt, model, startIndex, PageSize);
                                lblnodata.Visible = false;
                                int record = com.GetTotalCount(model, sWhere);
                                AspNetPager1.RecordCount = record;
                                rpttablelist.DataSource = binddt;
                                rpttablelist.DataBind();
                                pagerdiv.Visible = true;
                                string sorttext = "";
                                if (ViewState[this.ID + "text"] != null)
                                {
                                    sorttext = ViewState[this.ID + "text"].ToString();
                                }
                                hidsorttext.Value = sorttext;
                                hiddisplaytype.Value = model.GridBuilder.DisplayType;
                                hidtemplatename.Value = model.TemplateName;
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "visbileexport", this.ID + "visbileexport('" + this.ID + "_exportclass');", true);

                            }
                            else
                            {
                                //btntrsearch_Click(sender, e);
                                rpttablelist.DataSource = null;
                                rpttablelist.DataBind();
                                pagerdiv.Visible = false;
                                lblnodata.Visible = true;
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "disvisbileexport", this.ID + "disvisbileexport('" + this.ID + "_exportclass');", true);
                            }
                        }
                        else//假分页
                        {
                            List<SqlParameter> parameterlist = new List<SqlParameter>();
                            SqlParameter[] parameterss = null;
                            var parainfo = hidparainfo.Value.Trim().TrimEnd(';');
                            if (!string.IsNullOrEmpty(parainfo))//存储过程
                            {
                                string[] paralist = parainfo.Split(';');
                                foreach (string para in paralist)
                                {
                                    if (!string.IsNullOrEmpty(para))
                                    {
                                        string[] plist = para.Split('!');
                                        SqlParameter sp = new SqlParameter(plist[0], plist[1]);
                                        parameterlist.Add(sp);
                                    }
                                }
                                if (parameterlist != null && parameterlist.Count > 0)
                                {
                                    parameterss = parameterlist.ToArray();
                                }
                            }
                            //DataTable dt = new DataTable();
                            //bool 
                            //if (ViewState[this.ID + "NotTrueTable"] != null && ViewState[this.ID + "NotTrueWhere"].ToString() == sWhere)
                            //{
                            //    dt = ViewState[this.ID + "NotTrueTable"] as DataTable;
                            //}
                            //else
                            //{
                            DataTable dt = com.GetDataIsNotTruePaged(model, sWhere, parameterss, orderby);//新加orderby参数
                                //ViewState[this.ID + "NotTrueTable"] = dt;
                                //ViewState[this.ID + "NotTrueWhere"] = sWhere;
                            //}
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                DataTable binddttemp = com.GetDataTablePaged(dt, startIndex, PageSize, "");
                                DataTable binddt = GetBindTable(binddttemp, model, startIndex, PageSize);
                                lblnodata.Visible = false;
                                AspNetPager1.RecordCount = dt.Rows.Count;
                                rpttablelist.DataSource = binddt;
                                rpttablelist.DataBind();
                                pagerdiv.Visible = true;
                                string sorttext = "";
                                if (ViewState[this.ID + "text"] != null)
                                {
                                    sorttext = ViewState[this.ID + "text"].ToString();
                                }
                                hidsorttext.Value = sorttext;
                                hiddisplaytype.Value = model.GridBuilder.DisplayType;
                                hidtemplatename.Value = model.TemplateName;
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "visbileexport", this.ID + "visbileexport('" + this.ID + "_exportclass');", true);

                            }
                            else
                            {
                                rpttablelist.DataSource = null;
                                rpttablelist.DataBind();
                                pagerdiv.Visible = false;
                                lblnodata.Visible = true;
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "disvisbileexport", this.ID + "disvisbileexport('" + this.ID + "_exportclass');", true);

                            }
                        }
                    }
                    //if (string.IsNullOrEmpty(hidclicktrid.Value))//默认点击第一行
                    //{
                    //    hidclicktrid.Value = this.ID + "_tr_1";
                    //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "defaultconetr", this.ID + "defaultclickonetr('"+this.ID+"');", true);
                    //}
                    //if (!string.IsNullOrEmpty(hidnorepeaterclick.Value))//点击
                    //{
                    //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "defaultconetrtwo", this.ID + "defaultclickonetr('" + this.ID + "');", true);
                    //    hidnorepeaterclick.Value = "";
                    //}
                    ReCodePager();
                }
            }
            catch (Exception ee)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "binderrorhghjy", "layer.alert('业务逻辑配置出错，请联系管理员！',8);", true);
                BaseComponent.Error(ee.Message);
            }
        }

        protected void ReCodePager()
        {
            if (AspNetPager1.RecordCount <= AspNetPager1.PageSize)
            {
                AspNetPager1.ShowFirstLast = false;
                AspNetPager1.ShowPageIndex = false;
                AspNetPager1.ShowPrevNext = false;
                AspNetPager1.ShowPageIndexBox = ShowPageIndexBox.Never;
            }
            else
            {
                AspNetPager1.ShowFirstLast = true;
                AspNetPager1.ShowPageIndex = true;
                AspNetPager1.ShowPrevNext = true;
                AspNetPager1.ShowPageIndexBox = ShowPageIndexBox.Always;
                if (AspNetPager1.CurrentPageIndex == 1) //第一页时不显示首页与上一页
                {
                    AspNetPager1.FirstPageText = "";
                    AspNetPager1.PrevPageText = "";
                }
                else
                {
                    AspNetPager1.FirstPageText = "首页";
                    AspNetPager1.PrevPageText = "上一页";
                }
                if (AspNetPager1.PageCount == AspNetPager1.CurrentPageIndex)
                {
                    AspNetPager1.LastPageText = "";
                    AspNetPager1.NextPageText = "";
                }
                else
                {
                    AspNetPager1.LastPageText = "尾页";
                    AspNetPager1.NextPageText = "下一页";
                }
            }
        }
        //绑定查询条件
        protected bool BindSearchQuery(TemplateModel model)
        {
            bool result = true;
            try
            {
                if (model != null)
                {
                    string bussearchtitle = string.Empty;
                    if (!string.IsNullOrEmpty(BusSearchTitle))
                    {
                        bussearchtitle = "<div class=\"busstitle bustitlemargint\" title=\"" + model.TemplateDesc + "\">" + BusSearchTitle + "</div>";
                    }
                    string displaystr = "style='display:none;'";
                    if (IsAutoSearch == "Y")
                    {
                        displaystr = "";
                    }
                    int scontrolcount = 0;
                    StringBuilder qcinfo = new StringBuilder();
                    StringBuilder matchinfo = new StringBuilder();
                    StringBuilder matchvar = new StringBuilder(); List<QueryControls> qlist = model.QueryControls;
                    if (qlist != null && qlist.Count > 0)
                    {
                        int m = 0;
                        qcinfo.Append("<table class=\"filterTable\" style=\"table-layout: fixed;\">");
                        string queryinfo = string.Empty;
                        int nofastsearchnum = 0;//非快捷查询条件数量
                        foreach (QueryControls qc in qlist)
                        {
                            if (qc.ControlType.Trim() != "DATEFAST")//快捷查询
                            {
                                nofastsearchnum++;
                            }
                        }
                        //首先累加快捷查询
                        foreach (QueryControls qc in qlist)
                        {
                            string faststr = string.Empty;
                            if (qc.ControlType.Trim() == "DATEFAST")//快捷查询
                            {
                                string defaultvalue = qc.DefautValue.Trim();
                                string qcp = string.Empty;
                                if (qc.Compare.Contains("]"))
                                {
                                    qcp = qc.Compare.Split(']')[0].Replace("[", "");
                                }
                                else
                                {
                                    qcp = qc.Compare.Split('=')[0].Replace("[", "").Replace("]", "");
                                }
                                qcp = "[" + qcp + "]";
                                string[] multiitem = defaultvalue.Trim().TrimEnd(';').Split(';');
                                foreach (string fstr in multiitem)
                                {
                                    if (fstr.Trim() == "上个月")
                                    {
                                        DateTime dd = DateTime.Now.AddMonths(-1);
                                        string nowstartd = dd.Year + "-" + dd.Month + "-" + "1";
                                        DateTime lastmonthday = DateTime.Parse(nowstartd);
                                        int daysmonth = System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(lastmonthday.Year, lastmonthday.Month);
                                        faststr += "<a href='javascript:void(0);' class='" + this.ID + "fasta fastdateacolor' onclick=\"" + this.ID + "lastmonthsearch(this,'" + qcp + "','" + lastmonthday.ToString("yyyy-MM-dd") + "','" + lastmonthday.Year + "-" + lastmonthday.Month + "-" + daysmonth + "');\">" + fstr + "</a>&nbsp;&nbsp;";
                                    }
                                    else if (fstr.Trim() == "2个月")
                                    {
                                        DateTime NNow = DateTime.Now;
                                        DateTime startd = NNow.AddMonths(-2);
                                        faststr += "<a href='javascript:void(0);' class='" + this.ID + "fasta fastdateacolor' onclick=\"" + this.ID + "lastmonthsearch(this,'" + qcp + "','" + startd.ToString("yyyy-MM-dd") + "','" + NNow.ToString("yyyy-MM-dd") + "');\">最近" + fstr + "</a>&nbsp;&nbsp;";
                                    }
                                    else if (fstr.Trim() == "3个月")
                                    {
                                        DateTime NNow = DateTime.Now;
                                        DateTime startd = NNow.AddMonths(-3);
                                        faststr += "<a href='javascript:void(0);' class='" + this.ID + "fasta fastdateacolor' onclick=\"" + this.ID + "lastmonthsearch(this,'" + qcp + "','" + startd.ToString("yyyy-MM-dd") + "','" + NNow.ToString("yyyy-MM-dd") + "');\">最近" + fstr + "</a>&nbsp;&nbsp;";
                                    }
                                    else if (fstr.Trim() == "半年")
                                    {
                                        DateTime NNow = DateTime.Now;
                                        DateTime startd = NNow.AddMonths(-6);
                                        faststr += "<a href='javascript:void(0);' class='" + this.ID + "fasta fastdateacolor' onclick=\"" + this.ID + "lastmonthsearch(this,'" + qcp + "','" + startd.ToString("yyyy-MM-dd") + "','" + NNow.ToString("yyyy-MM-dd") + "');\">最近" + fstr + "</a>&nbsp;&nbsp;";
                                    }
                                    else if (fstr.Trim() == "1年")
                                    {
                                        DateTime NNow = DateTime.Now;
                                        DateTime startd = NNow.AddYears(-1);
                                        faststr += "<a href='javascript:void(0);' class='" + this.ID + "fasta fastdateacolor' onclick=\"" + this.ID + "lastmonthsearch(this,'" + qcp + "','" + startd.ToString("yyyy-MM-dd") + "','" + NNow.ToString("yyyy-MM-dd") + "');\">最近" + fstr + "</a>&nbsp;&nbsp;";
                                    }
                                    else
                                    {
                                        int sdays = Convert.ToInt32(fstr) * -1;
                                        DateTime NNow = DateTime.Now;
                                        DateTime startd = NNow.AddDays(sdays);
                                        faststr += "<a href='javascript:void(0);' class='" + this.ID + "fasta fastdateacolor' onclick=\"" + this.ID + "lastmonthsearch(this,'" + qcp + "','" + startd.ToString("yyyy-MM-dd") + "','" + NNow.ToString("yyyy-MM-dd") + "');\">最近" + fstr + "天</a>&nbsp;&nbsp;";
                                    }
                                }
                                qcinfo.Append("<tr><td class=\"filterTxtWidth fasttdtext\">" + qc.DisplayName + "：</td>");
                                if (nofastsearchnum > 0)
                                {
                                    qcinfo.Append("<td colspan=\"7\" class=\"bussearchmwidth fasttdcontent\">" + faststr + "</td></tr>");
                                }
                                else
                                {
                                    qcinfo.Append("<td colspan=\"7\" class=\"bussearchmwidthfast fasttdcontent\">" + faststr + "</td></tr>");
                                }
                            }
                            else
                            {
                                continue;
                            }
                        }
                        foreach (QueryControls qc in qlist)//非快捷查询
                        {
                            if (qc.ControlType.Trim() == "DATEFAST")//快捷查询
                            {
                                continue;
                            }
                            
                            string defaultvalue = qc.DefautValue.Trim();
                            scontrolcount++;
                            if (m == 0)
                            {
                                qcinfo.Append("<tr>");
                            }
                            if (qc.Compare.Contains("@"))
                            {
                                string[] qcp = qc.Compare.Split('=');
                                hidparainfo.Value += qcp[0] + "!" + defaultvalue + ";";
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(defaultvalue))
                                {
                                    if (qc.ControlType.Trim() != "DATEFAST")//日期快捷查询
                                    {
                                        #region 非快捷默认值处理　
                                        if (qc.ControlType.Trim() != "MULTENUM")
                                        {
                                            queryinfo += qc.Compare.Replace("{0}", defaultvalue.Trim()) + " and ";
                                        }
                                        else
                                        {
                                            string[] multiitem = defaultvalue.Trim().TrimEnd(';').Split(';');
                                            queryinfo += "(";
                                            for (int mm = 0; mm < multiitem.Length; mm++)
                                            {
                                                string gvalue = qc.Compare.Replace("{0}", multiitem[mm]);
                                                queryinfo += gvalue;
                                                if ((mm + 1) < multiitem.Length)
                                                {
                                                    queryinfo += " or ";
                                                }
                                            }
                                            queryinfo += ") and ";
                                        }
                                        #endregion
                                    }
                                }
                            }

                            qcinfo.Append("<td class=\"filterTxtWidth\"><label class=\"filterIptTxt fLeft\">" + qc.DisplayName + "：</label></td>");
                            
                            if (qc.ControlType.Trim() == "TEXT")
                            {
                                qcinfo.Append("<td class=\"bussearchmwidth\"><input title=\"" + qc.Reminder + "\" onfocus=\"" + this.ID + "checkenter('" + this.ID + "searchb')\" class=\"stepIpt widthMaxSource " + this.ID + "_UsageFilter fLeft\" type=\"text\" runat=\"server\" name=\"" + qc.DisplayName + "\" value=\"" + defaultvalue + "\" comparev=\"" + qc.Compare + "\" onblur=\"" + this.ID + "AntiSqlValid(this)\"/></td>");
                            }
                            if (qc.ControlType.Trim() == "NUM")
                            {
                                qcinfo.Append("<td class=\"bussearchmwidth\"><input title=\"" + qc.Reminder + "\" onfocus=\"" + this.ID + "checkenter('" + this.ID + "searchb')\" class=\"stepIpt widthMaxSource " + this.ID + "_UsageFilter fLeft\" type=\"text\" runat=\"server\" name=\"" + qc.DisplayName + "\" value=\"" + defaultvalue + "\" comparev=\"" + qc.Compare + "\" onkeyup=\"this.value=this.value.replace(/\\D/g,'')\" onafterpaste=\"this.value=this.value.replace(/\\D/g,'')\"/></td>");
                            }
                            if (qc.ControlType.Trim() == "DATE")
                            {
                                qcinfo.Append("<td class=\"bussearchmwidth\"><div class=\"widthMaxSource fLeft\"><input title=\"" + qc.Reminder + "\" class=\"dateIptLayout " + this.ID + "_UsageFilter fLeft laydate-icon\" type=\"text\" runat=\"server\" name=\"" + qc.DisplayName + "\" comparev=\"" + qc.Compare + "\" value=\"" + defaultvalue + "\" onfocus=\"" + this.ID + "checkenter('" + this.ID + "searchb');\"  onblur=\"" + this.ID + "AntiSqlValid(this)\" id=\"rad_" + this.ID + "_" + scontrolcount + "\"  onclick=\"laydate({istime: true, format: 'YYYY-MM-DD hh:mm:ss'})\"/></div></td>");
                            }
                            if (qc.ControlType.Trim() == "YDATE")
                            {
                                qcinfo.Append("<td class=\"bussearchmwidth\"><div class=\"widthMaxSource fLeft\"><input title=\"" + qc.Reminder + "\" class=\"dateIptLayout " + this.ID + "_UsageFilter fLeft laydate-icon\" type=\"text\" runat=\"server\" name=\"" + qc.DisplayName + "\" comparev=\"" + qc.Compare + "\" value=\"" + defaultvalue + "\" onfocus=\"" + this.ID + "checkenter('" + this.ID + "searchb');\"  onblur=\"" + this.ID + "AntiSqlValid(this)\" id=\"rad_" + this.ID + "_" + scontrolcount + "\"  onclick=\"laydate({format: 'YYYY'})\"/></div></td>");
                            }
                            if (qc.ControlType.Trim() == "YMDATE")
                            {
                                qcinfo.Append("<td class=\"bussearchmwidth\"><div class=\"widthMaxSource fLeft\"><input title=\"" + qc.Reminder + "\" class=\"dateIptLayout " + this.ID + "_UsageFilter fLeft laydate-icon\" type=\"text\" value=\"" + defaultvalue + "\" runat=\"server\" name=\"" + qc.DisplayName + "\" comparev=\"" + qc.Compare + "\" onfocus=\"" + this.ID + "checkenter('" + this.ID + "searchb');\"  onblur=\"" + this.ID + "AntiSqlValid(this)\" id=\"rad_" + this.ID + "_" + scontrolcount + "\"  onclick=\"laydate({format: 'YYYY-MM'})\"/></div></td>");
                            }
                            if (qc.ControlType.Trim() == "MULTENUM")
                            {
                                DataTable ddlTable = GetEnumOrMatchSource(model, qc.SourceSql, qc.ColumnName);
                                if (ddlTable != null && ddlTable.Rows.Count > 0)
                                {
                                    string[] multiitem = defaultvalue.Trim().TrimEnd(';').Split(';');
                                    StringBuilder ddlstr = new StringBuilder();
                                    if (!qc.Compare.Contains("@"))
                                    {
                                        ddlstr.Append("<td class=\"bussearchmwidth\"><div class='widthMaxSource fLeft'>");
                                        ddlstr.Append("<select title=\"" + qc.Reminder + "\" onchange=\"" + this.ID + "checkenter('" + this.ID + "searchb')\" name=\"" + qc.DisplayName + "\" class=\"widthMaxSource " + this.ID + "_UsageFilter multisel\" size=\"5\"  multiple=\"multiple\" comparev=\"" + qc.Compare + "\">");
                                        //ddlstr.Append("<option value =\"all\">全部</option>");
                                    }
                                    else
                                    {
                                        ddlstr.Append("<td class=\"bussearchmwidth\"><div class='widthMaxSource fLeft'>");
                                        ddlstr.Append("<select title=\"" + qc.Reminder + "\" name=\"" + qc.DisplayName + "\" class=\"default " + this.ID + "_UsageFilter fLeft\" comparev=\"" + qc.Compare + "\">");
                                        ddlstr.Append("<option value =\"全部\">全部</option>");
                                    }
                                    foreach (DataRow dr in ddlTable.Rows)
                                    {
                                        int iss = 0;
                                        foreach (string devstr in multiitem)
                                        {
                                            if (devstr.ToLower().Trim() == dr["EnumValue"].ToString().Trim())//如匹配到默认值
                                            {
                                                iss = 1;
                                                ddlstr.Append("<option value =\"" + dr["EnumValue"].ToString() + "\" selected=\"selected\">" + dr["EnumValue"].ToString() + "</option>");
                                                break;
                                            }
                                        }
                                        if(iss==0)
                                        {
                                            ddlstr.Append("<option value =\"" + dr["EnumValue"].ToString() + "\">" + dr["EnumValue"].ToString() + "</option>");
                                        }
                                    }
                                    ddlstr.Append("</select>");
                                    qcinfo.Append(ddlstr.ToString() + "</div></td>");
                                }
                                else
                                {
                                    qcinfo.Append("<td class=\"bussearchmwidth\"><div class='widthMaxSource fLeft'>");
                                    qcinfo.Append("<select title=\"" + qc.Reminder + "\" onchange=\"" + this.ID + "checkenter('" + this.ID + "searchb')\" name=\"" + qc.DisplayName + "\" class=\"" + this.ID + "_UsageFilter multisel fLeft\" size=\"5\"  multiple=\"multiple\" comparev=\"" + qc.Compare + "\">");
                                    qcinfo.Append("<option value =\"全部\">全部</option>");
                                    qcinfo.Append("</select></div></td>");
                                }

                            }
                            if (qc.ControlType.Trim() == "ENUM")
                            {
                                
                                DataTable ddlTable = GetEnumOrMatchSource(model, qc.SourceSql, qc.ColumnName);
                                if (ddlTable != null && ddlTable.Rows.Count > 0)
                                {
                                    string initselect = "selected=\"selected\"";
                                    foreach (DataRow dr in ddlTable.Rows)
                                    {
                                        if (defaultvalue.ToLower().Trim() == dr["EnumValue"].ToString().Trim())//如匹配到默认值
                                        {
                                            initselect = "";
                                        }

                                    }
                                    StringBuilder ddlstr = new StringBuilder();
                                    if (!qc.Compare.Contains("@"))
                                    {
                                        ddlstr.Append("<td class=\"bussearchmwidth\"><div class='widthMaxSource fLeft'>");
                                        ddlstr.Append("<select title=\"" + qc.Reminder + "\" onchange=\"" + this.ID + "checkenter('" + this.ID + "searchb')\" name=\"" + qc.DisplayName + "\" class=\"widthMaxSource " + this.ID + "_UsageFilter default\" size=\"5\" comparev=\"" + qc.Compare + "\">");
                                       // ddlstr.Append("<option value =\"全部\" " + initselect + ">全部</option>");
                                    }
                                    else
                                    {
                                        ddlstr.Append("<td class=\"bussearchmwidth\"><div class='widthMaxSource fLeft'>");
                                        ddlstr.Append("<select title=\"" + qc.Reminder + "\" name=\"" + qc.DisplayName + "\" class=\"default " + this.ID + "_UsageFilter fLeft\" comparev=\"" + qc.Compare + "\">");
                                       // ddlstr.Append("<option value =\"全部\" " + initselect + ">全部</option>");
                                    }
                                    int sf = 0;
                                    foreach (DataRow dr in ddlTable.Rows)
                                    {
                                        if (defaultvalue.ToLower().Trim() == dr["EnumValue"].ToString().Trim())
                                        {
                                            ddlstr.Append("<option value =\"" + dr["EnumValue"].ToString() + "\" selected=\"selected\">" + dr["EnumValue"].ToString() + "</option>");
                                        }
                                        else
                                        {
                                            if (sf == 0)
                                            {
                                                ddlstr.Append("<option value =\"" + dr["EnumValue"].ToString() + "\" selected=\"selected\">" + dr["EnumValue"].ToString() + "</option>");
                                            }
                                            else
                                            {
                                                ddlstr.Append("<option value =\"" + dr["EnumValue"].ToString() + "\">" + dr["EnumValue"].ToString() + "</option>");
                                            }
                                        }
                                        sf++;
                                    }

                                    //if (ddlTable.Rows.Count > 1)
                                    //    ddlstr.Append("<option value =\"全部\" " + initselect + "></option>");
                                    ddlstr.Append("</select>");
                                    qcinfo.Append(ddlstr.ToString() + "</div></td>");

                                
                                    queryinfo += " "+qc.Compare.Replace("{0}", ddlTable.Rows[0]["EnumValue"].ToString())+" and"; 
                                }
                                else
                                {
                                    qcinfo.Append("<td class=\"bussearchmwidth\"><div class='widthMaxSource fLeft'>");
                                    qcinfo.Append("<select title=\"" + qc.Reminder + "\" onchange=\"" + this.ID + "checkenter('" + this.ID + "searchb')\" name=\"" + qc.DisplayName + "\" class=\"" + this.ID + "_UsageFilter default fLeft\" size=\"5\" comparev=\"" + qc.Compare + "\">");
                                    qcinfo.Append("<option value =\"全部\" selected=\"selected\">全部</option>");
                                    qcinfo.Append("</select></div></td>");
                                }

                           

                            }
                            if (qc.ControlType.Trim() == "MATCH")
                            {

                                DataTable ddlTable = GetEnumOrMatchSource(model, qc.SourceSql, qc.ColumnName);
                                if (ddlTable != null && ddlTable.Rows.Count > 0)
                                {
                                    matchvar.Append("var strlist_" + this.ID + "_" + scontrolcount + "=[");
                                    string listc = string.Empty;
                                    foreach (DataRow dr in ddlTable.Rows)
                                    {
                                        listc += "'" + dr["EnumValue"].ToString() + "',";
                                    }
                                    listc = listc.TrimEnd(',');
                                    matchvar.Append(listc.ToString() + "];");
                                    qcinfo.Append("<td class=\"bussearchmwidth\"><input title=\"" + qc.Reminder + "\" onfocus=\"" + this.ID + "checkenter('" + this.ID + "searchb')\" class=\"stepIpt widthMaxSource " + this.ID + "_UsageFilter fLeft\" id='match_" + this.ID + "_" + scontrolcount + "' type=\"text\" value=\"" + defaultvalue + "\" runat=\"server\" name=\"" + qc.DisplayName + "\" comparev=\"" + qc.Compare + "\"  onblur=\"" + this.ID + "AntiSqlValid(this)\"/></td>");
                                    matchinfo.Append("$(\"#match_" + this.ID + "_" + scontrolcount + "\").focus().autocomplete({source:strlist_" + this.ID + "_" + scontrolcount + "});");
                                }

                            }
                            m++;
                            if (m % 4 == 0)
                            {
                                qcinfo.Append("</tr>");
                                m = 0;
                            }

                        }
                        if (!string.IsNullOrEmpty(queryinfo))
                        {
                            hidqueryinfo.Value = queryinfo.Trim().Substring(0, queryinfo.Trim().LastIndexOf(' '));
                        }
                        if (!string.IsNullOrEmpty(qcinfo.ToString()))
                        {
                            if (m == 0)
                            {
                                qcinfo.Append("<tr><td colspan=\"8\" class=\"bussearchcoltdh\">");
                            }
                            else
                            {
                                for (int i = 0; i < (8 - (m * 2)); i++)
                                {
                                    qcinfo.Append("<td>&nbsp;</td>");
                                }
                                qcinfo.Append("<tr><td colspan=\"8\" class=\"bussearchcoltdh\">");
                            }
                            //添加操作按钮
                            qcinfo.Append(bussearchtitle+"<div class=\"operationBox\"><ul class=\"operationBtnBox\">");
                            qcinfo.Append("<li><button class=\"operationBtn export " + this.ID + "_exportclass\" " + displaystr + " type=\"button\" onclick=\"" + this.ID + "exportform();return false;\">导出</button></li>");
                            if (scontrolcount > 0)
                            {
                                qcinfo.Append("<li><button class=\"operationBtn query\" id=\"" + this.ID + "searchb\" type=\"button\" onclick=\"" + this.ID + "searchform();return false;\">查询</button></li>");
                            }
                            
                            if (scontrolcount > 0)
                            {
                                qcinfo.Append("<li><button class=\"operationBtn export\" type=\"button\" onclick=\"" + this.ID + "resetform();return false;\">重置</button></li>");
                            }
                            qcinfo.Append("<li><button class=\"operationBtn query\" id=\"" + this.ID + "showvis\" type=\"button\" onclick=\"" + this.ID + "visableform(this);return false;\">隐藏</button></li>");
                            qcinfo.Append("</ul></div></td></tr></table>");
                            ulquerylist.InnerHtml = qcinfo.ToString();
                            //ulquerylist.Controls.Add(li);
                            if (!string.IsNullOrEmpty(matchvar.ToString()) && !string.IsNullOrEmpty(matchinfo.ToString()))//注册智能匹配
                            {
                                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "matchreg" + this.ID, matchvar.ToString() + "$(function ($) {" + matchinfo.ToString() + " });", true);
                            }
                        }
                        else
                        {
                            //添加操作按钮
                            qcinfo.Append("<table class=\"filterTable\"><tr><td class=\"bussearchcoltdh\">" + bussearchtitle + "<div class=\"operationBox\"><ul class=\"operationBtnBox\">");
                            //qcinfo.Append("<li><button class=\"operationBtn query\" type=\"button\" id=\"" + this.ID + "searchb\" onclick=\"" + this.ID + "searchform();return false;\">查询</button></li>");
                            qcinfo.Append("<li><button class=\"operationBtn export " + this.ID + "_exportclass\" type=\"button\" " + displaystr + " onclick=\"" + this.ID + "exportform();return false;\">导出</button></li>");
                            qcinfo.Append("<li><button class=\"operationBtn query\" id=\"" + this.ID + "showvis\" type=\"button\" onclick=\"" + this.ID + "visableform(this);return false;\">隐藏</button></li>");
                            qcinfo.Append("</ul></div></td></tr></table>");
                            ulquerylist.InnerHtml = qcinfo.ToString();
                        }
                    }
                    else
                    {
                        //添加操作按钮
                        qcinfo.Append("<table class=\"filterTable\"><tr><td class=\"bussearchcoltdh\">" + bussearchtitle + "<div class=\"operationBox\"><ul class=\"operationBtnBox\">");
                        //qcinfo.Append("<li><button class=\"operationBtn query\" type=\"button\" id=\"" + this.ID + "searchb\" onclick=\"" + this.ID + "searchform();return false;\">查询</button></li>");
                        qcinfo.Append("<li><button class=\"operationBtn export " + this.ID + "_exportclass\" " + displaystr + " type=\"button\" onclick=\"" + this.ID + "exportform();return false;\">导出</button></li>");
                        qcinfo.Append("<li><button class=\"operationBtn query\" id=\"" + this.ID + "showvis\" type=\"button\" onclick=\"" + this.ID + "visableform(this);return false;\">隐藏</button></li>");
                        qcinfo.Append("</ul></div></td></tr></table>");
                        ulquerylist.InnerHtml = qcinfo.ToString();
                    }
                }
            }
            catch (Exception ee)
            {
                ulquerylist.InnerHtml = "";
                result = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "binderrorhghjy", "layer.alert('业务逻辑配置出错，请联系管理员！',8);", true);
                BaseComponent.Error(ee.Message);
            }
            return result;
        }
        //得到枚举或智能匹配数据源
        protected DataTable GetEnumOrMatchSource(TemplateModel model, string sourcesql, string columnname)
        {
            DataTable ddlTable = null;
            if (!model.SQLBuilder.IsProcudure)
            {
                if (!string.IsNullOrEmpty(sourcesql))
                {
                    ddlTable = com.GetTableBySql(model.SQLBuilder.ConnectionStrings, sourcesql.Replace("[本人]", BaseComponent.GetCurrentUserLoginName()));
                }
            }
            else
            {
                List<SqlParameter> parameterlist = new List<SqlParameter>();
                SqlParameter[] parameterss = null;
                List<QueryControls> qlist = model.QueryControls;
                if (qlist != null && qlist.Count > 0)
                {
                    foreach (QueryControls qc in qlist)
                    {
                        if (qc.ColumnName.IndexOf('@') != -1)
                        {
                            SqlParameter sp = new SqlParameter(qc.ColumnName, "");
                            parameterlist.Add(sp);
                        }
                    }
                }
                if (parameterlist != null && parameterlist.Count > 0)
                {
                    parameterss = parameterlist.ToArray();
                }
                DataTable dtall = com.GetDataIsNotTruePaged(model, "", parameterss);
                if (dtall != null && dtall.Rows.Count > 0)
                {
                    ddlTable = com.GetDistinctEnumValue(dtall, columnname);
                }
            }
            return ddlTable;
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            hidautosearchvalue.Value = "Y";
            hidsorttext.Value = "";
            ViewState[this.ID + "hidqueryinfo"] = hidqueryinfo.Value.Trim();
            DateTime dtstart = DateTime.Now;
            AspNetPager1.CurrentPageIndex = 1;
            AspNetPager1.PageSize = Convert.ToInt32(hidpagesize.Value);
            string orderby = (ViewState[this.ID + "orderby"] == null ? "" : ViewState[this.ID + "orderby"].ToString());
            BindTableList(orderby);
            //hidfastdate.Value = "";//查询后清空快捷查询条件
            //自动点击第一行
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "defaultconetr", this.ID + "defaultclickonetr('" + this.ID + "');", true);

            DateTime dtend = DateTime.Now;
            TimeSpan ts = dtend - dtstart;
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setvsel", "inittableonetrstyle();search('" + this.ID + "_UsageFilter', " + ts.Milliseconds.ToString() + ",'" + hidtemplatename.Value + "','" + TemplateID + "');", true);
        }

        protected void AspNetPager1_PageChanged(object sender, EventArgs e)
        {
            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setwebparthid", "setwebparthidvalue('N');", true);
            hidclicktrid.Value = "";
            string orderby = (ViewState[this.ID + "orderby"] == null ? "" : ViewState[this.ID + "orderby"].ToString());
            BindTableList(orderby);
            //自动点击第一行
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "defaultconetr", this.ID + "defaultclickonetr('" + this.ID + "');$('html,body').animate({scrollTop:$('#" + this.ID + "_top').offset().top}, 200);", true);
        }

        #region Send
        //IWebPartParameters需实现的借口
        public System.ComponentModel.PropertyDescriptorCollection Schema
        {
            get
            {
                return _schema;
            }
        }
        private PropertyDescriptorCollection _schema;
        public void SetConsumerSchema(System.ComponentModel.PropertyDescriptorCollection schema)
        {
            _schema = schema;
        }

        [ConnectionProvider("数据", "IWebPartParameters")]
        public IWebPartParameters ProviderIWebPartParameters()
        {
            EnsureChildControls();
            return this;
        }



        public void GetParametersData(ParametersCallback callback)
        {
            hdIsSender.Value = "Y";
            //if (ViewState[this.ID + "ispager"] != null)
            //{
            if (Convert.ToString(ViewState[this.ID + "Send"]).Contains(((BusinessSearch)callback.Target).ID))
                return;
            ViewState[this.ID + "Send"] += ((BusinessSearch)callback.Target).ID + ";";
            callback(CommunicationData);
            //  ViewState[this.ID + "ispager"] = null;
            //}
        }
        #endregion

        #region Receive
        private IWebPartParameters provider;
        //检索接口实例
        [ConnectionConsumer("数据")]
        public void RecvObject(IWebPartParameters prov)
        {
            provider = prov;
        }
        //获取接口实例中的数据
        private void GetData(object fieldValue)
        {
            BusinessSearchComponent com = new BusinessSearchComponent();
            if (fieldValue == null)
            {
                return;
            }
                //return;
            Dictionary<string, Dictionary<string, string>> dic = (Dictionary<string, Dictionary<string, string>>)fieldValue;
            if (dic.Count == 0)
            {
                return;
            }
               // return;
            foreach (var d in dic)
            {
                if (d.Key=="null")
                {
                    string where = "1=2";
                    ViewState[this.ID + "trswhere"] = where;
                    BindTableList("", where, 1);
                }
                else
                {
                    Dictionary<string, string> fields = com.GetCommunicationFields(TemplateModel, CommunicationID, d.Key);
                    if (fields != null && fields.Count > 0)
                    {
                        string where = string.Empty;
                        foreach (var field in fields)
                        {
                            where += " " + field.Value + "='" + d.Value[field.Key] + "' and";
                        }
                        where = where.Substring(0, where.Length - 4);
                        ViewState[this.ID + "trswhere"] = where;
                        string orderby = (ViewState[this.ID + "orderby"] == null ? "" : ViewState[this.ID + "orderby"].ToString());
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setcommication", "setcommunicationwhere('" + where + "');", true);
                        BindTableList(orderby, where, 1);

                        //自动点击第一行
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "defaultconetr", this.ID + "defaultclickonetr('" + this.ID + "');", true);

                    }
                }
            }

        }


        //重写OnPreRender：在Render前去接口实例获取值
        protected override void OnPreRender(EventArgs e)
        {
            if (provider != null)
                provider.GetParametersData(new ParametersCallback(GetData));
            base.OnPreRender(e);
        }

        #endregion

        protected void btntrsearch_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<string, Dictionary<string, string>> dclist = new Dictionary<string, Dictionary<string, string>>();
                Dictionary<string, string> dinfolist = new Dictionary<string, string>();

                ViewState[this.ID + "ispager"] = "Y";
                string trcontent = hidtrcontent.Value.Trim().TrimEnd(';');
                if (!string.IsNullOrEmpty(trcontent))
                {
                    string[] content = trcontent.Split(';');
                    foreach (string str in content)
                    {
                        if (!string.IsNullOrEmpty(str))
                        {
                            string[] strlist = str.Split('!');
                            string tkey = strlist[0].Replace("<tr>", "!").Replace("<td>", ";");
                            string tvalue = strlist[1].Replace("<tr>", "!").Replace("<td>", ";");
                            dinfolist.Add(tkey, tvalue);
                        }
                    }
                    dclist.Add(TemplateID, dinfolist);
                    this.CommunicationData = dclist;
                }
                else
                {
                    dinfolist.Add("null", "null");
                    dclist.Add("null", dinfolist);
                    this.CommunicationData = dclist;
                }
                string sorttext = "";
                if (ViewState[this.ID + "text"] != null)
                {
                    sorttext = ViewState[this.ID + "text"].ToString();
                }
                hidsorttext.Value = sorttext;
                string displaytype = "";
                if (ViewState[this.ID + "DisplayType"] != null)
                {
                    displaytype = ViewState[this.ID + "DisplayType"].ToString();
                }
                hiddisplaytype.Value = displaytype;
                hidtemplatename.Value = TemplateModel.TemplateName;

                ViewState[this.ID + "Send"] = string.Empty;
            }
            catch (Exception ee)
            {
                BaseComponent.Error("businesssearch.aspx:" + ee.Message);
            }
        }
        //导出
        protected void btnExport_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                try
                {
                    string fileName = "";
                    string filepath = string.Empty;
                    if (ViewState[this.ID + "templatemodel"] != null)
                    {
                        TemplateModel model = ViewState[this.ID + "templatemodel"] as TemplateModel;
                        //fileName = model.TemplateName + "-"+DateTime.Now.ToShortDateString()+".xlsx";
                        if (model != null)
                        {
                            string fwhere = string.Empty;
                            if (ViewState[this.ID + "trswhere"] != null)
                            {
                                fwhere = ViewState[this.ID + "trswhere"].ToString();
                            }
                            string sWhere = (ViewState[this.ID + "hidqueryinfo"] == null ? hidqueryinfo.Value.Trim() : ViewState[this.ID + "hidqueryinfo"].ToString());
                            if (!string.IsNullOrEmpty(sWhere))
                            {
                                if (!string.IsNullOrEmpty(fwhere))
                                {
                                    sWhere += " and " + fwhere;
                                }
                            }
                            else
                            {
                                sWhere = fwhere;
                            }
                            string orderby = (ViewState[this.ID + "orderby"] == null ? "" : ViewState[this.ID + "orderby"].ToString());
                            if (model.IsTruePaged)
                            {
                                DataTable dt = com.GetDataIsTruePaged(model, 0, 0, sWhere, orderby, false);
                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    filepath = com.ExportFile(model, dt, BusSearchTitle);
                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertexporterror", "layer.alert('未查询到数据！',8);", true);
                                }
                            }
                            else
                            {
                                List<SqlParameter> parameterlist = new List<SqlParameter>();
                                SqlParameter[] parameterss = null;
                                var parainfo = hidparainfo.Value.Trim().TrimEnd(';');
                                if (!string.IsNullOrEmpty(parainfo))//存储过程
                                {
                                    string[] paralist = parainfo.Split(';');
                                    foreach (string para in paralist)
                                    {
                                        if (!string.IsNullOrEmpty(para))
                                        {
                                            string[] plist = para.Split('!');
                                            SqlParameter sp = new SqlParameter(plist[0], plist[1]);
                                            parameterlist.Add(sp);
                                        }
                                    }
                                    if (parameterlist != null && parameterlist.Count > 0)
                                    {
                                        parameterss = parameterlist.ToArray();
                                    }
                                }
                                DataTable dt = new DataTable();

                                dt = com.GetDataIsNotTruePaged(model, sWhere, parameterss);

                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    DataTable binddttemp = com.GetDataTablePaged(dt, 1, dt.Rows.Count, orderby);
                                    filepath = com.ExportFile(model, dt,BusSearchTitle);

                                }
                                else
                                {
                                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertexporterror", "layer.alert('未查询到数据！',8);", true);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(filepath))
                        {
                            //Page.Response.ClearHeaders();
                            //Page.Response.Clear();
                            //Page.Response.Expires = 0;
                            //Page.Response.Buffer = true;
                            //Page.Response.AddHeader("Accept-Language", "zh-cn");
                            System.IO.FileStream files = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
                            long filesize = files.Length;
                            byte[] byteFile = null;
                            if (files.Length == 0)
                            {
                                byteFile = new byte[1];
                            }
                            else
                            {
                                byteFile = new byte[files.Length];
                            }
                            files.Read(byteFile, 0, (int)byteFile.Length);
                            files.Close();

                            if (File.Exists(filepath))
                            {
                                //删除生成文件
                                File.Delete(filepath);
                            }
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "loadfiletip", "layer.closeAll();", true);
                            Page.Response.ContentType = "application/octet-stream";
                            Page.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(model.TemplateName, System.Text.Encoding.UTF8) + "-" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx");
                            Page.Response.AddHeader("Content-Length", filesize.ToString());
                            Page.Response.BinaryWrite(byteFile);
                            Page.Response.Flush();
                            Page.Response.Close();
                            //Page.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
                            //Page.Response.ContentType = "application/octet-stream;charset=gbk";
                            //Page.Response.BinaryWrite(byteFile);
                            //HttpContext.Current.ApplicationInstance.CompleteRequest();
                            ////return;
                            //Page.Response.End();
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertexporterrored", "layer.alert('导出失败！',8);", true);
                        }
                    }
                }
                catch (Exception ee)
                {
                    BaseComponent.Error("业务查询导出：" + ee.Message);
                }
            });

        }


    }
}
