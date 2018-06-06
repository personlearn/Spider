using Spider.Models;
using Spider.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Spider.Service
{
    public class TYSpiderService : IService.ISpider
    {
        internal static TYArticle ty;
        private string url;
        public TYSpiderService(string url)
        {
            this.url = url;
            ty = new TYArticle();
            ty.TYAriclelist = new List<TYAricleBody>();
        }

        public string run()
        {
            return GetHtml(url);
        }

        private string GetHtml(string url)
        {         
            string strhtml = HttpHelper.HttpGet(new TYHttpHeader(url));
            TYRegexp.TYRegexTitle(strhtml);
            List<string> urllist = TYRegexp.TYRegexUrl(url, ty.PageCount);
            for(int i = 0; i < ty.PageCount; i++)
            {
                if (i != 0)
                {                
                    strhtml = HttpHelper.HttpGet(new TYHttpHeader(urllist[i]));
                }
                TYRegexp.TYRegexArticle(strhtml, i);
            }
            return ToXml() ? "success" : "defeat";
        }

        private bool ToXml()
        {
            List<XElement> lisxe = new List<XElement>();
            foreach (var lis in ty.TYAriclelist)
            {
                XElement xe = new XElement("Article",
                        new XElement("author", lis.author),
                        new XElement("body", lis.article),
                        new XElement("applyid", lis.applyid),
                        new XElement("pageindex", lis.pageindex)
                        );
                lisxe.Add(xe);
            }
            return StrToFile.toXml(lisxe, "source//tianya", ty.Title, "TYArticle");
        }
    }
}
