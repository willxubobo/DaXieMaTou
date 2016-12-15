using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.BLL.Components
{
    public class UploadFileToFtpComponent
    {
        private static string ftpServerIP = ConfigurationManager.AppSettings["ftpServerIP"].ToString();//服务器ip
        private static string ftpUserID = ConfigurationManager.AppSettings["ftpUserID"].ToString();//用户名
        private static string ftpPassword = ConfigurationManager.AppSettings["ftpPassword"].ToString();//密码
        private static string serverDir = ConfigurationManager.AppSettings["serverDir"].ToString();//上传文件夹
        //filename 为本地文件的绝对路径
        //serverDir为服务器上的目录
        public static bool Upload(string filename)
        {
            bool result = true;
            try
            {
                FileInfo fileInf = new FileInfo(filename);

                string uri = string.Empty;
                if (!string.IsNullOrEmpty(serverDir))
                {
                    uri = string.Format("ftp://{0}/{1}/{2}", ftpServerIP, serverDir, fileInf.Name);
                }
                else
                {
                    uri = string.Format("ftp://{0}/{1}", ftpServerIP, fileInf.Name);
                }
                FtpWebRequest reqFTP;

                // 根据uri创建FtpWebRequest对象 
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));

                // ftp用户名和密码
                reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);

                // 默认为true，连接不会被关闭
                // 在一个命令之后被执行
                reqFTP.KeepAlive = false;

                // 指定执行什么命令
                reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

                // 指定数据传输类型
                reqFTP.UseBinary = true;

                // 上传文件时通知服务器文件的大小
                reqFTP.ContentLength = fileInf.Length;

                // 缓冲大小设置为2kb
                int buffLength = 2048;

                byte[] buff = new byte[buffLength];
                int contentLen;

                // 打开一个文件流 (System.IO.FileStream) 去读上传的文件
                FileStream fs = fileInf.OpenRead();

                // 把上传的文件写入流
                Stream strm = reqFTP.GetRequestStream();

                // 每次读文件流的2kb
                contentLen = fs.Read(buff, 0, buffLength);

                // 流内容没有结束
                while (contentLen != 0)
                {
                    // 把内容从file stream 写入 upload stream
                    strm.Write(buff, 0, contentLen);

                    contentLen = fs.Read(buff, 0, buffLength);
                }

                // 关闭两个流
                strm.Close();
                fs.Close();
            }
            catch (Exception ex)
            {
                BaseComponent.Error("上传到ftp错误：" + ex.Message);
                result = false;
                // MessageBox.Show(ex.Message, "Upload Error");
                //Response.Write("Upload Error：" + ex.Message);
            }
            return result;
        }


        /// <summary>
        /// method to check the existance of a file on the server
        /// </summary>
        /// <param name="fileName">file name e.g file1.txt</param>
        /// <param name="strFTPPath">FTP server path i.e: ftp://yourserver/foldername</param>
        /// <param name="strftpUserID">username</param>
        /// <param name="strftpPassword">password</param>
        /// <returns>true (if file exists) or false</returns>
        public static bool CheckFTPFile(string fileName, string strFTPPath, string strftpUserID, string strftpPassword)
        {
            bool result = false;
            FtpWebRequest reqFTP;

            // dirName = name of the directory to create.
            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(strFTPPath));
            reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
            reqFTP.UseBinary = true;
            reqFTP.Credentials = new NetworkCredential(strftpUserID, strftpPassword);
            FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
            StreamReader ftpStream = new StreamReader(response.GetResponseStream());

            //List<string> files = new List<string>();
            string line = ftpStream.ReadLine();
            while (line != null)
            {
                if (line.ToLower().Trim() == fileName.ToLower().Trim())
                {
                    result = true;
                    break;
                }
                //files.Add(line);
                line = ftpStream.ReadLine();
            }

            ftpStream.Close();
            response.Close();
            return result;
            //return files.Contains(fileName);
        }
        ////调用方法
        //string filename = "D:\\test.txt"; //本地文件，需要上传的文件
        //string serverDir = "img"; //上传到服务器的目录，必须存在
        //Upload(filename,serverDir);
    }
}
