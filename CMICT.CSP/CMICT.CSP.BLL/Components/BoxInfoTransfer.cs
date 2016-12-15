using NET.Framework.Common.ExcelHelper;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;

namespace CMICT.CSP.BLL.Components
{
    public class BoxInfoTransfer : BaseComponent
    {
        DateTime currentDate = DateTime.MinValue;

        private static string ContainerInfoConnection = ConfigurationSettings.AppSettings["CMICTContainerInfoConnection"].ToString();

        /// <summary>
        /// 校验修箱信息，返回错误信息
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string VerifyExcel(string path)
        {
            if (string.IsNullOrEmpty(path))
                return "";
            StringBuilder errorMsg = new StringBuilder();
            var dic = NOPI.ExcelToDataTable(path);//EPPlus.ExcelToDataTable(path);
            if (dic["DM"].Rows.Count == 0 && dic["RP"].Rows.Count == 0 && dic["EA"].Rows.Count == 0)
                return "未检测到箱信息，请修正后再上传！";
            foreach (var table in dic)
            {
                if (table.Key == "DM" || table.Key == "RP" || table.Key == "EA")
                {
                    if (table.Value.Rows.Count > 0)
                    {

                        foreach (DataRow dr in table.Value.Rows)
                        {
                            DateTime date = DateTime.MinValue;
                            errorMsg.Append(VerifyData(dr[0], dr[1], dr[2], dr[3], table.Key));
                        }
                    }
                }
            }
            return errorMsg.ToString();
        }

        private string VerifyData(object num, object boxNum, object time, object type, string sheetType)
        {
            string result = string.Empty;
            if (string.IsNullOrWhiteSpace(Convert.ToString(num)))
                return result;
            if (string.IsNullOrEmpty(VerifyBoxNum(boxNum)) && string.IsNullOrEmpty(VerifyTime(time)) && string.IsNullOrEmpty(VerifyType(type, sheetType)))
                return result;
            result = "工作表：" + sheetType + "，序号：" + num.ToString() + " ：" + VerifyBoxNum(boxNum) + VerifyTime(time) + VerifyType(type, sheetType) + Environment.NewLine;
            return result;
        }

        private string VerifyType(object type, string sheetType)
        {
            if (sheetType.Equals(Convert.ToString(type)))
                return "";
            return "业务类型异常；";
        }

        private string VerifyTime(object time)
        {
            IFormatProvider ifp = new CultureInfo("zh-CN", true);
            DateTime dt = new DateTime();
            bool result = DateTime.TryParseExact(Convert.ToString(time), "yyyyMMddHHmm", ifp, DateTimeStyles.None, out dt);
            if (result)
            {
                if (currentDate == DateTime.MinValue)
                {
                    currentDate = dt;
                }
                if (currentDate.Date == dt.Date)
                    return "";
                else
                    return "上传的日期不一致；";
            }
            return "时间格式错误；";

        }

        private string VerifyBoxNum(object boxNum)
        {
            if (boxNum == null || Convert.ToString(boxNum).Length != 11)
                return "箱号必须为11位；";
            DbHelperSQL da = new DbHelperSQL();
            SqlParameter para = new SqlParameter("@CONTAINERNO", Convert.ToString(boxNum));

            //string connectionString = "Data Source=10.2.2.31;Initial Catalog=EDB-DW;user id=mengtuo;pwd=mengtuo4321";
            string sql = "KF_CONTAINER_INFO";
            //BaseComponent.GetLookupNameBuValue("CSP_CONNECTIONS", "CONTAINER_INFO")
            var result = da.ExecuteDataSet(ContainerInfoConnection, CommandType.StoredProcedure, sql, para);
            if (result != null && result.Tables.Count > 0)
            {
                if (result.Tables[0].Rows.Count > 0)
                {
                    if (string.IsNullOrWhiteSpace(Convert.ToString(result.Tables[0].Rows[0][0])))
                        return Convert.ToString(boxNum) + "号集装箱不是阳明箱或已出场；";
                    else
                        return "";
                }
                return "";
            }
            return Convert.ToString(boxNum) + "号集装箱不是阳明箱或已出场；";
        }

        /// <summary>
        /// 报文内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string TransferFileContent(string path, out string date)
        {
            date = "";
            if (string.IsNullOrEmpty(path))
            {

                return "";
            }

            StringBuilder txtContent = new StringBuilder();
            var dic = NOPI.ExcelToDataTable(path);
            string header = string.Empty;
            foreach (var table in dic)
            {
                if (table.Key == "DM" || table.Key == "RP" || table.Key == "EA")
                {
                    if (table.Value.Rows.Count > 0)
                    {
                        //分块报文头 “$$X/CNNGB/年月日(8)/类型代码(RP/EA/DM)”
                        txtContent.Append("$$X/CNNGB/");
                        txtContent.Append(table.Value.Rows[0][2].ToString().Substring(0, 8));
                        txtContent.Append("/");
                        txtContent.AppendLine(table.Key);

                        if (string.IsNullOrEmpty(date)) //文件名 取上传内容中时间字段第一条记录的前8位日期的后一日
                        {
                            DateTime time = new DateTime(Convert.ToInt32(table.Value.Rows[0][2].ToString().Substring(0, 4)), Convert.ToInt32(table.Value.Rows[0][2].ToString().Substring(4, 2)), Convert.ToInt32(table.Value.Rows[0][2].ToString().Substring(6, 2)));
                            //time=time.AddDays(1);
                            date = "RPCNNGBP03" + time.ToString("yyyyMMdd") + "_" + DateTime.Now.ToString("HHmmss") + ".txt";
                        }


                        foreach (DataRow dr in table.Value.Rows)
                        {
                            if (dr[0] == null || string.IsNullOrEmpty(Convert.ToString(dr[0])))
                                continue;
                            //分块报文体 “箱号/时分(4)//CNNGBP03/ CNNGBP03”
                            txtContent.Append(dr[1].ToString());
                            txtContent.Append("/");
                            txtContent.Append(dr[2].ToString().Substring(8, 4));
                            txtContent.AppendLine("//CNNGBP03/CNNGBP03");


                            //总报文头 “$$$/NGB/GIC/年月日(8)/年(4)DX月日(4)PM”
                            if (string.IsNullOrEmpty(header))
                            {
                                header = "$$$/NGB/GIC/" + dr[2].ToString().Substring(0, 8) + "/";
                                header += "RP" + dr[2].ToString().Substring(0, 4) + "DX";
                                header += dr[2].ToString().Substring(4, 4) + "PM";
                                header += Environment.NewLine;
                            }
                        }
                    }
                }
            }
            string result = header + txtContent.ToString();

            return result;
        }
    }
}
