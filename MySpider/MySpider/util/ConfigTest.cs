using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace MySpider.util
{
    public sealed class ConfigurationServiceSection : ConfigurationSection
    {
        [ConfigurationProperty("ConfigurationServices", IsDefaultCollection = true, IsRequired = true)]
        [ConfigurationCollection(typeof(ConfigurationServices))]
        public ConfigurationServices ConfigurationServices
        {
            get
            {
                return (ConfigurationServices)base["ConfigurationServices"];
            }
        }
    }

    //public sealed class ConfigurationServiceSection : ConfigurationSection
    //{
    //    [ConfigurationProperty("ConfigurationServices", IsDefaultCollection = true, IsRequired = true)]
    //    [ConfigurationCollection(typeof(ConfigurationServices), AddItemName = "ConfigurationService")]
    //    public ConfigurationServices ConfigurationServices
    //    {
    //        get
    //        {
    //            return (ConfigurationServices)base["ConfigurationServices"];
    //        }
    //    }
    //}

    public sealed class ConfigurationServices : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ConfigurationService();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            ConfigurationService configService = (ConfigurationService)element;
            return configService.Name;
        }

        //public override ConfigurationElementCollectionType CollectionType
        //{
        //    get { return ConfigurationElementCollectionType.BasicMap; }
        //}
        //protected override string ElementName
        //{
        //    get { return "ConfigurationService"; }
        //}
    }

    public sealed class ConfigurationService : ConfigurationElement
    {
        /// <summary>
        /// name
        /// </summary>
        [ConfigurationProperty("name", IsKey = true, IsRequired = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        /// <summary>
        /// host
        /// </summary>
        [ConfigurationProperty("host", IsKey = false, IsRequired = true)]
        public string Host
        {
            get { return (string)this["host"]; }
            set { this["host"] = value; }
        }

        /// <summary>
        /// port
        /// </summary>
        [ConfigurationProperty("port", IsKey = false, IsRequired = true)]
        public string Port
        {
            get { return (string)this["port"]; }
            set { this["port"] = value; }
        }

        /// <summary>
        /// location
        /// </summary>
        [ConfigurationProperty("location", IsKey = false, IsRequired = true)]
        public string Location
        {
            get { return (string)this["location"]; }
            set { this["location"] = value; }
        }
    }


    //: IConfigurationSectionHandler
    //public object Create(object parent, object configContext, XmlNode section)
    //{
    //    WebConfigSection config = ConfigHelper.GetConfig("spiderconfig");

    //    if (section != null)
    //    {
    //        switch (config.SaveType)
    //        {
    //            case "FILE":
    //                foreach (XmlNode snode in section.ChildNodes)
    //                {
    //                    switch (snode.Name)
    //                    {
    //                        //case "SaveType":
    //                        //    config.SaveType = node.SelectSingleNode("@value").InnerText;
    //                        //    break;
    //                        case "SavePath":
    //                            config.SavePath = snode.SelectSingleNode("@value").InnerText;
    //                            break;
    //                        case "SaveFileType":
    //                            config.SaveFileType = snode.SelectSingleNode("@value").InnerText;
    //                            break;
    //                        case "SaveArticleType":
    //                            config.SaveArticleType = snode.SelectSingleNode("@value").InnerText;
    //                            break;
    //                    }
    //                }
    //                break;
    //        }

    //    }
    //    return config;
    //}
}
