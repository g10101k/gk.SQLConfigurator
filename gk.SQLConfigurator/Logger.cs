using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;
using log4net.Config;
using log4net.Core;

namespace gk.SQLConfigurator
{
    public static class Logger
    {
        private static ILog log;

        private static ILog Log
        {
            get
            {
                if (log == null)
                {
                    InitLogger();
                    log = LogManager.GetLogger("LOGGER");
                }
                return log;
            }
        }
           
        private static void InitLogger()
        {
            XmlConfigurator.Configure();
        }

        public static void Debug(object message)
        {
            Log.Debug(message);
        }

        public static void Debug(object message, Exception exception)
        {
            Log.Debug(message, exception);
        }

        public static void DebugFormat(string format, params object[] args)
        {
            Log.DebugFormat(format, args);
        }

        public static void DebugFormat(string format, object arg0)
        {
            Log.DebugFormat(format, arg0);
        }

        public static void DebugFormat(string format, object arg0, object arg1)
        {
            Log.DebugFormat(format, arg0, arg1);
        }

        public static void DebugFormat(string format, object arg0, object arg1, object arg2)
        {
            Log.DebugFormat(format, arg0, arg1, arg2);
        }

        public static void DebugFormat(IFormatProvider provider, string format, params object[] args)
        {
            Log.DebugFormat(provider, format, args);
        }

        public static void Error(object message)
        {
            Log.Error(message);
        }

        public static void Error(object message, Exception exception)
        {
            Log.Error(message, exception);
        }

        public static void ErrorFormat(string format, params object[] args)
        {
            Log.ErrorFormat(format, args);
        }

        public static void ErrorFormat(string format, object arg0)
        {
            Log.ErrorFormat(format, arg0);
        }

        public static void ErrorFormat(string format, object arg0, object arg1)
        {
            Log.ErrorFormat(format, arg0, arg1);
        }

        public static void ErrorFormat(string format, object arg0, object arg1, object arg2)
        {
            Log.ErrorFormat(format, arg0, arg1, arg2);
        }

        public static void ErrorFormat(IFormatProvider provider, string format, params object[] args)
        {
            Log.ErrorFormat(provider, format, args);
        }

        public static void Fatal(object message)
        {
            Log.Fatal(message);
        }

        public static void Fatal(object message, Exception exception)
        {
            Log.Fatal(message, exception);
        }

        public static void FatalFormat(string format, params object[] args)
        {
            Log.FatalFormat(format, args);
        }

        public static void FatalFormat(string format, object arg0)
        {
            Log.FatalFormat(format, arg0);
        }

        public static void FatalFormat(string format, object arg0, object arg1)
        {
            Log.FatalFormat(format, arg0, arg1);
        }

        public static void FatalFormat(string format, object arg0, object arg1, object arg2)
        {
            Log.FatalFormat(format, arg0, arg1, arg2);
        }

        public static void FatalFormat(IFormatProvider provider, string format, params object[] args)
        {
            Log.FatalFormat(provider, format, args);
        }

        public static void Info(object message)
        {
            Log.Info(message);
        }

        public static void Info(object message, Exception exception)
        {
            Log.Info(message, exception);
        }

        public static void InfoFormat(string format, params object[] args)
        {
            Log.InfoFormat(format, args);
        }

        public static void InfoFormat(string format, object arg0)
        {
            Log.InfoFormat(format, arg0);
        }

        public static void InfoFormat(string format, object arg0, object arg1)
        {
            Log.InfoFormat(format, arg0, arg1);
        }

        public static void InfoFormat(string format, object arg0, object arg1, object arg2)
        {
            Log.InfoFormat(format, arg0, arg1, arg2);
        }

        public static void InfoFormat(IFormatProvider provider, string format, params object[] args)
        {
            Log.InfoFormat(provider, format, args);
        }

        public static void Warn(object message)
        {
            Log.Warn(message);
        }

        public static void Warn(object message, Exception exception)
        {
            Log.Warn(message, exception);
        }

        public static void WarnFormat(string format, params object[] args)
        {
            Log.WarnFormat(format, args);
        }

        public static void WarnFormat(string format, object arg0)
        {
            Log.WarnFormat(format, arg0);
        }

        public static void WarnFormat(string format, object arg0, object arg1)
        {
            Log.WarnFormat(format, arg0, arg1);
        }

        public static void WarnFormat(string format, object arg0, object arg1, object arg2)
        {
            Log.WarnFormat(format, arg0, arg1, arg2);
        }

        public static void WarnFormat(IFormatProvider provider, string format, params object[] args)
        {
            Log.WarnFormat(provider, format, args);
        }
    }
}
