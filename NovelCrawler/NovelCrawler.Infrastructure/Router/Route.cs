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
            var hashcode = GetStringHashcode(id);
            return (hashcode % 8).ToString();
        }

        /// <summary>
        /// 返回平台无关的hashcode
        /// </summary>
        private static int GetStringHashcode(string s)
        {
            if (string.IsNullOrEmpty(s)) return 0;

            unchecked
            {
                int hash = 23;
                foreach (char c in s)
                {
                    hash = (hash << 5) - hash + c;
                }
                if (hash < 0)
                {
                    hash = Math.Abs(hash);
                }
                return hash;
            }
        }
    }
}
