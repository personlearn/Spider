using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.util
{
    internal class RegExpHelper
    {
        internal const string regTitle = @"<title>([\s\S]*?)</title>";
        internal const string regTYPage = @"return goPage.*?(\d{1,4})\)";
        internal const string regTYArticel = @"<div class=""atl-info"".*?>(?<author>[\s\S]*?)</div>[\s\S]*?<div class=""bbs-content.*?"">(?<article>[\s\S]*?)</div>";
        internal const string regTYUrl = @"\d{1,4}(?=\.shtml$)";
        internal const string regWipeOffHTMLSign1 = @"<a.*?>|</a>|<strong.*?>|</strong>|<span.*?>|</span>|\s+";
        internal const string regWipeOffHTMLSign2 = @"<br>|<br />";
        internal const string regTYArticel_20180606 = @"<div class=""answer-item atl-item"" id=""(?<id>\d{1,5})""[\s\S]*?js_username=""(?<author>[\s\S]*?)""[\s\S]*?<div class=""content"">(?<article>[\s\S]*?)</div>";
        internal const string regTYArticel_201806061111 = @"<div class=""atl-item""[\w\W].*?id=""(?<id>\d{1,5})""[\s\S]*?<div class=""atl-info"".*?>(?<author>[\s\S]*?)</div>[\s\S]*?<div class=""bbs-content.*?"">(?<article>[\s\S]*?)</div>";
    }
}
