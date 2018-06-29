using Spider.Core.Infrastructure;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core
{
    public class Site
    {
        private Encoding _encoding = Encoding.UTF8;
        private string _encodingName;
        private ConcurrentBag<Request> _startRequests = new ConcurrentBag<Request>();

        public IReadOnlyCollection<Request> StartRequests => new ReadOnlyEnumerable<Request>(_startRequests);

        public Dictionary<string, string> Headers { get; set; } = new Dictionary<string, string>();

        public Encoding Encoding => _encoding;

        public string UserAgent { get; set; } = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/47.0.2526.106 Safari/537.36";

        public string Accept { get; set; } = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";

        public string EncodingName
        {
            get { return _encodingName; }
            set
            {
                if (_encodingName != value)
                {
                    _encodingName = value;
                    _encoding = string.IsNullOrEmpty(_encodingName) ? null : Encoding.GetEncoding(_encodingName);
                }
            }
        }

        public void AddStartUrl(string url)
        {
            AddStartUrls(url);
        }

        public void AddStartUrls(params string[] urls)
        {
            if (urls == null)
            {
                throw new ArgumentNullException($"{nameof(urls)} should not be null.");
            }
            AddStartUrls(urls.AsEnumerable());
        }

        public void AddStartUrls(IEnumerable<string> urls)
        {
            if (urls == null)
            {
                throw new ArgumentNullException($"{nameof(urls)} should not be null.");
            }
            foreach (var url in urls)
            {
                AddStartRequest(new Request(url, null));
            }
        }

        public void AddStartUrl(string url,IDictionary<string,dynamic> datas)
        {
            AddStartRequest(new Request(url, datas));
        }

        public void AddStartRequest(Request request)
        {
            AddStartRequests(request);
        }

        public void AddStartRequests(params Request[] requests)
        {
            if (requests == null)
            {
                throw new ArgumentNullException($"{nameof(requests)} should not be null.");
            }
            AddStartRequests(requests.AsEnumerable());
        }

        public void AddStartRequests(IEnumerable<Request> requests)
        {
            if (requests == null)
            {
                throw new ArgumentNullException($"{nameof(requests)} should not be null.");
            }
            foreach (var request in requests)
            {
                _startRequests.Add(request);
            }
        }
    }
}
