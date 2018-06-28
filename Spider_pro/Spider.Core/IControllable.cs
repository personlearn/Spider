using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core
{
    public interface IControllable
    {
        void Pause(Action action = null);

        void Contiune();

        void Exit(Action action = null);
    }
}
