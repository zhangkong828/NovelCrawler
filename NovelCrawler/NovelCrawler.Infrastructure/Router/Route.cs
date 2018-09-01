using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Infrastructure.Router
{
    public class Route
    {
        /// <summary>
        /// 获取分片id
        /// </summary>
        public static string GetShardingId(string id)
        {
            //long.TryParse(id, out long LId);
            //return (LId % 8).ToString();

            var hashcode =Math.Abs(id.GetHashCode());
            return (hashcode % 8).ToString();
        }
    }
}
