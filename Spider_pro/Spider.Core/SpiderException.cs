using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core
{
    public class SpiderException : Exception
    {
        public SpiderException(string msg) : base(msg) { }

        public SpiderException(string msg, Exception e) : base(msg, e) { }
    }
}
