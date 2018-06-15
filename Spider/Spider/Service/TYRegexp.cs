using Spider.IService;
using Spider.Models;
using Spider.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spider.Service
{
    public class TYRegexp
    {
        private static readonly string classinfo = typeof(TYRegexp).ToString();
        public static Redishelper redis = Redishelper.GetRedis("127.0.0.1:6379");
        public static void TYRegexTitle(string html)
        {
            string pattern = RegExpHelper.regTitle;
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            Match title = r.Match(html);

            int pagecount;
            pattern = RegExpHelper.regTYPage;
            r = new Regex(pattern, RegexOptions.IgnoreCase);
            int.TryParse(r.Match(html).Result("$1"), out pagecount);

            TYSpiderService.ty.Title = title.Result("$1");
            TYSpiderService.ty.PageCount = pagecount;
        }

        public static void TYRegexArticle(string html,int pageindex)
        {
            string pattern = RegExpHelper.regTYArticel_host;
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);

            //return r.Matches(html);
            MatchCollection articles = r.Matches(html);
            List<Match> listmatch = new List<Match>();
            //StringBuilder sb = new StringBuilder();
            //foreach (Match item in articles)
            //{
            //    sb.Append(WipeOffHTMLSign(item.Groups["author"].Value.Trim()) + "\n");
            //    sb.Append(WipeOffHTMLSign(item.Groups["article"].Value.Trim()) + "\n");
            //}
            //return sb.ToString();
         
            List<TYAricleBody> list = new List<TYAricleBody>();
            foreach (Match item in articles)
            {
                TYAricleBody body = new TYAricleBody();
                body.author = WipeOffHTMLSign(item.Groups["author"].Value.Trim());
                body.article = WipeOffHTMLSign(item.Groups["article"].Value.Trim());
                body.applyid = WipeOffHTMLSign(item.Groups["id"].Value.Trim());
                body.pageindex = pageindex + 1;
                list.Add(body);
                //redis.HashSet("TYArticle", body.applyid, body);
                //TYSpiderService.ty.TYAriclelist.Add(body);
            }
            redis.HashSet("TYArticleHOST", pageindex.ToString(), list);            
            Loghelper.Info(classinfo, string.Format("第{0}页已装填完毕,字节长度{1}", pageindex + 1, html.Length));
        }

        public static List<string> TYRegexUrl(string url, int pagecount)
        {
            List<string> urllist = new List<string>();
            string input = url;
            string pattern = RegExpHelper.regTYUrl;
            for (int i = 1; i < pagecount + 1; i++)
            {
                string replacement = i.ToString();
                string val = Regex.Replace(input, pattern, replacement);
                urllist.Add(val);
            }
            return urllist;
        }

        public static string WipeOffHTMLSign(string Article)
        {
            string pattern = RegExpHelper.regWipeOffHTMLSign1;
            string pattern1 = RegExpHelper.regWipeOffHTMLSign2;
            string replacement = " ";
            string replacement1 = "\n";
            string val = Regex.Replace(Article, pattern, replacement);
            string article = Regex.Replace(val, pattern1, replacement1);

            return article;
        }
    }
}
