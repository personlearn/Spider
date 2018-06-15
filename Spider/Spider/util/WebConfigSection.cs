using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.util
{
    /// <summary>
    /// 网站信息配置节点
    /// </summary>
    public class WebConfigSection : ConfigurationSection
    {

        [ConfigurationProperty("SpiderName", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string WebName
        {

            get { return (string)this["SpiderName"]; }
            set { this["SpiderName"] = value; }
        }

        [ConfigurationProperty("SaveType", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string SaveType
        {

            get { return (string)this["SaveType"]; }
            set { this["SaveType"] = value; }
        }

        //public string SaveType { get; set; }

        public string SavePath { get; set; }

        public string SaveFileType { get; set; }

        public string SaveArticleType { get; set; }

    }
    
}
