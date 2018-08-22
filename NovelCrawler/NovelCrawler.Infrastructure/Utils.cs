using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace NovelCrawler.Infrastructure
{
    public class Utils
    {
        /// <summary>
        /// 对象的属性是否都不为null或空,仅限string类型，可设置忽略项
        /// </summary>
        /// <param name="o"></param>
        /// <param name="ignores">忽略的属性名</param>
        /// <returns></returns>
        public static bool ObjectIsNotNull<T>(T o, params string[] ignores) where T : class
        {
            var t = typeof(T);
            foreach (var pi in t.GetProperties())
            {
                if (pi.PropertyType.Name == "String")
                {
                    if (ignores.Contains(pi.Name))
                        continue;
                    var v = pi.GetValue(o);
                    if (v == null || string.IsNullOrWhiteSpace(v.ToString()))
                        return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 统计字数。目前只统计汉字和英文单词
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static int GetWordCount(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return 0;

            //清理html标签
            text = Regex.Replace(text, "<\\/?.+?\\/?>", " ");

            //英文单词  连词不算1个，算2个单词
            var reg1 = new Regex(@"[A-Za-z0-9][A-Za-z0-9\-.]*");
            int count = reg1.Matches(text).Count;

            var chats = text.ToCharArray();
            int count1 = 0;//汉字
            int count2 = 0;//字母(单个字母)
            int count3 = 0;//数字(单个数字)

            for (int i = 0; i < chats.Length; i++)
            {
                var c = (int)chats[i];
                if (c >= 0x4E00 && c <= 0x9FA5)//汉字
                {
                    count1++;
                }
                else if ((c >= 65 && c <= 90) || c >= 97 && c <= 122)//字母
                {
                    count2++;
                }
                else if (c >= 48 && c <= 57)//数字
                {
                    count3++;
                }
            }
            return count + count1;
        }


    }
}
