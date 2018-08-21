using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Infrastructure
{
    public static class ObjectId
    {
        private static readonly IdWorker _idWorker;
        static ObjectId()
        {
            _idWorker = new IdWorker(0, 0);//机器标识  数据中心标识
        }

        public static string NextId()
        {
            return _idWorker.NextId().ToString();
        }

    }
}
