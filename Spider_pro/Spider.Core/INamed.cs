using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core
{
    public interface INamed
    {
        string Name { get; set; }
    }

    public abstract class Named : INamed
    {
        public string Name { get; set; }
    }
}
