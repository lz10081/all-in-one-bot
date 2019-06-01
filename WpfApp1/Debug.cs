using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZenAIO
{
    /// <summary>
    /// Debug class for conditionally writing statements to the debug console.
    /// </summary>
    public class Debug
    {

        public enum LogLevel
        {
            INFO, WARN, ERROR, BUG
        }

        private static string[] logLevelStr = { "INFO", "WARN", "ERROR", "BUG" };

        private Debug()
        {

        }

        public static void Log(LogLevel level, string message)
        {
            // Only write to console if in Debug build.
#if DEBUG
            Console.WriteLine("[{0}: {1}]", logLevelStr[(int) level], message);
#endif
        }

        public static void Log(LogLevel level, string format, params object[] objects)
        {
            string message = string.Format(format, objects);
            Log(level, message);
        }

        public static void Info(string message)
        {
            Log(LogLevel.INFO, message);
        }

        public static void Info(string format, string message)
        {
            Log(LogLevel.INFO, format, message);
        }

        public static void Warn(string message)
        {
            Log(LogLevel.WARN, message);
        }

        public static void Warn(string format, string message)
        {
            Log(LogLevel.WARN, format, message);
        }

        public static void Error(string message)
        {
            Log(LogLevel.ERROR, message);
        }

        public static void Error(string format, string message)
        {
            Log(LogLevel.ERROR, format, message);
        }

        public static void Bug(string message)
        {
            Log(LogLevel.BUG, message);
        }

        public static void Bug(string format, string message)
        {
            Log(LogLevel.BUG, format, message);
        }

    }
}
