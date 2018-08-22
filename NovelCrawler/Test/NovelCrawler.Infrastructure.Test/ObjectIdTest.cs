using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Infrastructure.Test
{
    [TestClass]
    public class ObjectIdTest
    {
        [TestMethod]
        public void NextId()
        {
            for (int i = 0; i < 1000000; i++)
            {
                var id = ObjectId.NextId();
                Console.WriteLine(id);
            }
        }

    }
}
