using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using System.Configuration;
using System.Reflection;

namespace CMICT.CSP.Web.Features.CMICT.CategoryEventReceiver
{
    /// <summary>
    /// 此类用于处理在激活、停用、安装、卸载和升级功能的过程中引发的事件。
    /// </summary>
    /// <remarks>
    /// 附加到此类的 GUID 可能会在打包期间使用，不应进行修改。
    /// </remarks>

    [Guid("7441b3e4-4879-4984-ad74-eef17ef49231")]
    public class CMICTEventReceiver : SPFeatureReceiver
    {
        private static string webName = ConfigurationSettings.AppSettings["CMICTSPWebUrl"].ToString();

        private static string usertypeListName = webName+"/Lists/USER_LOOKUP_TYPES";
        private static string uservalueListName = webName+"/Lists/USER_LOOKUP_VALUES";
        private static string assemblyName1 = "CMICT.CSP.Web, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3c0c7570c89dcf01";
        


        // 取消对以下方法的注释，以便处理激活某个功能后引发的事件。

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite site = (SPSite)properties.Feature.Parent;
            using (SPWeb web = site.OpenWeb(webName))
            {
                SPList typeList=web.GetList(usertypeListName);
                string className1 = "CMICT.CSP.Web.CategoryEvents.LookupTypesEvent";

                SPEventReceiverDefinition eventReceiverDefinition = typeList.EventReceivers.Add();
                eventReceiverDefinition.Class = className1; // String
                eventReceiverDefinition.Assembly = assemblyName1; // String
                eventReceiverDefinition.Type = SPEventReceiverType.ItemDeleting; // SPEventReceiverType
                eventReceiverDefinition.Update();
                typeList.Update(true);



                SPList valueList = web.GetList(uservalueListName);

                string className2 = "CMICT.CSP.Web.CategoryEvents.LookupValuesEvent";

                SPEventReceiverDefinition eventReceiverDefinition2 = valueList.EventReceivers.Add();
                eventReceiverDefinition2.Class = className2; // String
                eventReceiverDefinition2.Assembly = assemblyName1; // String
                eventReceiverDefinition2.Type = SPEventReceiverType.ItemDeleting; // SPEventReceiverType
                eventReceiverDefinition2.Update();
                valueList.Update(true);
                
            }
        }


        // 取消对以下方法的注释，以便处理在停用某个功能前引发的事件。

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSite site = (SPSite)properties.Feature.Parent;
            using (SPWeb web = site.OpenWeb(webName))
            {
                SPList typeList = web.GetList(usertypeListName);
                string className1 = "CMICT.CSP.Web.CategoryEvents.LookupTypesEvent";
                foreach (SPEventReceiverDefinition definition in typeList.EventReceivers)
                {
                    if (definition.Class != className1 && definition.Assembly != assemblyName1
                    && definition.Type != SPEventReceiverType.ItemDeleting)
                    {
                        continue;
                    }
                    definition.Delete();
                    typeList.Update(true);
                    break;
                }

                SPList valueList = web.GetList(uservalueListName);

                string className2 = "CMICT.CSP.Web.CategoryEvents.LookupValuesEvent";
                foreach (SPEventReceiverDefinition definition in valueList.EventReceivers)
                {
                    if (definition.Class != className2 && definition.Assembly != assemblyName1
                    && definition.Type != SPEventReceiverType.ItemDeleting)
                    {
                        continue;
                    }
                    definition.Delete();
                    typeList.Update(true);
                    break;
                }
            }
        }


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
