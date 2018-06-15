using MySpider.TYSpider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MySpider.Controllers
{
    public class SpiderController : ApiController
    {
        [HttpGet]
        public string TYSpider(string url)
        {
            TYSpiderService ty = new TYSpiderService();
            ty.Run(url);
            return "hello world";
        }
    }
}
