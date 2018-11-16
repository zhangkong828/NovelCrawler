using NovelCrawler.Infrastructure;
using NovelCrawler.Processer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

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

            ProcessEngine.Create().Start();
            Console.WriteLine("Services.Start");

            while (true)
            {
                Console.ReadKey();
            }
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Logger.Fatal("Services.ProcessExit");
            Stop();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Logger.Fatal("Services.CancelKeyPress");
            Stop();
        }

        private static void Stop()
        {
            ProcessEngine.Create().Stop();
            int count = 10;
            while (count > 0)
            {
                Console.WriteLine("Waiting Stop ... {0}", count);
                Thread.Sleep(1000);
                count--;
            }
            Logger.Fatal("Services.Stop");
        }
    }
}
