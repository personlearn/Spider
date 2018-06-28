using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider_pro
{
    public interface ISpider
    {
        void Run();
        IDownloader Downloader { get; }
        IPipline Pipline { get; }
    }
}
