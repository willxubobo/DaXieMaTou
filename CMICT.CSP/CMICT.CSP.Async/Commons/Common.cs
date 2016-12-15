using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint;
using Microsoft.SharePoint.ApplicationPages.Calendar.Exchange;

namespace CMICT.CSP.Async.Commons
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
            
            int start = (pageIndex - 1)*pageSize + 1;;
            int end = pageIndex*pageSize;

            string sql =
                "SELECT *,RowNumber FROM (SELECT *,ROW_NUMBER() OVER( ORDER BY {0}) AS RowNumber FROM {1} ) AS RowNumberTableSource WHERE RowNumber BETWEEN {2} AND {3}";
            sql = string.Format(sql, key, table, start, end);

            DataTable dt=new DataTable();
            string constring = ConfigurationManager.AppSettings["DbConnection"];
            SqlHelper sqlHelper = new SqlHelper(constring);
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

        public static DataTable GridView2DataTable(GridView gv)
        {
            DataTable table = new DataTable();
            int rowIndex = 0;
            List<string> cols = new List<string>();
            if (!gv.ShowHeader && gv.Columns.Count == 0)
            {
                return table;
            }
            GridViewRow headerRow = gv.HeaderRow;
            int columnCount = headerRow.Cells.Count;
            for (int i = 0; i < columnCount; i++)
            {
                string text = GetCellText(headerRow.Cells[i]);
                cols.Add(text);
            }
            foreach (GridViewRow r in gv.Rows)
            {
                if (r.RowType == DataControlRowType.DataRow)
                {
                    DataRow row = table.NewRow();
                    int j = 0;
                    for (int i = 0; i < columnCount; i++)
                    {
                        string text = GetCellText(r.Cells[i]);
                        if (!String.IsNullOrEmpty(text))
                        {
                            if (rowIndex == 0)
                            {
                                string columnName = cols[i];
                                if (String.IsNullOrEmpty(columnName))
                                {
                                    continue;
                                }
                                if (table.Columns.Contains(columnName))
                                {
                                    continue;
                                }
                                DataColumn dc = table.Columns.Add();
                                dc.ColumnName = columnName;
                                dc.DataType = typeof(string);
                            }
                            row[j] = text;
                            j++;
                        }
                    }
                    rowIndex++;
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        public static string GetCellText(TableCell cell)
        {
            string text = cell.Text;
            if (!string.IsNullOrEmpty(text))
            {
                return text;
            }
            foreach (Control control in cell.Controls)
            {
                if (control != null && control is IButtonControl)
                {
                    IButtonControl btn = control as IButtonControl;
                    text = btn.Text.Replace("\r\n", "").Trim();
                    break;
                }
                if (control != null && control is ITextControl)
                {
                    LiteralControl lc = control as LiteralControl;
                    if (lc != null)
                    {
                        continue;
                    }
                    ITextControl l = control as ITextControl;

                    text = l.Text.Replace("\r\n", "").Trim();
                    break;
                }
            }
            return text;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sitebUrl"></param>
        /// <param name="webURI"></param>
        /// <param name="listName"></param>
        /// <param name="folderUrl"></param>
        /// <param name="fileName"></param>
        /// <param name="fileStream"></param>
        /// <param name="listItemDictionary"></param>
        /// <param name="isOverwrite"></param>
        /// <param name="isPublish"></param>
        /// <returns></returns>
        public static SPFile UploadSPFileToDocumentLibrary(string sitebUrl, string webURI, string listName,
                                                           string folderUrl,
                                                           string fileName, Stream fileStream,
                                                           Dictionary<string, object> listItemDictionary,
                                                           bool isOverwrite, out string errorMessage)
        {
            SPFile file = null;
            var msg = "";
            SPSecurity.RunWithElevatedPrivileges(
                delegate()
                {
                    try
                    {
                        using (SPSite siteCollection = new SPSite(sitebUrl))
                        {
                            using (SPWeb webSite = siteCollection.OpenWeb())
                            {
                                fileName = fileName.Replace("&", "").Replace("#", "").Replace("%", "").Replace("{", "").Replace("}", "");

                                string folderfullUrl = string.Empty;
                                if (!string.IsNullOrEmpty(folderUrl))
                                {
                                    folderfullUrl = folderUrl + "/" + fileName;
                                }
                                else
                                {
                                    folderfullUrl = fileName;
                                }

                                SPList splist = webSite.Lists.TryGetList(listName);
                                if (splist == null)
                                {
                                    webSite.Lists.Add(listName, "DangerPicture", SPListTemplateType.DocumentLibrary);
                                    webSite.Update();
                                }

                                splist = webSite.Lists[listName];

                                folderfullUrl = listName + "/" + folderfullUrl;
                                SPFile spFile =
                                  webSite.GetFile(folderfullUrl);
                                if (spFile.Exists)
                                {
                                    fileName = string.Concat(fileName.Split('.')[0], DateTime.Now.ToString("yyyyMMddHHmmss"), ".",
                                                             fileName.Split('.')[1]);
                                }



                                // 检查文件夹。
                                SPFolder docLibraryFolder = null;
                                if (string.IsNullOrEmpty(folderUrl))
                                {
                                    docLibraryFolder = splist.RootFolder;
                                }
                                else
                                {
                                    string relativedUrl = string.Empty;
                                    if (webURI != "/")
                                    {
                                        relativedUrl = folderUrl.Replace(webURI, "");
                                    }
                                    else
                                    {
                                        relativedUrl = folderUrl;
                                    }
                                    string folderUrlNow = string.Empty;
                                    string firstUrl = webURI + "/" + listName;
                                    foreach (string folder in relativedUrl.Split('/'))
                                    {
                                        if (folder == splist.Title || folder == "")
                                            continue;
                                        folderUrlNow += "/" + folder;
                                        string folderUrlNew = firstUrl + folderUrlNow;
                                        docLibraryFolder = webSite.GetFolder(folderUrlNew);
                                        // 创建文件夹。
                                        if (docLibraryFolder == null || !docLibraryFolder.Exists)
                                        {
                                            webSite.AllowUnsafeUpdates = true;
                                            docLibraryFolder = splist.RootFolder.SubFolders.Add(folderUrlNew);
                                            docLibraryFolder.Update();
                                            webSite.AllowUnsafeUpdates = false;
                                        }

                                    }
                                }
                                // 上传文件。
                                webSite.AllowUnsafeUpdates = true;

                                file = docLibraryFolder.Files.Add(fileName, fileStream, isOverwrite);
                                // 更新文件元数据。
                                foreach (KeyValuePair<string, object> pair in listItemDictionary)
                                {
                                    if (pair.Value != null)
                                        file.Item[pair.Key] = pair.Value;
                                }
                                file.Item.Update();
                                webSite.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        msg = ex.Message;
                    }

                });
            errorMessage = msg;
            return file;
        }

    }
}
