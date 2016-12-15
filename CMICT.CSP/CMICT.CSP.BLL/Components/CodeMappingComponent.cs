using CMICT.CSP.Model;
using Microsoft.SharePoint;
using NET.Framework.Common.ConstantClass;
using NET.Framework.Common.ExcelHelper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL.Components
{
    public class CodeMappingComponent
    {
        DbHelperSQL dbhelper = new DbHelperSQL();
        BS_CODEMAPPINGBLL codeDll = new BS_CODEMAPPINGBLL();
        private static string companyList = "/Lists/CODE_MAPPING_COMPANY";
        private static string codeList = "/Lists/CODE_MAPPING_SYNC";
        private static string webName = ConfigurationSettings.AppSettings["CMICTSPWebUrl"].ToString();

        private string ConString = System.Configuration.ConfigurationSettings.AppSettings["CMICTCodeMappingConnectionString"].ToString();


        public DataTable GetCodeMappingData(string targetCompanyName, string semanticContent, string customerID,
            string businessCode, string CMICTCode, int PageIndex, int PageSize, out int RecordCount, out int PageCount)
        {
            RecordCount = 0;
            PageCount = 0;
            string sql = " where 1=1 ";

            if (!string.IsNullOrWhiteSpace(targetCompanyName))
            {
                sql += " and CustomerName like N'%" + targetCompanyName + "%'";
            }

            if (!string.IsNullOrWhiteSpace(semanticContent))
            {
                sql += " and SemanticDesc like N'%" + semanticContent + "%'";
            }
            if (!string.IsNullOrWhiteSpace(customerID))
            {
                sql += " and TargetCode like N'%" + customerID + "%'";
            }
            if (!string.IsNullOrWhiteSpace(businessCode))
            {
                sql += " and BusinessCode like N'%" + businessCode + "%'";
            }
            if (!string.IsNullOrWhiteSpace(CMICTCode))
            {
                sql += " and CMICTCode like N'%" + CMICTCode + "%'";
            }

            if (sql.Contains("and"))
            {
                sql += " collate Chinese_PRC_CS_AI";
            }


            string AllFields = "MappingID,CustomerID,CustomerName,CustomerDesc,SemanticDesc,BusinessCode,BusinessCodeDesc,"
                + "Author,BusinessTranslation,TargetCode,TargetCodeEnDesc,TargetCodeCnDesc,Created"
                + ",CMICTCode,CMICTCodeEnDesc,CMICTCodeCnDesc,case CONVERT(varchar(100), StartDate, 23) "
                + " when '1753-01-01' then '-' else CONVERT(varchar(100), StartDate, 23) end as StartDate"
                + ",case CONVERT(varchar(100), ExpireDate, 23) when '9999-12-31' then '-' else"
                + " CONVERT(varchar(100), ExpireDate, 23) end as ExpireDate";
            string Condition = " BS_CODEMAPPING " + sql;
            string IndexField = "MappingID";
            string OrderFields = "order by Created desc,CustomerName";

            return dbhelper.ExecutePage(ConString,AllFields, Condition, IndexField, OrderFields, PageIndex, PageSize, out RecordCount, out PageCount, null);
        }

        public bool SaveCodeMapping(BS_CODEMAPPING codeEntity)
        {
            return codeDll.Add( codeEntity);
        }

        public bool UpdateCodeMapping(BS_CODEMAPPING codeEntity)
        {
            return codeDll.Update( codeEntity);
        }

        public bool DeleteCodeMapping(Guid guid)
        {
            return codeDll.Delete( guid);
        }

        /// <summary>
        /// 检查代码映射关系是否唯一
        /// </summary>
        /// <param name="codeEntity"></param>
        /// <returns></returns>
        public bool CheckCodeMappingOnly(BS_CODEMAPPING codeEntity, bool isUpdate)
        {

            string sql = @"select count(MappingID) as count from BS_CODEMAPPING where 1=1"
                + " and CustomerID=N'{0}' and SemanticDesc=N'{1}' and BusinessCode=N'{2}' "
                + " and (TargetCode=N'{3}' or CMICTCode=N'{4}') ";

            string filterSql1 = " and (('{5}' between StartDate and ExpireDate) or ('{6}' between StartDate and ExpireDate))";

            string filterSql2 = " and ((StartDate between '{5}' and '{6}') or (ExpireDate between '{7}' and '{8}'))";

            //+ " and ('{11}'= ExpireDate or '{12}'= StartDate or '{13}' = ExpireDate or '{14}' = StartDate)";

            string sql1 = sql + filterSql1;

            string sql2 = sql + filterSql2;

            if (isUpdate)
            {
                sql1 = sql1 + " and MappingID!='{7}'  collate Chinese_PRC_CS_AI";
                sql2 = sql2 + " and MappingID!='{9}'  collate Chinese_PRC_CS_AI";

                sql1 = string.Format(sql1, codeEntity.CustomerID, codeEntity.SemanticDesc, codeEntity.BusinessCode,
                codeEntity.TargetCode, codeEntity.CMICTCode, codeEntity.StartDate, codeEntity.ExpireDate,
                codeEntity.MappingID);

                sql2 = string.Format(sql2, codeEntity.CustomerID, codeEntity.SemanticDesc, codeEntity.BusinessCode,
                codeEntity.TargetCode, codeEntity.CMICTCode, codeEntity.StartDate, codeEntity.ExpireDate,
                codeEntity.StartDate, codeEntity.ExpireDate, codeEntity.MappingID);

            }
            else
            {
                sql1 = sql1 + " and MappingID!='00000000-0000-0000-0000-000000000000'  collate Chinese_PRC_CS_AI";
                sql2 = sql2 + " and MappingID!='00000000-0000-0000-0000-000000000000'  collate Chinese_PRC_CS_AI";

                sql1 = string.Format(sql1, codeEntity.CustomerID, codeEntity.SemanticDesc, codeEntity.BusinessCode,
               codeEntity.TargetCode, codeEntity.CMICTCode, codeEntity.StartDate, codeEntity.ExpireDate);

                sql2 = string.Format(sql2, codeEntity.CustomerID, codeEntity.SemanticDesc, codeEntity.BusinessCode,
                codeEntity.TargetCode, codeEntity.CMICTCode, codeEntity.StartDate, codeEntity.ExpireDate,
                codeEntity.StartDate, codeEntity.ExpireDate);
            }


            int count = Convert.ToInt32(dbhelper.ExecuteScalar(ConString, CommandType.Text, sql1));

            //判断时间区间

            if (count > 0)
            {
                return false;
            }
            else
            {
                count = Convert.ToInt32(dbhelper.ExecuteScalar(ConString, CommandType.Text, sql2));

                if (count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }


            }
        }

        public string ImportCodeMapping(string filePath, string currentUserName)
        {
            Dictionary<string, DataTable> dicImportData = NOPI.ExcelToDataTable(filePath);
            string checkResult = string.Empty;

            if (dicImportData.Count > 0)
            {
                foreach (var data in dicImportData)
                {
                    DataTable dtData = data.Value;

                    if (dtData.Rows.Count > 1)
                    {
                        string result = CheckData(dtData);

                        if (!string.IsNullOrWhiteSpace(result))
                        {
                            checkResult = result;
                        }
                        else
                        {
                            InsertTableFromDataTable(dtData, currentUserName);
                        }
                    }
                    else
                    {
                        checkResult = "请先维护数据再上传。";
                    }
                }

                return checkResult;
            }
            else
            {
                return "文件上传失败！";
            }
        }

        /// <summary>
        /// 检查Excel数据的必填字段和唯一性验证
        /// </summary>
        /// <param name="dtData"></param>
        /// <returns></returns>
        private string CheckData(DataTable dtData)
        {
            try
            {
                string checkOnlyResult = string.Empty;
                string checkEmptyResult = string.Empty;
                string emptyRow = string.Empty;
                string emptyColumnName = string.Empty;
                string notOnlyRow = string.Empty;
                List<BS_CODEMAPPING> listCodeMapping = new List<BS_CODEMAPPING>();

                for (int rowNum = dtData.Rows.Count - 1; rowNum >= 0; rowNum--)
                {
                    emptyColumnName = "";

                    BS_CODEMAPPING codeMapping = new BS_CODEMAPPING();
                    codeMapping.CustomerID = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.CustomerID)]).Trim();

                    if (codeMapping.CustomerID.Contains("注"))
                    {
                        dtData.Rows[rowNum].Delete();
                        continue;
                    }

                    codeMapping.SemanticDesc = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.SemanticDesc)]).Trim();
                    codeMapping.BusinessCode = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.BusinessCode)]).Trim();
                    codeMapping.TargetCode = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.TargetCode)]).Trim();
                    codeMapping.CMICTCode = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.CMICTCode)]).Trim();

                    string startDate = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.StartDate)]).Trim();
                    string ExpireDate = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.ExpireDate)]).Trim();


                    if (string.IsNullOrWhiteSpace(startDate))
                    {
                        codeMapping.StartDate = DateTime.MinValue.Date;
                        //codeMapping.StartDate = Convert.ToDateTime("1753/1/1 00:00:00");
                    }
                    else
                    {
                        if (startDate.Length < 10)
                        {
                            startDate = startDate + " 00:00:00";
                        }
                        codeMapping.StartDate = Convert.ToDateTime(startDate);
                    }
                    if (string.IsNullOrWhiteSpace(ExpireDate))
                    {
                        codeMapping.ExpireDate = DateTime.MaxValue.Date;
                    }
                    else
                    {
                        if (ExpireDate.Length < 10)
                        {
                            ExpireDate = ExpireDate + " 00:00:00";
                        }

                        codeMapping.ExpireDate = Convert.ToDateTime(ExpireDate);
                    }

                    if (string.IsNullOrWhiteSpace(codeMapping.CustomerID) || string.IsNullOrWhiteSpace(codeMapping.TargetCode)
                        || string.IsNullOrWhiteSpace(codeMapping.CMICTCode))
                    {
                        emptyRow = Convert.ToString(rowNum);
                    }

                    if (string.IsNullOrWhiteSpace(codeMapping.CustomerID))
                    {
                        emptyColumnName = emptyColumnName + "，" + GetEnumDescription(ConstantClass.CodeMappingColumn.CustomerID);
                    }

                    if (string.IsNullOrWhiteSpace(codeMapping.TargetCode))
                    {
                        emptyColumnName = emptyColumnName + "，" + GetEnumDescription(ConstantClass.CodeMappingColumn.TargetCode);
                    }

                    if (string.IsNullOrWhiteSpace(codeMapping.CMICTCode))
                    {
                        emptyColumnName = emptyColumnName + "，" + GetEnumDescription(ConstantClass.CodeMappingColumn.CMICTCode);
                    }

                    //唯一性检查
                    bool isOnly = CheckCodeMappingOnly(codeMapping, false);

                    if (!isOnly)
                    {
                        notOnlyRow = rowNum + "," + notOnlyRow;
                    }

                    if (emptyRow.Length > 0)
                    {
                        if (emptyColumnName.Length > 0)
                        {
                            emptyColumnName = emptyColumnName.Substring(1);
                        }

                        checkEmptyResult = checkEmptyResult + "\n第" + emptyRow + "行," + emptyColumnName + "为空，请修正后上传！";
                    }
                }

                if (notOnlyRow.Length > 0)
                {
                    notOnlyRow = notOnlyRow.Substring(0, notOnlyRow.Length - 1);
                }

                //if (emptyRow.Length > 0)
                //{
                //    emptyRow = emptyRow.Substring(1);
                //}

                //if (!string.IsNullOrWhiteSpace(emptyRow))
                //{
                //    checkEmptyResult = "第" + emptyRow + "行," + emptyColumnName + "为空，请修正后上传！";
                //}
                if (!string.IsNullOrWhiteSpace(notOnlyRow))
                {
                    checkOnlyResult = "第" + notOnlyRow + "行的唯一性索引冲突，请修正后上传！";
                }

                if (string.IsNullOrWhiteSpace(checkOnlyResult))
                {
                    return checkEmptyResult;
                }
                else
                {
                    return checkOnlyResult;
                }
            }
            catch (Exception ex)
            {
                BaseComponent.Error("代码映射批量导入，文件导入： " + ex.Message + "--------" + ex.StackTrace);
                return "请按照系统提供的模板进行上传。";
            }

        }

        /// <summary>
        /// 将DataTable中的数据存入表中
        /// </summary>
        /// <param name="dtData"></param>
        /// <param name="created"></param>
        /// <returns></returns>
        public void InsertTableFromDataTable(DataTable dtData, string created)
        {
            for (int rowNum = 0; rowNum < dtData.Rows.Count; rowNum++)
            {
                BS_CODEMAPPING codeMapping = new BS_CODEMAPPING();
                codeMapping.MappingID = Guid.NewGuid();
                codeMapping.CustomerID = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.CustomerID)]).Trim();
                codeMapping.CustomerName = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.CustomerName)]).Trim();
                codeMapping.CustomerDesc = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.CustomerDesc)]).Trim();
                codeMapping.SemanticDesc = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.SemanticDesc)]).Trim();
                codeMapping.BusinessCode = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.BusinessCode)]).Trim();
                codeMapping.BusinessCodeDesc = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.BusinessCodeDesc)]).Trim();
                codeMapping.BusinessTranslation = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.BusinessTranslation)]).Trim();
                codeMapping.TargetCode = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.TargetCode)]).Trim();
                codeMapping.TargetCodeEnDesc = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.TargetCodeEnDesc)]).Trim();
                codeMapping.TargetCodeCnDesc = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.TargetCodeCnDesc)]).Trim();
                codeMapping.CMICTCode = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.CMICTCode)]).Trim();
                codeMapping.CMICTCodeEnDesc = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.CMICTCodeEnDesc)]).Trim();
                codeMapping.CMICTCodeCnDesc = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.CMICTCodeCnDesc)]).Trim();
                string startDate = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.StartDate)]).Trim();
                string ExpireDate = Convert.ToString(dtData.Rows[rowNum][GetEnumDescription(ConstantClass.CodeMappingColumn.ExpireDate)]).Trim();
                if (string.IsNullOrWhiteSpace(startDate))
                {
                    //codeMapping.StartDate = DateTime.MinValue;
                    codeMapping.StartDate = Convert.ToDateTime("1753/1/1 00:00:00");
                }
                else
                {
                    codeMapping.StartDate = Convert.ToDateTime(startDate);
                }
                if (string.IsNullOrWhiteSpace(ExpireDate))
                {
                    codeMapping.ExpireDate = DateTime.MaxValue;
                }
                else
                {
                    codeMapping.ExpireDate = Convert.ToDateTime(ExpireDate);
                }
                codeMapping.Created = DateTime.Now;
                codeMapping.Modified = DateTime.Now;
                codeMapping.Author = created;
                codeMapping.Editor = created;

                SaveCodeMapping(codeMapping);
            }
        }

        private string GetEnumDescription(Enum enumValue)
        {
            string str = enumValue.ToString();
            System.Reflection.FieldInfo field = enumValue.GetType().GetField(str);
            object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
            if (objs == null || objs.Length == 0) return str;
            System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
            return da.Description;
        }

        public DataTable  GetCompanies()
        {
            DataTable result = new DataTable();
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(webName))
                    {
                        string listUrl = webName + companyList;
                        SPList list = web.GetList(listUrl);
                        if (list.ItemCount > 0)
                        {
                            result.Columns.Add("VALUE");
                            result.Columns.Add("TEXT");
                            foreach (SPItem item in list.Items)
                            {
                                DataRow newRow = result.NewRow();
                                newRow["TEXT"] = item["COMPANY_NAME"];
                                newRow["VALUE"] = Convert.ToString(item["COMPANY_ID"]) +";"+ Convert.ToString(item["INTERFACE_CODE"]);
                                result.Rows.Add(newRow);
                            }
                        }
                        
                    }
                }

            });
            return result;
        }

        public DataTable GetCodeTypes(string company)
        {
            DataTable result=null;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {

                using (SPSite site = new SPSite(SPContext.Current.Site.ID))
                {
                    using (SPWeb web = site.OpenWeb(webName))
                    {
                        string listUrl = webName + codeList;
                        SPList list = web.GetList(listUrl);
                        if (list.ItemCount > 0)
                        {
                            DataTable dt = list.Items.GetDataTable();
                            result = dt.Clone();
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (Convert.ToString(dr["COMPANY_NAME"]) == company)
                                {
                                    DataRow newRow = result.NewRow();
                                    newRow["COMPANY_NAME"] = dr["COMPANY_NAME"];
                                    newRow["CODE_TYPE"] = dr["CODE_TYPE"];
                                    newRow["DESCRIPTION"] = dr["DESCRIPTION"];
                                    result.Rows.Add(newRow);
                                }
                                    
                            }
                        }
                        
                    }
                }

            });
            return result;
        }

        //根据comid与busid删除codemapping
        public void DelCodeMapByComIDAndBusID(string ComID, string BusID)
        {
            if (!string.IsNullOrEmpty(ComID))
            {
                string sql = "delete from [BS_CODEMAPPING] where CustomerID='"+ComID+"'";
                if (!string.IsNullOrEmpty(BusID)&&BusID.ToLower()!="all")
                {
                    sql += " and BusinessCode='"+BusID+"'";
                }
                dbhelper.ExecuteNonQuery(ConString, CommandType.Text, sql, null);
            }
        }
    }
}

