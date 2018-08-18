using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Infrastructure
{
    public class SpiderException : Exception
    {
        public SpiderException(string msg) : base(msg)
        {

        }
    }
}
