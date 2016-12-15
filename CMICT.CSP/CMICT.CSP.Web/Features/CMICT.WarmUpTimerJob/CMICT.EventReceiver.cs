using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using CMICT.CSP.BLL.TimerJobModels;
using Microsoft.SharePoint.Administration;

namespace CMICT.CSP.Web.Features.CMICT.WarmUpTimerJob
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("cf1e43f9-85f8-4bde-ac7e-d86fca93d402")]
    public class CMICTEventReceiver : SPFeatureReceiver
    {
        const string MY_TASK = "CMICT WarmUp Timer Job";
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            {
                if (job.Name == MY_TASK)
                    job.Delete();
            }
            WarmUpTimerJobCS timer = new WarmUpTimerJobCS(MY_TASK, site.WebApplication);
            SPMinuteSchedule schedule = new SPMinuteSchedule();
            schedule.BeginSecond = 0;
            schedule.EndSecond = 59;
            schedule.Interval = 10;
            timer.Schedule = schedule;
            timer.Update();
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            {
                if (job.Name == MY_TASK)
                    job.Delete();
            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
