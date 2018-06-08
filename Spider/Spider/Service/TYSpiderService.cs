using Spider.Models;
using Spider.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            for (int i = 0; i < ty.PageCount; i++)
            {
                if (TYRegexp.redis.IsExistsHashKey("TYArticle", i.ToString()) && TYRegexp.redis.GetHashValue("TYArticle", i.ToString()) != "[]")
                {
                    continue;
                }
                if (i != 0)
                {
                    strhtml = HttpHelper.HttpGet(new TYHttpHeader(urllist[i]));
                }
                TYRegexp.TYRegexArticle(strhtml, i);
            }
            //return ToXmlByRedis() ? "success" : "defeat";
            return ToTxt() ? "success" : "defeat";
        }

        private bool ToXml()
        {
            foreach (var lis in ty.TYAriclelist)
            {
                XElement xe = new XElement("Article",
                        new XElement("author", lis.author),
                        new XElement("body", lis.article),
                        new XElement("applyid", lis.applyid),
                        new XElement("pageindex", lis.pageindex)
                        );
                StrToFile.toXml(xe, "source//tianya", ty.Title, "TYArticle");
            }
            return true;
        }

        private bool ToXmlByRedis()
        {
            for (int i = 0; i < ty.PageCount; i++)
            {
                if (TYRegexp.redis.IsExistsHashKey("TYArticle", i.ToString()))
                {
                    IEnumerable<TYAricleBody> bodylis = TYRegexp.redis.GetHashValue<IEnumerable<TYAricleBody>>("TYArticle", i.ToString());
                    List<XElement> lis = new List<XElement>();
                    foreach (var body in bodylis)
                    {
                        XElement xe = new XElement("Article",
                                new XElement("author", body.author),
                                new XElement("body", body.article),
                                new XElement("applyid", body.applyid),
                                new XElement("pageindex", body.pageindex)
                                );
                        lis.Add(xe);
                    }
                    StrToFile.toXml(lis, "source//tianya", ty.Title, "TYArticle");
                    Loghelper.Info(typeof(TYSpiderService).ToString(), string.Format("第{0}页发射成功", i + 1));
                }
            }
            return true;
        }

        public bool ToTxt()
        {
            for (int i = 0; i < ty.PageCount; i++)
            {
                if (TYRegexp.redis.IsExistsHashKey("TYArticle", i.ToString()))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(string.Format("\n----------------第{0}页--------------\n", i + 1));
                    IEnumerable<TYAricleBody> bodylis = TYRegexp.redis.GetHashValue<IEnumerable<TYAricleBody>>("TYArticle", i.ToString());
                    foreach (var body in bodylis)
                    {
                        sb.Append(body.article + "\n");
                    }
                    StrToFile.toTxt(sb.ToString(), "source//tianya", ty.Title);
                    Loghelper.Info(typeof(TYSpiderService).ToString(), string.Format("第{0}页发射成功", i + 1));
                }
            }
            return true;
        }
    }
}
