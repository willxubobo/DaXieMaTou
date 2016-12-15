using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;


namespace CMICT.CSP.WebDataSubscribe.Commons
{
    public class Common
    {
        public static string EdiPwd
        {
            get
            {
                return ConfigurationManager.AppSettings["CallEDIPwd"];
            }
        }

        public static string ApplyDataUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["ApplyDataUserName"];
            }
        }

        public static string ApplyDataUserPwd
        {
            get
            {
                return ConfigurationManager.AppSettings["ApplyDataUserPwd"];
            }
        }


        public static string DataUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["DataUserName"];
            }
        }

        public static string DataPwd
        {
            get
            {
                return ConfigurationManager.AppSettings["DataPwd"];
            }
        }

        public static string EdiUserName
        {
            get
            {
                return ConfigurationManager.AppSettings["CallEDIUserName"];
            }
        }

        public static DataTable GetRecordFromPage(string table, string key, int pageIndex, int pageSize, out int total)
        {

            int start = (pageIndex - 1) * pageSize + 1; ;
            int end = pageIndex * pageSize;

            string sql =
                "SELECT *,RowNumber FROM (SELECT *,ROW_NUMBER() OVER( ORDER BY {0}) AS RowNumber FROM {1} ) AS RowNumberTableSource WHERE RowNumber BETWEEN {2} AND {3}";
            sql = string.Format(sql, key, table, start, end);

            DataTable dt = new DataTable();
            SqlHelper sqlHelper = new SqlHelper();
            DataSet ds = sqlHelper.ExecuteDataSet(sql);
            if (ds.Tables.Count > 0)
            {
                dt = ds.Tables[0];
                dt.Columns.Remove("RowNumber");
                dt.Columns.Remove("RowNumber1");
            }
            total = Convert.ToInt32(sqlHelper.ExecuteScalar("select count(1) from " + table));

            return dt;
        }

        public static string GetKeyByEntityName(string name)
        {
            string filePath = HttpContext.Current.Server.MapPath("/EntityKey.xml");
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            XmlNode node = doc.SelectSingleNode("//EntitiesList//Entity[Title='" + name + "']");
            if (node != null)
            {
                return node.LastChild.InnerText;
            }
            else
            {
                return "";
            }
        }

    }
}
