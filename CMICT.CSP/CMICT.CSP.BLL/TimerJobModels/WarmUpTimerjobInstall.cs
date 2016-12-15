using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL.TimerJobModels
{
    public class WarmUpTimerjobInstall : SPFeatureReceiver
    {
        const string MY_TASK = "CMICT WarmUp Timer Job";
        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            {
                if (job.Title == MY_TASK)
                    job.Delete();
            }
            WarmUpTimerJobCS timer = new WarmUpTimerJobCS(MY_TASK, site.WebApplication);
            SPMinuteSchedule schedule = new SPMinuteSchedule();
            schedule.BeginSecond = 0;
            schedule.EndSecond = 59;
            schedule.Interval = 1;
            timer.Schedule = schedule;
            timer.Update();
        }
        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            {
                if (job.Title == MY_TASK)
                    job.Delete();
            }
        }
        public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        {
        }

        public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        {
            SPSite site = properties.Feature.Parent as SPSite;
            foreach (SPJobDefinition job in site.WebApplication.JobDefinitions)
            {
                if (job.Title == MY_TASK)
                    job.Delete();
            }
        }
    }
}
