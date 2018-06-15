using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.util
{
    internal class RegExpHelper
    {
        internal static readonly string regTitle = @"<title>([\s\S]*?)</title>";
        internal static readonly string regTYPage = @"return goPage.*?(\d{1,4})\)";
        internal static readonly string regTYArticel = @"<div class=""atl-info"".*?>(?<author>[\s\S]*?)</div>[\s\S]*?<div class=""bbs-content.*?"">(?<article>[\s\S]*?)</div>";
        internal static readonly string regTYUrl = @"\d{1,4}(?=\.shtml$)";
        internal static readonly string regWipeOffHTMLSign1 = @"<a.*?>|</a>|<strong.*?>|</strong>|<span.*?>|</span>|\s+";
        internal static readonly string regWipeOffHTMLSign2 = @"<br>|<br />";
        internal static readonly string regTYArticel_20180606 = @"<div class=""answer-item atl-item"" id=""(?<id>\d{1,5})""[\s\S]*?js_username=""(?<author>[\s\S]*?)""[\s\S]*?<div class=""content"">(?<article>[\s\S]*?)</div>";
        internal static readonly string regTYArticel_201806061111 = @"<div class=""atl-item""[\w\W].*?id=""(?<id>\d{1,9})""[\s\S]*?<div class=""atl-info"".*?>(?<author>[\s\S]*?)</div>[\s\S]*?<div class=""bbs-content.*?"">(?<article>[\s\S]*?)</div>";
        internal static readonly string regTYArticel_host = @"<div class=""atl-item""[\w\W].*?id=""(?<id>\d{1,9})""[\s\S]*?<div class=""atl-info"".*?>[\s\S]*?<strong class=""host"">楼主</strong>(?<author>[\s\S]*?)</div>[\s\S]*?<div class=""bbs-content.*?"">(?<article>[\s\S]*?)</div>";
    }
}
