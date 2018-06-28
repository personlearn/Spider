using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core.Downloader
{
    public interface IDownloader : IDisposable
    {
        Task<Page> Download(Request request, ISpider spider);

        IDownloader Clone();
    }
}
