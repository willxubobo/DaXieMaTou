using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NET.Framework.Common.ExcelHelper
{
    public class EPPlus
    {
        public static string CreateFile(string filePath)
        {
            return CreateFile(filePath, null);
        }
        public static string CreateFile(string filePath, DataTable data)
        {
            return CreateFile(filePath, data, new List<string>(), new List<string>(),"","");
        }
        public static string CreateFile(string filePath, DataTable data, List<string> columnsName, List<string> mergeColumns,string PageTitle,string TemplateName)
        {
            return CreateFile(filePath, data, columnsName, false, mergeColumns, PageTitle,TemplateName);
        }
        public static int getmercolspan(List<string> mernamelist, int sindex, string comparev)
        {
            var csnum = 0;
            for (var i = (sindex + 1); i < mernamelist.Count; i++)
            {
                if (comparev.Trim() == mernamelist[i].Trim())
                {
                    csnum++;
                }
                else
                {
                    break;
                }
            }
            return csnum;
        }
        //计算字符串的长度
        public static double GetTextExeclWidth(string vtext)
        {
            double result = 0;
            char[] slist = vtext.ToCharArray();
            for (int i = 0; i < slist.Count(); i++)
            {
                if (slist[i] >= 0x4e00 && slist[i] <= 0x9fbb)
                {
                    result += 1;
                }
                else
                {
                    result += 0.5;
                }
                //if (char.IsLetter(vtext,i))//判断是否是字母或数字
                //{
                //    result += 0.5;
                //}
                //else if (char.IsNumber(vtext, i))
                //{
                //    result += 0.5;
                //}
                //else
                //{
                //    result += 1;
                //}
            }
            return result * 18 * 0.14;
        }
        public static double FormatExcelContent(string vtext, int ecount)
        {
            double result = 0;
            char[] slist = vtext.ToCharArray();
            if (slist.Count() > ecount)
            {
                if (!char.IsLetterOrDigit(slist[ecount - 1]))//判断是否是字母或数字
                {
                    for (int i = 0; i < ecount; i++)
                    {
                        if (char.IsLetterOrDigit(slist[i]))//判断是否是字母或数字
                        {
                            result += 10;
                        }
                        else
                        {
                            result += 20;
                        }
                    }
                }
                else
                {
                    for (int i = ecount; i < slist.Count(); i++)
                    {
                        if (char.IsLetterOrDigit(slist[i]))//判断是否是字母或数字
                        {
                            result += 10;
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int i = 0; i < ecount; i++)
                    {
                        if (char.IsLetterOrDigit(slist[i]))//判断是否是字母或数字
                        {
                            result += 10;
                        }
                        else
                        {
                            result += 20;
                        }
                    }
                    //result += ecount * 14;
                }
            }
            return result*0.14;
        }
        //导出
        public static string CreateFile(string filePath, DataTable data, List<string> columnsName, bool overwriteIfFileExist, List<string> mergeColumns, string PageTitle, string TemplateName)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo.Exists)
            {
                if (!overwriteIfFileExist)
                {
                    throw new Exception("文件已经存在！如果需要覆盖请指定覆盖已经存在的文件。");
                }
                fileInfo.Delete();
                fileInfo = new FileInfo(filePath);
            }
            using (ExcelPackage excelPackage = new ExcelPackage())
            {
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add(TemplateName);
                if (data != null && (columnsName == null || columnsName.Count == 0))
                {
                    columnsName = new List<string>();
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        columnsName.Add(data.Columns[i].ColumnName);
                    }
                }
                bool ishavemc = false;
                string[] zmlist = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z AA BB CC DD EE FF GG HH II JJ KK LL MM NN OO PP QQ RR SS TT UU VV WW XX YY ZZ".Split(' ');
                int ss = 1;
                string mers = "1";
                string merss = "2";
                //添加标题在第一行
                if (!string.IsNullOrEmpty(TemplateName))
                {
                    ss = 2;
                    mers = "2";
                    merss = "3";
                    string startCell = "A1";
                    string endCell = zmlist[(data.Columns.Count-1)] + "1";
                    excelWorksheet.Cells[startCell + ":" + endCell].Merge = true;
                    excelWorksheet.Cells[startCell + ":" + endCell].Value = TemplateName;
                    excelWorksheet.Cells[startCell + ":" + endCell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    excelWorksheet.Cells[startCell + ":" + endCell].Style.Font.Bold = true;
                }

                //合并表头
                if (mergeColumns != null && mergeColumns.Count > 0)
                {
                    foreach (string mc in mergeColumns)
                    {
                        if (mc.Trim() != "")
                        {
                            ishavemc = true;
                            //ss = 3;
                            ss++;
                            break;
                        }
                    }
                    if (ishavemc)
                    {
                        for (int i = 0; i < mergeColumns.Count; i++)
                        {
                            if (mergeColumns[i].Trim() != "")
                            {
                                int mccount = getmercolspan(mergeColumns, i, mergeColumns[i].Trim());
                                if (mccount > 0)
                                {
                                    string startCell = zmlist[i] + mers;
                                    string endCell = zmlist[(i + mccount)] + mers;
                                    excelWorksheet.Cells[startCell+":"+endCell].Merge = true;
                                    excelWorksheet.Cells[startCell + ":" + endCell].Value = mergeColumns[i].Trim();
                                    excelWorksheet.Cells[startCell + ":" + endCell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    excelWorksheet.Cells[startCell + ":" + endCell].Style.Font.Bold = true;
                                    i = i + mccount;
                                }
                                else
                                {
                                    excelWorksheet.Cells[1, i + 1].Value = mergeColumns[i].Trim();
                                    excelWorksheet.Cells[1, i + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    excelWorksheet.Cells[1, i + 1].Style.Font.Bold = true;
                                }
                            }

                        }
                    }
                    
                }
                if (columnsName != null && columnsName.Count > 0)
                {
                    for (int j = 0; j < columnsName.Count; j++)
                    {
                        if (ishavemc&&string.IsNullOrEmpty(mergeColumns[j]))
                        {
                            string startCell = zmlist[j] + mers;
                            string endCell = zmlist[j] + merss;
                            excelWorksheet.Cells[startCell + ":" + endCell].Merge = true;
                            excelWorksheet.Cells[startCell + ":" + endCell].Value = columnsName[j].Trim();
                            excelWorksheet.Cells[startCell + ":" + endCell].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            excelWorksheet.Cells[startCell + ":" + endCell].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            excelWorksheet.Cells[startCell + ":" + endCell].Style.Font.Bold = true;
                        }
                        else
                        {
                            excelWorksheet.Cells[ss, j + 1].Value = columnsName[j].ToString();
                            excelWorksheet.Cells[ss, j + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            excelWorksheet.Cells[ss, j + 1].Style.Font.Bold = true;
                        }
                    }
                    ss++;
                }
                string entercount = Convert.ToString(ConfigurationManager.AppSettings["ExcelColumnItemCount"]);//菜单类型
                if (data != null && data.Rows.Count > 0)
                {
                    for (int k = 0; k < data.Rows.Count; k++)
                    {
                        for (int l = 0; l < data.Columns.Count; l++)
                        {
                            string dtvstr = Convert.ToString(data.Rows[k][l]).Trim();
                            if (l == 0)//第一列序号
                            {
                                excelWorksheet.Cells[k + ss, l + 1].Value = dtvstr;
                                excelWorksheet.Cells[k + ss, l + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                if (!string.IsNullOrEmpty(dtvstr))
                                {
                                    excelWorksheet.Cells[k + ss, l + 1].AutoFitColumns();
                                }
                                continue;
                            }
                            string name;
                            switch (name = data.Columns[l].DataType.Name)
                            {
                                case "Int64":
                                    if (!string.IsNullOrEmpty(dtvstr))
                                    {
                                        excelWorksheet.Cells[k + ss, l + 1].Value = Convert.ToInt64(dtvstr);
                                        excelWorksheet.Cells[k + ss, l + 1].AutoFitColumns();
                                    }
                                    else
                                    {
                                        excelWorksheet.Cells[k + ss, l + 1].Value = dtvstr;
                                    }
                                    excelWorksheet.Cells[k + ss, l + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                    excelWorksheet.Cells[ss-1, l + 1].AutoFitColumns();
                                    break;
                                case "Int32":
                                    if (!string.IsNullOrEmpty(dtvstr))
                                    {
                                        excelWorksheet.Cells[k + ss, l + 1].Value = Convert.ToInt32(dtvstr);
                                        excelWorksheet.Cells[k + ss, l + 1].AutoFitColumns();
                                    }
                                    else
                                    {
                                        excelWorksheet.Cells[k + ss, l + 1].Value = dtvstr;
                                    }
                                    excelWorksheet.Cells[k + ss, l + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                    excelWorksheet.Cells[ss - 1, l + 1].AutoFitColumns();
                                    break;
                                case "Int16":
                                    if (!string.IsNullOrEmpty(dtvstr))
                                    {
                                        excelWorksheet.Cells[k + ss, l + 1].Value = Convert.ToInt16(dtvstr);
                                        excelWorksheet.Cells[k + ss, l + 1].AutoFitColumns();
                                    }
                                    else
                                    {
                                        excelWorksheet.Cells[k + ss, l + 1].Value = dtvstr;
                                    }
                                    excelWorksheet.Cells[k + ss, l + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                    excelWorksheet.Cells[ss - 1, l + 1].AutoFitColumns();
                                    //excelWorksheet.Cells[k + ss, l + 1].Style.Numberformat.Format = "0";
                                    break;
                                case "Decimal":
                                    if (!string.IsNullOrEmpty(dtvstr))
                                    {
                                        excelWorksheet.Cells[k + ss, l + 1].Value = Convert.ToDecimal(dtvstr);
                                        excelWorksheet.Cells[k + ss, l + 1].AutoFitColumns();
                                    }
                                    else
                                    {
                                        excelWorksheet.Cells[k + ss, l + 1].Value = dtvstr;
                                    }
                                     excelWorksheet.Cells[k + ss, l + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                     excelWorksheet.Cells[ss - 1, l + 1].AutoFitColumns();
                                    break;
                                case "Double":
                                    if (!string.IsNullOrEmpty(dtvstr))
                                    {
                                        excelWorksheet.Cells[k + ss, l + 1].Value = Convert.ToDouble(dtvstr);
                                        excelWorksheet.Cells[k + ss, l + 1].AutoFitColumns();
                                    }
                                    else
                                    {
                                        excelWorksheet.Cells[k + ss, l + 1].Value = dtvstr;
                                    }
                                    
                                     excelWorksheet.Cells[k + ss, l + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                     
                                     excelWorksheet.Cells[ss - 1, l + 1].AutoFitColumns();
                                    break;
                                case "DateTime":
                                    excelWorksheet.Cells[k + ss, l + 1].Value = dtvstr;
                                    excelWorksheet.Cells[k + ss, l + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                    if (!string.IsNullOrEmpty(dtvstr))
                                    {
                                        excelWorksheet.Cells[k + ss, l + 1].AutoFitColumns();
                                    }
                                    break;
                                case "Date":
                                    excelWorksheet.Cells[k + ss, l + 1].Value = dtvstr;
                                    excelWorksheet.Cells[k + ss, l + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                    if (!string.IsNullOrEmpty(dtvstr))
                                    {
                                        excelWorksheet.Cells[k + ss, l + 1].AutoFitColumns();
                                    }
                                    break;
                                case "String":
                                    if (k == 0)//根据配置的需要换行的字数设置宽度,最后一行时判断
                                    {
                                        if (!string.IsNullOrEmpty(entercount))
                                        {
                                            int ecount = 0;
                                            int.TryParse(entercount, out ecount);
                                            if (ecount > 0)
                                            {
                                                double clength = 0;
                                                for (int rr = 0; rr < data.Rows.Count;rr++)
                                                {
                                                    double dtitemlen = GetTextExeclWidth(Convert.ToString(data.Rows[rr][l]));
                                                    if (clength < dtitemlen)
                                                    {
                                                        clength = dtitemlen;
                                                    }
                                                }
                                                double rowtitlelen = GetTextExeclWidth(columnsName[l]);
                                                if (clength < rowtitlelen)
                                                {
                                                    clength = rowtitlelen;
                                                }
                                                //if (ecount > clength)
                                                //{
                                                //    ecount = clength;
                                                //}
                                                //double gwidth=FormatExcelContent(Convert.ToString(data.Rows[k][l]), ecount);
                                                double gwidth = ecount * 18 * 0.14;
                                                if (gwidth > clength)
                                                {
                                                    gwidth = clength;
                                                }
                                                //double gwidth = 10;
                                                if (gwidth > 0)
                                                {
                                                    excelWorksheet.Column(l + 1).Style.WrapText = true;
                                                    excelWorksheet.Column(l + 1).Width = gwidth;
                                                }
                                            }
                                        }
                                    }
                                    if (string.IsNullOrEmpty(entercount)&&!string.IsNullOrEmpty(dtvstr))
                                    {
                                        excelWorksheet.Cells[k + ss, l + 1].AutoFitColumns();
                                    }
                                    excelWorksheet.Cells[k + ss, l + 1].Value = Convert.ToString(data.Rows[k][l]);
                                     excelWorksheet.Cells[k + ss, l + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                                    break;
                                case "Single":
                                    if (!string.IsNullOrEmpty(dtvstr))
                                    {
                                        excelWorksheet.Cells[k + ss, l + 1].Value = Convert.ToSingle(dtvstr);
                                        excelWorksheet.Cells[k + ss, l + 1].AutoFitColumns();
                                    }
                                    else
                                    {
                                        excelWorksheet.Cells[k + ss, l + 1].Value = dtvstr;
                                    }
                                    excelWorksheet.Cells[k + ss, l + 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                    excelWorksheet.Cells[ss - 1, l + 1].AutoFitColumns();
                                    //excelWorksheet.Cells[k + ss, l + 1].Style.Numberformat.Format = "0.00";FormatExcelContent
                                    break;
                                default:
                                    excelWorksheet.Cells[k + ss, l + 1].Value = dtvstr;
                                    if (!string.IsNullOrEmpty(dtvstr))
                                    {
                                        excelWorksheet.Cells[k + ss, l + 1].AutoFitColumns();
                                    }
                                    //excelWorksheet.Cells[ss - 1, l + 1].AutoFitColumns();
                                    break;
                            }

                            //excelWorksheet.Cells[k + ss, l + 1].Value = FormatExcelContent(Convert.ToString(data.Rows[k][l]));excelWorksheet.Cells[k + ss, l + 1]
                        }
                    }
                }
                
                //excelWorksheet.Cells.AutoFitColumns();
                excelPackage.SaveAs(fileInfo);
            }
            return filePath;
        }

        public static Dictionary<string, DataTable> ExcelToDataTable(string filePath)
        {
            Dictionary<string, DataTable> result = new Dictionary<string, DataTable>();
            FileInfo info = new FileInfo(filePath);
            ExcelPackage package = new ExcelPackage(info);
            ExcelWorkbook workbook = package.Workbook;
            foreach (ExcelWorksheet sheet in workbook.Worksheets)
            {
                DataTable dt = WorksheetToDataTable(sheet);
                result.Add(dt.TableName, dt);
            }
            return result;
        }

        private static DataTable WorksheetToDataTable(ExcelWorksheet oSheet)
        {
            int totalRows = oSheet.Dimension.End.Row;
            int totalCols = oSheet.Dimension.End.Column;
            DataTable dt = new DataTable(oSheet.Name);
            DataRow dr = null;
            for (int i = 1; i <= totalRows; i++)
            {
                if (i > 1) dr = dt.Rows.Add();
                for (int j = 1; j <= totalCols; j++)
                {
                    if (i == 1)
                        dt.Columns.Add(oSheet.Cells[i, j].Value.ToString());
                    else
                        dr[j - 1] = Convert.ToString(oSheet.Cells[i, j].Value);
                }
            }
            return dt;
        }
    }
}
