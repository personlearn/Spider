using Spider.Core.Downloader;
using Spider.Core.Pipeline;
using Spider.Core.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core
{
    public interface ISpider : IDisposable, IControllable, IAppBase
    {
        Site Site { get; }
        IDownloader Downloader { get; set; }  

        IReadOnlyCollection<IPageProcessor> PageProcessors { get; }

        IReadOnlyCollection<IPipeline> Pipelines { get; }
    }
}
