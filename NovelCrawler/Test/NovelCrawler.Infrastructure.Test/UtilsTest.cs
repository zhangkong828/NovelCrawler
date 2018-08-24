using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace NovelCrawler.Infrastructure.Test
{
    [TestClass]
    public class UtilsTest
    {
        [TestMethod]
        public void LevenshteinDistance()
        {
            var t = "第001章";
            Utils.ReplaceNumberToChinese(t);

            double similarity;
            var text1 = "第1章 奇遇，气死人不偿命";
            var text2 = "第001章奇遇气死人不偿命！";
            var text3 = "第一章 奇遇，气死人不偿命！";

            Utils.LevenshteinDistance(text1, text2, out similarity);
            Console.WriteLine("{0} >>> {1}\r\n相似度: {2}\r\n", text1, text2, similarity);

            Utils.LevenshteinDistance(text1, text3, out similarity);
            Console.WriteLine("{0} >>> {1}\r\n相似度: {2}\r\n", text1, text3, similarity);

            Utils.LevenshteinDistance(text2, text3, out similarity);
            Console.WriteLine("{0} >>> {1}\r\n相似度: {2}\r\n", text2, text3, similarity);


            var text4 = "第1章 泼皮";
            var text5 = "第001章泼皮";
            var text6 = "第一章 泼皮";

            Utils.LevenshteinDistance(text4, text5, out similarity);
            Console.WriteLine("{0} >>> {1}\r\n相似度: {2}\r\n", text4, text5, similarity);

            Utils.LevenshteinDistance(text4, text6, out similarity);
            Console.WriteLine("{0} >>> {1}\r\n相似度: {2}\r\n", text4, text6, similarity);

            Utils.LevenshteinDistance(text5, text6, out similarity);
            Console.WriteLine("{0} >>> {1}\r\n相似度: {2}\r\n", text5, text6, similarity);


        }

    }
}
