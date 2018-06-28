using Spider.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Service
{
    public class TYSpiderService1
    {
        private static readonly string classinfo = nameof(TYSpiderService);
        private Site _site;

        public Site Site
        {
            get { return _site; }
            set
            {
                if (value == null)
                {
                    throw new Exception("");
                }
                _site = value;
            }
        }

        protected TYSpiderService1()
        {

        }

        public TYSpiderService1(Site site) : this()
        {
            Site = site;
        }

        
    }
}
