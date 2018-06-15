using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySpider.util
{
    public static class Loghelper
    {
        public static void Error(string typename, string errormsg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(typename);
            log.Error(errormsg);
        }

        public static void Info(string typename, string infomsg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(typename);
            log.Info(infomsg);
        }

        public static void Warning(string typename, string warningmsg)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(typename);
            log.Warn(warningmsg);
        }

        public static void Error(string typename, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(typename);
            log.Error("Error", ex);
        }

        public static void Info(string typename, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(typename);
            log.Info("Info", ex);
        }

        public static void Warning(string typename, Exception ex)
        {
            log4net.ILog log = log4net.LogManager.GetLogger(typename);
            log.Warn("Warning", ex);
        }
    }
}
