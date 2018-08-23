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

            ProcessEngine.Create().Start();


            Console.WriteLine("over");
            Console.ReadKey();
        }
    }
}
