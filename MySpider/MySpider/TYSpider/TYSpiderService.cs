using MySpider.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace MySpider.TYSpider
{
    class TYSpiderService
    {
        private string SavePath;
        private const string SpiderName = "tyspider";
        private string SaveFileType;
        private string SaveArticleType;
        public TYSpiderService()
        {
            WebConfigSection config = ConfigHelper.GetConfig("spiderconfig");
            if (config.SpiderName == SpiderName)
            {
                foreach (param par in config.FILE)
                {
                    switch (par.Name)
                    {
                        case "path":
                            SavePath = par.value;
                            break;
                        case "savetype":
                            SaveFileType = par.value;
                            break;
                        case "SaveArticleType":
                            SaveArticleType = par.value;
                            break;
                    }
                }
            }
        }

        public void Run(string url)
        {
            GetHtml(url);
        }

        public bool GetHtml(string url)
        {
            HtmlWeb webClient = new HtmlWeb();
            HtmlDocument doc = webClient.Load(url);
            return false;
        }
    }
}
