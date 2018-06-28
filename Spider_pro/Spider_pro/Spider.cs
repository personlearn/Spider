using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider_pro
{
    public class Spider : ISpider
    {
        private IDownloader _downloader;
        private IPipline _pipline;

        public IDownloader Downloader
        {
            get
            {
                return _downloader;
            }
        }

        public IPipline Pipline
        {
            get
            {
                return _pipline;
            }
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
