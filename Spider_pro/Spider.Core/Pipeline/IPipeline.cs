using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core.Pipeline
{
    public interface IPipeline:IDisposable
    {
        void Process(IEnumerable<ResultItems> resultItems, ISpider spider);
    }
}
