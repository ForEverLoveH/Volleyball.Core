using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Volleyball.Core.GameSystem.GameHelper
{
    public class LoggerHelper
    {
        ///记录Error日志
        public static void Error(string errorMsg, Exception ex = null)
        {
            if (ex != null)
            {
                string message = GameRoot.GetExceptionMsg(ex);
                Log.Debug(message);
                //LogError.Error(message);
            }
            else
            {
                Log.Debug(errorMsg);
            }
        }

        public static void Debug(Exception ex)
        {
            if (ex != null)
            {
                string message = GameRoot.GetExceptionMsg(ex);
                Log.Debug(message);
            }
        }

        /// 记录Info日志
        public static void Info(string msg, Exception ex = null)
        {
            if (ex != null)
            {
                string message = GameRoot.GetExceptionMsg(ex);
                Log.Information(message);
                //LogInfo.Info(message);
            }
            else
            {
                Log.Information(msg);
            }
        }

        /// 记录Monitor日志
        public static void Monitor(string msg)
        {
            Log.Information(msg);
            //LogMonitor.Info(msg);
        }

        public static void Warn(string msg, Exception ex = null)
        {
            if (ex != null)
            {
                string message = GameRoot.GetExceptionMsg(ex);
                Log.Warning(message);
            }
            else
            {
                Log.Warning(msg);
            }
        }

        public static void Fatal(string message)
        {
            Log.Fatal(message);
        }
    }
}