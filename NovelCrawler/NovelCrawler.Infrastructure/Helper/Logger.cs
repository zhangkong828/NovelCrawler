using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Infrastructure
{
    public class Logger
    {
        private static ILogger logger = LogManager.GetLogger("");

        public static Action<string> _customAction = null;

        public static void Trace(string msg, params object[] args)
        {
            logger.Trace(msg, args);
        }

        public static void Debug(string msg, params object[] args)
        {
            logger.Debug(msg, args);
        }

        public static void Info(string msg, params object[] args)
        {
            logger.Info(msg, args);
        }

        public static void Warn(string msg, params object[] args)
        {
            logger.Warn(msg, args);
        }

        public static void Error(string msg, params object[] args)
        {
            logger.Error(msg, args);
        }

        public static void Error(Exception ex, string msg, params object[] args)
        {
            logger.Error(ex, msg, args);
        }

        public static void Fatal(string msg, params object[] args)
        {
            logger.Fatal(msg, args);
        }

        public static void Fatal(Exception ex, string msg, params string[] args)
        {
            logger.Fatal(ex, msg, args);
        }


        public static void ColorConsole(string msg, ConsoleColor consoleColor = ConsoleColor.White)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = consoleColor;
            Console.WriteLine($"{msg}");
            Console.ForegroundColor = old;

            _customAction?.Invoke(msg);
        }

        public static void ColorConsole2(string msg, ConsoleColor consoleColor = ConsoleColor.Green)
        {
            var old = Console.ForegroundColor;
            Console.ForegroundColor = consoleColor;
            Console.WriteLine($"[{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}]{msg}");
            Console.ForegroundColor = old;

            _customAction?.Invoke(msg);
        }

    }
}
