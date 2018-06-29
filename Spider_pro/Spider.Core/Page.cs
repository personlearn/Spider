using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core
{
    public class Page
    {
        private string _content;


        public string TargetUrl { get; set; }

        public Request Request { get; }

        public string Url { get; }

        public Exception Exception { get; set; }

        public ResultItems ResultItems { get; } = new ResultItems();

        public Page(Request request)
        {
            Request = request;
            Url = request.Url;
            ResultItems.Request = request;
        }

        public string Content
        {
            get { return _content; }
            set
            {
                if (!Equals(value, _content))
                {
                    _content = value;
                }
            }
        }
    }
}
