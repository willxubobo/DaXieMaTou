using CMICT.CSP.BLL;
using CMICT.CSP.BLL.Components;
using CMICT.CSP.Model;
using System;
using System.ComponentModel;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace CMICT.CSP.Web.TemplateConfig.DisplayConfig
{
    [ToolboxItemAttribute(false)]
    public partial class DisplayConfig : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public DisplayConfig()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        DisplayConfigComponent dc = new DisplayConfigComponent();
        DisplayConfigComponent dcserver = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrEmpty(Page.Request.QueryString["sourceID"]))
                {
                    hidsourceid.Value = Page.Request.QueryString["sourceID"];
                    string sourceid = hidsourceid.Value.Trim();
                    BS_DATASOURCEBLL bll = new BS_DATASOURCEBLL();
                    CMICT.CSP.Model.BS_DATASOURCE model = bll.GetModel(Guid.Parse(sourceid));
                    if (model != null)
                    {
                        dcserver = new DisplayConfigComponent(model.SourceIP, model.UserName, model.Password, model.DBName);
                        if (model.ObjectType.ToLower() == "proc")
                        {
                            hiddbtype.Value = "proc";
                        }
                    }
                }
                try
                {
                    if (!string.IsNullOrEmpty(Page.Request.QueryString["templateID"]))
                    {
                        hidTemplateID.Value = Page.Request.QueryString["templateID"];


                        BindDisplayType();
                        BindColumnIfEdit(hidTemplateID.Value);
                        BindColumnList();
                        BindCalColumnList();
                        BindDecimal();//绑定小数位数
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertnomodel", "layer.alert('未获取到模板信息，请联系管理员！',8);", true);
                    }
                }catch(Exception ex){
                    BaseComponent.Error("DisplayConfig-pageload:"+ex.Message);
                    ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "alertnomodelerror", "layer.alert('数据源连接失败，请联系管理员！',8);", true);
                }
            }
        }
        //绑定小数位数
        protected void BindDecimal()
        {
            //绑定计算方式
            DataTable dtt = BaseComponent.GetLookupValuesByType("BS_DECIMAL_COUNT");
            ddlDecimal.DataSource = dtt;
            ddlDecimal.DataTextField = "LOOKUP_VALUE_NAME";
            ddlDecimal.DataValueField = "LOOKUP_VALUE";
            ddlDecimal.DataBind();
            ddlComDecimal.DataSource = dtt;
            ddlComDecimal.DataTextField = "LOOKUP_VALUE_NAME";
            ddlComDecimal.DataValueField = "LOOKUP_VALUE";
            ddlComDecimal.DataBind();
        }
        //判断是编辑还是新增
        protected void BindColumnIfEdit(string TemplateID)
        {
            if (!string.IsNullOrEmpty(TemplateID))
            {
                DataTable datacolumn = dc.GetColumnListByTemplateID(Guid.Parse(TemplateID));
                if (datacolumn != null && datacolumn.Rows.Count > 0)
                {
                    ViewState["datacolumn"] = datacolumn;
                    hidopertype.Value = "edit";
                    ddlDisplayType.SelectedValue = Convert.ToString(datacolumn.Rows[0]["DiaplayType"]);//编辑时选中展示类型
                    txtDisplayType.Text = ddlDisplayType.SelectedValue;
                    txtColumnSize.Text = Convert.ToString(datacolumn.Rows[0]["ColumnSize"]);

                    //绑定排序信息
                    DataTable sorttable = dc.GetSortListByTemplateID(Guid.Parse(TemplateID));
                    if (sorttable != null && sorttable.Rows.Count > 0)
                    {
                        string sortinfo = string.Empty;
                        foreach (DataRow dr in sorttable.Rows)
                        {
                            sortinfo += dr["SortColumn"].ToString() + "|" + dr["Type"].ToString() + "|" + dr["Sequence"].ToString() + ";";
                        }
                        hidSortContentEdit.Value = sortinfo.TrimEnd(';');
                        //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setsortvalue", "bindsortlist();", true);
                    }
                    if (ddlDisplayType.SelectedValue == "COLUMN" || ddlDisplayType.SelectedValue == "ROW")
                    {
                        //绑定计算列信息
                        DataTable caltable = dc.GetCalListByTemplateID(Guid.Parse(TemplateID));
                        if (caltable != null && caltable.Rows.Count > 0)
                        {
                            string calinfo = string.Empty;
                            foreach (DataRow dr in caltable.Rows)
                            {
                                calinfo += dr["ComputeColumn"].ToString().Replace("[", "").Replace("]", "") + "|" + dr["DisplayName"].ToString() + "|" + dr["MergeColumnName"].ToString() + "|" + dr["Sequence"].ToString() + "|" + dr["DecimalCount"].ToString() + ";";
                            }
                            hidCalContentEdit.Value = calinfo.TrimEnd(';');
                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setsortvalue", "bindcallist();", true);
                        }
                    }
                    if (ddlDisplayType.SelectedValue == "ROW")
                    {
                        DataTable gbtable = dc.GetGroupByListByTemplateID(Guid.Parse(TemplateID));
                        if (gbtable != null && gbtable.Rows.Count > 0)
                        {
                            string calinfo = string.Empty;
                            foreach (DataRow dr in gbtable.Rows)
                            {
                                calinfo += dr["Columns"].ToString() + "|" + dr["ComputeColumn"].ToString() + "|" + dr["Location"].ToString() + "$";
                            }

                            hidgroupbyeditcontent.Value = calinfo.TrimEnd('$');
                            //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "setsortvalue", "bindcallist();", true);
                        }
                    }
                    BindDataByDType();//根据展现类型绑定相应数据
                }
                else
                {
                    hidopertype.Value = "add";
                }
            }
        }
        //根据sourceid获取数据源信息
        protected void BindColumnList()
        {
            string sourceid = hidsourceid.Value.Trim();
            BS_DATASOURCEBLL bll = new BS_DATASOURCEBLL();
            CMICT.CSP.Model.BS_DATASOURCE model = bll.GetModel(Guid.Parse(sourceid));
            if (model != null)
            {
                DataTable dt = dcserver.GetColumnListByType(model.ObjectType, model.ObjectName);
                ColumnList.DataSource = dt;
                ColumnList.DataBind();
                //绑定排序中列名下拉
                ddlColumnName.DataSource = dt;
                ddlColumnName.DataTextField = "name";
                ddlColumnName.DataValueField = "name";
                ddlColumnName.DataBind();
                ddlColumnName.Items.Insert(0, new ListItem("请选择", ""));
                //if (ddlDisplayType.SelectedValue == "ROW")
                //{
                //    rptgroupcol.DataSource = dt;
                //    rptgroupcol.DataBind();
                //}
            }
        }
        //绑定计算列信息
        protected void BindCalColumnList()
        {
            //if (ddlDisplayType.SelectedValue == "COLUMN" || ddlDisplayType.SelectedValue == "ROW")
            //if (!string.IsNullOrEmpty(ddlDisplayType.SelectedValue))
            //{
            string sourceid = hidsourceid.Value.Trim();
            BS_DATASOURCEBLL bll = new BS_DATASOURCEBLL();
            CMICT.CSP.Model.BS_DATASOURCE model = bll.GetModel(Guid.Parse(sourceid));
            if (model != null)
            {
                DataTable dt = dcserver.GetColumnListByTypeOnlyNum(model.ObjectType, model.ObjectName);
                //if (ddlDisplayType.SelectedValue == "COLUMN")
                //{
                rptcallist.DataSource = dt;
                rptcallist.DataBind();
                //}
                //if (ddlDisplayType.SelectedValue == "ROW")
                //{
                //string temid = hidTemplateID.Value.Trim();
                //QueryConfigComponent qcc = new QueryConfigComponent();
                //DataTable gcdt = qcc.GetUserQueryColListByTemplateID(Guid.Parse(temid), 1);
                //if (gcdt != null && gcdt.Rows.Count > 0)
                //{
                //    ddlgroupcalcol.DataSource = dt;
                //    ddlgroupcalcol.DataTextField = "name";
                //    ddlgroupcalcol.DataValueField = "name";
                //    ddlgroupcalcol.DataBind();
                    //ddlgroupcalcol.Items.Insert(0, new ListItem("请选择", ""));
                //}
                //绑定计算方式
                DataTable dtt = BaseComponent.GetLookupValuesByType("BS_GROUPY_COMPUTERTYPE");
                ddlcomputertype.DataSource = dtt;
                ddlcomputertype.DataTextField = "LOOKUP_VALUE_NAME";
                ddlcomputertype.DataValueField = "LOOKUP_VALUE";
                ddlcomputertype.DataBind();
                ddlcomputertype.Items.Insert(0, new ListItem("请选择", ""));
                // }
            }
            //}

        }
        //绑定展现方式
        protected void BindDisplayType()
        {
            //BaseComponent bc = new BaseComponent();
            DataTable dt = BaseComponent.GetLookupValuesByType("BS_DISPLAY_TYPE");
            ddlDisplayType.DataSource = dt;
            ddlDisplayType.DataTextField = "LOOKUP_VALUE_NAME";
            ddlDisplayType.DataValueField = "LOOKUP_VALUE";
            ddlDisplayType.DataBind();
            ddlDisplayType.Items.Insert(0, new ListItem("请选择", ""));
        }

        //展现方式改变时执行方法
        protected void BindDataByDType()
        {
            //if (ddlDisplayType.SelectedValue != "")
            //{
            //    //BindColumnList();

            //    //BindCalColumnList();//绑定计算列
            //}
            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "changetypedses", "controlshow('" + ddlDisplayType.SelectedValue + "');bindsortlist();bindcallist();bindgroupbylist();", true);
        }
        protected void ddlDisplayType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindDataByDType();

        }
        protected void lbtnPrev_Click(object sender, EventArgs e)
        {
            string SourceID = hidsourceid.Value;
            string TemplateID = Page.Request.QueryString["templateID"];
            if (!string.IsNullOrEmpty(TemplateID))
            {
                string edit = string.Empty;
                if (!string.IsNullOrEmpty(Page.Request.QueryString["type"]))
                {
                    edit = "&type=edit";
                }
                Page.Response.Redirect("DataSourceConfig.aspx?templateID=" + TemplateID + "&sourceID=" + SourceID+edit);
            }
        }

        //protected override void Render(System.Web.UI.HtmlTextWriter writer)
        //{
        //    Page.ClientScript.RegisterForEventValidation(this.ddlgroupcalcol.UniqueID, "4");
        //    base.Render(writer);
        //}

        //保存
        protected void lbtnSave_Click(object sender, EventArgs e)
        {
            string displaytype = ddlDisplayType.SelectedValue;

            if (!string.IsNullOrEmpty(hidTemplateID.Value))
            {
                bool iddel = true;
                bool addrow = true;
                if (hidopertype.Value == "edit")//如果是更新，先删除之前的配置信息
                {
                    iddel = dc.DeleteTemInfoByTemplateID(hidTemplateID.Value);
                }
                //保存报表展现类型
                bool adddisplaytype = true;
                adddisplaytype = dc.UpdateDisplayTypeByTemplateID(Guid.Parse(hidTemplateID.Value), displaytype);
                #region 列配置与排序配置保存
                bool addcolumn = true;//标识列配置是否添加成功
                bool addsort = true;//标识排序配置是否添加成功
                bool addgrid = true;//标识田字型配置是否添加成功
                bool addcal = true;//计算列
                string ColumnConfigContent = hidColumnContent.Value.TrimEnd(';');//列配置内容
                string SortContent = hidSortContent.Value.TrimEnd(';');//排序配置内容
                string[] ColumnList = ColumnConfigContent.Split(';');
                BS_TEMPLATE_COLUMNSBLL btcbll = new BS_TEMPLATE_COLUMNSBLL();
                double addm = 10;
                foreach (string cl in ColumnList)//添加列配置到数据库
                {
                    addm++;
                    string[] ColumnInfoList = cl.Split('|');
                    BS_TEMPLATE_COLUMNS btc = new BS_TEMPLATE_COLUMNS();
                    btc.TemplateID = Guid.Parse(hidTemplateID.Value);
                    btc.ColumnName = ColumnInfoList[0];
                    btc.Visiable = bool.Parse(ColumnInfoList[1]);
                    btc.DisplayName = ColumnInfoList[2];
                    if (!string.IsNullOrEmpty(ColumnInfoList[3]))
                    {
                        btc.Sequence = decimal.Parse(ColumnInfoList[3]);
                    }
                    else
                    {
                        btc.Sequence = -1;
                    }
                    btc.MergeColumnName = ColumnInfoList[4];
                    btc.ColumnDataType = ColumnInfoList[5];
                    btc.Created = DateTime.Now.AddMilliseconds(addm); ;
                    btc.Modified = DateTime.Now;
                    btc.Author = BaseWebPart.GetCurrentUserLoginId();
                    btc.Editor = BaseWebPart.GetCurrentUserLoginId();
                    addcolumn = btcbll.Add(btc);
                }
                //添加排序
                if (!string.IsNullOrEmpty(SortContent))
                {
                    BS_TEMPLATE_SORTBLL btsbll = new BS_TEMPLATE_SORTBLL();
                    string[] sortlist = SortContent.Split(';');
                    addm = 10;
                    foreach (string sort in sortlist)
                    {
                        addm++;
                        string[] sortinfo = sort.Split('|');
                        BS_TEMPLATE_SORT model = new BS_TEMPLATE_SORT();
                        model.TemplateID = Guid.Parse(hidTemplateID.Value);
                        model.SortColumn = sortinfo[0];
                        model.Type = (sortinfo[1] == "升序" ? "asc" : "desc");
                        model.Sequence = decimal.Parse(sortinfo[2]);
                        model.Created = DateTime.Now.AddMilliseconds(addm); ;
                        model.Modified = DateTime.Now;
                        model.Author = BaseWebPart.GetCurrentUserLoginId();
                        model.Editor = BaseWebPart.GetCurrentUserLoginId();
                        addsort = btsbll.Add(model);
                    }
                }
                #endregion
                if (displaytype == "GRID")//田字型
                {
                    string columnsize = txtColumnSize.Text.Trim();
                    addgrid = dc.UpdatePageSizeByTemplateID(Guid.Parse(hidTemplateID.Value), decimal.Parse(columnsize));
                }
                #region 列配置
                if (displaytype == "COLUMN" || displaytype == "ROW")//列配置
                {
                    var calcontent = hidCalContent.Value.TrimEnd(';');
                    if (!string.IsNullOrEmpty(calcontent))
                    {
                        string[] CalList = calcontent.Split(';');
                        BS_COMPUTEBLL combll = new BS_COMPUTEBLL();
                        addm = 10;
                        foreach (string cl in CalList)//添加计算列到数据库
                        {
                            addm++;
                            string[] CalInfoList = cl.Split('|');
                            BS_COMPUTE btc = new BS_COMPUTE();
                            btc.TemplateID = Guid.Parse(hidTemplateID.Value);
                            if (hiddbtype.Value.Trim() == "proc")
                            {
                                btc.ComputeColumn = CalInfoList[0];
                            }
                            else
                            {
                                btc.ComputeColumn =  CalInfoList[0];
                            }
                            btc.DisplayName = CalInfoList[1];
                            btc.MergeColumnName = CalInfoList[2];
                            if (!string.IsNullOrEmpty(CalInfoList[3]))
                            {
                                btc.Sequence = decimal.Parse(CalInfoList[3]);
                            }
                            else
                            {
                                btc.Sequence = -1;
                            }
                            btc.DecimalCount = Convert.ToInt32(CalInfoList[4]);
                            btc.Created = DateTime.Now.AddMilliseconds(addm); ;
                            btc.Modified = DateTime.Now;
                            btc.Author = BaseWebPart.GetCurrentUserLoginId();
                            btc.Editor = BaseWebPart.GetCurrentUserLoginId();
                            addcal = combll.Add(btc);
                        }
                    }
                }
                #endregion
                if (displaytype == "ROW")//行汇总
                {
                    string rowcontent = hidgroupbycontent.Value.Trim().TrimEnd('$');
                    if (!string.IsNullOrEmpty(rowcontent))
                    {
                        string[] gblist = rowcontent.Split('$');
                        BS_GROUPBYBLL gbll = new BS_GROUPBYBLL();
                        addm = 10;
                        foreach (string gbinfo in gblist)
                        {
                            addm++;
                            string[] CalInfoList = gbinfo.Split('|');
                            BS_GROUPBY btc = new BS_GROUPBY();
                            btc.TemplateID = Guid.Parse(hidTemplateID.Value);
                            btc.Columns = CalInfoList[0].TrimEnd(',');
                            btc.Location = CalInfoList[2];
                            btc.ComputeColumn = CalInfoList[1];
                            btc.Sequence = 0;
                            btc.SQL = "";
                            btc.Created = DateTime.Now.AddMilliseconds(addm);
                            btc.Modified = DateTime.Now;
                            btc.Author = BaseWebPart.GetCurrentUserLoginId();
                            btc.Editor = BaseWebPart.GetCurrentUserLoginId();
                            addrow = gbll.Add(btc);
                        }
                    }
                }
                if (addcolumn && addsort && addgrid && addcal && adddisplaytype && iddel && addrow)
                {
                    string TemplateID = hidTemplateID.Value;
                    string SourceID = hidsourceid.Value;
                    if (!string.IsNullOrEmpty(TemplateID) && !string.IsNullOrEmpty(SourceID))
                    {
                        BusinessSearchComponent bsbll = new BusinessSearchComponent();
                        bsbll.RemoveTemplateByGuid(TemplateID);
                        string edit = string.Empty;
                        if (!string.IsNullOrEmpty(Page.Request.QueryString["type"]))
                        {
                            edit = "&type=edit";
                        }
                        Page.Response.Redirect("QueryConfig.aspx?templateID=" + TemplateID + "&sourceID=" + SourceID+edit);
                    }
                    //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "subsuc", "layer.alert('展示配置成功！');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "suberror", "layer.alert('未获取到模板编号！');", true);
            }
        }

        protected void ColumnList_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {

            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                if (ViewState["datacolumn"] != null)//编辑绑定值
                {
                    DataTable datacolumn = ViewState["datacolumn"] as DataTable;
                    Literal cname = (Literal)e.Item.FindControl("lblName");
                    DataRow[] dr = datacolumn.Select("ColumnName='" + cname.Text.Trim() + "'");
                    if (dr != null && dr.Length > 0)
                    {
                        DataRow cdr = dr[0];
                        CheckBox isc = (CheckBox)e.Item.FindControl("chkIsShow");
                        isc.Checked = true;
                        TextBox showname = (TextBox)e.Item.FindControl("txtShowName");
                        showname.Text = cdr["DisplayName"].ToString();
                        TextBox mergename = (TextBox)e.Item.FindControl("txtMergeName");
                        mergename.Text = cdr["MergeColumnName"].ToString();
                        TextBox sort = (TextBox)e.Item.FindControl("txtSort");
                        if (cdr["Sequence"] != null)
                        {
                            sort.Text = cdr["Sequence"].ToString();
                        }
                    }
                }
            }

        }
    }
}
