using CMICT.CSP.BLL.Components;
using CMICT.CSP.Model.InspectionModels;
using CMICT.CSP.Web.BusinessOnlineServices;
using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Web.UI.WebControls.WebParts;
using System.Linq;
using System.Web.UI;
using System.Collections.Generic;


namespace CMICT.CSP.Web.BusinessOnLine.InspectionBooking
{
    [ToolboxItemAttribute(false)]
    public partial class InspectionBooking : WebPart
    {

        BusinessOnLineComponent com = new BusinessOnLineComponent();

        private CUSTOMSINSPECTIONS customsInspections
        {
            get
            {
                return ViewState["CUSTOMSINSPECTIONS"] as CUSTOMSINSPECTIONS;
            }
            set
            {
                ViewState["CUSTOMSINSPECTIONS"] = value;
            }
        }

        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public InspectionBooking()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack)
                return;

            SPUser user = SPContext.Current.Web.CurrentUser;
            if (user != null)
            {
                Company company = com.GetCompanyInfo(BaseWebPart.GetCurrentUserLoginId());
                userName.Value = BaseWebPart.GetCurrentUserLoginId(1);
                txtcompany.Value = company.Costomer;
                linkMan.Value = SPContext.Current.Web.CurrentUser.Name;
                phone.Value = company.phone;
            }
            QGCostcoSoapClient client = new QGCostcoSoapClient();
            string preDate = DateTime.Now.ToString("yyyyMMddHHmmss");//Text1.Value;
            string errorMsg = "";
            string errorCode = "";
            string result = "";
            try
            {
                result = client.getBookTime(preDate, out errorMsg, out errorCode);
                BaseComponent.Info("WebService Paras getBookTime:preDate " + preDate + ";errorMsg " + errorMsg + ";errorCode " + errorCode);
                if (errorCode == "0")
                {
                    var appointment = com.GetAppointment(result);
                    beginTime.Value = DateTime.ParseExact(appointment.STARTTIME, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm");
                    endTime.Value = DateTime.ParseExact(appointment.ENDTIME, "yyyyMMddHHmmss", CultureInfo.InvariantCulture).ToString("yyyy-MM-dd HH:mm");
                    DateTime time = DateTime.ParseExact(appointment.INSPECTIONDATE, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
                    inspectionTime.Value = time.ToString("yyyy-MM-dd");

                    string[] weekdays = { "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六" };
                    string week = weekdays[Convert.ToInt32(time.DayOfWeek)];

                    inspectionDay.Value = week;

                    if (DateTime.Now < DateTime.ParseExact(appointment.STARTTIME, "yyyyMMddHHmmss", CultureInfo.InvariantCulture) || DateTime.Now > DateTime.ParseExact(appointment.ENDTIME, "yyyyMMddHHmmss", CultureInfo.InvariantCulture))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.alert('现在不是预约时间！');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.alert('今天不允许预约！');", true);
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.alert('加载预约时间失败啦！');", true);

                BaseComponent.Error(ex.ToString());
            }
        }

        protected void btn_Search_Click(object sender, EventArgs e)
        {
            string containerNo = boxNo.Value;
            string blNo = BLNo.Value;
            if (string.IsNullOrWhiteSpace(containerNo) && string.IsNullOrWhiteSpace(blNo))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.alert('请输入正确的箱号或提单号！');", true);
                return;
            }
            QGCostcoSoapClient client = new QGCostcoSoapClient();
            string errorMsg = "";
            string errorCode = "";
            string result = "";


            //string reserved = isReserved.Checked ? "Y" : "N";
            string reserved = "Y";
            result = client.reserve(containerNo, blNo, reserved, "", out errorMsg, out errorCode);
            BaseComponent.Info("WebService Paras reserve Y:containerNo " + containerNo + ";blNo " + blNo + ";reserved " + reserved + ";errorMsg " + errorMsg + ";errorCode " + errorCode);
            customsInspections = null;
            if (errorCode == "0")
            {
                customsInspections = com.GetCustomsInspections(result);
                //if (isReserved.Checked)
                if (customsInspections != null && customsInspections.CONTAINER.Count > 0)
                {
                    DataSourceList.Visible = false;
                    DataSourceList2.Visible = true;
                    DataSourceList2.DataSource = customsInspections.CONTAINER;
                    DataSourceList2.DataBind();
                }
                else
                {
                    DataSourceList.Visible = false;
                    DataSourceList2.Visible = true;
                    DataSourceList2.DataSource = null;
                    DataSourceList2.DataBind();
                }
                //if (customsInspections != null && customsInspections.CONTAINER.Count > 0 && !string.IsNullOrWhiteSpace(blNo) && string.IsNullOrWhiteSpace(containerNo) && !string.IsNullOrWhiteSpace(inspectionTime.Value) && !isReserved.Checked)
                if (customsInspections != null && customsInspections.CONTAINER.Count > 0 && reserved == "N")
                    btn_Booking.Visible = true;
                else
                    btn_Booking.Visible = false;
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.closeAll();", true);

            }
            else
            {
                reserved = "N";
                result = client.reserve(containerNo, blNo, reserved, "", out errorMsg, out errorCode);
                BaseComponent.Info("WebService Paras reserve N:containerNo " + containerNo + ";blNo " + blNo + ";reserved " + reserved + ";errorMsg " + errorMsg + ";errorCode " + errorCode);
                customsInspections = null;
                if (errorCode == "0")
                {
                    customsInspections = com.GetCustomsInspections(result);
                    if (customsInspections != null && customsInspections.CONTAINER.Count > 0)
                    {
                        DataSourceList.Visible = true;
                        DataSourceList2.Visible = false;

                        DataSourceList.DataSource = customsInspections.CONTAINER;
                        DataSourceList.DataBind();
                    }
                    else
                    {
                        DataSourceList.Visible = true;
                        DataSourceList2.Visible = false;

                        DataSourceList.DataSource = null;
                        DataSourceList.DataBind();
                    }
                    if (customsInspections != null && customsInspections.CONTAINER.Count > 0)
                        btn_Booking.Visible = true;
                    else
                        btn_Booking.Visible = false;
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsucclose", "layer.closeAll();", true);
                }
                else
                {
                    if (reserved == "N")
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.closeAll();layer.alert('如果无法查询出数据，请在“海关查验进度查询”中查看是否已经预约');", true);
                    else
                        ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.closeAll();layer.alert('" + errorMsg + "');", true);
                    btn_Booking.Visible = false;

                    DataSourceList.DataSource = null;
                    DataSourceList.DataBind();
                    DataSourceList2.DataSource = null;
                    DataSourceList2.DataBind();
                }

                //if (reserved == "N")
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.closeAll();layer.alert('如果无法查询出数据，请在“海关查验进度查询”中查看是否已经预约');", true);
                //else
                //    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.closeAll();layer.alert('" + errorMsg + "');", true);
                //btn_Booking.Visible = false;

                //DataSourceList.DataSource = null;
                //DataSourceList.DataBind();
                //DataSourceList2.DataSource = null;
                //DataSourceList2.DataBind();

            }


        }

        protected void btn_PreBooking_Click(object sender, EventArgs e)
        {
            if (customsInspections == null)
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.closeAll();layer.alert('没有可预约的箱！');", true);

                return;
            }
            lblCompany.Text = txtcompany.Value;
            string containers = "";
            foreach (var con in customsInspections.CONTAINER)
            {
                containers += con.CONTAINERNO + ",";
            }
            lblContainer.Text = containers.TrimEnd(',');
            lblCount.Text = customsInspections.CONTAINER.Count.ToString();
            lblDate.Text = inspectionTime.Value + " " + inspectionDay.Value;
            lblno.Text = BLNo.Value;

            ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "ensureBooking();", true);

        }
        protected void btn_Booking_Click(object sender, EventArgs e)
        {
            if (DateTime.Now < DateTime.Parse(beginTime.Value) || DateTime.Now > DateTime.Parse(endTime.Value))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.alert('已经超过预约时间啦！');", true);
                return;
            }


            //2次验证
            string containerNo = boxNo.Value;
            string blNo = BLNo.Value;
            QGCostcoSoapClient client = new QGCostcoSoapClient();
            string errorMsg = "";
            string errorCode = "";
            string result = "";


            string reserved = "N";

            result = client.reserve(containerNo, blNo, reserved, "", out errorMsg, out errorCode);
            BaseComponent.Info("WebService Paras reserve:containerNo " + containerNo + ";blNo " + blNo + ";reserved " + reserved + ";errorMsg " + errorMsg + ";errorCode " + errorCode);

            if (errorCode == "0")
            {
                CUSTOMSINSPECTIONS customsInspectionsCheck = com.GetCustomsInspections(result);
                if (customsInspectionsCheck.CONTAINER.Count != customsInspections.CONTAINER.Count)
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.alert('预约失败，请确认部分箱是否已处理！');", true);
                    return;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.alert('预约失败，请确认部分箱是否已处理！');", true);
                return;
            }
            //

            string reservedDate = DateTime.Now.ToString("yyyyMMddHHmmss"); //Text1.Value;// 
            string customer = lblCompany.Text;
            string linkman = txtlinkman.Value;
            string phone = txtphone.Value;

            if (string.IsNullOrWhiteSpace(linkman) || string.IsNullOrWhiteSpace(phone))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.alert('请输入联系人和联系电话！');", true);
                return;
            }

            //Containers containers = new Containers();
            //containers.ContainerNo = new System.Collections.Generic.List<Model.InspectionModels.Container>();
            //foreach (var no in customsInspections.CONTAINER)
            //{
            //    Model.InspectionModels.Container container = new Model.InspectionModels.Container();
            //    container.ContainerNo = no.CONTAINERNO;
            //    containers.ContainerNo.Add(container);
            //}

            string containers = "<CONTAINERS>";
            foreach (var no in customsInspections.CONTAINER)
            {
                containers += "<CONTAINERNO>" + no.CONTAINERNO + "</CONTAINERNO>";
            }

            containers += "</CONTAINERS>";

            client.reserveContainers(containers, reservedDate, customer, linkman, phone, out errorMsg, out errorCode);
            BaseComponent.Info("WebService Paras reserve:containers " + containers + ";reservedDate " + reservedDate + ";customer " + customer + ";errorMsg " + errorMsg + ";errorCode " + errorCode);
            if (errorCode == "0")
            {
                //Page.Response.Redirect("InspectionSearch.aspx?blno=" + lblno.Text + "", true);
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.alert('恭喜您，预约成功，请查看预约结果！',9);", true);
                this.isReserved.Checked = true;
                btn_Search_Click(null, null);

            }
            else
            {
                if (errorMsg == "The reservation is full") errorMsg = "今天预约数量已经满啦，请下个工作日再来预约！";
                if (errorMsg == "Illegal reserveDate") errorMsg = "今天不允许预约！";
                if (errorMsg == "parameters error") errorMsg = "系统发生了参数错误！";
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "ensureBooking();layer.alert('" + errorMsg + "');", true);

            }
        }

    }
}
