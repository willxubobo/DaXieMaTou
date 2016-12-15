using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CMICT.CSP.Async.Module
{
    internal partial class ResultEntity
    {
        internal class ResultInfo
        {

            [JsonProperty("fk")]
            public string Fk { get; set; }

            [JsonProperty("msg")]
            public string Msg { get; set; }

            [JsonProperty("pk")]
            public string Pk { get; set; }

            [JsonProperty("success")]
            public string Success { get; set; }
        }
    }
    internal partial class ResultEntity
    {
        internal class Result2
        {

            [JsonProperty("reqId")]
            public string ReqId { get; set; }

            [JsonProperty("resultInfos")]
            public ResultInfo[] ResultInfos { get; set; }
        }
    }
    internal partial class ResultEntity
    {

        [JsonProperty("msgDesc")]
        public string MsgDesc { get; set; }

        [JsonProperty("msgId")]
        public string MsgId { get; set; }

        [JsonProperty("result")]
        public Result2 Result { get; set; }
    }
}
