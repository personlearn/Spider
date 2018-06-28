using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core
{
    public interface IExecuteRecord
    {
        ILogger Logger { get; set; }

        bool Add(string taskId, string name, string identity);

        void Remove(string taskId, string name, string identity);
    }
}
