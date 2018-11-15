using NovelCrawler.Infrastructure.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace NovelCrawler.WindowsServices
{
    class Program
    {
        static void Main(string[] args)
        {
            var c=ConfigurationManager.GetValue("MongoDB:connectionString");
            Console.WriteLine(c);
            Console.ReadKey();
            //HostFactory.Run(c =>
            //{
            //    c.RunAsLocalSystem();
            //    //服务名称
            //    c.SetServiceName("NovelCrawler.WindowsServices");
            //    //服务显示名称
            //    c.SetDisplayName("CrawlerService");
            //    //服务描述
            //    c.SetDescription("CrawlerService");

            //    c.Service<CrawlerService>(s =>
            //    {
            //        s.ConstructUsing(b => new CrawlerService());
            //        s.WhenStarted(o => o.Start());
            //        s.WhenStopped(o => o.Stop());
            //    });
            //});
        }
    }
}
