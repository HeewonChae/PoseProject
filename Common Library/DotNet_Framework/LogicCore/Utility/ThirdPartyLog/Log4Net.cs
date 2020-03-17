using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicCore.Utility.ThirdPartyLog
{
    public class Log4Net
    {
        public enum Level
        {
            DEBUG,
            INFO,
            WARN,
            ERROR,
            FATAL,
        }

        public static bool IsInitialize = false;
        private static readonly string DefaultFilePath = AppDomain.CurrentDomain.BaseDirectory;
        private static ILog _Ilog = null;

        public static bool Initialize(string defaultLogger = null)
        {
            if (log4net.Config.XmlConfigurator.Configure() == null)
                throw new Exception($"Failed Initailize Log4Net");

            if (string.IsNullOrEmpty(defaultLogger))
                _Ilog = log4net.LogManager.GetCurrentLoggers().First();
            else
                _Ilog = log4net.LogManager.GetLogger(defaultLogger);

            return IsInitialize = true;
        }

        public static ILog GetLog4Net(string name)
        {
            if (_Ilog == null)
                throw new Exception($"Not Initialize Log4Net");

            _Ilog = log4net.LogManager.GetLogger(name);

            return _Ilog;
        }

        public static ILog GetLog4Net(Type type)
        {
            if (_Ilog == null)
                throw new Exception($"Not Initialize Log4Net");

            _Ilog = log4net.LogManager.GetLogger(type);

            return _Ilog;
        }

        public static void WriteLog(string message, Level logLevel = Level.DEBUG,
            [System.Runtime.CompilerServices.CallerLineNumber] int line = 0,
            [System.Runtime.CompilerServices.CallerFilePath] string fileName = "")
        {
            if (_Ilog == null)
                return;

            message = $"[fileName: {fileName}] [line: {line}] [message: {message}]";

            switch (logLevel)
            {
                case Level.DEBUG:
                    if (_Ilog.IsDebugEnabled)
                        _Ilog.Debug(message);
                    break;

                case Level.INFO:
                    if (_Ilog.IsInfoEnabled)
                        _Ilog.Info(message);
                    break;

                case Level.WARN:
                    if (_Ilog.IsWarnEnabled)
                        _Ilog.Warn(message);
                    break;

                case Level.ERROR:
                    if (_Ilog.IsErrorEnabled)
                        _Ilog.Error(message);
                    break;

                case Level.FATAL:
                    if (_Ilog.IsFatalEnabled)
                        _Ilog.Fatal(message);
                    break;
            }
        }
    }
}