using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core
{
    public class Page
    {
        public bool Skip { get; set; }
        public Exception Exception { get; set; }

        public Request Request { get; }
    }
}
