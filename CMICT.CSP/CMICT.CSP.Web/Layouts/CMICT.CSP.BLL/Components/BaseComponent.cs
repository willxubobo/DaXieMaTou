using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL.Components
{
    public class BaseComponent
    {
        public DataTable GetLookupValuesByType(string lookupType)
        {
            #region mock data
            DataTable dt=new DataTable();
            dt.Columns.Add("LOOKUP_VALUE");
            dt.Columns.Add("LOOKUP_VALUE_NAME");
            if (lookupType == "BS_TEMPLATE_STATUS")
            {
                DataRow dr1 = dt.NewRow();
                dr1["LOOKUP_VALUE"]="ENABLE";
                dr1["LOOKUP_VALUE_NAME"]="启用";
                dt.Rows.Add(dr1);

                DataRow dr2 = dt.NewRow();
                dr2["LOOKUP_VALUE"] = "DISABLE";
                dr2["LOOKUP_VALUE_NAME"] = "禁用";
                dt.Rows.Add(dr2);
            }
            return dt;
            #endregion
        }
    }
}
