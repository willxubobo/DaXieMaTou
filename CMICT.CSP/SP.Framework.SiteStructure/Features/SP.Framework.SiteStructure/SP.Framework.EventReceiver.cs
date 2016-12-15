using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using System.Xml.Linq;
using System.Reflection;
using System.Collections.Generic;
using Microsoft.SharePoint.Utilities;
using NET.Framework.Common.ExcelHelper;
using System.Data;
using CamlexNET;
using SP.Framework.DAL;

namespace SP.Framework.SiteStructure.Features.SP.Framework.SiteStructure
{
    /// <summary>
    /// 此类用于处理在激活、停用、安装、卸载和升级功能的过程中引发的事件。
    /// </summary>
    /// <remarks>
    /// 附加到此类的 GUID 可能会在打包期间使用，不应进行修改。
    /// </remarks>

    [Guid("72c1f89a-10ab-4c64-88da-9724ce571224")]
    public class SPFrameworkEventReceiver : SPFeatureReceiver
    {
        // 取消对以下方法的注释，以便处理激活某个功能后引发的事件。

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            //SPSite site = (SPSite)properties.Feature.Parent;
            using (SPWeb web = (SPWeb)properties.Feature.Parent)//site.RootWeb;
            {
                List<string> fields = new List<string>();
                foreach (SPField field in web.Fields)
                {
                    if (field.Group.Equals("Custom Lookup"))
                    {
                        fields.Add(field.InternalName);
                    }
                }

                foreach (string fieldName in fields)
                {
                    SPField field = web.Fields.GetField(fieldName);
                    XDocument fieldSchema = XDocument.Parse(field.SchemaXml);
                    XElement root = fieldSchema.Root;
                    if (root.Attribute("List") != null)
                    {
                        // 得到List对应的url
                        string listurl = root.Attribute("List").Value;

                        SPFolder listFolder = web.GetFolder(listurl);
                        SPFieldLookup lookupField = field as SPFieldLookup;
                        Guid g = Guid.Empty;
                        if (!Guid.TryParse(lookupField.LookupList, out g))
                        {
                            try
                            {
                                Type t = typeof(SPField);
                                MethodInfo mInfo = t.GetMethod("SetFieldAttributeValue", BindingFlags.NonPublic | BindingFlags.Instance);
                                mInfo.Invoke(lookupField, new object[] { "List", "" });

                                //mInfo.Invoke(lookupField, new object[] { "SourceID", web.ID.ToString() });
                                //lookupField.LookupList = listFolder.ParentListId.ToString();
                                lookupField.Update();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                       
                    }

                }
               
                InitSPList(web);
            }
        }

        public void InitSPList(SPWeb web)
        {
            string path = SPUtility.GetCurrentGenericSetupPath(@"TEMPLATE\LAYOUTS\SP.Framework.SiteStructure\SPList_Init.xlsx");
            Dictionary<string, DataTable> dicTable = EPPlus.ExcelToDataTable(path);

            string typeListName = "SYS_LOOKUP_TYPES";
            DataTable typeTable = dicTable[typeListName];
            SPList typeList = web.Lists.TryGetList(typeListName);

            foreach (DataRow dr in typeTable.Rows)
            {
                SPListItem item = typeList.Items.Add();
                item["APP_CODE"] = dr["APP_CODE"];
                item["SYSCODE_FLAG"] = dr["SYSCODE_FLAG"];
                item["LOOKUP_CODE"] = dr["LOOKUP_CODE"];
                item["LOOKUP_NAME"] = dr["LOOKUP_NAME"];
                item["REMARK"] = dr["REMARK"];
                item["ENABLE"] = dr["ENABLE"];
                item.Update();
            }

            string valueListName = "SYS_LOOKUP_VALUES";
            DataTable valueTable = dicTable[valueListName];
            SPList valueList = web.Lists.TryGetList(valueListName);

            foreach (DataRow dr in valueTable.Rows)
            {
                SPListItem item = valueList.Items.Add();
                string caml = Camlex.Query().Where(x => (string)x["LOOKUP_CODE"] == dr["LOOKUP_CODE"].ToString()).ToString();
                SPQuery query = new SPQuery();
                query.Query = caml;

                var code = SPHelper.GetListItems(web, typeListName, query);

                SPFieldLookupValue value = new SPFieldLookupValue(code[0]["ID"].ToString());
                item["LOOKUP_CODE_LINE"] = value;
                item["LOOKUP_VALUE"] = dr["LOOKUP_VALUE"];
                item["LOOKUP_VALUE_NAME"] = dr["LOOKUP_VALUE_NAME"];
                item["REMARK"] = dr["REMARK"];
                item["ENABLE"] = dr["ENABLE"];
                item.Update();
            }
        }

        // 取消对以下方法的注释，以便处理在停用某个功能前引发的事件。

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //    SPWeb web = (SPWeb)properties.Feature.Parent;
        //    List<string> fields = new List<string>();
        //    foreach (SPField field in web.Fields)
        //    {
        //        if (field.Group.Equals("Custom Lookup"))
        //        {
        //            fields.Add(field.InternalName);
        //        }
        //    }
        //    foreach (string fieldName in fields)
        //    {
        //        SPField field = web.Fields.GetField(fieldName);
        //        field.Delete();
        //    }
        //}


        // 取消对以下方法的注释，以便处理在安装某个功能后引发的事件。

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // 取消对以下方法的注释，以便处理在卸载某个功能前引发的事件。

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // 取消对以下方法的注释，以便处理在升级某个功能时引发的事件。

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
