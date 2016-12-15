using CMICT.CSP.BLL.Components;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.ComponentModel;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace CMICT.CSP.Web.BusinessOnLine.BoxInfoTransfer
{
    [ToolboxItemAttribute(false)]
    public partial class BoxInfoTransfer : WebPart
    {

        CMICT.CSP.BLL.Components.BoxInfoTransfer com = new BLL.Components.BoxInfoTransfer();

        // 仅当使用检测方法对场解决方案进行性能分析时才取消注释以下 SecurityPermission
        // 特性，然后在代码准备进行生产时移除 SecurityPermission 特性
        // 特性。因为 SecurityPermission 特性会绕过针对您的构造函数的调用方的
        // 安全检查，不建议将它用于生产。
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public BoxInfoTransfer()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btn_Verify_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {

                VerifyFile();

            });
            
        }

        private void VerifyFile()
        {
            string strName = FileUpload1.PostedFile.FileName;//使用fileupload控件获取上传文件的文件名
            if (strName != "")//如果文件名存在
            {
                bool fileOK = false;
                int i = strName.LastIndexOf(".");
                string fileExtension = strName.Substring(i);

                string newName = Guid.NewGuid().ToString();//生成新的文件名，保证唯一性

                string path = SPUtility.GetCurrentGenericSetupPath(@"TEMPLATE\LAYOUTS\CMICT.CSP.Web\BoxInfoTransferFolder\");

                filepath.Value = path + newName + fileExtension;

                if (FileUpload1.HasFile)//验证 FileUpload 控件确实包含文件
                {

                    String[] allowedExtensions = { ".xls" };
                    for (int j = 0; j < allowedExtensions.Length; j++)
                    {
                        if (fileExtension == allowedExtensions[j])
                        {
                            fileOK = true;
                            break;
                        }
                    }
                }
                if (fileOK)
                {
                    try
                    {
                        // 判定该路径是否存在
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        FileUpload1.PostedFile.SaveAs(filepath.Value);//存储到服务器上
                    }
                    catch (Exception)
                    {
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.alert('请根据模板填写，并上传正确格式的文件！');", true);
                    return;
                }

                string result = com.VerifyExcel(filepath.Value);
                if (string.IsNullOrEmpty(result))
                {
                    btn_Download.Visible = false;

                    string fileName = "";
                    string fileContent = com.TransferFileContent(filepath.Value, out fileName);

                    string pathupload = SPUtility.GetCurrentGenericSetupPath(@"TEMPLATE\LAYOUTS\CMICT.CSP.Web\BoxInfoTransferFolder\");
                    //string newFileName = Guid.NewGuid().ToString();
                    string newFilePath = pathupload + fileName;
                    StreamWriter sw = new StreamWriter(newFilePath);
                    sw.Write(fileContent);
                    sw.Close();
                    if (UploadFileToFtpComponent.Upload(newFilePath))
                    {
                        lblError.Text = "数据验证通过，已成功上传！";
                    }
                    else
                    {
                        lblError.Text = "上传失败！";
                    }
                }
                else
                {
                    btn_Download.Visible = false;
                    lblError.Text = ("请修正以下错误后重新上传：" + Environment.NewLine + result).Replace(Environment.NewLine, "<br/>");
                }
            }
            else {
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "connsuc", "layer.alert('请先选择需上传的文件！');", true);
            }
        }

        protected void btn_Download_Click(object sender, EventArgs e)
        {
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {

                DownloadFile();

            });

            
        }

        private void DownloadFile()
        {
            string fileName = "";
            string fileContent = com.TransferFileContent(filepath.Value, out fileName);

            string path = SPUtility.GetCurrentGenericSetupPath(@"TEMPLATE\LAYOUTS\CMICT.CSP.Web\BoxInfoTransferFolder\");
            string newFileName = Guid.NewGuid().ToString();
            string newFilePath = path + newFileName + ".txt";
            StreamWriter sw = new StreamWriter(newFilePath);
            sw.Write(fileContent);
            sw.Close();


            Page.Response.ClearHeaders();
            Page.Response.Clear();
            Page.Response.Expires = 0;
            Page.Response.Buffer = true;
            Page.Response.AddHeader("Accept-Language", "zh-cn");
            string name = System.IO.Path.GetFileName(newFilePath);
            System.IO.FileStream files = new FileStream(newFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] byteFile = null;
            if (files.Length == 0)
            {
                byteFile = new byte[1];
            }
            else
            {
                byteFile = new byte[files.Length];
            }
            files.Read(byteFile, 0, (int)byteFile.Length);
            files.Close();

            if (File.Exists(filepath.Value))
            {
                //删除上传文件
                File.Delete(filepath.Value);
            }

            if (File.Exists(newFilePath))
            {
                //删除生成文件
                File.Delete(newFilePath);
            }
            btn_Download.Visible = false;

            Page.Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName, System.Text.Encoding.UTF8));
            Page.Response.ContentType = "application/octet-stream;charset=gbk";
            Page.Response.BinaryWrite(byteFile);
            Page.Response.End();

            
        }
    }
}
