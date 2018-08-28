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
            double similarity = 0;
            bool result = false;


            var text1 = "第1 章 奇遇，气死人不偿命";
            var text2 = "第001 章奇遇气死人不偿命！";
            var text3 = "第一章 奇遇，气死人不偿命！";

            result = Utils.CompareChapter(text1, text2, out similarity);
            Console.WriteLine("{0} >>> {1}\r\n相似度: {2}\r\n判断结果：{3}\r\n", text1, text2, similarity, result);

            result = Utils.CompareChapter(text1, text3, out similarity);
            Console.WriteLine("{0} >>> {1}\r\n相似度: {2}\r\n判断结果：{3}\r\n", text1, text3, similarity, result);

            result = Utils.CompareChapter(text2, text3, out similarity);
            Console.WriteLine("{0} >>> {1}\r\n相似度: {2}\r\n判断结果：{3}\r\n", text2, text3, similarity, result);


            var text4 = "第1章 泼皮";
            var text5 = "第001章泼皮";
            var text6 = "第一章 泼皮";

            result = Utils.CompareChapter(text4, text5, out similarity);
            Console.WriteLine("{0} >>> {1}\r\n相似度: {2}\r\n判断结果：{3}\r\n", text4, text5, similarity, result);

            result = Utils.CompareChapter(text4, text6, out similarity);
            Console.WriteLine("{0} >>> {1}\r\n相似度: {2}\r\n判断结果：{3}\r\n", text4, text6, similarity, result);

            result = Utils.CompareChapter(text5, text6, out similarity);
            Console.WriteLine("{0} >>> {1}\r\n相似度: {2}\r\n判断结果：{3}\r\n", text5, text6, similarity, result);


        }

    }
}
