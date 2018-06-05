using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Spider.util
{
    public abstract class HttpHeader
    {
        protected HttpWebRequest httpreq;
        protected string userAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
        protected string contentType = "application/x-www-form-urlencoded";
        protected string accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, */*";
        protected int TimeOut = 10000;

        public HttpHeader(string url)
        {
            httpreq = (HttpWebRequest)WebRequest.Create(url);
            httpreq.UserAgent = userAgent;
            httpreq.ContentType = contentType;
            httpreq.Accept = accept;
            httpreq.Timeout = TimeOut;
        }

        public virtual HttpWebRequest returnreq()
        {
            return httpreq;
        }
    }
}
