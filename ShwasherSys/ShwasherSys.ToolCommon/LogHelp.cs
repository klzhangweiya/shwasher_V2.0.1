using System;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace ShwasherSys
{
    public static class LogHelper
    {
        private static ILog Log { get; set; }

        public static void LogInfo<T>(this T t, object message) where T : class
        {
            Log = LogManager.GetLogger(t.GetType());
            Log.Info(message);
        }

        public static void LogDebug<T>(this T t, object message) where T : class
        {
            Log = LogManager.GetLogger(t.GetType());
            Log.Debug(message);
        }

        public static void LogError<T>(this T t, object message) where T : class
        {
            Log = LogManager.GetLogger(t.GetType());
            Log.Error(message);
        }
        public static void LogError<T>(this T t, Exception e) where T : class
        {
            Log = LogManager.GetLogger(t.GetType());
            Log.Error(e);
        }

        public static void LogFatal<T>(this T t, object message) where T : class
        {
            Log = LogManager.GetLogger(t.GetType());
            Log.Fatal(message);
        }

        public static void LogFatal<T>(this T t, Exception e) where T : class
        {
            Log = LogManager.GetLogger(t.GetType());
            Log.Fatal(e);
        }

        public static void LogWarn<T>(this T t, object message) where T : class
        {
            Log = LogManager.GetLogger(t.GetType());
            Log.Warn(message);
        }
    }
}
