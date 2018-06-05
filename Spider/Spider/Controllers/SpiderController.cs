using Spider.IService;
using Spider.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Spider.Controllers
{
    public class SpiderController : ApiController
    {
        [HttpGet]
        public string TYSpider(string url)
        {
            ISpider s = new TYSpiderService(url);
            return s.run();
        }

        [HttpGet]
        public string TYSpider()
        {
            return "helloworld";
        }
    }
}
