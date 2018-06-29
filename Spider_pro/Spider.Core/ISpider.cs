using Spider.Core.Downloader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core
{
    public interface ISpider : IDisposable
    {
        Site Site { get; }
        IDownloader Downloader { get; set; }
    }
}
