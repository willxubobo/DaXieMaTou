using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CMICT.CSP.WebDataSubscribe.Entities
{
    internal partial class SearchResultsData
    {
        internal class Result2
        {

            [JsonProperty("total")]
            public string Total { get; set; }

            [JsonProperty("totalPage")]
            public string TotalPage { get; set; }

            [JsonProperty("pagesize")]
            public string Pagesize { get; set; }

            [JsonProperty("page")]
            public string Page { get; set; }

            [JsonProperty("isMore")]
            public string IsMore { get; set; }

            [JsonProperty("dataList")]
            public DataTable DataList { get; set; }
        }
    }

    internal partial class SearchResultsData
    {

        [JsonProperty("msgID")]
        public string MsgID { get; set; }

        [JsonProperty("msgDesc")]
        public string MsgDesc { get; set; }

        [JsonProperty("result")]
        public Result2 Result { get; set; }
    }
}