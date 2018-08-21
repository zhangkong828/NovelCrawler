using NovelCrawler.Infrastructure.Configuration;
using NovelCrawler.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Services
{
    class Program
    {
        static void Main(string[] args)
        {
            var _constr = ConfigurationManager.GetValue("MongoDB:connectionString");
            var _dbName = ConfigurationManager.GetValue("MongoDB:defaultDBName");
            var _collectionName = ConfigurationManager.GetValue("MongoDB:defaultCollectionName");

            Console.WriteLine(_constr);
            Console.WriteLine(_dbName);
            Console.WriteLine(_collectionName);

            var novelSort = ConfigurationManager.GetSection<List<NovelSortSettings>>("SpiderSettings:NovelSort");
            Console.WriteLine(novelSort.Count);

            Console.WriteLine("啊大大");
            Console.ReadKey();
        }
    }
}
