using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Spider.util
{
    public class ConfigHelper : IConfigurationSectionHandler
    {
        private static WebConfigSection config;
        public object Create(object parent, object configContext, XmlNode section)
        {
            WebConfigSection config = new WebConfigSection();

            if (section != null)
            {
                switch (config.SaveType)
                {
                    case "FILE":
                        foreach (XmlNode node in section.ChildNodes)
                        {
                            if (node.Name == "FILE")
                            {
                                foreach (XmlNode snode in node.ChildNodes)
                                {
                                    switch (snode.Name)
                                    {
                                        //case "SaveType":
                                        //    config.SaveType = node.SelectSingleNode("@value").InnerText;
                                        //    break;
                                        case "SavePath":
                                            config.SavePath = snode.SelectSingleNode("@value").InnerText;
                                            break;
                                        case "SaveFileType":
                                            config.SaveFileType = snode.SelectSingleNode("@value").InnerText;
                                            break;
                                        case "SaveArticleType":
                                            config.SaveArticleType = snode.SelectSingleNode("@value").InnerText;
                                            break;
                                    }
                                }
                                break;
                            }

                        }
                        break;
                }
                
            }
            return config;
        }

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
