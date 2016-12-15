using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NET.Framework.Common.Extensions;
using System.Configuration;

namespace NET.Framework.Common.ConfigHelper
{
    public class AppConfig : IConfiguration
    {
        private static Dictionary<string, string> allConfigs;
        public Dictionary<string, string> AllConfigs
        {
            get
            {
                if (allConfigs == null)
                { 
                    allConfigs = new Dictionary<string, string>();
                    foreach (string key in ConfigurationManager.AppSettings)
                    {
                        if (!allConfigs.ContainsKey(key))
                        {
                            allConfigs.Add(key, ConfigurationManager.AppSettings[key]);
                        }
                        else 
                        {
                            throw new Exception("AppSettings包含重复Key");
                        }
                    }
                }
                return allConfigs;
            }
        }
        public  string GetValue(string key)
        {
            key.CheckNotNullOrEmpty("key");
            return AllConfigs.ContainsKey(key) ? AllConfigs[key] : "";
        }

        

    }
}
