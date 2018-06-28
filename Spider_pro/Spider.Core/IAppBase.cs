using Serilog;
using Spider.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core
{
    public interface IAppBase : IRunable, IIdentity, ITask, INamed
    {
        ILogger Logger { get; }
    }

    public abstract class AppBase : Named, IAppBase
    {
        private ILogger _logger;
        public virtual string Identity { get; set; }

        public ILogger Logger
        {
            get
            {
                if (_logger == null && !string.IsNullOrWhiteSpace(Identity))
                {
                    _logger = LogUtil.Create(Identity);
                }
                return _logger;
            }
            set
            {
                if (value != null)
                {
                    _logger = value;
                }
            }
        }

        public string TaskId { get; set; }

        public void Run(params string[] arguments)
        {
            Execute(arguments);
        }

        public Task RunAsnyc(params string[] arguments)
        {
            return Task.Factory.StartNew(() => Run(arguments));
        }

        protected AppBase()
        {

        }

        protected AppBase(string name) : base()
        {
            Name = name;
        }

        public IExecuteRecord ExecuteRecord { get; set; }

        protected abstract void Execute(params string[] arguments);
    }
}
