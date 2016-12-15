using CMICT.CSP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL.Components
{
    public class UsageAnalysisComponent : BaseComponent
    {
        public void WriteUsage(UA_USAGE usage, List<UA_USAGE_DETAIL> query)
        {
            Guid guid = Guid.NewGuid();
            usage.ID = guid;
            usage.Created = DateTime.Now;
            UA_USAGEBLL ua_usAgeBll = new UA_USAGEBLL();
            UA_USAGE_DETAILBLL ua_usageDetailBll = new UA_USAGE_DETAILBLL();

            ua_usAgeBll.Add(usage);

            if (query != null)
            {
                foreach (UA_USAGE_DETAIL detail in query)
                {
                    detail.ID = Guid.NewGuid();
                    detail.UsageID = guid;
                    ua_usageDetailBll.Add(detail);

                }
            }

        }
    }
}
