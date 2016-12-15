using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CMICT.CSP.Async.Module
{
    internal partial class GlobalRequestEntity
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
        public string Pagesize { get; set; }

        [JsonProperty("page")]
        public string Page { get; set; }
    }
}
