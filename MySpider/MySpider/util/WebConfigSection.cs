using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MySpider.util
{
    /// <summary>
    /// 网站信息配置节点
    /// </summary>
    public class WebConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("FILE", IsDefaultCollection = true, IsRequired = true)]
        [ConfigurationCollection(typeof(FILE), AddItemName = "FILE")]
        public FILE FILE
        {
            get
            {
                return (FILE)base["FILE"];
            }
        }

        [ConfigurationProperty("SpiderName", DefaultValue = "", IsRequired = true, IsKey = false)]
        public string SpiderName
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
    }

    public class FILE : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new param();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            param configService = (param)element;
            return configService.Name;
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }
        protected override string ElementName
        {
            get { return "param"; }
        }

    }

    public sealed class param : ConfigurationElement
    {

        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("value", IsKey = false, IsRequired = true)]
        public string value
        {
            get { return (string)this["value"]; }
            set { this["value"] = value; }
        }
    }
}
