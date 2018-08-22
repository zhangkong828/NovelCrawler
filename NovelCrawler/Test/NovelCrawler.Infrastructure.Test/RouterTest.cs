using Microsoft.VisualStudio.TestTools.UnitTesting;
using NovelCrawler.Infrastructure.Router;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Infrastructure.Test
{
    [TestClass]
    public class RouterTest
    {
        [TestMethod]
        public void GetShardingId()
        {
            for (int i = 0; i < 1000; i++)
            {
                var id = ObjectId.NextId();
                Console.WriteLine("ObjectId:{0}", id);
                Console.WriteLine("ShardingId:{0}\r\n", Route.GetShardingId(id));
            }
        }
    }
}
