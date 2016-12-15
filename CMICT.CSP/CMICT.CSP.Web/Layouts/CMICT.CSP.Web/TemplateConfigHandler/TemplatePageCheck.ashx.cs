using System;
using System.Web;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI;
using System.Text;
using CMICT.CSP.BLL.Components;

namespace CMICT.CSP.Web.Layouts.CMICT.CSP.Web.TemplateConfigHandler
{
    public partial class TemplatePageCheck : IHttpHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            string result = string.Empty;
            ////在此处写入您的处理程序实现。
            var opertype = context.Request["OperType"];
            switch (opertype)
            {
                case "CheckTemplateName": result = CheckTemplateName(context); break;
                case "CheckSourceName": result = CheckSourceName(context); break;
                default: break;
            }
            
            context.Response.Write(result);

        }

        public string CheckTemplateName(HttpContext context)
        {
            string result = "false";
            string TemplateName = context.Server.UrlDecode(context.Request["TemplateName"]);
            string TemplateID = context.Request["TemplateID"];
            if (!string.IsNullOrEmpty(TemplateName))
            {
                TemplateConfigComponent tc = new TemplateConfigComponent();
                if (tc.GetTemplateNameIsExists(TemplateName, TemplateID))
                {
                    result = "true";
                }
            }
            return result;
        }

        public string CheckSourceName(HttpContext context)
        {
            string result = "false";
            string SourceName = context.Server.UrlDecode(context.Request["SourceName"]);
            string SourceID = context.Request["SourceID"];
            if (!string.IsNullOrEmpty(SourceName))
            {
                ConnectionConfigComponent tc = new ConnectionConfigComponent();
                if (tc.GetSourceNameIsExists(SourceName, SourceID))
                {
                    result = "true";
                }
            }
            return result;
        }

       
    }



}
