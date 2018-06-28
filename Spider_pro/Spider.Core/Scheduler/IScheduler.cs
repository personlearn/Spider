using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core.Scheduler
{
    public interface IScheduler
    {
        void Init(ISpider spider);

        void Push(Request request);

        Request Poll();

        void Import(IEnumerable<Request> requests);

        void Export();
    }
}
