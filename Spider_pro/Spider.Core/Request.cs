using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core
{
    public class Request
    {
        private string _url;
        private Uri _uri;

        public string Url
        {
            get { return _url; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    _url = null;
                    return;
                }
                if (Uri.TryCreate(value.TrimEnd('#'), UriKind.RelativeOrAbsolute, out _uri))
                {
                    _url = _uri.ToString();
                }
                else
                {
                    _url = null;
                }
            }
        }
    }
}
