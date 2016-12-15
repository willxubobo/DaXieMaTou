using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.Model.BusinessSearchModels
{
    public class CommunicationModel
    {
        /// <summary>
        /// 发送方的模板ID
        /// </summary>
        public string TemplateID { get; set; }
        /// <summary>
        /// 发送方的字段
        /// </summary>
        public Dictionary<string, string> Filters { get; set; }
    }
}
