using NovelCrawler.Infrastructure.Configuration;
using System;

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

            Console.ReadKey();
        }
    }
}
