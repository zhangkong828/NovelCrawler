using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Infrastructure.Test
{
    [TestClass]
    public class WordCountTest
    {
        [TestMethod]
        public void GetWordCount()
        {
            var text = "中华人民共和国";
            var count = Utils.GetWordCount(text);
            Assert.AreEqual(count, 7);
        }


        [TestMethod]
        public void GetWordCount2()
        {
            var text = "\"I looked at it and said, 'We'll be back in a few days,' \" Byron Largent said of the china. ";
            var count = Utils.GetWordCount(text);
            Assert.AreEqual(count, 20);
        }

        [TestMethod]
        public void GetWordCount3()
        {
            var text = "I have 100$";
            var count = Utils.GetWordCount(text);
            Assert.AreEqual(count, 3);
        }

        [TestMethod]
        public void GetWordCount4()
        {
            var text = "我有50.5元";
            var count = Utils.GetWordCount(text);
            Assert.AreEqual(count, 4);
        }

        [TestMethod]
        public void GetWordCount5()
        {
            var text = "<p>十大撒旦<br/>阿达</p><div class=\"sax\">黄蜡石的三</div>";
            var count = Utils.GetWordCount(text);
            Assert.AreEqual(count, 11);
        }
    }
}
