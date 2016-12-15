using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NET.Framework.Common.ConfigHelper
{
    public interface IConfiguration
    {
        Dictionary<string, string> AllConfigs { get;  }
        string GetValue(string key);


        
    }
}
