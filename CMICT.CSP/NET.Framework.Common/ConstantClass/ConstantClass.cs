using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Framework.Common.ConstantClass
{
    public class ConstantClass
    {
        public const string FULLCONTROL = "full control";
        public const string READ = "read";
        public const string FULLCONTROLCN = "完全控制";
        public const string READCN = "读取";
        public const string MANAGE = "管理";
        public const string VIEW = "查看";
        public const string PAGESFORM = "页面";
        public const string CHECKIN = "已签入";
        public const string CHECKEDOUT = "签出";
        public const int PAGETITLESUBLEN = 20;
        public const string SPGROUP = "SPGroup";
        public const string LIMITEDACCESS = "limited access";
        public const string LIMITEDACCESSCN = "受限访问";
        public const string SYSMANAGEMENT = "sysmanagement";
        public const string TEMPLATEMANAGEMENT = "templatemanagement";
        public const string BUSINESSONLINE= "businessonline";
        public const string CHANGEPASSWORD = "changepassword";
        public const string ALLUSERGROUP = "所有人";
        public enum CodeMappingColumn
        {
            [Description("目标客户公司代码")]
            CustomerID,
            [Description("目标客户公司名称")]
            CustomerName,
            [Description("目标客户说明")]
            CustomerDesc,
            [Description("语义内容")]
            SemanticDesc,
            [Description("业务代码")]
            BusinessCode,
            [Description("业务代码说明")]
            BusinessCodeDesc,
            [Description("业务翻译说明")]
            BusinessTranslation,
            [Description("目标客户代码")]
            TargetCode,
            [Description("目标客户代码英文描述")]
            TargetCodeEnDesc,
            [Description("目标客户代码中文描述")]
            TargetCodeCnDesc,
            [Description("我方代码")]
            CMICTCode,
            [Description("我方代码英文描述")]
            CMICTCodeEnDesc,
            [Description("我方代码中文描述")]
            CMICTCodeCnDesc,
            [Description("生效时间")]
            StartDate,
            [Description("失效时间")]
            ExpireDate
        }
    }


}
