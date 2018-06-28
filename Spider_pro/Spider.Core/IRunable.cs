using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core
{
    public interface IRunable
    {
        void Run(params string[] arguments);
        Task RunAsnyc(params string[] arguments);
    }
}
