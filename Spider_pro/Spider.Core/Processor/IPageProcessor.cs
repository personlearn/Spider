using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core.Processor
{
    public interface IPageProcessor
    {
        void Process(Page page, ISpider spider);
    }
}
