using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CMICT.CSP.WebDataSubscribe.Entities
{

    internal partial class ResultData
    {
        internal class Result2
        {

            [JsonProperty("reqId")]
            public string ReqId { get; set; }

            [JsonProperty("dataList")]
            public DataTable DataList { get; set; }

            [JsonProperty("total")]
            public int Total { get; set; }

            [JsonProperty("pagesize")]
            public int Pagesize { get; set; }

            [JsonProperty("page")]
            public int Page { get; set; }

            [JsonProperty("totalpage")]
            public int Totalpage { get; set; }

            [JsonProperty("isMore")]
            public string IsMore { get; set; }
        }
    }

    internal partial class ResultData
    {
        [JsonProperty("msgDesc")]
        public string MsgDesc { get; set; }

        [JsonProperty("msgId")]
        public string MsgId { get; set; }

        [JsonProperty("result")]
        public Result2 Result { get; set; }
    }
}