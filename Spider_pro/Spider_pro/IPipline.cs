using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider_pro
{
    public interface IPipline : IDisposable
    {
        void Process();
    }
}
