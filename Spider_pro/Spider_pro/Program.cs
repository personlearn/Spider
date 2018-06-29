using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spider.Core;

namespace Spider_pro
{
    class Program
    {
        static void Main(string[] args)
        {
            Site site = new Site();
            site.AddStartUrl("http://bbs.tianya.cn/post-worldlook-223829-1.shtml");
            Spider.Core.Spider spider = new Spider.Core.Spider(site);
            spider.Execute();
        }
    }
}
