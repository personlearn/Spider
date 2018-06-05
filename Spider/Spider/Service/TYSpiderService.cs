using Spider.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Service
{
    public class TYSpiderService : IService.ISpider
    {
        string url;
        public TYSpiderService(string url)
        {
            this.url = url;
        }

        public string run()
        {
            return GetHtml(url);
        }

        private string GetHtml(string url)
        {
            var h = new TYHttpHeader(url);
            string strhtml = HttpHelper.HttpGet(h);
            return strhtml;
        }
    }
}
