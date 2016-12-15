using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.Web.UI.WebControls.WebParts;
using System.Data;
using CMICT.CSP.BLL;
using CMICT.CSP.Model;
using CMICT.CSP.BLL.Components;

namespace CMICT.CSP.Web.BusinessSearch.PagesEventReceiver
{
    /// <summary>
    /// 列表项事件
    /// </summary>
    public class PagesEventReceiver : SPItemEventReceiver
    {


        public override void ItemDeleting(SPItemEventProperties properties)
        {
            if (properties.List.BaseTemplate != (SPListTemplateType)850)
                return;
            EventFiringEnabled = false;
            DeletePageLink(properties.ListItem.File, properties.Web);
            //base.ItemDeleted(properties);
            EventFiringEnabled = true;
        }


        /// <summary>
        /// 已更新项.
        /// </summary>
        public override void ItemUpdated(SPItemEventProperties properties)
        {
            if (properties.List.BaseTemplate != (SPListTemplateType)850)
                return;
            base.ItemUpdated(properties);

            EventFiringEnabled = false;
            try
            {
                DeletePageLink(properties.ListItem.File, properties.Web);
                AddPageLink(properties.ListItem, properties.Web);
                //base.ItemUpdated(properties);
                //根据是否配置页面信息更新模板状态
            }
            catch (Exception ex)
            {
                BaseComponent.Error("template pages update error:" + ex.ToString());
            }
            //try
            //{
            //    SPFile file = properties.ListItem.File;
            //    if (file.Level == SPFileLevel.Checkout)
            //    {
            //        file.CheckIn("auto check in");
            //    }
            //    if (file.Level != SPFileLevel.Published)
            //    {
            //        file.Publish("auto publish");
            //    }


            //}
            //catch (Exception ex)
            //{
            //    BaseComponent.Error("auto publish error:" + ex.ToString());
            //}
            EventFiringEnabled = true;
        }




        public void DeletePageLink(SPFile page, SPWeb web)
        {

            var wm = page.GetLimitedWebPartManager(PersonalizationScope.Shared);
            var webparts = wm.WebParts;
            foreach (WebPart webpart in webparts)
            {
                if (webpart.WebBrowsableObject.ToString() == "CMICT.CSP.Web.BusinessSearch.BusinessSearch.BusinessSearch")
                {
                    CMICT.CSP.Web.BusinessSearch.BusinessSearch.BusinessSearch BS = (BusinessSearch.BusinessSearch)webpart;
                    if (string.IsNullOrEmpty(BS.TemplateID))
                        continue;
                    Guid templateID = new Guid(BS.TemplateID);
                    string selectSql = "select templateid from BS_TEMPLATE_PAGES where Url=N'" + page.ServerRelativeUrl + "'";
                    string sql = "delete from BS_TEMPLATE_PAGES where Url=N'" + page.ServerRelativeUrl + "'";
                    DbHelperSQL dbhelper = new DbHelperSQL();

                    DataTable templateDt = dbhelper.ExecuteTable(CommandType.Text, selectSql);

                    dbhelper.ExecuteNonQuery(CommandType.Text, sql);


                    if (templateDt != null && templateDt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in templateDt.Rows)
                        {
                            RefreshTemplateStatus(Convert.ToString(dr["templateid"]));
                        }
                    }
                }
            }
        }

        public void AddPageLink(SPListItem item, SPWeb web)
        {
            SPFile page = item.File;

            var wm = page.GetLimitedWebPartManager(PersonalizationScope.Shared);
            var webparts = wm.WebParts;
            foreach (WebPart webpart in webparts)
            {
                if (webpart.WebBrowsableObject.ToString() == "CMICT.CSP.Web.BusinessSearch.BusinessSearch.BusinessSearch")
                {
                    CMICT.CSP.Web.BusinessSearch.BusinessSearch.BusinessSearch BS = (BusinessSearch.BusinessSearch)webpart;
                    if (BS.TemplateID == null)
                        return;
                    Guid templateID = new Guid(BS.TemplateID);
                    BS_TEMPLATE_PAGESBLL bll = new BS_TEMPLATE_PAGESBLL();
                    BS_TEMPLATE_PAGES model = new BS_TEMPLATE_PAGES();

                    model.Author = item["Author"].ToString();
                    model.Created = Convert.ToDateTime(item["Created"]);
                    model.Editor = item["Editor"].ToString();
                    model.Modified = Convert.ToDateTime(item["Modified"]);

                    model.PageName = item.Web.Title + "-" + item.Title;
                    model.TemplateID = templateID;
                    model.Url = item.File.ServerRelativeUrl;

                    bll.Add(model);

                    RefreshTemplateStatus(BS.TemplateID);
                }
            }
        }

        public void RefreshTemplateStatus(string id)
        {
            string sql = "select count(*) from BS_TEMPLATE_PAGES where templateid='" + id + "'";
            DbHelperSQL db = new DbHelperSQL();
            int count = (int)db.ExecuteScalar(CommandType.Text, sql);
            if (count == 0)
            {
                string updateSql = "update BS_TEMPLATE_MAIN set TemplateStatus='FREE' where templateid='" + id + "'";
                db.ExecuteNonQuery(CommandType.Text, updateSql);
            }
            else
            {
                string updateSql = "update BS_TEMPLATE_MAIN set TemplateStatus='ENABLE' where templateid='" + id + "'";
                db.ExecuteNonQuery(CommandType.Text, updateSql);
            }
        }

    }
}