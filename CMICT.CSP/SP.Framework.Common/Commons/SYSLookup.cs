using CamlexNET;
using Microsoft.SharePoint;
//using NET.Framework.Common.CacheHelper;
//using SP.Framework.DAL;
using System;
using System.Configuration;
using System.Data;
using System.Linq;

namespace SP.Framework.Common.Commons
{
    public static class SYSLookup
    {
        private static Guid siteID = SPContext.Current.Site.ID;
        private static string typeListName = "/Lists/SYS_LOOKUP_TYPES";
        private static string valueListName = "/Lists/SYS_LOOKUP_VALUES";
        private static string usertypeListName = "/Lists/USER_LOOKUP_TYPES";
        private static string uservalueListName = "/Lists/USER_LOOKUP_VALUES";

        private static string webName = ConfigurationSettings.AppSettings["CMICTSPWebUrl"].ToString();

        private static DataTable lookupTable;
        private static DataTable LookupTable
        {
            get
            {
                if (lookupTable == null)
                    lookupTable = GetLookupTable(valueListName);
                return lookupTable;

            }
        }

        private static DataTable lookupTypeTable;
        private static DataTable LookupTypeTable
        {
            get
            {
                if (lookupTypeTable == null)
                    lookupTypeTable = GetLookupTable(typeListName);
                return lookupTypeTable;

            }
        }


        private static DataTable UserLookupTable
        {
            get
            {
                return GetLookupTable(uservalueListName);

            }
        }

        private static DataTable UserLookupTypeTable
        {
            get
            {
                return GetLookupTable(usertypeListName);

            }
        }


