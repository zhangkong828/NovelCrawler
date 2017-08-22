using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovelCrawler.Common
{
    public class ClaCommon
    {
        private static readonly Random random = new Random();

        public static int Random()
        {
            return random.Next();
        }

        public static int Random(int min, int max)
        {
            return random.Next(min, max);
        }
    }
}
