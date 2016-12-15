using CMICT.CSP.BLL;
using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.Web.CategoryEvents
{
    public class LookupTypesEvent : SPItemEventReceiver
    {
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            string id = Convert.ToString(properties.ListItem["ID"]);
            if (IsExist(id))
            {
                properties.ErrorMessage = "该项已被使用，无法删除！";
                properties.Status = SPEventReceiverStatus.CancelWithError;
            }
            else
            {
                base.ItemDeleting(properties);
            }
        }

        bool IsExist(string id)
        {
            DbHelperSQL dbhelper = new DbHelperSQL();
            string sql1 = "SELECT count(*)  FROM BS_TEMPLATE_MAIN where BigCategory=@BigCategory";
            string sql2 = "SELECT count(*)  FROM BS_DATASOURCE where BigCategory=@BigCategory";
            SqlParameter[] parameters = {
                                            new SqlParameter("@BigCategory",id)
			};

            if ((int)dbhelper.ExecuteScalar(System.Data.CommandType.Text, sql1, parameters) == 0 && (int)dbhelper.ExecuteScalar(System.Data.CommandType.Text, sql2, parameters) == 0)
                return false;

            return true;
        }
    }
}
