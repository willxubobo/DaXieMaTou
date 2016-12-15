using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CMICT.CSP.Async.Module
{

    internal partial class RegisterResultsEntity
    {
        internal class DataList2
        {

            [JsonProperty("ENTITY_CODE")]
            public string ENTITYCODE { get; set; }

            [JsonProperty("MD_CODE")]
            public string MDCODE { get; set; }

            [JsonProperty("SERVICE_NAME")]
            public string SERVICENAME { get; set; }

            [JsonProperty("SYS_CODE")]
            public string SYSCODE { get; set; }

            [JsonProperty("TO_NODE")]
            public string TONODE { get; set; }

            [JsonProperty("password")]
            public string Password { get; set; }

            [JsonProperty("username")]
            public string Username { get; set; }
        }
    }

    internal partial class RegisterResultsEntity
    {
        internal class Result2
        {

            [JsonProperty("reqId")]
            public string ReqId { get; set; }

            [JsonProperty("dataList")]
            public DataList2[] DataList { get; set; }
        }
    }

    internal partial class RegisterResultsEntity
    {

        [JsonProperty("msgDesc")]
        public string MsgDesc { get; set; }

        [JsonProperty("msgId")]
        public string MsgId { get; set; }

        [JsonProperty("result")]
        public Result2 Result { get; set; }
    }

}

