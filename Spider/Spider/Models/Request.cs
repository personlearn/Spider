using Newtonsoft.Json;
using Spider.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Models
{
    public class Request
    {
        private readonly object _locker = new object();
        private string _url;
        private Uri _uri;

        /// <summary>
        /// 站点信息
        /// </summary>
        [JsonIgnore]
        public Site Site { get; set; }

        [JsonIgnore]
        public bool IsAvailable
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Url))
                {
                    return false;
                }
                if (Url.Length < 6)
                {
                    return false;
                }
                var schema = Url.Substring(0, 5).ToLower();
                if (!schema.StartsWith("http") && !schema.StartsWith("https"))
                {
                    return false;
                }
                return true;
            }
        }

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

        public string Referer { get; set; }

        public string Origin { get; set; }

        public HttpMethod Method { get; set; }

        public int Priority { get; set; }

        public string PostBody { get; set; }

        public Dictionary<string, dynamic> Extras { get; set; }

        [JsonIgnore]
        public Uri Uri => _uri;

        [JsonIgnore]
        public string Identity => CryptoUtil.Md5Encrypt(Url + PostBody);

        [JsonIgnore]
        public HttpStatusCode? StatusCode { get; set; }

        public string DownloaderGroup { get; set; }


        public int Depth { get; set; } = 1;

        [JsonIgnore]
        public int NextDepth => Depth + 1;

        public int CycleTriedTimes { get; set; }

        public Request()
        {
        }

        public Request(string url) : this(url, null)
        {
        }

        public Request(string url, IDictionary<string, dynamic> extras = null)
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

        public dynamic GetExtra(string key)
        {
            lock (_locker)
            {
                if (Extras == null)
                {
                    return null;
                }

                if (Extras.ContainsKey(key))
                {
                    return Extras[key];
                }
                return null;
            }
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;

            Request request = (Request)obj;

            if (!Url.Equals(request.Url)) return false;

            return true;
        }

        public override int GetHashCode()
        {
            return Identity.GetHashCode();
        }

        public void Dispose()
        {
            Extras.Clear();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public Request Clone()
        {
            lock (_locker)
            {
                IDictionary<string, dynamic> extras = new Dictionary<string, dynamic>();
                if (Extras != null)
                {
                    foreach (var entry in Extras)
                    {
                        extras.Add(entry.Key, entry.Value);
                    }
                }
                Request newObj = new Request(Url, extras)
                {
                    Method = Method,
                    Priority = Priority,
                    Referer = Referer,
                    PostBody = PostBody,
                    Origin = Origin,
                    Depth = Depth,
                    CycleTriedTimes = CycleTriedTimes,
                    //Proxy = Proxy,
                    StatusCode = StatusCode
                };
                return newObj;
            }
        }
    }
}
