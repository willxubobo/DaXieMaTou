using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMICT.CSP.Async.Module
{
    public class RegisterServiceEntity
    {
        public string action { get; set; }
        public string type { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public List<DataCollection> data { get; set; }
    }

    public class DataCollection
    {
        public string MD_CODE { get; set; }
        public string SYS_CODE { get; set; }
        public string ENTITY_CODE { get; set; }
        public string SERVICE_NAME { get; set; }
        public string TO_NODE { get; set; }
        public string password { get; set; }

        public string username { get; set; }
    }
}
