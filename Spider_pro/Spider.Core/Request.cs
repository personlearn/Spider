using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core
{
    public class Request
    {
        private readonly object _locker = new object();
        private string _url;
        private Uri _uri;

        public Dictionary<string, dynamic> Extras { get; set; }

        public HttpMethod Method { get; set; }

        public string Referer { get; set; }

        public string Origin { get; set; }

        public string PostBody { get; set; }

        [JsonIgnore]
        public HttpStatusCode? StatusCode { get; set; }

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

        public Request(string url,IDictionary<string,dynamic> extras = null)
        {
            Url = url;

            if (string.IsNullOrWhiteSpace(Url))
            {
                throw new ArgumentException("Url should not be empty or null.");
            }

            if (extras != null)
            {
                foreach (var extra in extras)
                {
                    PutExtra(extra.Key, extra.Value);
                }
            }
        }

        public void PutExtra(string key, dynamic value)
        {
            lock (_locker)
            {
                if (key != null)
                {
                    if (Extras == null)
                    {
                        Extras = new Dictionary<string, dynamic>();
                    }

                    if (Extras.ContainsKey(key))
                    {
                        Extras[key] = value;
                    }
                    else
                    {
                        Extras.Add(key, value);
                    }
                }
            }
        }
    }
}
