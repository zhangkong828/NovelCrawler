using NovelCrawler.Infrastructure;
using NovelCrawler.Processer;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Services
{
    class Program
    {
        static void Main(string[] args)
        {
            //捕获Ctrl+C事件
            Console.CancelKeyPress += Console_CancelKeyPress;
            //进程退出事件
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            Console.WriteLine("开始了");
            //ProcessEngine.Create().Start();

            while (true)
            {
                Console.ReadKey();
            }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Console.WriteLine("ProcessExit");
            Logger.Fatal("Services.ProcessExit");
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("CancelKeyPress");
            Logger.Fatal("Services.CancelKeyPress");
        }
    }
}
