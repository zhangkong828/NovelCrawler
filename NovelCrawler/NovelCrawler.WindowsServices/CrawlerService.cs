using NovelCrawler.Infrastructure;
using NovelCrawler.Processer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelCrawler.WindowsServices
{
    public class CrawlerService
    {
        private static ProcessEngine _engine = null;
        public void Start()
        {
            try
            {
                _engine = ProcessEngine.Create();
                _engine.Start();
            }
            catch (Exception ex)
            {
                Logger.Fatal("CrawlerService Start:{0}\r\n{1}", ex.Message, ex.StackTrace);
            }
        }

        public void Stop()
        {
            try
            {
                _engine.Stop();
            }
            catch (Exception ex)
            {
                Logger.Fatal("CrawlerService Stop:{0}\r\n{1}", ex.Message, ex.StackTrace);
            }
        }
    }
}
