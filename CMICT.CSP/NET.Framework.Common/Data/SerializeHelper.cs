using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using NET.Framework.Common.Extensions;
using Newtonsoft.Json;
using System.Text.RegularExpressions;

namespace NET.Framework.Common.Data
{
    public class SerializeHelper
    {
        #region Json 序列化

        /// <summary>
        /// 处理Json的时间格式为正常格式
        /// </summary>
        public static string JsonDateTimeFormat(string json)
        {
            json = Regex.Replace(json,
                @"\\/Date\((\d+)\)\\/",
                match =>
                {
                    DateTime dt = new DateTime(1970, 1, 1);
                    dt = dt.AddMilliseconds(long.Parse(match.Groups[1].Value));
                    dt = dt.ToLocalTime();
                    return dt.ToString("yyyy-MM-dd HH:mm:ss.fff");
                });
            return json;
        }

        /// <summary>
        /// 把对象序列化成Json字符串格式
        /// </summary>
        /// <param name="object"></param>
        /// <returns></returns>
        public static string ToJson(object @object)
        {
            string json = JsonConvert.SerializeObject(@object);
            return JsonDateTimeFormat(json);
        }

        /// <summary>
        /// 把Json字符串转换为强类型对象
        /// </summary>
        public static T FromJson<T>(string json)
        {
            json = JsonDateTimeFormat(json);
            return JsonConvert.DeserializeObject<T>(json);
        }

        #endregion

        #region 二进制序列化

        /// <summary>
        /// 将数据序列化为二进制数组
        /// </summary>
        public static byte[] ToBinary(object data)
        {
            data.CheckNotNull("data");
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(ms, data);
                ms.Seek(0, 0);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// 将二进制数组反序列化为强类型数据
        /// </summary>
        public static T FromBinary<T>(byte[] bytes)
        {
            bytes.CheckNotNullOrEmpty("bytes");
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(ms);
            }
        }

        /// <summary>
        /// 将数据序列化为二进制数组并写入文件中
        /// </summary>
        public static void ToBinaryFile(object data, string fileName)
        {
            data.CheckNotNull("data");
            fileName.CheckFileExists("fileName");
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, data);
            }
        }

        /// <summary>
        /// 将指定二进制数据文件还原为强类型数据
        /// </summary>
        public static T FromBinaryFile<T>(string fileName)
        {
            fileName.CheckFileExists("fileName");
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(fs);
            }
        }

        #endregion

        #region XML序列化

        /// <summary>
        /// 将数据序列化为XML形式
        /// </summary>
        public static string ToXml(object data)
        {
            data.CheckNotNull("data");
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(data.GetType());
                serializer.Serialize(ms, data);
                ms.Seek(0, 0);
                return Encoding.Default.GetString(ms.ToArray());
            }
        }

        /// <summary>
        /// 将XML数据反序列化为强类型
        /// </summary>
        public static T FromXml<T>(string xml)
        {
            xml.CheckNotNull("xml");
            //byte[] bytes = Encoding.Default.GetBytes(xml);
            using (StringReader rdr = new StringReader(xml))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(rdr);
            }
        }

        /// <summary>
        /// 将数据序列化为XML并写入文件
        /// </summary>
        public static void ToXmlFile(object data, string fileName)
        {
            data.CheckNotNull("data");
            fileName.CheckNotNullOrEmpty("fileName");
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                XmlSerializer serializer = new XmlSerializer(data.GetType());
                serializer.Serialize(fs, data);
            }
        }

        /// <summary>
        /// 将指定XML数据文件还原为强类型数据
        /// </summary>
        public static T FromXmlFile<T>(string fileName)
        {
            fileName.CheckFileExists("fileName");
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                fs.Seek(0, 0);
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                return (T)serializer.Deserialize(fs);
            }
        }

        #endregion
    }
}
