using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;

namespace SP.Framework.SiteStructure.Features.CMICT.SiteStructure.Lists
{
    /// <summary>
    /// 此类用于处理在激活、停用、安装、卸载和升级功能的过程中引发的事件。
    /// </summary>
    /// <remarks>
    /// 附加到此类的 GUID 可能会在打包期间使用，不应进行修改。
    /// </remarks>

    [Guid("8f6b73e5-7a49-43d2-a846-8f8a4cabc39e")]
    public class CMICTSiteStructureEventReceiver : SPFeatureReceiver
    {
        // 取消对以下方法的注释，以便处理激活某个功能后引发的事件。

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {

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
