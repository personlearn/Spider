using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core
{
    [Flags]
    public enum Status
    {
        /// <summary>
        /// 初始化
        /// </summary>
        Init = 1,

        /// <summary>
        /// 正在运行
        /// </summary>
        Running = 2,

        /// <summary>
        /// 暂停
        /// </summary>
        Paused = 4,

        /// <summary>
        /// 完成
        /// </summary>
        Finished = 8,

        /// <summary>
        /// 退出
        /// </summary>
        Exited = 16
    }
}
