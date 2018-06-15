using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MySpider.util
{
    public class ConfigHelper 
    {
        private static WebConfigSection config;

        public static WebConfigSection GetConfig(string configname)
        {
            if (config == null)
            {
                config = ConfigurationManager.GetSection(configname) as WebConfigSection;
            }
            return config;
        }
    }
}
