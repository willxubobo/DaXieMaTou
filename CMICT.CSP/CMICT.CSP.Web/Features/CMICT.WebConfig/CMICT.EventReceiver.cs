using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace CMICT.CSP.Web.Features.CMICT.WebConfig
{
    /// <summary>
    /// 此类用于处理在激活、停用、安装、卸载和升级功能的过程中引发的事件。
    /// </summary>
    /// <remarks>
    /// 附加到此类的 GUID 可能会在打包期间使用，不应进行修改。
    /// </remarks>

    [Guid("64d14a29-3ae2-4c65-99b4-eb04ef5effdd")]
    public class CMICTEventReceiver : SPFeatureReceiver
    {
        // 取消对以下方法的注释，以便处理激活某个功能后引发的事件。

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            //SPWebConfigModification configModFound = null;
            //SPWebApplication webApplication = SPWebApplication.Lookup(new Uri("http://localhost/"));
            //Collection<SPWebConfigModification> modsCollection = webApplication.WebConfigModifications;

            //// Find the most recent modification of a specified owner
            //int modsCount1 = modsCollection.Count;
            //for (int i = modsCount1 - 1; i > -1; i--)
            //{
            //    if (modsCollection[i].Owner == "User Name")
            //    {
            //        configModFound = modsCollection[i];
            //    }
            //}

            //// Remove it and save the change to the configuration database  
            //modsCollection.Remove(configModFound);
            //webApplication.Update();

            //// Reapply all the configuration modifications
            //webApplication.Farm.Services.GetValue<SPWebService>().ApplyWebConfigModifications();




            //SPWebService service = SPWebService.ContentService;

            //SPWebConfigModification myModification = new SPWebConfigModification();
            //myModification.Path = "configuration/SharePoint/SafeControls";
            //myModification.Name = "SafeControl[@Assembly='MyCustomAssembly'][@Namespace='MyCustomNamespace'][@TypeName='*'][@Safe='True']";
            //myModification.Sequence = 0;
            //myModification.Owner = "User Name";
            //myModification.Type = SPWebConfigModification.SPWebConfigModificationType.EnsureChildNode;
            //myModification.Value = "<SafeControl Assembly='MyCustomAssembly' Namespace='MyCustomNamespace' TypeName='*' Safe='True' />";


            //SPSite site = (SPSite)properties.Feature.Parent;
            //site.WebApplication.WebConfigModifications.Add(myModification);
            ///*Call Update and ApplyWebConfigModifications to save changes*/
            //service.Update();
            //service.ApplyWebConfigModifications();
        }


        // 取消对以下方法的注释，以便处理在停用某个功能前引发的事件。

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
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
