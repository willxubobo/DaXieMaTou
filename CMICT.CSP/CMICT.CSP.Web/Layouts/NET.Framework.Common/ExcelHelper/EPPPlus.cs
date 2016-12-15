using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Framework.Common.ExcelHelper
{
    public class EPPPlus
    {
        public static string CreateFile(string filePath)
        {
            return CreateFile(filePath, null);
        }
        public static string CreateFile(string filePath, DataTable data)
        {
            return CreateFile(filePath, data, new List<string>());
        }
        public static string CreateFile(string filePath, DataTable data, List<string> columnsName)
        {
            return CreateFile(filePath, data, columnsName, false);
        }
        public static string CreateFile(string filePath, DataTable data, List<string> columnsName, bool overwriteIfFileExist)
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
                ExcelWorksheet excelWorksheet = excelPackage.Workbook.Worksheets.Add("export data");
                if (data != null && (columnsName == null || columnsName.Count == 0))
                {
                    columnsName = new List<string>();
                    for (int i = 0; i < data.Columns.Count; i++)
                    {
                        columnsName.Add(data.Columns[i].ColumnName);
                    }
                }
                if (columnsName != null && columnsName.Count > 0)
                {
                    for (int j = 0; j < columnsName.Count; j++)
                    {
                        excelWorksheet.Cells[1, j + 1].Value = columnsName[j].ToString();
                    }
                }
                if (data != null && data.Rows.Count > 0)
                {
                    for (int k = 0; k < data.Rows.Count; k++)
                    {
                        for (int l = 0; l < data.Columns.Count; l++)
                        {
                            string name;
                            switch (name = data.Columns[l].DataType.Name)
                            {
                                case "System.Int64":
                                case "System.Int32":
                                case "System.Int16":
                                    excelWorksheet.Cells[k + 2, l + 1].Style.Numberformat.Format = "0";
                                    break;
                                case "System.Decimal":
                                case "System.Double":
                                case "System.Single":
                                    excelWorksheet.Cells[k + 2, l + 1].Style.Numberformat.Format = "0.00";
                                    break;
                            }
                            excelWorksheet.Cells[k + 2, l + 1].Value = data.Rows[k][l];
                        }
                    }
                }
                excelWorksheet.Cells.AutoFitColumns();
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
                        dr[j - 1] = oSheet.Cells[i, j].Value.ToString();
                }
            }
            return dt;
        }
    }
}
