﻿using Spider.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Spider.Service
{
    public class TYRegexp : IRegexp
    {
        public string TYRegexTitle(string html, ref int pagecount)
        {
            string pattern = @"<title>([\s\S]*?)</title>";
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);
            Match title = r.Match(html);

            pattern = @"return goPage.*?(\d{1,4})\)";
            r = new Regex(pattern, RegexOptions.IgnoreCase);
            int.TryParse(r.Match(html).Result("$1"), out pagecount);
            return title.Result("$1");
        }

        public string TYRegexArticle(string html)
        {
            string pattern = @"<div class=""atl-info"".*?>(?<author>[\s\S]*?)</div>[\s\S]*?<div class=""bbs-content.*?"">(?<article>[\s\S]*?)</div>";
            Regex r = new Regex(pattern, RegexOptions.IgnoreCase);

            //return r.Matches(html);
            MatchCollection articles = r.Matches(html);
            List<Match> listmatch = new List<Match>();
            StringBuilder sb = new StringBuilder();
            foreach (Match item in articles)
            {
                sb.Append(WipeOffHTMLSign(item.Groups["author"].Value.Trim()) + "\n");
                sb.Append(WipeOffHTMLSign(item.Groups["article"].Value.Trim()) + "\n");
            }
            return sb.ToString();
        }

        public List<string> TYRegexUrl(string url, int pagecount)
        {
            List<string> urllist = new List<string>();
            string input = url;
            string pattern = @"\d{1,4}(?=\.shtml$)";
            for (int i = 1; i < pagecount + 1; i++)
            {
                string replacement = i.ToString();
                string val = Regex.Replace(input, pattern, replacement);
                urllist.Add(val);
            }
            return urllist;
        }

        public string WipeOffHTMLSign(string Article)
        {
            string pattern = @"<a.*?>|</a>|<strong.*?>|</strong>|<span.*?>|</span>|\s+";
            string pattern1 = @"<br>|<br />";
            string replacement = " ";
            string replacement1 = "\n";
            string val = Regex.Replace(Article, pattern, replacement);
            string article = Regex.Replace(val, pattern1, replacement1);

            return article;
        }

        public string RunRegexp()
        {
            throw new NotImplementedException();
        }
    }
}
