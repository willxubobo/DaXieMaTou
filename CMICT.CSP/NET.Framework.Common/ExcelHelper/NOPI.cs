using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Framework.Common.ExcelHelper
{
    public class NOPI
    {
        public static Dictionary<string, DataTable> ExcelToDataTable(string filePath)
        {
            Dictionary<string, DataTable> result = new Dictionary<string, DataTable>();
            HSSFWorkbook hssfworkbook;
            int ColumnDataNum = 0;
            #region//初始化信息
            try
            {
                using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    hssfworkbook = new HSSFWorkbook(file);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            #endregion
            for (int i = 0; i < hssfworkbook.NumberOfSheets; i++)
            {
                NPOI.SS.UserModel.ISheet sheet = hssfworkbook.GetSheetAt(i);
                System.Collections.IEnumerator rows = sheet.GetRowEnumerator();
                DataTable dt = new DataTable();
                rows.MoveNext();
                HSSFRow row = (HSSFRow)rows.Current;
                for (int j = 0; j < (sheet.GetRow(0).LastCellNum); j++)
                {
                    //dt.Columns.Add(Convert.ToChar(((int)'A') + j).ToString());  
                    //将第一列作为列表头  
                    dt.Columns.Add(row.GetCell(j).ToString());
                }
                while (rows.MoveNext())
                {
                    row = (HSSFRow)rows.Current;
                    DataRow dr = dt.NewRow();
                    for (int j = 0; j < row.LastCellNum; j++)
                    {
                        NPOI.SS.UserModel.ICell cell = row.GetCell(j);
                        if (cell == null)
                        {
                            dr[j] = null;
                        }
                        else
                        {
                            dr[j] = cell.ToString();
                            if (cell.ToString() == "")
                            {
                                ColumnDataNum = ColumnDataNum + 1;
                            }
                        }
                    }
                    if (ColumnDataNum != row.LastCellNum)
                    {
                        dt.Rows.Add(dr);
                    }

                    ColumnDataNum = 0;
                }
                result.Add(sheet.SheetName, dt);
            }


            //文件是否存在  
            if (System.IO.File.Exists(filePath))
            {

            }
            return result;
        }



    }
}
