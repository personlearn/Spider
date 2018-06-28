using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Core.Downloader
{
    public class DownloadException : SpiderException
    {
        public DownloadException() : base("Download Exception")
        {
        }

        public DownloadException(string message) : base(message)
        {
        }
    }
}
