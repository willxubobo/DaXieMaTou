using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace CMICT.CSP.WebDataSubscribe.Entities
{
    internal partial class SearchEntityData
    {
        [JsonProperty("action")]
        public string Action { get; set; }

        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("entity")]
        public string Entity { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("pagesize")]
        public int PageSize { get; set; }
        [JsonProperty("page")]
        public int Page { get; set; }
    }
}