        public static DataTable GetLookupTable(string valueListName, SPQuery query = null)
        {
                DataTable returnDt = null;
                DataTable dt = null;
                SPSecurity.RunWithElevatedPrivileges(delegate
                {

                    using (SPSite site = new SPSite(siteID))
                    {
                        using (SPWeb web = site.OpenWeb(webName))
                        {
                            string listUrl = webName + valueListName;
                            SPList list = web.GetList(listUrl);
                            if (list.ItemCount > 0)
                            {
                                if (query == null)
                                {
                                    dt = list.Items.GetDataTable();
                                    DataView dv = dt.DefaultView;
                                    dv.Sort = "SEQUENCE Asc";
                                    returnDt = dv.ToTable();

                                    var rows = returnDt.Select("ENABLE='0'");

                                    if (rows.Count() > 0)
                                    {
                                        foreach (var row in rows)
                                        {
                                            returnDt.Rows.Remove(row);
                                        }
                                    }
                                }
                                else
                                {
                                    var items = list.GetItems(query);
                                    if (items != null && items.Count > 0)
                                    {
                                        dt = items.GetDataTable();
                                        DataView dv = dt.DefaultView;
                                        dv.Sort = "SEQUENCE Asc";
                                        returnDt = dv.ToTable();

                                        var rows = returnDt.Select("ENABLE='0'");

                                        if (rows.Count() > 0)
                                        {
                                            foreach (var row in rows)
                                            {
                                                returnDt.Rows.Remove(row);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                });
                return returnDt;
        }


        public static DataTable GetLookupTypesByCode(string code)
        {
            var rows = LookupTypeTable.AsEnumerable().Where(p => p.Field<string>("APP_CODE") == code);
            DataTable dt = new DataTable();
            dt.Columns.Add("LOOKUP_CODE");
            dt.Columns.Add("LOOKUP_NAME");
            if (rows != null && rows.Count() > 0)
            {
                foreach (var row in rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["LOOKUP_CODE"] = row["LOOKUP_CODE"];
                    dr["LOOKUP_NAME"] = row["LOOKUP_NAME"];
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public static DataTable GetLookupValuesByType(string type)
        {
            var rows = LookupTable.AsEnumerable().Where(p => p.Field<string>("LOOKUP_CODE_LINE") == type);
            DataTable dt = new DataTable();
            dt.Columns.Add("LOOKUP_VALUE");
            dt.Columns.Add("LOOKUP_VALUE_NAME");
            if (rows != null && rows.Count() > 0)
            {
                foreach (var row in rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["LOOKUP_VALUE"] = row["LOOKUP_VALUE"];
                    dr["LOOKUP_VALUE_NAME"] = row["LOOKUP_VALUE_NAME"];
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public static string GetLookupNameByValue(string type, string value)
        {
            var rows = LookupTable.AsEnumerable().Where(p => p.Field<string>("LOOKUP_CODE_LINE") == type && p.Field<string>("LOOKUP_VALUE") == value).ToList();
            if (rows != null && rows.Count() > 0)
                return rows[0]["LOOKUP_VALUE_NAME"].ToString();
            return "";
        }

        public static string GetTypeNameByCode(string type)
        {
            var rows = LookupTypeTable.AsEnumerable().Where(p => p.Field<string>("LOOKUP_CODE") == type).ToList();
            if (rows != null && rows.Count() > 0)
                return rows[0]["LOOKUP_NAME"].ToString();
            return "";
        }






        //USER


        public static DataTable GetUserLookupTypesByCode(string code)
        {
            if (UserLookupTypeTable == null)
                return null;
            var rows = UserLookupTypeTable.AsEnumerable();
            DataTable dt = new DataTable();
            dt.Columns.Add("LOOKUP_CODE");
            dt.Columns.Add("LOOKUP_NAME");
            if (rows != null && rows.Count() > 0)
            {
                foreach (var row in rows)
                {
                    if (Convert.ToString(row["REPORTTYPE"]).Trim()==code)
                    {
                        DataRow dr = dt.NewRow();
                        dr["LOOKUP_CODE"] = row["ID"];
                        dr["LOOKUP_NAME"] = row["LinkTitle"];
                        dt.Rows.Add(dr);
                    }
                }
            }
            return dt;
        }

        public static DataTable GetUserLookupValuesByType(string type)
        {
            //if (UserLookupTable == null)
            //    return null;
            //var rows = UserLookupTable.AsEnumerable().Where(p => p["USER_LOOKUP_CODE_LINE"] == (DataTypes.LookupId)type);

            SPQuery query = new SPQuery();
            query.Query = query.Query = @"<Where>
  <Eq>
    <FieldRef Name='USER_LOOKUP_CODE_LINE' LookupId='True' />
    <Value Type='Lookup'>" + type + @"</Value>
  </Eq>
</Where>";//Camlex.Query().Where(x => x["USER_LOOKUP_CODE_LINE"] == (DataTypes.LookupId)type).ToString();
            DataTable dtSub = GetLookupTable(uservalueListName, query);


            DataTable dt = new DataTable();
            dt.Columns.Add("LOOKUP_VALUE");
            dt.Columns.Add("LOOKUP_VALUE_NAME");
            if (dtSub != null && dtSub.Rows.Count > 0)
            {
                foreach (DataRow row in dtSub.Rows)
                {
                    DataRow dr = dt.NewRow();
                    dr["LOOKUP_VALUE"] = row["ID"];
                    dr["LOOKUP_VALUE_NAME"] = row["LinkTitle"];
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public static string GetUserLookupNameByValue(string type, string value)
        {
            SPQuery query = new SPQuery();
            query.Query = @"<Where><And><Eq><FieldRef Name='ID' /><Value Type='Counter'>" + value + "</Value></Eq><Eq><FieldRef Name='USER_LOOKUP_CODE_LINE'  LookupId='True' /><Value Type='Lookup'>" + type + "</Value></Eq></And></Where>";
            // Camlex.Query().Where(x => x["USER_LOOKUP_CODE_LINE"] == (DataTypes.LookupId)type && x["ID"] == value).ToString();
            DataTable dt = GetLookupTable(uservalueListName, query);
            //var rows = UserLookupTable.AsEnumerable().Where(p => p["USER_LOOKUP_CODE_LINE"] == (DataTypes.LookupId)type && p.Field<int>("ID") == Convert.ToInt32(value)).ToList();
            if (dt != null && dt.Rows.Count > 0)
                return dt.Rows[0]["LinkTitle"].ToString();
            return "";
        }

        public static string GetUserTypeNameByCode(string type)
        {
            int typeValue = 0;
            if (int.TryParse(type, out typeValue))
            {
                var rows = UserLookupTypeTable.AsEnumerable().Where(p => p.Field<int>("ID") == typeValue).ToList();
                if (rows != null && rows.Count() > 0)
                    return rows[0]["LinkTitle"].ToString();
            }
            return "";
        }
    }
}
