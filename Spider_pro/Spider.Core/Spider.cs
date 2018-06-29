using Spider.Core.Downloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Spider.Core
{
    public class Spider : ISpider
    {
        private Site _site = new Site();
        private IDownloader _downloader = new HttpClientDownloader();
        private List<Request> Queue = new List<Request>();

        public Status Status { get; private set; } = Status.Init;

        public Site Site
        {
            get
            {
                return _site;
            }
            set
            {
                if (value == null) throw new Exception();
                _site = value;
            }
        }

        public IDownloader Downloader
        {
            get { return _downloader; }
            set
            {
                _downloader = value;
                if (value == null)
                {
                    throw new ArgumentNullException($"{nameof(Downloader)} should not be null.");
                }
            }
        }

        protected Spider()
        {

        }

        public Spider(Site site) : this()
        {
            Site = site;
        }

        public void Execute()
        {
            Status = Status.Running;
            foreach (var req in Site.StartRequests)
            {
                Queue.Add(req);
            }
            while (Status == Status.Running || Status == Status.Paused)
            {
                // 暂停则一直停在此处
                if (Status == Status.Paused)
                {
                    Thread.Sleep(50);
                    continue;
                }
                var downloader = Downloader.Clone();
                Parallel.For(0, 1, new ParallelOptions
                {
                    MaxDegreeOfParallelism = 1
                }, i =>
                {
                    while (Status == Status.Running)
                    {
                        if (Queue.Count != 0)
                        {
                            Request req = Queue.Last();
                            Queue.RemoveAt(0);
                            var page = downloader.Download(req, this).Result;

                            if (page.Exception == null)
                            {

                            }
                        }

                    }
                });
            }
        }

        public void Dispose()
        {
            Downloader.Dispose();
        }
    }
}
