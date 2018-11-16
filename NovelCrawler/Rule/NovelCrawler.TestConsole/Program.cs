using NovelCrawler.Infrastructure;
using NovelCrawler.Models;
using NovelCrawler.Processer;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NovelCrawler.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var rule = XmlHelper.XmlDeserializeFromFile<RuleModel>("testRule.xml", Encoding.UTF8);
          
            var spider = new Spider(null, rule);
            spider.TestRule();
            Console.WriteLine("over");

            Console.ReadKey();
        }

       
       
    }
}
