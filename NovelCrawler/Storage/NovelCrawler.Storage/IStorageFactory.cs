using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Storage
{
    public interface IStorageFactory
    {
        IStorage GetOrCreate();
    }
}
